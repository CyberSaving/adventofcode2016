using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day6
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] filelns = File.ReadAllLines(@"input.txt");
            const int lengthOfAlbabeth = ('z' - 'a')+1;
            int[][] counting = { new int[lengthOfAlbabeth], new int[lengthOfAlbabeth], new int[lengthOfAlbabeth], new int[lengthOfAlbabeth], new int[lengthOfAlbabeth] };

            int linelength = filelns[0].Length;
            counting = new int[linelength][];
            for (int i = 0; i < linelength; i++)
                counting[i] = new int[lengthOfAlbabeth];

            foreach (var line in filelns)
            {
                for (int i = 0; i < line.Length; i++)
                    counting[i][line[i] - 'a']++;
            }

            char[] resultinc = new char[linelength];
            char[] resultinc2 = new char[linelength];

            for (int i = 0; i < linelength; i++)
            {
                int max = (char)counting[i].Max();
                resultinc[i] = (char)('a'+ Array.IndexOf(counting[i], max));
                //for part 2
                int min = counting[i].Where(x => x != 0).Min();
                resultinc2[i] = (char)('a' + Array.IndexOf(counting[i], min));
            }

            Console.WriteLine("result day6.1 = {0}.\n", new string(resultinc));
            Console.WriteLine("result day6.2 = {0}.\n", new string(resultinc2));

            Console.Write("Presse eny key ...");
            Console.ReadKey();
        }
    }
}
