using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion1911;

namespace TestConsole
{
    static class IOHandler
    {
        public static void Push(Return x = null)
        {
            Console.WriteLine(x.Value);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(x.Name);
            Console.ResetColor();
        }
    }

    class Program
    { 
        static void Main(string[] args)
        {
            GlobalHandler.Start();
            GlobalPointers.PushFunction = IOHandler.Push;
            string unos = "";
            do
            {
                Console.Write("> ");
                unos = Console.ReadLine();
                GlobalHandler.Do(unos, true);
            }
            while (unos != STRINGS._COMMAND_EXIT);
            GlobalHandler.End();
        }
    }
}
