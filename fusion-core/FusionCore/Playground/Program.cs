using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Fusion;
using Newtonsoft.Json;

namespace Playground
{
    static class helpfulmethods
    {
        public static void Flush()
        {
            while (Console.KeyAvailable)
            {
                Console.ReadKey(true);
            }
        }
        public static void LoadAndWrite(string[] args)
        {
            Console.Write("Click anything to load _METADATA\n");
            Console.ReadKey(true);
            Console.WriteLine();
            __METADATA.Start(args);
            Flush();
        }
        public static void DeleteReWriteMetadata()
        {
            Console.WriteLine(__RESTCLIENT.DeleteMetadataFile().Result);
            Type[] types = Assembly.LoadFile(System.IO.Directory.GetCurrentDirectory() + ".\\Item.dll").GetExportedTypes();
            __DATABASE_TYPE db = new __DATABASE_TYPE(types[0], "ITEMS");
            __METADATA_FILE mf = new __METADATA_FILE();
            mf.ARGS.Add(".\\Item.dll");
            mf.DATA.Add(db);
            Console.WriteLine(__RESTCLIENT.PostMetadataFile(mf).Result);
        }
        public static dynamic NewOfType(Type T)
        {
            return Activator.CreateInstance(T);
        }
        public static dynamic DatabaseOfType(Type T)
        {
            return Activator.CreateInstance(typeof(Database<>).MakeGenericType(T));
        }
        public static dynamic[] ThreeItems(Type t)
        {
            dynamic[] d = new dynamic[3];
            d[0] = NewOfType(t);
            d[0].ID = 1;
            d[0].Name = "First Item";
            d[0].Description = "First item to see the database!";
            d[1] = NewOfType(t);
            d[1].ID = 2;
            d[1].Name = "Second Item";
            d[1].Description = "Second item to see the database!";
            d[2] = NewOfType(t);
            d[2].ID = 3;
            d[2].Name = "Third Item";
            d[2].Description = "Third item to see the database!";
            return d;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            helpfulmethods.LoadAndWrite(args);
            Type t = __METADATA.DLLList.ElementAt(0).Types[0];
            dynamic db = helpfulmethods.DatabaseOfType(t);
            db.LoadEverything();
            Console.WriteLine(JsonConvert.SerializeObject(db.Data,Formatting.Indented));
            Console.WriteLine(JsonConvert.SerializeObject(__METADATA.MessageDB.Data));
            __MESSAGE m = new __MESSAGE(true, 101, "Hello dashboard!");
            __RESTCLIENT.PostMessage(m).Wait();
            Console.ReadKey(true);
        }
    }
}
