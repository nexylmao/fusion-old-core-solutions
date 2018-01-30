using System;
using Fusion1911;
using System.IO;

namespace TestingValuesConsole
{
    class Program
    {
        static UInt64 NewSurvHealth(UInt64 Surv, UInt64 Level)
        {
            UInt64 sum = 0;
            sum = (UInt64)(((CONSTANTS.LevelHP(100, Level) * 3) * ((double)Surv / (double)100)) + CONSTANTS.LevelHP(Surv, Level) + 100);
            return sum;
        }

        static void Main(string[] args)
        {
            TextWriter tr = new StreamWriter("outputnew 22122017.txt");

            for (int Level = 1; Level <= 65535; Level++)
            {
                int Durability = 0;
                Console.Write("Level = {0} || Surv0 = {1} ", Level, CONSTANTS.HealthToString(NewSurvHealth((ulong)Durability, (ulong)Level)));
                Durability = 1;
                Console.Write("|| Surv{0} = {1} ", Durability, CONSTANTS.HealthToString(NewSurvHealth((ulong)Durability, (ulong)Level)));
                for (Durability = 25; Durability <= 100; Durability += 25)
                {
                    Console.Write("|| Surv{0} = {1} ", Durability, CONSTANTS.HealthToString(NewSurvHealth((ulong)Durability, (ulong)Level)));
                }
                Console.WriteLine("||");

                Durability = 0;
                tr.Write("Level = {0} || Surv0 = {1} ", Level, CONSTANTS.HealthToString(NewSurvHealth((ulong)Durability,(ulong)Level)));
                Durability = 1;
                tr.Write("|| Surv{0} = {1} ", Durability, CONSTANTS.HealthToString(NewSurvHealth((ulong)Durability, (ulong)Level)));
                for (Durability = 10; Durability <= 100; Durability += 10)
                {
                    tr.Write("|| Surv{0} = {1} ", Durability, CONSTANTS.HealthToString(NewSurvHealth((ulong)Durability, (ulong)Level)));
                }
                tr.WriteLine("||");
            }
            Console.WriteLine("Calculated!");
            tr.Close();
            Console.ReadKey();
        }
    }
}
