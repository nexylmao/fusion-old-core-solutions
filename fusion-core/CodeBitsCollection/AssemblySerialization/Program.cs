using System;
using Newtonsoft.Json;

namespace AssemblySerialize
{
    class Program
    {
        static void Main(string[] args)
        {
            // get Assembly of any type in the program, serialize with JsonConvert, and write it out
            Console.WriteLine(JsonConvert.SerializeObject(typeof(Program).Assembly, Formatting.Indented));
            Console.ReadKey();
        }
    }
}
