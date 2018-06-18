using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge;
using AForge.Neuro;
using AForge.Neuro.Learning;
using System.IO;

//neural network creator
namespace trainer
{
    class Program
    {


        /*
          Copyright (c) 2018 Mihai Pruna

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.*/

        //Uses Aforge.NET: http://www.aforgenet.com/framework/
        //Aforge.NET license: http://www.aforgenet.com/framework/license.html

        static void Main(string[] args)
        {
            //args: data folder, learning ,momentum,error,iterations
            List<string> inputRows = File.ReadAllLines(args[0] + @"\input.csv").ToList();
            int inputSampleCount = inputRows.Count;
            char[] sep = { ',' };
            int inputCount = inputRows[0].Split(sep, StringSplitOptions.None).Length;
             
            List<string> outputRows = File.ReadAllLines(args[0] + @"\output.csv").ToList();
            int outputCount = outputRows[0].Split(sep, StringSplitOptions.None).Length;
            List<int> numberNeurons = File.ReadAllLines(args[0] + @"\layers.csv").ToList().ConvertAll(new Converter<string, Int32>(ConvMeInt));
            numberNeurons.Add(outputCount);
            //convert data from file to format used by teacher
            double[][] inputs = new double[inputSampleCount][];
            double[][] outputs = new double[inputSampleCount][];

            PopulateFromStrings(inputs, inputRows);
            PopulateFromStrings(outputs, outputRows);

            // create multi-layer neural network
            ActivationNetwork network = new ActivationNetwork(new BipolarSigmoidFunction(), inputCount, numberNeurons.ToArray());
            // create teacher
             BackPropagationLearning teacher = new BackPropagationLearning(network);
            teacher.LearningRate = Convert.ToDouble(args[1]);
            teacher.Momentum = Convert.ToDouble(args[2]);
            double maxerror = Convert.ToDouble(args[3]);
            int maxiter = Convert.ToInt32(args[4]);
            double error = double.MaxValue;
            int iter = 0;
            Console.WriteLine("Training...");
            while (error > maxerror && iter < maxiter)
            {
                //squared error (difference between current network's output and desired output) divided by 2.
                error = teacher.RunEpoch(inputs, outputs);

                iter++;
                Console.WriteLine("Error " + error + " Iteration " + iter);
            }
            Console.WriteLine("Writing network to " + args[0] + @"\network.nn");
            network.Save(args[0] + @"\network.nn");
            //Console.ReadKey();
        }
        //used to convert string list to integers
        static Int32 ConvMeInt(string str)
        {
            return Convert.ToInt32(str);
        }
        //used to convert string list to doubles
        static double ConvMeDouble(string str)
        {
            return Convert.ToDouble(str);
        }
        //generates training data array from list of strings
        static void PopulateFromStrings(double[][] myArray,List<string>rows)
        {
            for (int i=0;i<rows.Count;i++)
            {
                char[] sep = { ',' };
                List<string> trow = rows[i].Split(sep, StringSplitOptions.None).ToList();


                double[] rarray = trow.ConvertAll(new Converter<string, double>(ConvMeDouble)).ToArray();
                myArray[i] = rarray;
            }
        }
    }
}
