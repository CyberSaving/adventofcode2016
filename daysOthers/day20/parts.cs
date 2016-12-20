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

    partial class day20
    {

        class MinMax<T> {
            public MinMax (T min, T max) { this.min = min; this.max = max; }
            public T min;
            public T max;

        }
        public static uint part1(bool debug)
        {
            string[] filelns = File.ReadAllLines(@"day20\input.txt");

            Regex reg = new Regex(@"(\d*)-(\d*)");

            MinMax<uint> first = new MinMax<uint>(0,0);
            foreach (var item in filelns.Select(v => {
                    var match = reg.Match(v);
                    return new MinMax<uint>(uint.Parse(match.Groups[1].Value), uint.Parse(match.Groups[2].Value));
                }).OrderBy(v => v.min))
            {

                if (item.min > first.max + 1)
                    break;
                else
                    first.max = Math.Max(first.max, item.max);

            }
            

            return first.max+1;

        }

        public static uint part2()
        {
            uint totalIPs = 0;
            string[] filelns = File.ReadAllLines(@"day20\input.txt");

            Regex reg = new Regex(@"(\d*)-(\d*)");

            MinMax<uint> first = new MinMax<uint>(0, 0);
            foreach (var item in filelns.Select(v => {
                var match = reg.Match(v);
                return new MinMax<uint>(uint.Parse(match.Groups[1].Value), uint.Parse(match.Groups[2].Value));
            }).OrderBy(v => v.min))
            {
                //!important risk overflow
                if ( (long)item.min > (first.max + 1L)) 
                    totalIPs += (item.min - first.max - 1);

                first.max = Math.Max(first.max, item.max);
            }

            return totalIPs + (4294967295 - first.max);
        }



    
    }


}
