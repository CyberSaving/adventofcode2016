using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace adventofcode2016
{

    partial class day19
    {

        const int input = 3004953;
        //const int input = 5;


        public static int part1(bool debug)
        {
            bool[] elfes = new bool[input];
            for (int i = 0; i < elfes.Length; i++) elfes[i] = true;
            

            int count = input;
            int t = 0;
            bool mode = false;
            while (count > 2)
            {
                
                if (elfes[t] && !mode) mode = true;
                else if(elfes[t] && mode)
                {
                    mode = elfes[t] = false;
                    count--;
                }
                if (debug && t == 0) {
                    Console.CursorTop = 0;
                    Console.WriteLine("{0}******",count);
                }
                t = (t + 1) % input;
            }
            return Array.IndexOf(elfes,true)+1;

        }

        /// <summary>Very slow solution!</summary>
        /// <returns></returns>
        public static int part2()
        {
            Stopwatch sp = new Stopwatch();

            List<int> elfes = new List<int>(input);
            for (int i = 0; i < input; i++) elfes.Add(i+1);            
            int t = 0;
            sp.Start();
            while (elfes.Count > 1)
            {

                bool backshift = ((t + elfes.Count / 2) >= elfes.Count);

                elfes.RemoveAt( (t + elfes.Count / 2) % elfes.Count);
                t = (t + (!backshift ? 1:0) ) % elfes.Count;

                if (sp.ElapsedMilliseconds > 1000)
                {
                    sp.Restart();
                    Console.CursorTop = 0;
                    Console.WriteLine("{0}******", elfes.Count);
                }
            }
            return elfes[0];
        }



    
    }


}
