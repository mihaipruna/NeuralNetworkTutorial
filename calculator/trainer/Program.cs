using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge;
using AForge.Neuro;
using AForge.Neuro.Learning;

using System.IO;

//neural network calculator
//uses neural network created with trainer
namespace calculator
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
            //args: network file, data folder,
           if (args.Length<2)
            {
                Console.WriteLine("Usage: calculator.exe neural_network_file_path path_to_folder_containing_input.csv");
                Console.WriteLine("Saves to output.csv in same folder");
                return;
            }
            //reads input data on row separated by commas
            //this only computes one set of inputs
            char[] sep = { ',' };
            double[] inputs = File.ReadAllText(args[1]+@"\input.csv" ).Split(sep,StringSplitOptions.None).ToList().ConvertAll(new Converter<string, double>(ConvMeDouble)).ToArray();
            
            //load network from file saved by trainer
            Network network = Network.Load(args[0]);
            //calculate 
            double[] outs=network.Compute(inputs);
            //display and save to file
            string myOutput = "";
            for (int i=0;i<outs.Length;i++)
            {
                Console.WriteLine(outs[i]);
                myOutput += outs[i].ToString();
                if (i < outs.Length - 1)
                    myOutput += ",";

            }
            File.WriteAllText(args[1] + @"\output.csv", myOutput);
          
           
        }
        
        //used to convert list of string to list of double
        static double ConvMeDouble(string str)
        {
            return Convert.ToDouble(str);
        }

        

        
    }
}
