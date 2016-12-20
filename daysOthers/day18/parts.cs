using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace adventofcode2016
{

    partial class day18
    {
        //const string input = "ulqzkmiv";
        const string input = "dmypynyp";
        
        private static readonly MD5 MD5istance = MD5.Create();
        


        public static int part1(bool debug)
        {
            string input = File.ReadAllText(@"day18\input.txt");

            char[] newline = new char[input.Length];
            Array.Copy(input.ToCharArray(), newline, input.Length);
            int total = 0;
            for (int i = 0; i < 40; i++)
            {
                if(debug)Console.WriteLine(input);
                total += newline.Count(c => c == '.');
                newline[0] = input[1];
                for (int t = 1; t < newline.Length - 1; t++)
                    newline[t] = input[t - 1] != input[t + 1] ? '^' : '.';
                newline[newline.Length - 1] = input[newline.Length - 2];

                input = new string(newline);
                
            }
            return total;

        }

        public static int part2()
        {

            string input = File.ReadAllText(@"day18\input.txt");

            char[] newline = new char[input.Length];
            Array.Copy(input.ToCharArray(), newline, input.Length);
            int total = 0;
            for (int i = 0; i < 400000; i++)
            {
                Console.WriteLine(input);
                total += newline.Count(c => c == '.');
                newline[0] = input[1];
                for (int t = 1; t < newline.Length - 1; t++)
                    newline[t] = input[t - 1] != input[t + 1] ? '^' : '.';
                newline[newline.Length - 1] = input[newline.Length - 2];

                input = new string(newline);

            }
            return total;
        }

    
    }
}
