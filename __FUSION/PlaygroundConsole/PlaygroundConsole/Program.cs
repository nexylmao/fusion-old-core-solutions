using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Fusion;
using Item;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization.Attributes;

namespace PlaygroundConsole
{
    class Program
    {
        class __METADATA_BIT
        {
            public Type TYPEINFO;
            public string DATABASEPATH;
            public string TYPECATEGORYID;

            public __METADATA_BIT(Type Type, string Path, string ID)
            {
                TYPEINFO = Type;
                DATABASEPATH = Path;
                TYPECATEGORYID = ID;
            }
        }

        class __METADATA_FILESTRUCTURE
        {
            public List<string> ARGS;
            public List<__METADATA_BIT> DATA;
        }

        static void DebugWrite(string[] __write)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("///////////////////////////");
            foreach (string x in __write)
            {
                Console.WriteLine(x);
            }
            Console.WriteLine("///////////////////////////");
            Console.ResetColor();
        }

        static void Main(string[] args)
        {
            /* THIS IS USED FOR WRITING THE FILE
            __METADATA.IMPORT(@".\Item.dll");
            __METADATA.IMPORT(typeof(Item.Item), @".\ItemDatabasePath.json", "ItemDatabaseIdentification");
            __METADATA.SAVE();
            Console.WriteLine(__METADATA.ToString());
            Console.ReadKey(); */

            /* MADE CONVERSION FROM MONGODB
            var connection = new MongoClient("mongodb://localhost:27017");
            var db = connection.GetDatabase("fusion");
            var collection = db.GetCollection<BsonDocument>("startJSON");
            var findoptions = new FindOptions<BsonDocument>();
            findoptions.Projection = "{_id:0}";
            var list = collection.FindSync(new BsonDocument(), findoptions).ToList();
            var x = JsonConvert.DeserializeObject<__METADATA_FILESTRUCTURE>(list[0].ToJson());
            __METADATA.JSON = x;

            DebugWrite();
            */

            Console.ReadKey();
        }
    }
}
