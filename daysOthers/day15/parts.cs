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

    partial class day15
    {
        const string input = "jlmsuwbz";
       
        class disc
        {
            public int index;
            public int size;
            public int initialPosition;

            public int Slot(int time) => (time + initialPosition) % size;
        }
        public static long part1(bool debug)
        {
            string[] filelns = File.ReadAllLines(@"day15\input.txt");
            Regex reg = new Regex(@"Disc #(\d*) has (\d*) positions; at time=0, it is at position (\d*).");
            var discs = filelns.Select(v => reg.Match(v)).Select(m => new disc()
            {
                index = int.Parse(m.Groups[1].Value)
                ,
                size = int.Parse(m.Groups[2].Value)
                ,
                initialPosition = int.Parse(m.Groups[3].Value)
            }).ToArray();

            int time = 0;
            do {
                time += discs[0].size - discs[0].Slot(time);
            } while (!discs.Skip(1).All(disc => disc.Slot(time + disc.index-1) == 0));

            if (debug)
            {
                for (int i = 0; i < discs.Length; i++)
                    Console.WriteLine("disk[{0}]={1}/{2}", i, discs[i].size, discs[i].Slot(time+ discs[i].index-1));
            }
            return time-1;

        }

        public static long part2()
        {

            string[] filelns = File.ReadAllLines(@"day15\input.txt");
            Regex reg = new Regex(@"Disc #(\d*) has (\d*) positions; at time=0, it is at position (\d*).");
            var discs = filelns.Select(v => reg.Match(v)).Select(m => new disc()
            {
                index = int.Parse(m.Groups[1].Value)
                ,
                size = int.Parse(m.Groups[2].Value)
                ,
                initialPosition = int.Parse(m.Groups[3].Value)
            }).ToList();

            discs.Add(new disc() { index = discs[discs.Count - 1].index + 1, size = 11});

            int time = 0;
            do
            {
                time += discs[0].size - discs[0].Slot(time);
            } while (!discs.Skip(1).All(disc => disc.Slot(time + disc.index - 1) == 0));

            
            for (int i = 0; i < discs.Count; i++)
                Console.WriteLine("disk[{0}]={1}/{2}", i, discs[i].size, discs[i].Slot(time + discs[i].index - 1));
            
            return time - 1;


        }

    
    }
}
