using System;
using Descendants.Game;

namespace Playground
{
    class Program
    {
        class ConsoleColorPixel
        {
            public ConsoleColor ForeGround, BackGround;

            public ConsoleColorPixel()
            {
                ForeGround = ConsoleColor.White;
                BackGround = ConsoleColor.Black;
            }

            public ConsoleColorPixel(ConsoleColor ForeGround, ConsoleColor BackGround)
            {
                this.ForeGround = ForeGround;
                this.BackGround = BackGround;
            }

            public override bool Equals(object obj)
            {
                return (this == (ConsoleColorPixel)obj);
            }

            public static bool isNull(ConsoleColorPixel x)
            {
                try
                {
                    ConsoleColor temp = x.BackGround;
                    x.BackGround = temp;
                    return false;
                }
                catch
                {
                    return true;
                }
            }

            public static bool operator==(ConsoleColorPixel a, ConsoleColorPixel b)
            {
                if(a.ForeGround == b.ForeGround && a.BackGround == b.BackGround)
                {
                    return true;
                }
                return false;
            }

            public static bool operator!=(ConsoleColorPixel a, ConsoleColorPixel b)
            {
                return !(a == b);
            }

            public void Use()
            {
                Console.ForegroundColor = ForeGround;
                Console.BackgroundColor = BackGround;
            }

            public void Reset()
            {
                Console.ResetColor();
            }
        }

        static void Write(char[,] screen, ConsoleColorPixel[,] screencolor)
        {
            Console.Clear();
            ConsoleColorPixel current = new ConsoleColorPixel();
            for(int i = 0; i < screen.GetLength(0); i++)
            {
                for(int j = 0; j < screen.GetLength(1); j++)
                {
                    if(i == (screen.GetLength(0) - 1) && j == (screen.GetLength(1)-1))
                    {
                        break;
                    }
                    if(ConsoleColorPixel.isNull(screencolor[i,j]))
                    {
                        screencolor[i,j] = new ConsoleColorPixel();
                    }
                    if(current != screencolor[i,j])
                    {
                        current = screencolor[i, j];
                        current.Use();
                    }
                    Console.Write(screen[i, j]);
                }
            }
        }

        static void CreateBar(char[,] screen, ConsoleColorPixel[,] screencolor, int x, int y, string value, ConsoleColor color, bool percentage = false, double percent = 0)
        {
            if(y + 9 <= Console.WindowWidth)
            {
                int blocks = ((int)Math.Round(percent, 0)) / 10;
                for(int i = y; i < y + blocks; i++)
                {
                    screencolor[x, i] = new ConsoleColorPixel(color, ConsoleColor.Black);
                }
                for (int i = y; i < y + blocks; i++)
                {
                    screencolor[x, i] = new ConsoleColorPixel(ConsoleColor.Black, color);
                }
                string perc = ((int)Math.Round(percent, 0)).ToString();
                if (perc.Length == 1)
                {
                    perc = "  " + perc;
                }
                if (perc.Length == 2)
                {
                    perc = " " + perc;
                }
                perc += "%";

                for (int i = y; i < y + 10; i++)
                {
                    if(i - y < value.Length)
                    {
                        screen[x, i] = value[i - y];
                    }
                    if(i >= y + 6)
                    {
                        screen[x, i] = perc[i - (y + 6)];
                    }
                }
                Write(screen, screencolor);
            }
        }

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            char[,] screen = new char[Console.WindowHeight, Console.WindowWidth];
            ConsoleColorPixel[,] screencolor = new ConsoleColorPixel[Console.WindowHeight, Console.WindowWidth];

            CreateBar(screen, screencolor, 5, 20, "320M", ConsoleColor.Green, true, 100);
            CreateBar(screen, screencolor, 6, 20, "1000K", ConsoleColor.Blue, true, 50);

            while (true)
            {
                char c = Console.ReadKey(true).KeyChar;
                
            }
        }
    }
}
