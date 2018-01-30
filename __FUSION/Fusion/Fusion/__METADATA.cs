using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Fusion
{
    public static class __METADATA
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

        static string PATH;
        static __METADATA_FILESTRUCTURE JSON;

        const string __DEFAULTPATH = @".\__METADATA";

        public static string GetPath(int Index)
        {
            return JSON.DATA[Index].DATABASEPATH;
        }

        public static string GetID(int Index)
        {
            return JSON.DATA[Index].TYPECATEGORYID;
        }

        public static Type GetType(int Index)
        {
            return JSON.DATA[Index].TYPEINFO;
        }
        
        public static void START(string[] args)
        {
            if(args.Count() != 0)
            {
                PATH = args.Last();
            }
            else
            {
                PATH = __DEFAULTPATH;
            }
            LOAD(PATH);
            for (int i = 0; i < JSON.ARGS.Count()-1; i++)
            {
                var DLL = Assembly.LoadFrom(@JSON.ARGS[i]);
            }
        }

        public static void DEFINELIST(int Length = 0)
        {
            if(JSON == null)
            {
                JSON = new __METADATA_FILESTRUCTURE();
            }
            if(JSON.DATA == null)
            {
                JSON.DATA = new List<__METADATA_BIT>(Length);
            }
            else
            {
                JSON.DATA.Capacity = Length;
            }
        }

        public static void DEFINEARGS(int Length = 0)
        {
            if (JSON == null)
            {
                JSON = new __METADATA_FILESTRUCTURE();
            }
            if (JSON.ARGS == null)
            {
                JSON.ARGS = new List<string>(Length);
            }
            else
            {
                JSON.ARGS.Capacity = Length;
            }
        }

        public static void LOAD(string __metafile)
        {
            if(__metafile.Contains("mongodb"))
            {
                try
                {
                    var connection = new MongoClient(__metafile);
                    var db = connection.GetDatabase("fusion");
                    var collection = db.GetCollection<BsonDocument>("startJSON");
                    var findoptions = new FindOptions<BsonDocument>();
                    findoptions.Projection = "{_id:0}";
                    var list = collection.FindSync(new BsonDocument(), findoptions).ToList();
                    JSON = JsonConvert.DeserializeObject<__METADATA_FILESTRUCTURE>(list[0].ToJson());
                }
                catch (Exception ex)
                {
                    throw ex; // through the window ayyy
                }
            }
            else
            {
                TextReader tr = new StreamReader(__metafile);
                try
                {
                    JSON = JsonConvert.DeserializeObject<__METADATA_FILESTRUCTURE>(tr.ReadToEnd());
                    tr.Close();
                }
                catch (Exception ex)
                {
                    // CHANGE THIS LATER
                    tr.Close();
                    throw ex;
                }
            }
        }

        public static void SAVE(string __metafile = __DEFAULTPATH)
        {
            try
            {
                TextWriter tw = new StreamWriter(__metafile);
                tw.Write(ToString());
                tw.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void IMPORT(string Path)
        {
            try
            {
                if(JSON == null || JSON.ARGS == null)
                {
                    DEFINEARGS();
                }
                JSON.ARGS.Add(Path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void IMPORT(Type Type, string Path, string ID)
        {
            try
            {
                if(JSON == null || JSON.DATA == null)
                {
                    DEFINELIST();
                }
                JSON.DATA.Add(new __METADATA_BIT(Type, Path, ID));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DELETE(string Path)
        {
            try
            {
                JSON.ARGS.Remove(Path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DELETE(Type Type, string Path, string ID)
        {
            try
            {
                __METADATA_BIT Queue = new __METADATA_BIT(Type, Path, ID);
                if(JSON.DATA.Contains(Queue))
                {
                    JSON.DATA.ToList().Remove(Queue);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DELETE(int Index)
        {
            try
            {
                JSON.DATA.RemoveAt(Index);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static new string ToString()
        {
            try
            {
                return JsonConvert.SerializeObject(JSON, Formatting.Indented);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
