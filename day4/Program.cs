using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace day4
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] filelns = File.ReadAllLines(@"input.txt");
            Regex r = new Regex(@"^([a-z]+\-)+(\d+)\[([a-z]+)\]");
            int total = 0;
            foreach (string l in filelns)
            {
                var metches = r.Matches(l);
                string sectorId = metches[0].Groups[2].Value;
                string check = metches[0].Groups[3].Value;

                
                StringBuilder sb = new StringBuilder();
                foreach (Capture cap in metches[0].Groups[1].Captures)
                    sb.Append(cap.Value.TrimEnd('-'));
                    

                string key = new string(
                    sb.ToString()
                        .GroupBy(x => x, x => x)
                        .OrderByDescending(v => v.Count()).ThenBy(v => v.Key)
                        .Take(check.Length)
                        .Select(group => group.Key).ToArray()
                    );
                if (key == check)
                {
                    total += int.Parse(sectorId);


                    //Part Two:
                    StringBuilder sb2 = new StringBuilder();
                    int off = int.Parse(sectorId) % 26;
                    foreach (Capture cap in metches[0].Groups[1].Captures)
                        sb2.Append(cap.Value.TrimEnd('-').Select(x =>(char)( (x - 'a' + off) % 26 + 'a')).ToArray() )
                            .Append(' ');
                    if(sb2.ToString(0, sb2.Length - 1).EndsWith(" storage"))
                        Console.WriteLine(sb2.ToString(0, sb2.Length - 1) + " -> " + sectorId);
                }

            }
            Console.WriteLine("result day4.1 = {0}.\n", total);

            Console.Write("Presse eny key ...");
            Console.ReadKey();
        }
    }
}
