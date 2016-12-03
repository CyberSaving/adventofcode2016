using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day2
{
    class Program
    {
        static void Main(string[] args)
        {
            //For Point1
            string[] filelns = File.ReadAllLines(@"input.txt");   
            short x = 1;
            short y = 1;
            string code = "";

            //For Point2

            //   __-2__-1__0___1___2__
            // -2[   ,   ,{1},   ,   ]
            // -1[   ,{2},   ,{4},   ]
            //  0[{5},   ,   ,   ,{9}]
            //  1[   ,   ,   ,   ,   ]
            //  2[   ,   ,   ,   ,   ]
            int[][] start = {
                new int[] { 0, 0, 1, 0, 0 }
                ,new int[]{ 0, 2, 3, 4, 0 }
                ,new int[]{ 5, 6, 7, 8, 9 }
                ,new int[]{ 0, 10, 11, 12, }
                ,new int[]{ 0, 0, 13 , 0 ,0 }
            };

            short x2 = -2;
            short y2 = 0;
            string code2 = "";

            foreach (string l in filelns)
            {
                for (int i = 0; i < l.Length; i++)
                {
                    char c = l[i];
                    switch (c)
                    {
                        case 'U':
                            y = (short)Math.Max(y - 1, 0);
                            y2 = (short)Math.Max(y2 - 1, -2 + Math.Abs(x2));
                            break;
                        case 'D':
                            y = (short)Math.Min(y + 1, 2);
                            y2 = (short)Math.Min(y2 + 1, 2 - Math.Abs(x2));
                            break;
                        case 'L':
                            x = (short)Math.Max(x - 1, 0);
                            x2 = (short)Math.Max(x2 - 1, -2 +Math.Abs(y2));
                            break;
                        case 'R':
                            x = (short)Math.Min(x + 1, 2);
                            x2 = (short)Math.Min(x2 + 1, 2-Math.Abs(y2));
                            break;
                    }

                }
                code += (x + 1 + 3*y).ToString();
                code2 += start[y2 + 2][x2 + 2].ToString("X");
            }
            Console.WriteLine("result day2.1 = {0}.\n", code);
            Console.WriteLine("result day2.2 = {0}.\n", code2);


            Console.Write("Presse eny key ...");
            Console.ReadKey();

        }
    }
}
