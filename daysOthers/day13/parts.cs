using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace adventofcode2016
{

    partial class day13
    {
        public class point {
            public int x;
            public int y;
            public short accros = 0;
            public short axis= -1;
            public bool clockwise = true;


            public void setDirection(double angle)
            {
                double ang = angle;
                clockwise = (angle % (Math.PI / 2)) < (Math.PI / 4);

                if (( Math.PI/4) <= angle &&  angle < (3* Math.PI)/4) axis = 1;
                else if (angle < (5*Math.PI)/4) axis = 2;
                else if (angle < (7 * Math.PI) / 4) axis = 3;
                else axis = 0;

            }
            public bool goNext(out point newPoint)
            {
                newPoint = new point() { x = x, y = y };
                short acurrentAxe = axis;
                short step = 0;
                short t = 0;
                while (t++ < 4 && step<4)
                {
                    
                    if ((acurrentAxe == 0) && (x< matrix.GetUpperBound(1)) &&  !matrix[y,x + 1] && !matrixpath[y, x + 1])
                    {
                        if (accros < ++step)
                            newPoint.x++;
                    }else if( (acurrentAxe == 1) && (y<matrix.GetUpperBound(0) ) && !matrix[y + 1,x ] && !matrixpath[y + 1, x])
                    {
                        if (accros < ++step)
                            newPoint.y++;
                    }
                    else if ((acurrentAxe == 2) && (x > 0) && !matrix[y,x-1] && !matrixpath[y, x - 1])
                    {
                        if (accros < ++step)
                            newPoint.x--;
                    }
                    else if ((acurrentAxe == 3) && (y > 0) && !matrix[ y-1,x] && !matrixpath[y - 1, x])
                    {
                        if (accros < ++step)
                            newPoint.y--;
                    }

                    if (accros < step)
                    {
                        accros = step;
                        break;
                    }
                    acurrentAxe=  (short)((acurrentAxe + (clockwise?1:-1)) % 4);
                    if (acurrentAxe < 0)
                        acurrentAxe = (short)(4 + acurrentAxe);


                }

                return (t <= 4);
                
            }
        }

        const int pointX = 31;
        const int pointY = 39;

        const int input = 1350;

        static public bool[,] matrix = new bool[50, 50 ];
        static public bool[,] matrixpath = new bool[50 , 50];

       
        static void init()
        {
            for (int y = 0; y < matrix.GetLength(0); y++)
                for (int x = 0; x < matrix.GetLength(1); x++)
                    matrix[y, x] = Convert.ToString(((x * x + 3 * x + 2 * x * y + y + y * y) + input), 2).Count(v => v == '1') % 2 == 1 ? true : false;
        }

        static void draw(int curx,int cury,bool[,] matrixoffixed )
        {
            int count = 0;
            Console.CursorTop = 0;
            Console.CursorLeft = 0;

            for (int y = 0; y < matrix.GetLength(0); y++)
            {
                Console.Write("{0} ", y.ToString("00"));
                for (int x = 0; x < matrix.GetLength(1); x++)
                {
                    if (pointY == y && pointX == x)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write('X');
                    }else if(y == cury && curx == x)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write("O");
                    }
                    else if(matrixoffixed[y,x])
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.Write("O");
                    }
                    else {
                        Console.ResetColor();
                        Console.Write(matrix[y, x] ? '#' : '.');
                    }

                    if (matrixoffixed[y, x])
                        count++;
                }
                Console.WriteLine();
                
            }
            Console.WriteLine("Count:{0}", count);
        }

        public static int part1(bool debug)
        {
            init();
            Stack<point> _path = new Stack<point>();
            point _lastpoint = new point() { x = 1, y = 1 };
            matrixpath[_lastpoint.y, _lastpoint.x] = true;
            _path.Push(_lastpoint);
            do
            {
                draw(_lastpoint.x, _lastpoint.y, matrixpath);
                var keypressed = Console.ReadKey();
                switch (keypressed.Key)
                {
                    case ConsoleKey.Q:
                        return -1;
                    case ConsoleKey.Enter:
                        double angle = Math.Atan2(pointY - _lastpoint.y, pointX - _lastpoint.x);
                        _lastpoint.setDirection(angle);
                        point _newpoint;
                        if (_lastpoint.goNext(out _newpoint))
                        {
                            _path.Push(_newpoint);
                            _lastpoint = _newpoint;
                            matrixpath[_lastpoint.y, _lastpoint.x] = true;
                        }
                        else
                        {
                            matrixpath[_lastpoint.y, _lastpoint.x] = false;
                            _path.Pop();
                            _lastpoint = _path.Peek();
                        }
                        break;
                    case ConsoleKey.Backspace:
                        matrixpath[_lastpoint.y, _lastpoint.x] = false;
                        _path.Pop();
                        _lastpoint = _path.Peek();
                        break;
                }
            } while ((_lastpoint.x != pointX || _lastpoint.y != pointY) && _path.Count > 0);
            return _path.Count;
        }

        public static long part2()
        {

            init();
            bool[,] matrixfixed = new bool[50, 50];

            Stack<point> _path = new Stack<point>();
            point _lastpoint = new point() { x = 1, y = 1 };
            matrixpath[_lastpoint.y, _lastpoint.x] = true;
            matrixfixed[_lastpoint.y, _lastpoint.x] = true;
            _path.Push(_lastpoint);
            do
            {

                double angle = Math.Atan2(50 - _lastpoint.y, 50 - _lastpoint.x);
                _lastpoint.setDirection(angle);
                point _newpoint;
                if (_lastpoint.goNext(out _newpoint) && _path.Count<=50)
                {   
                    _path.Push(_newpoint);
                    _lastpoint = _newpoint;
                    matrixpath[_lastpoint.y, _lastpoint.x] = matrixfixed[_lastpoint.y, _lastpoint.x] = true;
                }
                else
                {
                    matrixpath[_lastpoint.y, _lastpoint.x] = false;
                    _path.Pop();
                    if(_path.Count>0)
                        _lastpoint = _path.Peek();
                }
                
            } while ( _path.Count > 0);
            draw(_lastpoint.x, _lastpoint.y, matrixfixed);

            int count = 0;
            for (int y = 0; y < matrix.GetLength(0); y++)
                for (int x = 0; x < matrix.GetLength(1); x++)
                    if (matrixfixed[y, x])
                        count++;
            return count;

        }


    }
}
