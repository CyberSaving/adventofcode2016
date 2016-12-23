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

    partial class day21
    {
        const string input = "abcdefgh"; 
        const string input2 = "fbgdceah";
        static Regex reg = new Regex(@"^swap (position|letter) (\w|\d*) with \1 (\w|\d*)");
        static Regex regRotate = new Regex(@"^rotate (left|right|based on position of letter) (\w|\d*)");
        static Regex regRevOrMove = new Regex(@"^(reverse|move) positions? (\d*) (through|to position) (\d*)");

        private static void scramble(ref char[] buffer,string instruction)
        {
            Match match = reg.Match(instruction);
            if (match.Success)
            {
                if (match.Groups[1].Value == "position")
                {
                    int from = int.Parse(match.Groups[2].Value);
                    int to = int.Parse(match.Groups[3].Value);
                    char temp = buffer[from]; buffer[from] = buffer[to]; buffer[to] = temp;
                }
                else
                {
                    char cfrom = match.Groups[2].Value[0];
                    char cto = match.Groups[3].Value[0];
                    for (int t = 0; t < buffer.Length; t++)
                    {
                        if (buffer[t] == cfrom) buffer[t] = cto;
                        else if (buffer[t] == cto) buffer[t] = cfrom;
                    }
                }
                return;
            }
            match = regRotate.Match(instruction);
            if (match.Success)
            {
                int X = 0; string pattern = match.Groups[1].Value;
                if (pattern.StartsWith("based"))
                {
                    X = Array.IndexOf(buffer, match.Groups[2].Value[0]);
                    X += X > 3 ? 2 : 1;
                    pattern = "right";
                }
                else
                    X = int.Parse(match.Groups[2].Value);
                X = X % buffer.Length;

                if (X == 0)
                {
                    Console.WriteLine("Nothing! X is 0.");
                    return;
                }


                if (pattern == "left")
                {
                    char[] newbuffer = new char[buffer.Length];
                    Array.Copy(buffer, X, newbuffer, 0, buffer.Length - X);
                    Array.Copy(buffer, 0, newbuffer, buffer.Length - X, X);
                    buffer = newbuffer;
                }
                else
                {
                    char[] newbuffer = new char[buffer.Length];
                    Array.Copy(buffer, 0, newbuffer, X, buffer.Length - X);
                    Array.Copy(buffer, buffer.Length - X, newbuffer, 0, X);
                    buffer = newbuffer;
                }
                return;
            }

            match = regRevOrMove.Match(instruction);
            if (match.Success)
            {
                string pattern = match.Groups[1].Value;
                int from = int.Parse(match.Groups[2].Value);
                int to = int.Parse(match.Groups[4].Value);

                if (pattern == "reverse")
                {
                    int l = to - from;
                    for (int t = 0; t <= l / 2; t++)
                    {
                        //SWAP t <-> 2L-t:
                        char temp = buffer[from + t]; buffer[from + t] = buffer[to - t]; buffer[to - t] = temp;
                    }
                }
                else
                {
                    char fromx = buffer[from];
                    if (from < to)
                        Array.Copy(buffer, from + 1, buffer, from, to - from);
                    else
                        Array.Copy(buffer, to, buffer, to + 1, from - to);
                    buffer[to] = fromx;
                }

            }
        }

        private static void unscramble(ref char[] buffer, string instruction)
        {
            Match match = reg.Match(instruction);
            if (match.Success)
            {
                if (match.Groups[1].Value == "position")
                {
                    //CHANGED from part 1:
                    int from = int.Parse(match.Groups[3].Value);
                    int to = int.Parse(match.Groups[2].Value);
                    char temp = buffer[from]; buffer[from] = buffer[to]; buffer[to] = temp;
                }
                else
                {
                    char cfrom = match.Groups[2].Value[0];
                    char cto = match.Groups[3].Value[0];
                    for (int t = 0; t < buffer.Length; t++)
                    {
                        if (buffer[t] == cfrom) buffer[t] = cto;
                        else if (buffer[t] == cto) buffer[t] = cfrom;
                    }
                }

                return;
            }
            match = regRotate.Match(instruction);
            if (match.Success)
            {
                int X = 0; string pattern = match.Groups[1].Value;
                if (pattern.StartsWith("based"))
                {
                    X = Array.IndexOf(buffer, match.Groups[2].Value[0]);
                    //CHANGED from part 1:
                    if (X % 2 == 0)
                        X = ((X + buffer.Length) / 2 + 1);
                    else
                        X = (X / 2) + 1;

                    pattern = "right";
                }
                else
                    X = int.Parse(match.Groups[2].Value);
                X = X % buffer.Length;

                if (X == 0)
                    return;

                //CHANGED from part 1:
                if (pattern == "right")
                {
                    char[] newbuffer = new char[buffer.Length];
                    Array.Copy(buffer, X, newbuffer, 0, buffer.Length - X);
                    Array.Copy(buffer, 0, newbuffer, buffer.Length - X, X);
                    buffer = newbuffer;
                }
                else
                {
                    char[] newbuffer = new char[buffer.Length];
                    Array.Copy(buffer, 0, newbuffer, X, buffer.Length - X);
                    Array.Copy(buffer, buffer.Length - X, newbuffer, 0, X);
                    buffer = newbuffer;
                }
                return;
            }

            match = regRevOrMove.Match(instruction);
            if (match.Success)
            {
                string pattern = match.Groups[1].Value;
                int from = int.Parse(match.Groups[2].Value);
                int to = int.Parse(match.Groups[4].Value);

                if (pattern == "reverse")
                {
                    int l = to - from;
                    for (int t = 0; t <= l / 2; t++)
                    {
                        char temp = buffer[from + t]; buffer[from + t] = buffer[to - t]; buffer[to - t] = temp;
                    }
                }
                else
                {
                    //CHANGED from part 1:
                    int temp = from; from = to; to = temp;
                    char fromx = buffer[from];

                    if (from < to)
                        Array.Copy(buffer, from + 1, buffer, from, to - from);
                    else
                        Array.Copy(buffer, to, buffer, to + 1, from - to);
                    buffer[to] = fromx;
                }

            }
        }

        public static string part1(bool debug)
        {
            string[] filelns = File.ReadAllLines(@"day21\input.txt");

            char[] buffer = new char[input.Length];
            Array.Copy(input.ToCharArray(),buffer, input.Length);
            for (int i = 0; i < filelns.Length; i++)
            {
                string instruction = filelns[i];
                scramble(ref buffer, instruction);
                if (debug)
                {
                    Console.Write(buffer);
                    Console.WriteLine("->{0}:\t{1}", new string(buffer), instruction);
                }
            }
            return new string(buffer);
        }

        public static string part2(bool debug)
        {
            string[] filelns = File.ReadAllLines(@"day21\input.txt");

            char[] buffer = new char[input2.Length];
            Array.Copy(input2.ToCharArray(), buffer, input2.Length);
            for (int i = filelns.Length-1; i >=0 ; i--)
            {
                string instruction = filelns[i];
                string scrubbled = new string(buffer);
                unscramble(ref buffer, instruction);
                if (debug)
                {
                    Console.Write(new string(buffer));
                    Console.WriteLine("->{0}:\t{1}",scrubbled, instruction);
                }

                //test:
                char[] newbuffer = new char[buffer.Length]; Array.Copy(buffer, newbuffer, buffer.Length);
                scramble(ref newbuffer, instruction);
                if(new string(newbuffer)!= scrubbled)
                {
                    //ops! I made a mistake!
                    Console.WriteLine("I made a mistake!!");
                    return scrubbled;
                }
            }
            return new string(buffer);
        }



    
    }


}
