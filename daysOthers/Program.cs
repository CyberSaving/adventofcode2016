﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adventofcode2016
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sp = new Stopwatch();
            sp.Start();
            //Console.WriteLine("result day7.1 {0}, day7.2 {1} ", day7.part1(false), day7.part2());
            //Console.WriteLine("result day8.1 {0}, day8.2 {1} ", day8.part1(true), day8.part2());
            //Console.WriteLine("result day9.1 {0}, day9.2 {1} ", day9.part1(true), day9.part2());
            //Console.WriteLine("result day10.1 {0}, day10.2 {1} ", day10.part1(true), day10.part2());
            //Console.WriteLine("result day11.1 {0}, day11.2 {1} ", day11.part1(true), day11.part2());
            //Console.WriteLine("result day12.1 {0}, day12.2 {1} ", day12.part1(true), day12.part2());
            //Console.WriteLine("result day13.1 {0}, day13.2 {1} ", day13.part1(false) , day13.part2());
            //Console.WriteLine("result day14.1 {0}, day14.2 {1} ", day14.part1(false) , day14.part2());
            //Console.WriteLine("result day15.1 {0}, day15.2 {1} ", day15.part1(true) , day15.part2());
            //Console.WriteLine("result day16.1 {0}, day16.2 {1} ", day16.part1(true) , day16.part2());
            //Console.WriteLine("result day17.1 {0}, day17.2 {1} ", day17.part1(true), day17.part2());
            //Console.WriteLine("result day18.1 {0}, day18.2 {1} ", day18.part1(true), day18.part2());
            //Console.WriteLine("result day19.1 {0}, day19.2 {1} ", day19.part1(true), day19.part2());
            //Console.WriteLine("result day20.1 {0}, day20.2 {1} ", day20.part1(true), day20.part2());
            //Console.WriteLine("result day21.1 {0}, day21.2 {1} ", day21.part1(false), day21.part2(true));
            //Console.WriteLine("result day22.1 {0}, day22.2 {1} ", day22.part1(false), day22.part2(true));
            //Console.WriteLine("result day23.1 {0}, day23.2 {1} ", day23.part1(), day23.part2(true));
            //Console.WriteLine("result day24.1 {0}, day24.2 {1} ", day24.part1(true), day24.part2(false));
            Console.WriteLine("result day25.1 {0}", day25.part1(false));
            Console.WriteLine("Ended in {0:0.00#} sec", sp.Elapsed.TotalSeconds);

            Console.Write("Presse eny key ...");
            Console.ReadKey();
        }
    }
}
