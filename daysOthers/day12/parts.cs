using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace adventofcode2016
{

    partial class day12
    {
        
        private static void MakeExercize(Dictionary<char, int> _stack)
        {
            string[] filelns = File.ReadAllLines(@"day12\input.txt");
            Regex rg = new Regex(@"(\w{3}) ([a-z0-9\-]+)( [a-z0-9\-]+)?");

            Func<string, int> getvalue = (key) => {
                int val = 0;
                if (!int.TryParse(key, out val))
                    if (!_stack.TryGetValue(key[0], out val))
                        _stack.Add(key[0], 0);
                return val;
            };

            int t = 0;
            do
            {
                string istr = filelns[t];
                var mtc = rg.Match(istr);
                string secondterm = mtc.Groups[2].Value;
                switch (mtc.Groups[1].Value)
                {
                    case "cpy":
                        _stack[mtc.Groups[3].Value.Trim()[0]] = getvalue(mtc.Groups[2].Value);
                        t++;
                        break;
                    case "inc":

                        _stack[secondterm[0]] = getvalue(secondterm) + 1;
                        t++;
                        break;
                    case "dec":
                        _stack[secondterm[0]] = getvalue(secondterm) - 1;
                        t++;
                        break;
                    case "jnz":
                        if (getvalue(secondterm) != 0)
                            t += int.Parse(mtc.Groups[3].Value.Trim());
                        else
                            t++;
                        break;
                }
            } while (t < filelns.Length);
        }

        public static int part1(bool debug)
        {
            Dictionary<char, int> _stack = new Dictionary<char, int>();
            MakeExercize(_stack);
            return _stack['a'];
        }

        public static long part2()
        {
            Dictionary<char, int> _stack = new Dictionary<char, int>();
            _stack['c'] = 1;
            MakeExercize(_stack);
            return _stack['a'];

        }


    }
}
