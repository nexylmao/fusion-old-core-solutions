using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion;

namespace PublicTestConsole
{    
    class Program
    {
        // some useful interface methods...
        static void DebugWrite(string[] __write)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("///////////////////////////");
            foreach(string x in __write)
            {
                Console.WriteLine(x);
            }
            Console.WriteLine("///////////////////////////");
            Console.ResetColor();
        }
        static void WriteItem(dynamic __item)
        {
            Console.WriteLine("Item ID = {0}",__item.ItemID);
            Console.WriteLine("Name = {0}",__item.ItemName);
            Console.WriteLine("Desc = {0}", __item.ItemDesc);
        }

        static void Main(string[] args)
        {
            __METADATA.START(args);
            DebugWrite(new string[]{ __METADATA.ToString()});
            dynamic obj = Activator.CreateInstance(__METADATA.GetType(0));
            Console.WriteLine(obj);
            obj.ItemName = "Random Name";
            obj.ItemID = 120;
            obj.ItemDesc = "Random Desc";
            WriteItem(obj);
            Console.ReadKey();
        }
    }
}
