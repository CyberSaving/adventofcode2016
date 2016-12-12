using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace adventofcode2016
{

    partial class day10
    {
        enum ChipType { Value,Output,Bot}
        class MC {
            public ChipType type { get; set; }
            public int idx { get; set; }
            public virtual void assign(MC themc)
            {
                throw new NotSupportedException();
            }
            public override string ToString()
            {
                return string.Format("{0} [{1}]", type, idx);
            }
        }
        class Output : MC
        {
            public MC value { get; set; }
            public override void assign(MC themc)
            {
                this.value = themc;
            }
        }

            class bot: MC
        {

            public MC IN1 { get; set; }
            public MC IN2 { get; set; }

            public MC LowTO { get; set; }
            public MC HighTO { get; set; }

            public override void assign(MC themc)
            {
                if (IN1 != null)
                {
                    if (IN1.idx == themc.idx)
                        return;
                    else if(IN1.idx> themc.idx)
                    {
                        IN2 = IN1;
                        IN1 = themc;
                    }
                    else
                        IN2 = themc;
                    
                } else
                    IN1 = themc;
            }
        }


        private static Dictionary<int, bot> fixMotherBoard()
        {
            string[] filelns = File.ReadAllLines(@"day10\input.txt");
            Dictionary<int, bot> _bots = new Dictionary<int, bot>();
            Regex r1 = new Regex(@"value (\d+) goes to bot (\d+)");
            Regex r2 = new Regex(@"bot (\d+) gives low to (bot|output) (\d+) and high to (bot|output) (\d+)");

            Func<int, bot> getOrCreateBot = (idx) =>
            {
                bot thebot = null;
                if (!_bots.TryGetValue(idx, out thebot))
                {
                    thebot = new bot() { idx = idx, type = ChipType.Bot };
                    _bots.Add(idx, thebot);
                }
                return thebot;
            };

            foreach (var line in filelns)
            {
                var match = r1.Match(line);
                if (match.Success)
                {

                    var mc = new MC() { type = ChipType.Value, idx = int.Parse(match.Groups[1].Value) };
                    int indexofBot = int.Parse(match.Groups[2].Value);
                    getOrCreateBot(indexofBot).assign(mc);
                }
                else
                {
                    match = r2.Match(line);
                    bot thebot = getOrCreateBot(int.Parse(match.Groups[1].Value));
                    if (match.Groups[2].Value == "bot")
                        thebot.LowTO = getOrCreateBot(int.Parse(match.Groups[3].Value));
                    else
                        thebot.LowTO = new Output() { type = ChipType.Output, idx = int.Parse(match.Groups[3].Value) };

                    if (match.Groups[4].Value == "bot")
                        thebot.HighTO = getOrCreateBot(int.Parse(match.Groups[5].Value));
                    else
                        thebot.HighTO = new Output() { type = ChipType.Output, idx = int.Parse(match.Groups[5].Value) };

                }
            }

            int botNotFull = 0;
            do
            {
                botNotFull = 0;
                foreach (bot thebot in _bots.Values)
                {
                    if (thebot.IN2 != null)
                    {
                        thebot.LowTO.assign(thebot.IN1);
                        thebot.HighTO.assign(thebot.IN2);
                    }
                    else
                        botNotFull++;
                }
            } while (botNotFull > 0);

            return _bots;
        }

        public static int part1(bool debug)
        {
            return fixMotherBoard().First(bot => bot.Value.IN1.idx == 17 && bot.Value.IN2.idx == 61).Key;
        }

        public static long part2()
        {
            var key = new int[] { 0, 1, 2 };

            long rtval = 1;
            foreach (bot item in fixMotherBoard().Values)
            {
                if((item.HighTO.type == ChipType.Output) && Array.IndexOf(key, item.HighTO.idx) >= 0)
                    rtval*= ((Output)item.HighTO).value.idx;
                if ((item.LowTO.type == ChipType.Output) && Array.IndexOf(key, item.LowTO.idx) >= 0)
                    rtval *= ((Output)item.LowTO).value.idx;

            }
            return rtval;

        }


    }
}
