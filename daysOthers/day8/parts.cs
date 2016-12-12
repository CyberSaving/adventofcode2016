using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace adventofcode2016
{

    static class BiDimensionExtension {
        public static int fill(this bool[,] matrix, int y, int x)
        {
            int totalLit = 0;
            for (int i = 0; i < y; i++)
                for (int j = 0; j < x; j++)
                {
                    if (!matrix[i, j])
                    {
                        totalLit++; matrix[i, j] = true;
                    }
                }
            return totalLit;
        }

        public static void dump(this bool[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    if(j%5 == 0)
                    {
                        Console.Write(' ');
                    }
                    if (matrix[i, j])
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write('#');
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write('.');
                    }
                }
                Console.WriteLine();
            }
        }

        public static void rotateX(this bool[,] matrix, int row, int by)
        {
            bool[] shifted = new bool[by];
            for (int i = 0; i < by; i++)
                shifted[by-i-1] = matrix[row, 50-i-1];
                
            for (int j = 49; j >= by; j--)
                matrix[row, j] = matrix[row, j- by];

            for (int i = 0; i < by; i++)
                matrix[row, i] = shifted[i];
             
        }
        public static void rotateY(this bool[,] matrix, int column , int by)
        {
            bool[] shifted = new bool[by];
            for (int i = 0; i < by; i++)
                shifted[by - i - 1] = matrix[6 - i - 1, column];
            for (int j = 5; j >= by; j--)
                matrix[j,column] = matrix[ j - by, column];
            for (int i = 0; i < by; i++)
                matrix[i, column] = shifted[i];
        }
    }

    partial class day8
    {

        public static int part1(bool debug)
        {
            string[] filelns = File.ReadAllLines(@"day8\input.txt");
            Regex r = new Regex(@"(rect|rotate) (\d{1,2}x\d|column|row)( [x,y]=(\d{1,2}) by (\d{1,2}))?");

            bool[,] screen = new bool[6, 50];

            int count = 0;
            foreach (var line in filelns)
            {
                var match = r.Match(line);
                if (match.Groups[1].Value == "rect")
                {
                    var values = match.Groups[2].Value.Split('x');
                    count += screen.fill(int.Parse(values[1]), int.Parse(values[0]));
                    if (debug) Console.WriteLine("fill {0}x{1} for {2} lit", values[0], values[1], count);
                } else if (match.Groups[2].Value == "column") {
                    screen.rotateY(int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value));
                } else
                    screen.rotateX(int.Parse(match.Groups[4].Value), int.Parse(match.Groups[5].Value));
            }

            screen.dump();

            return count;
        }

        public static int part2()
        {
            return 0;
        }
    }
}
