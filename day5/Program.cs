using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace day5
{
    class Program
    {
        static void Main(string[] args)
        {
            const string secret = "ffykfhsq";
            int totalThread = 8;
            MD5[] md5s = new MD5[totalThread];
            for (int i = 0; i < totalThread; i++) md5s[i] = MD5.Create();
            

            long t = 0;

            long[] places = new long[8];
            char[] password = { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' };
            short missing = (short)password.Length;

            Func<long, int,byte[] > getLast = (v,i) => md5s[i].ComputeHash(Encoding.ASCII.GetBytes(secret + v.ToString()));
            
            //Part One
            string passwordstr = "";
            do
            {
                var hash = getLast(t++,0);
                if (hash[0] == 0 && hash[1] == 0 && hash[2] <= 0x0F)
                {
                    passwordstr += hash[2].ToString("X2")[1];
                    Console.WriteLine("password {0}", passwordstr);
                }
            } while (passwordstr.Length < 8);

            Console.WriteLine("result day5.1 = {0}.]\n", passwordstr);



            // Part two (single Thread)
            Stopwatch sp = new Stopwatch();
            sp.Start();
            do
            {
                var hash = getLast(t++,0);
                if (hash[0] == 0 && hash[1] == 0 && hash[2] <= 0x07)
                {
                    int posx = (int)hash[2];
                    if (password[posx] != ' ') continue;
                    missing--;
                    password[posx] = hash[3].ToString("x2")[0];
                    Console.WriteLine("password [{0}]", new String(password));
                }
            } while (missing > 0);
            sp.Stop();
            Console.WriteLine("result day5.2 (single) = [{0}] in {1} sec\n", new string(password), sp.Elapsed.TotalSeconds);


            // Part two (Multi Thread)
            password =new char[] { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ' };
            missing = (short)password.Length;
            t = 0;
            sp.Restart();
            //Paralleles:
            Parallel.For(0, totalThread, i =>
            {
                do
                {
                    long myt = Interlocked.Increment(ref t);
                    var hash = getLast(myt,i);

                    if (hash[0] == 0 && hash[1] == 0 && hash[2] <= 0x07)
                    {
                        lock (places)
                        {
                            int posx = (int)hash[2];
                            if (places[posx] > 0 && places[posx] < myt) continue;
                            places[posx] = myt;
                            missing--;
                            password[posx] = hash[3].ToString("x2")[0];
                            Console.WriteLine("password [{0}] by index:{1}", new String(password),i);
                        }
                    }
                } while (missing > 0);
            });
            sp.Stop();


            Console.WriteLine("result day5.2 (Multi) = [{0}] in {1} sec\n", new string(password), sp.Elapsed.TotalSeconds);

            Console.Write("Presse eny key ...");
            Console.ReadKey();
        }
    }
}
