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

    partial class day23
    {
        static readonly Regex rg = new Regex(@"(\w{3}) ([a-z0-9\-]+)( [a-z0-9\-]+)?");

        private static long getvalue(Dictionary<char, long> stack,string key)
        {
            long val = 0;
            if (!long.TryParse(key, out val))
                if (!stack.TryGetValue(key[0], out val))
                    stack.Add(key[0], 0);
            return val;
        }


        /// <summary>Without this optimization is impossibile to resolve part 2!</summary>
        /// <param name="stack"></param>
        /// <param name="pc"></param>
        /// <param name="lines"></param>
        /// <param name="multiply"></param>
        /// <param name="apply"></param>
        /// <returns></returns>
        private static int optimize(Dictionary<char, long> stack, int pc, string[] lines, long multiply = 1, bool apply = true)
        {
            var mtc = rg.Match(lines[pc]);
            string secondterm = mtc.Groups[2].Value;
            var offset = (int)getvalue(stack,mtc.Groups[3].Value.Trim());
            long thevalue = getvalue(stack,secondterm);

            if (offset == -5 && Regex.IsMatch(lines[pc - 1], "^(inc|dec) " + secondterm))
            {
                if(!Regex.IsMatch(lines[pc + offset], "^(jnz|tgl)") && 
                    optimize(stack, pc - 2, lines, Math.Abs(thevalue),false)==2)
                {
                    if (apply)
                    {
                        Exec(stack, pc + offset, lines);
                        optimize(stack, pc - 2, lines, Math.Abs(thevalue), true);
                        stack[secondterm[0]] = 0;
                    }
                    return 5;
                }
            }
            if (offset == -2 && Regex.IsMatch(lines[pc - 1], "^(inc|dec) " + secondterm))
            {
                var mat = Regex.Match(lines[pc - 2], @"^(inc|dec) (\w)");
                if (mat.Success)
                {
                    if (apply)
                    {
                        stack[mat.Groups[2].Value[0]] += multiply * ((mat.Groups[1].Value == "inc") ? Math.Abs(thevalue) : -Math.Abs(thevalue));
                        stack[secondterm[0]] = 0;
                    }
                    return 2;
                }
            }
            return 0;
        }

        private static int Exec(Dictionary<char, long> stack, int pc, string[] lines)
        {
            string istr = lines[pc];
            var mtc = rg.Match(istr);
            string secondterm = mtc.Groups[2].Value;
            int offset = 0;


            switch (mtc.Groups[1].Value)
            {
                case "cpy":
                    var key = mtc.Groups[3].Value.Trim()[0];
                    if (!char.IsDigit(key))
                        stack[key] = getvalue(stack, mtc.Groups[2].Value);
                    break;
                case "inc":
                    stack[secondterm[0]] = getvalue(stack, secondterm) + 1;
                    break;
                case "dec":
                    stack[secondterm[0]] = getvalue(stack, secondterm) - 1;
                    break;
                case "jnz":

                    if (optimize(stack, pc, lines)==0 && getvalue(stack, secondterm) != 0)
                        return (int)getvalue(stack, mtc.Groups[3].Value.Trim());
                    break;
                case "tgl":
                    offset = (int)getvalue(stack, secondterm);
                    if (offset == 0)
                        goto case "inc";
                    else if (pc + offset < lines.Length)
                    {
                        var futurematches = rg.Match(lines[pc + offset]);
                        if (!futurematches.Groups[3].Success)
                            lines[pc + offset] = (futurematches.Groups[1].Value == "inc" ? "dec" : "inc") + futurematches.Value.Substring(3);
                        else
                            lines[pc + offset] = (futurematches.Groups[1].Value == "jnz" ? "cpy" : "jnz") + futurematches.Value.Substring(3);
                    }
                    break;
            }
            return 1;
        }



        private static void MakeExercize(Dictionary<char, long> _stack, bool debug)
        {
            string[] filelns = File.ReadAllLines(@"day23\input.txt");
            int t = 0;
            Stopwatch sp = new Stopwatch();
            sp.Start();
            do
            {
                t += Exec(_stack, t, filelns);

                //only for debug and tuning:
                if (sp.ElapsedMilliseconds > 2000)
                {
                    sp.Restart();
                    Console.Clear();
                    Console.WriteLine("pc: {0}", t);
                    foreach (var item in _stack)
                        Console.WriteLine("{0}-> {1}", item.Key, item.Value);
                }
            } while (t < filelns.Length);
        }
        public static long part1()
        {
            Dictionary<char, long> _stack = new Dictionary<char, long>();
            _stack['a'] = 7;
            MakeExercize(_stack,false);
            return _stack['a'];

        }

         

        public static long part2(bool debug)
        {
            Dictionary<char, long> _stack = new Dictionary<char, long>();
            _stack['a'] = 12;
            MakeExercize(_stack, debug);
            return _stack['a'];

        }




    }


}
