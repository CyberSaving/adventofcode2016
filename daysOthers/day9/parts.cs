using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace adventofcode2016
{

    partial class day9
    {

        public static int part1(bool debug)
        {
            string file = File.ReadAllText(@"day9\input.txt");

            StringBuilder sb = new StringBuilder();
            int idx = 0, idxold =0;
            while((idx  = file.IndexOf('(', idxold)) >= 0)
            {
                sb.Append(file.Substring(idxold, idx - idxold));

                int idxend = file.IndexOf(')', idx);
                string[] marker = file.Substring(idx + 1, idxend - idx - 1).Split('x');
                string pattern = file.Substring(idxend + 1, int.Parse(marker[0]));
                for (int i = 0; i < int.Parse(marker[1]); i++)
                    sb.Append(pattern);
                idxold = idxend + int.Parse(marker[0])+1;

            }

            return sb.Append(file.Substring(idxold)).Length-1;
        }

        public static long part2()
        {
            return part2_decompress(File.ReadAllText(@"day9\input.txt")) - 1;
            //return part2_decompress(@"(27x12)(20x12)(13x14)(7x10)(1x12)A");
            //return part2_decompress(@"(25x3)(3x3)ABC(2x3)XY(5x2)PQRSTX(18x9)(3x2)TWO(5x7)SEVEN");
        }
        public static long part2_decompress(string what)
        {
            long result = 0;
            int idx = 0, idxold = 0;
            while ((idx = what.IndexOf('(', idxold)) >= 0)
            {
                result += idx - idxold;
                int idxend = what.IndexOf(')', idx);
                string[] marker = what.Substring(idx + 1, idxend - idx - 1).Split('x');
                string pattern = what.Substring(idxend + 1, int.Parse(marker[0]));
                result += int.Parse(marker[1]) * part2_decompress(pattern);
                idxold = idxend + int.Parse(marker[0]) + 1;
            }
            return result + (what.Length - idxold);
        }

    }
}
