using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace adventofcode2016
{

    partial class day17
    {
        //const string input = "ulqzkmiv";
        const string input = "dmypynyp";
        
        private static readonly MD5 MD5istance = MD5.Create();

        public class treenode
        {
            public int x;
            public int y;
            public string path = "";
        }
        
        static void draw(treenode nodes)
        {
            Console.CursorTop = 0;
            Console.CursorLeft = 0;

            Console.WriteLine("#########");
            for (int y = 0; y < 3; y++)
            {
                Console.WriteLine("# | | | #");
                Console.WriteLine("#-#-#-#-#");
            }
            Console.WriteLine("# | | |  ");
            Console.WriteLine("####### V");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.SetCursorPosition(1, 1);
            Console.Write("0"); Console.CursorLeft--;
            foreach (var item in nodes.path)
            {
                switch (item)
                {
                    case 'U':
                        Console.CursorTop-=2;
                        break;
                    case 'D':
                        Console.CursorTop += 2;
                        break;
                    case 'L':
                        Console.CursorLeft -= 2;
                        break;
                    case 'R':
                        Console.CursorLeft += 2;
                        break;

                }
                Console.Write("0"); Console.CursorLeft--;
            }
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("S");

            Console.ResetColor();
            Console.SetCursorPosition(1, 9);
            Console.Write(nodes.path.Length>50 ? "..." + nodes.path.Substring(nodes.path.Length-50) : nodes.path);
        }

        public static string part1(bool debug)
        {
            treenode root = new treenode();
            var MD5istance = MD5.Create();

            Queue<treenode> _toaccross = new Queue<treenode>();
            while (root.x != 3 || root.y != 3)
            {
                byte[] hashbyte = MD5istance.ComputeHash(Encoding.ASCII.GetBytes(input + root.path));                

                if (root.y > 0 && (hashbyte[0] >> 4) > 10) _toaccross.Enqueue(new treenode() { path = root.path + "U", x = root.x, y = root.y - 1 });
                if (root.y < 3 && (hashbyte[0] & 15) > 10) _toaccross.Enqueue(new treenode() { path = root.path + "D", x = root.x, y = root.y + 1 });
                if (root.x > 0 && (hashbyte[1] >> 4) > 10) _toaccross.Enqueue(new treenode() { path = root.path + "L", x = root.x - 1, y = root.y });
                if (root.x < 3 && (hashbyte[1] & 15) > 10) _toaccross.Enqueue(new treenode() { path = root.path + "R", x = root.x + 1, y = root.y });

                root = _toaccross.Dequeue();
                draw(root);
                Thread.Sleep(150);
            }


            return root.path;
        }

        public static string part2()
        {
            treenode root = new treenode();
            var MD5istance = MD5.Create();

            string biggestSolution = "";
            Queue<treenode> _toaccross = new Queue<treenode>();
            do
            {
                byte[] hashbyte = MD5istance.ComputeHash(Encoding.ASCII.GetBytes(input + root.path));

                if (root.y > 0 && (hashbyte[0] >> 4) > 10) _toaccross.Enqueue(new treenode() { path = root.path + "U", x = root.x, y = root.y - 1 });
                if (root.y < 3 && (hashbyte[0] & 15) > 10) _toaccross.Enqueue(new treenode() { path = root.path + "D", x = root.x, y = root.y + 1 });
                if (root.x > 0 && (hashbyte[1] >> 4) > 10) _toaccross.Enqueue(new treenode() { path = root.path + "L", x = root.x - 1, y = root.y });
                if (root.x < 3 && (hashbyte[1] & 15) > 10) _toaccross.Enqueue(new treenode() { path = root.path + "R", x = root.x + 1, y = root.y });

                if (_toaccross.Count > 0) root = _toaccross.Dequeue();
                else break;

                
                if (root.x == 3 && root.y == 3)
                { 
                    if (biggestSolution.Length < root.path.Length)
                        biggestSolution = root.path;

                    draw(root);
                    Console.WriteLine("\nBiggest ({1}):{0}", (biggestSolution.Length>50) ? "..." + biggestSolution.Substring(biggestSolution.Length-50) : biggestSolution, biggestSolution.Length);

                    root = _toaccross.Dequeue();
                }
            } while (true);


            return root.path;
        }

    
    }
}
