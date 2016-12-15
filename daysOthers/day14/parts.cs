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

    partial class day14
    {
        const string input = "jlmsuwbz";
        //const string input = "abc";
        const int FUTURE_SIZE = 1000;
        const int ONETIME_PADKEY = 64;


        public static long part1(bool debug)
        {
            var MD5istance = MD5.Create();
            long iter = 0;
            int nofkey = 0;
            Queue<string[]> future = new Queue<string[]>(FUTURE_SIZE);
            Regex reg = new Regex(@"([A-Z0-9])\1\1(\1\1)?");



            while (nofkey < ONETIME_PADKEY)
            {
                iter++;
                byte[] hash = MD5istance.ComputeHash(Encoding.ASCII.GetBytes(input + iter));
                string hashPassword = BitConverter.ToString(hash).Replace("-", "");
                var matches = reg.Matches(hashPassword);
                string[] matchesstring = new string[matches.Count];
                for (int i = 0; i < matches.Count; i++)
                    matchesstring[i] = matches[i].Value;
                future.Enqueue(matchesstring);


                if (future.Count > FUTURE_SIZE)
                {
                    matchesstring = future.Dequeue();
                    if (matchesstring.Length > 0)
                    {
                        string triple = matchesstring[0].Substring(0, 3);
                        string quintuple = triple + triple.Substring(0, 2);
                        if (future.FirstOrDefault(futureitem => futureitem.Length > 0 && futureitem.Contains(quintuple)) != null)
                        {
                            nofkey++;
                            Console.WriteLine("{0} at:{1}", nofkey, iter - 1000);
                            //break;
                        }
                    }           
                }

            }

            return iter-1000;

        }

        public static long part2()
        {

            var MD5istance = MD5.Create();
            long iter = -1;
            int nofkey = 0;
            Queue<string[]> future = new Queue<string[]>(FUTURE_SIZE);
            Regex reg = new Regex(@"([a-z0-9])\1\1(\1\1)?");

            while (nofkey < ONETIME_PADKEY)
            {
                iter++;
                byte[] hash = MD5istance.ComputeHash(Encoding.ASCII.GetBytes(input + iter));
                string hashPassword = BitConverter.ToString(hash).Replace("-", "").ToLower();
                for (int i = 0; i < 2016; i++)
                {
                    hash = MD5istance.ComputeHash(Encoding.ASCII.GetBytes(hashPassword));
                    hashPassword = BitConverter.ToString(hash).Replace("-", "").ToLower();
                }


                var matches = reg.Matches(hashPassword);
                string[] matchesstring = new string[matches.Count];
                for (int i = 0; i < matches.Count; i++)
                    matchesstring[i] = matches[i].Value;
                future.Enqueue(matchesstring);


                if (future.Count > FUTURE_SIZE)
                {
                    matchesstring = future.Dequeue();
                    if (matchesstring.Length > 0)
                    {
                        string triple = matchesstring[0].Substring(0, 3);
                        string quintuple = triple + triple.Substring(0, 2);
                        if (future.FirstOrDefault(futureitem => futureitem.Length > 0 && futureitem.Contains(quintuple)) != null)
                        {
                            nofkey++;
                            Console.WriteLine("{0} at:{1}", nofkey, iter - 1000);
                            //break;
                        }
                    }
                }

            }

            return iter - 1000;


        }

        class Container
        {
            public char Char;
            public int Counter;
            public int Index;
        }        
    }
}
