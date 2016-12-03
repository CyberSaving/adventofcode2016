using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day3
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] filelns = File.ReadAllLines(@"input.txt");
            Regex r = new Regex(@"\s*(\d+)");
            int numofvalid = 0;
            foreach (string l in filelns)
            {
                var sidesMatch = r.Matches(l);
                int[] sides = new int[] { int.Parse(sidesMatch[0].Groups[1].Value), int.Parse(sidesMatch[1].Groups[1].Value), int.Parse(sidesMatch[2].Groups[1].Value) };
                Array.Sort(sides);
                numofvalid += ((sides[0] + sides[1]) > sides[2]) ? 1 : 0;
            }

            Console.WriteLine("result day2.1 = {0}.\n", numofvalid);


            //part2 -----------
            numofvalid = 0;
            int[][] group = { new int[3], new int[3], new int[3] };
            int j = 0; 
            for (int i = 0; i < filelns.Length; i++)
            {
                var sidesMatch = r.Matches(filelns[i]);
                for (int k = 0; k < 3; k++)
                    group[k][j] = int.Parse(sidesMatch[k].Groups[1].Value);

                j = (j+1)% 3;
                if (j == 0)
                    for(int k=0; k<3; k++)
                    {
                        Array.Sort(group[k]);
                        numofvalid += ((group[k][0] + group[k][1]) > group[k][2]) ? 1 : 0;
                    }
            }
            Console.WriteLine("result day2.2 = {0}.\n", numofvalid);

            Console.Write("Presse eny key ...");
            Console.ReadKey();
        }
    }
}
