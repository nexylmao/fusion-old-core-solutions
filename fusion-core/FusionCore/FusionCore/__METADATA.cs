using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Net.Http;
using Newtonsoft.Json;

namespace Fusion
{
    public enum DatabaseMode { LOCALFILES, MONGODB }; // for now only these two

    public interface Script
    {
        void Start();
        void Update();
    }

    public class __DLL
    {
        #region Fields
        private string path;
        private Type[] types;
        #endregion
        #region Properties
        public Type[] Types
        {
            get
            {
                return types;
            }
        }
        public bool isLoaded
        {
            get
            {
                return GetLoaded();
            }
        }
        public string Path
        {
            get
            {
                return path;
            }
            set
            {
                if(!isLoaded)
                {
                    path = value;
                }
            }
        }
        #endregion
        #region AppDomain/Assembly
        static AppDomain Domain;
        Assembly Assembly;
        #endregion
        #region MessageSender
        __MESSAGE_SENDER Msg;
        #endregion
        public __DLL(string Path, bool LoadNow = false)
        {
            if(Path.Contains(".\\"))
            {
                Path = System.IO.Directory.GetCurrentDirectory() + Path;
            }
            this.Path = Path;
            Msg = new __MESSAGE_SENDER();
            if(LoadNow)
            {
                LoadThisDLL();
            }
        }
        public void LoadThisDLL()
        {
            if(!isLoaded)
            {
                try
                {
                    if(Domain == null)
                    {
                        Domain = AppDomain.CreateDomain("LoadedDLL");
                    }
                    Assembly = Assembly.LoadFile(Path);
                    Domain.Load(Assembly.FullName);
                    types = Assembly.GetTypes();
                    Msg.SendMessageAsync(new __MESSAGE(true,0,"Loaded.")); // replace this hardcoded mess
                }
                catch (Exception ex)
                {
                    Msg.SendMessageAsync(new __MESSAGE(false,0,ex.Message));
                }
            }
            else
            {
                Msg.SendMessageAsync(new __MESSAGE(true,0,"DLL already loaded!")); // replace this hardcoded mess
            }
        }
        public void UnloadAllDLL()
        {
            if(Domain != null)
            {
                try
                {
                    AppDomain.Unload(Domain);
                    Msg.SendMessageAsync(new __MESSAGE(true,0,"Unloaded.")); // replace this hardcoded mess
                }
                catch (Exception ex)
                {
                    Msg.SendMessageAsync(new __MESSAGE(false,0,ex.Message)); // replace this hardcoded mess
                }
            }
            else
            {
                Msg.SendMessageAsync(new __MESSAGE(true,0,"Nothing loaded.")); // replace this hardcoded mess
            }
        }
        public bool GetLoaded()
        {
            if(Domain == null)
            {
                return false;
            }
            else
            {
                foreach(Assembly x in Domain.GetAssemblies().ToList())
                {
                    if(x.CodeBase == Path)
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }

    public class __DATABASE_TYPE
    {
        #region Fields
        public Type TYPEINFO;
        public string TYPEID;
        #endregion
        public __DATABASE_TYPE(Type TYPEINFO, string TYPEID)
        {
            this.TYPEINFO = TYPEINFO;
            this.TYPEID = TYPEID;
        }
    }

    public class __METADATA_FILE
    {
        #region Fields
        public List<string> ARGS;
        public List<__DATABASE_TYPE> DATA;
        #endregion
        public __METADATA_FILE()
        {
            ARGS = new List<string>();
            DATA = new List<__DATABASE_TYPE>();
        }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }

    public static class __METADATA
    {
        public static string Path;
        public const string DefaultPath = @"https://fusion-backend.herokuapp.com/FusionAPI/";
        public static DatabaseMode UsingDatabase;
        public static LinkedList<__DLL> DLLList;
        public static Database<__MESSAGE> MessageDB;
        static __METADATA_FILE FILE;

        public static void Start(string[] args)
        {
            __MESSAGE_RECEIVER defaultReceiver = new __MESSAGE_RECEIVER();
            __MESSAGE_RECEIVER.Default = defaultReceiver;
            try
            {
                FILE = __RESTCLIENT.GetMetadataFile().Result;
                UsingDatabase = DatabaseMode.MONGODB;
            }
            catch (Exception ex)
            {
                // should be replaced with a push system
                Console.WriteLine(ex.Message);
                Console.WriteLine("\n\n\nApplication can't continue with work... Shutting down...");
                Console.ReadKey(true);
                Environment.Exit(1);
            }
            MessageDB = new Database<__MESSAGE>();
            MessageDB.LoadEverything();
            DLLList = new LinkedList<__DLL>();
            foreach(string DLLPATH in FILE.ARGS)
            {
                DLLList.AddLast(new __DLL(DLLPATH, true));
            }
            // should be removed, for now is here for testing
            Console.WriteLine(FILE.ToString());
        }
    }
}
