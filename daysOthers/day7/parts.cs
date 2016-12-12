using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace adventofcode2016
{
    partial class day7
    {
        public static int part1(bool debug)
        {
            string[] filelns = File.ReadAllLines(@"day7\input.txt");
            Regex r = new Regex(@"(\w)(\w)(?=\2\1)");

            int conunt = 0;
            foreach (var line in filelns)
            {
                var matches = r.Matches(line);
                if (matches.Count > 0)
                {
                    if(debug) Console.WriteLine(line);
                    bool valid = false;
                    for (int i = 0; i < matches.Count; i++)
                    {
                        var curmatch = matches[i];
                        valid |= (curmatch.Groups[1].Value != curmatch.Groups[2].Value);

                        if (debug)
                        {
                            if (i == 0)
                            {
                                Console.Write(line.Substring(0, curmatch.Index));
                            }
                            else
                            {
                                Console.Write(line.Substring(matches[i - 1].Index + 4, curmatch.Index - matches[i - 1].Index - 3));
                            }
                            Console.ForegroundColor = (curmatch.Groups[1].Value != curmatch.Groups[2].Value) ? ConsoleColor.Green : ConsoleColor.Yellow;
                            Console.Write(line.Substring(curmatch.Index, 4));
                            Console.ResetColor();
                        }

                        int idxbranches = line.IndexOfAny(new char[] { '[', ']' }, curmatch.Index + 4);
                        if (idxbranches > 0 && line[idxbranches] == ']')
                        {
                            if (debug)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.Write(line.Substring(curmatch.Index + 4, idxbranches - curmatch.Index - 3));
                                Console.ResetColor();
                            }
                            valid = false;
                            break;
                        }
                    }
                    if(valid) conunt++;
                    if (debug) Console.WriteLine();
                }
            }
            return conunt;
        }

        public static int part2()
        {
            string[] filelns = File.ReadAllLines(@"day7\input.txt");
            Regex r = new Regex(@"\[\w*(\w)(\w)\1\w*\]");

            int count = 0;
            foreach (var line in filelns)
            {
                bool valid = false;
                var matches = r.Matches(line);
                if (matches.Count > 0)
                {
                    
                    for (int i = 0; i < matches.Count; i++)
                    {
                        var curmatch = matches[i];
                        int j = 0;
                        while (!valid && (j++ < curmatch.Length - 3))
                            if (
                                (curmatch.Value[j] != curmatch.Value[j + 1]) &&
                                (curmatch.Value[j] == curmatch.Value[j + 2])
                                )
                            {

                                string lookinfor = line.Substring(curmatch.Index + j + 1, 2) + line[curmatch.Index + j + 1];
                                valid = Regex.IsMatch(line, @"\w*" + lookinfor + @"\w*(\[|$)");
                            }

                        if (valid)
                            break;
                    }
                    if (valid)
                        count++;

                }
            }
            return count;

        }
    }
}
