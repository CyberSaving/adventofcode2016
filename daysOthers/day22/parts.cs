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

    partial class day22
    {
        static Regex reg = new Regex(@"^\/dev\/grid\/node-x(\d*)-y(\d*)\s*(\d*)T\s*(\d*)T\s*(\d*)T\s*(\d*)%");
        
        public static long part1(bool debug)
        {
            var filelns = File.ReadAllLines(@"day22\input.txt")
                .Select(line => reg.Match(line))
                .Where(m => m.Success). Select(m =>
            {
                return new 
                {
                    x = int.Parse(m.Groups[1].Value)
                    ,
                    y = int.Parse(m.Groups[2].Value)
                    //Size  Used  Avail  Use%
                    ,
                    Size = int.Parse(m.Groups[3].Value)
                    ,
                    Used = int.Parse(m.Groups[4].Value)
                    ,
                    Avail = int.Parse(m.Groups[5].Value)
                    ,
                    UseP = int.Parse(m.Groups[6].Value)

                };
            });
            long count = 0;
            Stopwatch sp = new Stopwatch();
            sp.Start();
            var nodeordered = filelns.OrderBy(node => node.Avail);
            foreach (var nodeA in filelns.Where(v => v.Used != 0))
            {
                count += filelns.Count(nodeB => nodeB != nodeA && nodeA.Used <= nodeB.Avail);
            }
            Console.WriteLine("Count {0} in {1} dec", count, sp.Elapsed.TotalSeconds);
            return count;

        }

        public class node
        {
            public int x; public int y;
            public int Size; public int Used; public int Avail;
            public bool WillFit(node nodeb) =>(nodeb.Avail >= Used);
            public bool WillMove(node nodeb) => (nodeb.Size >= Used);

            public void Move(node nodeb)
            {
                if (!this.WillFit(nodeb))
                    throw new InvalidOperationException("Not space enable!");
                nodeb.Used+=this.Used;
                nodeb.Avail = nodeb.Size - nodeb.Used;

                this.Used = 0;
                this.Avail = this.Size;
            }

        }
        public static void draw(node[,] matrix, node Gpoint)
        {
            for (int y = 0; y < matrix.GetLength(0); y++)
            {
                for (int x = 0; x < matrix.GetLength(1); x++)
                {
                    var node = matrix[y, x];
                    Console.SetCursorPosition(node.x * 2, node.y * 2);
                    if (node.Used == 0)
                    {
                        Console.Write("_");
                    }
                    else
                    {
                        Console.Write(".");

                        bool locked = true;

                        if (y > 0)
                        {
                            var up = matrix[y - 1, x];
                            if (!up.WillMove(node))
                            {
                                Console.CursorLeft--; Console.Write("_");
                            }
                            else locked = false;
                            if (node.WillFit(up))
                            {
                                Console.CursorTop--; Console.CursorLeft--; Console.Write("{0}/{1}", node.Used, node.Size);
                            }
                        }

                        if (y < matrix.GetLength(0) - 1)
                        {
                            var down = matrix[y + 1, x];
                            if (!down.WillMove(node))
                            {
                                locked &= true;
                                Console.CursorLeft--; Console.Write(locked ? "=" : "-");
                            }
                            else locked = false;
                            if (node.WillFit(down))
                            {
                                Console.CursorTop++; Console.CursorLeft--; Console.Write("{0}/{1}", node.Used, node.Size);
                            }
                        }
                        if (x > 0)
                        {
                            var left = matrix[y, x - 1];
                            if (!left.WillMove(node))
                            {
                                locked &= true;
                                Console.CursorLeft-=2; Console.Write("|");
                            }
                            else locked = false;
                            if (node.WillFit(left))
                            {
                                Console.CursorLeft -= 2; Console.Write(">");
                            }
                        }
                        if (x < matrix.GetLength(1) - 1)
                        {
                            var right = matrix[y, x + 1];
                            if (!right.WillMove(node))
                            {
                                locked &= true;
                                Console.Write("|"); Console.CursorLeft--;
                            }
                            else locked = false;
                            if (node.WillFit(right))
                                Console.Write("<");
                        }
                        if (Gpoint == node)
                        {
                            Console.SetCursorPosition(node.x * 2, node.y * 2);
                            Console.Write("G");
                        }


                    }
                }
            }
            Console.SetCursorPosition(0, matrix.GetLength(0) * 2 + 1);
        }

        private static IEnumerable<Tuple<node, node>> allFittable(node[,] matrix)
        {
            for (int y = 0; y < matrix.GetLength(0); y++)
                for (int x = 0; x < matrix.GetLength(1); x++)
                {
                    var node = matrix[y, x];

                    if (node.Used == 0)
                        continue;

                    if (y > 0 && node.WillFit(matrix[y - 1, x]))
                        yield return new Tuple<node, node>(node, matrix[y - 1, x]);

                    if (y < (matrix.GetLength(0) - 1) && node.WillFit(matrix[y + 1, x]))
                        yield return new Tuple<node, node>(node, (matrix[y + 1, x]));

                    if (x > 0 && node.WillFit(matrix[y, x - 1]))
                        yield return new Tuple<node, node>(node, (matrix[y, x - 1]));

                    if (x < (matrix.GetLength(1) - 1) && node.WillFit(matrix[y, x + 1]))
                        yield return new Tuple<node, node>(node, (matrix[y, x + 1]));
                };
        }
         

        public static string part2(bool debug)
        {
            var filelns = File.ReadAllLines(@"day22\input.txt")
               .Select(line => reg.Match(line))
               .Where(m => m.Success).Select(m =>
               {
                   return new node
                   {
                       x = int.Parse(m.Groups[1].Value)
                        ,
                       y = int.Parse(m.Groups[2].Value)
                        //Size  Used  Avail  Use%
                        ,
                       Size = int.Parse(m.Groups[3].Value)
                        ,
                       Used = int.Parse(m.Groups[4].Value)
                        ,
                       Avail = int.Parse(m.Groups[5].Value)

                   };
               });
            node[,] matrix = new node[filelns.Max(nodea => nodea.y) + 1, filelns.Max(nodea => nodea.x) + 1];
            foreach (var anode in filelns)
                matrix[anode.y, anode.x] = anode;

            node Gpoint = matrix[0, matrix.GetUpperBound(1)];
            var keypressed = ConsoleKey.M;
            int moves = 0;
            Boolean foundg = false;
            do
            {
                draw(matrix, Gpoint);
                Console.WriteLine("Moves {0}", moves);
                if (!foundg)
                {
                    var solution = allFittable(matrix).OrderBy(nodes => Math.Sqrt(Math.Pow(nodes.Item1.x - Gpoint.x, 2) + Math.Pow(nodes.Item1.y - Gpoint.y, 2))).ToArray();
                    for (int i = 0; i < solution.Length; i++)
                    {
                        var asolution = solution[i];
                        Console.WriteLine("{0} Move {5}T from {1},{2} to {3},{4}**", i, asolution.Item1.y, asolution.Item1.x, asolution.Item2.y, asolution.Item2.x, asolution.Item1.Used);
                    }
                    Console.WriteLine("                                          ");

                    keypressed = Console.ReadKey().Key;
                    string keypressedstr = keypressed.ToString();
                    if (keypressedstr.Length > 1)
                        keypressedstr = keypressedstr.Substring(keypressedstr.Length - 1);
                    int solutionnumber = 0;
                    if (int.TryParse(keypressedstr, out solutionnumber))
                    {
                        moves++;
                        
                        solution[solutionnumber].Item1.Move(solution[solutionnumber].Item2);
                        if (solution[solutionnumber].Item1 == Gpoint)
                        {
                            foundg = true;
                            Gpoint = solution[solutionnumber].Item2;
                        }

                    }
                }else
                {
                    keypressed = Console.ReadKey().Key;
                    Console.Clear();
                    matrix[Gpoint.y + 1, Gpoint.x + 1].Move(matrix[Gpoint.y, Gpoint.x + 1]);
                    matrix[Gpoint.y + 1, Gpoint.x].Move(matrix[Gpoint.y + 1, Gpoint.x + 1]);
                    matrix[Gpoint.y + 1, Gpoint.x - 1].Move(matrix[Gpoint.y + 1, Gpoint.x]);
                    matrix[Gpoint.y, Gpoint.x - 1].Move(matrix[Gpoint.y + 1, Gpoint.x-1]);
                    matrix[Gpoint.y, Gpoint.x].Move(matrix[Gpoint.y , Gpoint.x - 1]);
                    moves += 5;
                    Gpoint = matrix[Gpoint.y, Gpoint.x - 1];

                }


            } while (keypressed != ConsoleKey.Q);

            return null;

        }




    }


}
