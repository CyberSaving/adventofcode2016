using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace adventofcode2016
{

    partial class day16
    {
        const string input = "10111011111001111";
        const int LENGTH_2 = 35651584;


        private static string partcore(string input,int length)
        {
            string newinput = input;
            while ((newinput = newinput + "0" + new string(newinput.Reverse().Select(c => c == '0' ? '1' : '0').ToArray())).Length < length) { };
            newinput = newinput.Substring(0, length);

            do
            {
                char[] checksum = new char[newinput.Length / 2];
                for (int i = 0; i < newinput.Length; i += 2)
                    checksum[i / 2] = newinput[i] == newinput[i + 1] ? '1' : '0';
                newinput = new string(checksum);
            }
            while (newinput.Length % 2 == 0);

            return newinput;
        }

        public static string part1(bool debug)
        {
            const string input = "10111011111001111";
            return partcore(input, 272);

        }

        public static string part2()
        {
            const string input = "10111011111001111";
            return partcore(input, 35651584);
        }

    
    }
}
