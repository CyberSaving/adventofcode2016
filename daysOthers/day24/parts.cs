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
    class board
    {
        public class treenode: IComparable<treenode>
        {
            public int x;
            public int y;
            public int steps { get; set; }

            public int CompareTo(treenode other)
            {
                if (other.x == x) return other.y.CompareTo(y);
                return other.x.CompareTo(x);
            }
        }

        public enum NodeType { OPEN,CLOSED,NUMBER}
        public int height { get; set; }
        public int width { get; set; }

        public Dictionary<int,treenode> allNumber { get; set; }
        private NodeType[,] nodesType = null;


        public NodeType getNodeType(int y, int x)
        {
            return nodesType[y, x];
        }
        public board(string filename)
        {
            
            allNumber = new Dictionary<int, treenode>(10);
            string[] filelns = File.ReadAllLines(filename);
            for (int y = 0; y < filelns.Length; y++)
            {
                string line = filelns[y];
                if (nodesType == null)
                {
                    width = line.Length;
                    height = filelns.Length;
                    nodesType = new NodeType[height, width];
                }
                for (int x = 0; x < line.Length; x++)
                {
                    nodesType[y, x] = line[x] == '#' ? NodeType.CLOSED : line[x] == '.' ? NodeType.OPEN : NodeType.NUMBER;
                    if(nodesType[y, x]== NodeType.NUMBER)
                        allNumber.Add(int.Parse(line[x].ToString()), new treenode() { x = x, y = y });
                }
            }
        }
        public void drawEmptyBoard()
        {
            Console.CursorTop = 0;
            Console.CursorLeft = 0;


            for (int y = 0; y < this.height; y++)
            {
                for (int x = 0; x < this.width; x++)
                {
                    if (nodesType[y, x] == NodeType.CLOSED)
                        Console.Write("#");
                    else if (nodesType[y, x] == NodeType.NUMBER)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write("N"); Console.ResetColor();
                    }
                    else
                        Console.Write(".");
                }
                Console.WriteLine("");
            }
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (var item in allNumber)
            {
                try
                {
                    Console.SetCursorPosition(item.Value.x, item.Value.y);
                    Console.Write(item.Key);
                }
                catch (Exception) { }
            }
            Console.ResetColor();
        }
        public void draw(bool[,] matrix,treenode root, ConsoleColor color, ConsoleColor haedcolor)
        {
            Console.CursorTop = 0;
            Console.CursorLeft = 0;
            

            for (int y = 0; y < this.height; y++)
            {
                for (int x = 0; x < this.width; x++)
                {
                    if (root.y == y && root.x == x)
                    {
                        try
                        {
                            Console.SetCursorPosition(x, y);
                            Console.ForegroundColor = haedcolor;
                            Console.Write("O");
                            Console.ResetColor();
                        }
                        catch (Exception) { }
                    }
                    else if (matrix[y, x])
                    {
                        try
                        {
                            Console.SetCursorPosition(x, y);
                            Console.ForegroundColor = color;
                            Console.Write("O");
                            Console.ResetColor();
                        }
                        catch (Exception) { }
                    };
                }
                Console.WriteLine("");
            }

        }
        public Dictionary<int,int> shortest(int number,treenode from, ConsoleColor color, ConsoleColor haedcolor,bool debug)
        {
            Queue<treenode> _toaccross = new Queue<treenode>();

            Dictionary<int, int> retval = new Dictionary<int, int>(10);
            retval.Add( allNumber.Where(item => item.Value.CompareTo(from) == 0).First().Key , 0);

            bool[,] matrix = new bool[height, width];
            treenode root = from;
            Stopwatch sp = new Stopwatch();
            sp.Start();
            
            while (retval.Count!=allNumber.Count)
            {
                if (root.y > 0 && getNodeType(root.y - 1, root.x) != NodeType.CLOSED && !matrix[root.y - 1, root.x])
                {
                    var node = new treenode() { x = root.x, y = root.y - 1, steps = root.steps + 1 };
                    _toaccross.Enqueue(node); matrix[node.y, node.x] = true;
                }

                if (root.y < height && getNodeType(root.y + 1, root.x) != NodeType.CLOSED && !matrix[root.y + 1, root.x])
                {
                    var node = new treenode() { x = root.x, y = root.y + 1, steps = root.steps + 1 };
                    _toaccross.Enqueue(node); matrix[node.y, node.x] = true;
                }

                if (root.x > 0 && getNodeType(root.y, root.x - 1) != NodeType.CLOSED && !matrix[root.y, root.x - 1]) {
                    var node = new treenode() { x = root.x - 1, y = root.y, steps = root.steps + 1 };
                    _toaccross.Enqueue(node); matrix[node.y, node.x] = true;
                }

                if (root.x < width && getNodeType(root.y, root.x + 1) != NodeType.CLOSED && !matrix[root.y, root.x + 1])
                {
                    var node = new treenode() { x = root.x + 1, y = root.y, steps = root.steps + 1 };
                    _toaccross.Enqueue(node); matrix[node.y, node.x] = true;
                }
                root = _toaccross.Dequeue();
                if (getNodeType(root.y, root.x) == NodeType.NUMBER)
                {
                    var key = allNumber.Where(item => item.Value.CompareTo(root) == 0).First().Key;
                    if (!retval.ContainsKey(key))
                        retval.Add(key, root.steps);
                }

                if (debug)
                {
                    Thread.Sleep(1);
                    if (sp.ElapsedMilliseconds > 1500)
                    {
                        lock (Console.Out)
                        {
                            Console.CursorTop = height + 1 + number;
                            Console.WriteLine("{0}. QUEUE Lenght:{1}", number, _toaccross.Count);
                            draw(matrix, root, color, haedcolor);
                        }
                        sp.Restart();
                    }
                }
            }
            if (debug) draw(matrix, root, color, haedcolor);
            return retval;
        }

    }

    partial class day24
    {
        static ConsoleColor[] arrayColor = new ConsoleColor[] { ConsoleColor.DarkYellow,ConsoleColor.DarkBlue, ConsoleColor.DarkCyan, ConsoleColor.DarkGray, ConsoleColor.DarkGreen, ConsoleColor.DarkMagenta, ConsoleColor.DarkRed};
        static ConsoleColor[] arrayHeadColor = new ConsoleColor[] { ConsoleColor.Yellow, ConsoleColor.Blue, ConsoleColor.Cyan, ConsoleColor.Gray, ConsoleColor.Green, ConsoleColor.Magenta, ConsoleColor.Red };

        static IEnumerable<IEnumerable<T>> GetPermutations<T>(IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });

            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        static string makekey(int a,int b) => string.Format("{0}.{1}", Math.Min(a, b), Math.Max(a, b));
        
        public static int  part1(bool debug)
        {
            var _board = new board(@"day24\input.txt");

            if(debug) _board.drawEmptyBoard();

            Dictionary<string,int> all = new Dictionary<string, int>();
            
            Parallel.For(0, _board.allNumber.Count - 1, i =>
            {
                foreach (var item in _board.shortest(i, _board.allNumber[i], arrayColor[i], arrayHeadColor[i], debug))
                    all[makekey(i, item.Key)] = item.Value;
            });

            if (debug)
                Console.Clear();

            var thebest = GetPermutations(Enumerable.Range(1, _board.allNumber.Count-1), _board.allNumber.Count-1)
                .Select(sel =>
                {
                    var selarray = sel.ToArray();
                    int count = all[makekey(0, selarray[0])];
                    for (int i = 1; i < selarray.Length; i++)
                        count += all[makekey(selarray[i - 1], selarray[i])];
                    return new { combination = sel, speps = count };
                }).OrderBy(v => v.speps).First();
            
            return thebest.speps;
        }



        public static long part2(bool debug)
        {
            var _board = new board(@"day24\input.txt");

            if (debug) _board.drawEmptyBoard();

            Dictionary<string, int> all = new Dictionary<string, int>();

            Parallel.For(0, _board.allNumber.Count - 1, i =>
            {
                foreach (var item in _board.shortest(i, _board.allNumber[i], arrayColor[i], arrayHeadColor[i], debug))
                    all[makekey(i, item.Key)] = item.Value;
            });

            if (debug)
                Console.Clear();

            var thebest = GetPermutations(Enumerable.Range(1, _board.allNumber.Count - 1), _board.allNumber.Count - 1)
                .Select(sel =>
                {
                    var selarray = sel.ToArray();
                    int count = all[makekey(0, selarray[0])];
                    for (int i = 1; i < selarray.Length; i++)
                        count += all[makekey(selarray[i - 1], selarray[i])];
                    
                    //only change from part 1:
                    count += all[makekey(selarray[selarray.Length - 1], 0)];


                    return new { combination = sel, speps = count };
                }).OrderBy(v => v.speps).First();

            return thebest.speps;

        }




    }


}
