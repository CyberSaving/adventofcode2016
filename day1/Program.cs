using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day1
{
    class Program
    {
        /// <summary>Orthogonal Line</summary>
        class OrthogonalLine
        {
            /// <summary>
            /// Constructor by a point and a vector
            /// </summary>
            /// <param name="FromX"></param>
            /// <param name="FromY"></param>
            /// <param name="axeX"></param>
            /// <param name="axeY"></param>
            /// <param name="lenght"></param>
            public OrthogonalLine(int FromX,int FromY,short axeX, short axeY,int lenght)
            {                
                this.FromX = Math.Min(FromX + axeX * lenght, FromX);
                this.ToX = Math.Max(FromX + axeX * lenght, FromX);
                this.FromY = Math.Min(FromY + axeY * lenght, FromY);
                this.ToY = Math.Max(FromY + axeY * lenght, FromY);

            }
            int FromX;
            int FromY;

            int ToX;
            int ToY;


            public bool Intersect(OrthogonalLine line,out int x, out int y)
            {
                x = 0;
                y = 0;

                bool intersect = false;
                if (line.FromX != line.ToX)
                {
                    x = this.FromX;
                    intersect = line.FromX < this.FromX && this.FromX < line.ToX;
                }
                else
                {
                    x = line.FromX;
                    intersect = this.FromX < line.FromX && line.FromX < this.ToX;
                }
                if (!intersect) return false;

                if (line.FromY != line.ToY)
                {
                    y = this.FromY;
                    intersect = line.FromY < this.FromY && this.FromY < line.ToY;
                }
                else
                {
                    y = line.FromY;
                    intersect = this.FromY < line.FromY && line.FromY < this.ToY;
                }
                return intersect;

            }

        }


        static void Main(string[] args)
        {
            //For Point1
            string filestr = File.ReadAllText(@"input.txt");

            short axeX = 0;
            short axeY = 1;

            int sumX = 0;
            int sumY = 0;

            //For Point2
            List<OrthogonalLine> horizontalLines = new List<OrthogonalLine>();
            List<OrthogonalLine> verticalLines = new List<OrthogonalLine>();
            int pointX = 0;
            int pointY = 0;
            OrthogonalLine founded = null;
            int foundedx=0, foundedy=0;

            foreach (string splitted in filestr.Split(','))
            {

                string trimmed = splitted.TrimStart();
                if (trimmed[0] == 'L')
                {
                    short oldx = axeX;
                    axeX = (short)-axeY;
                    axeY = oldx;
                }
                else
                {
                    short oldx = axeX;
                    axeX = axeY;
                    axeY = (short)-oldx;
                }
                int howmany = int.Parse(trimmed.Substring(1));

                #region Point2

                // In this frame we let intersect all Horizontal lines with last Vertical or
                // all Vertical Lines with last Horizantal untill an intersaction found!
                var line = new OrthogonalLine(pointX, pointY, axeX, axeY, howmany);
                if (founded == null)
                    if (axeX == 0) //Vertical
                    {
                        if (horizontalLines.Count > 1)
                            founded = horizontalLines.Take(verticalLines.Count - 1).FirstOrDefault(vline => vline.Intersect(line, out foundedx, out foundedy));
                        if (founded == null) verticalLines.Add(line);
                    }
                    else
                    {
                        if(verticalLines.Count>1)
                            founded = verticalLines.Take(verticalLines.Count - 1).FirstOrDefault(vline => vline.Intersect(line, out foundedx, out foundedy));
                        if (founded == null) horizontalLines.Add(line);
                    }

                pointX += (axeX * howmany);
                pointY += (axeY * howmany);
                #endregion


                sumX += (axeX * howmany);
                sumY += (axeY * howmany);


            }
            Console.WriteLine("result day1.1 = {0}.", Math.Abs(sumX)+ Math.Abs(sumY));
            
           
            Console.WriteLine( "result day1.2 = {0}.\n", Math.Abs(foundedx) + Math.Abs(foundedy));
            Console.Write("Presse eny key ...");
            Console.ReadKey();
        }
    }
}
