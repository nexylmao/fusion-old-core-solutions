using System;
using System.Collections.Generic;
using System.Text;

namespace Descendants.Game
{
    public class Bar
    {
        public ConsoleColor Color;
    }

    public class BarStack
    {

    }

    public class Pixel
    {
        #region Fields
        public char Character;
        public ConsoleColor Foreground;
        public ConsoleColor Background;
        #endregion
        #region Static
        public static char DefaultChar = ' ';
        public static ConsoleColor DefaultForeground = ConsoleColor.White;
        public static ConsoleColor DefaultBackground = ConsoleColor.Black;
        #endregion
        #region Constructors
        public Pixel()
        {
            Character = DefaultChar;
            Foreground = DefaultForeground;
            Background = DefaultBackground;
        }
        public Pixel(char Character)
        {
            this.Character = Character;
            Foreground = DefaultForeground;
            Background = DefaultBackground;
        }
        public Pixel(ConsoleColor Foreground, ConsoleColor Background)
        {
            Character = DefaultChar;
            this.Foreground = Foreground;
            this.Background = Background;
        }
        public Pixel(char Character, ConsoleColor Foreground, ConsoleColor Background)
        {
            this.Character = Character;
            this.Foreground = Foreground;
            this.Background = Background;
        }
        #endregion
    }

    public class Screen
    {
        #region Fields
        Pixel[,] Pixels;
        #endregion
        #region Static
        public static ConsoleColor CurrentForeground
        {
            get
            {
                return Console.ForegroundColor;
            }
            set
            {
                Console.ForegroundColor = value;
            }
        }
        public static ConsoleColor CurrentBackground
        {
            get
            {
                return Console.BackgroundColor;
            }
            set
            {
                Console.BackgroundColor = value;
            }
        }
        public static int ConsoleWidth
        {
            get
            {
                return Console.WindowWidth;
            }
        }
        public static int ConsoleHeight
        {
            get
            {
                return Console.WindowHeight;
            }
        }
        public static Pixel DefaultPixel
        {
            get
            {
                return new Pixel();
            }
        }
        #endregion
        #region Constructors
        public Screen()
        {
            CurrentForeground = Pixel.DefaultForeground;
            CurrentBackground = Pixel.DefaultBackground;
            Pixels = new Pixel[ConsoleHeight, ConsoleWidth];
            Foreach((pxl) => { pxl = new Pixel(); Write(pxl); });
        }
        #endregion
        #region Methods
        public void WriteAll()
        {
            Foreach(Write);
        }
        public void Write(Pixel pxl)
        {
            if (CurrentBackground != pxl.Background)
            {
                CurrentBackground = pxl.Background;
            }
            if (CurrentForeground != pxl.Foreground)
            {
                CurrentForeground = pxl.Foreground;
            }
            Console.Write(pxl.Character);
        }
        public void Foreach(Action<Pixel> Method)
        {
            for (int i = 0; i < ConsoleHeight; i++)
            {
                for (int j = 0; j < ConsoleWidth; j++)
                {
                    if (i + 1 == ConsoleHeight && j + 1 == ConsoleWidth)
                    {
                        break;
                    }
                    Method.Invoke(Pixels[i, j]);
                }
            }
        }
        #endregion
    }
}
