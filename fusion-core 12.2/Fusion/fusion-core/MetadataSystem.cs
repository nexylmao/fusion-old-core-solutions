using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Core.Logging;
using Fusion.Core.Database;
using Fusion.Core.AssemblyLoading;
using Fusion.Core.Messaging;
using Fusion.Core.StoringSystems;

namespace Fusion
{
    public class MetadataFile : StorableObject
    {
        #region Fields
        public string DefaultPath;
        public HashSet<string> PATHS;
        #endregion
        #region Constructors
        public MetadataFile()
        {
            PATHS = new HashSet<string>();
        }
        public MetadataFile(string[] args)
        {
            PATHS = new HashSet<string>();
            foreach (string x in args)
            {
                PATHS.Add(x);
            }
        }
        #endregion
    }

    public static class ApplicationCore
    {
        #region Fields
        static string DefaultPath;
        static HashSet<string> PATHS;
        static ILogWriter DefaultLogger;
        static MessageReceiver DefaultReceiver;
        static MessageSender DefaultSender;
        static Database<MetadataFile> Metadata;
        static DLLManager DLLManager;
        static bool DebugMode;
        #endregion
        #region Properties
        public static string Path
        {
            get
            {
                return DefaultPath;
            }
        }
        public static bool Debug
        {
            get
            {
                return DebugMode;
            }
        }
        #endregion
        #region Methods/Public
        public static void Start(string[] args, bool Debug = false)
        {
            DebugMode = Debug;
            DefaultLogger = new LocalLogger();
            DefaultReceiver = DefaultLogger.Receiver;
            DefaultLogger.SendToAll(101);
            DefaultSender = Integrated.CoreSender();
            DefaultPath = DetermineArgs(args, out PATHS);
            Console.WriteLine(DefaultPath);
            foreach(string x in PATHS)
            {
                Console.WriteLine(x);
            }
            try
            {
                Metadata = new Database<MetadataFile>(DefaultPath, true);
                if (Metadata.Count == 0 && args.Count() > 1)
                {
                    Metadata.Delete();
                    Metadata.Add(new MetadataFile(PATHS.ToArray()));
                    Metadata.Save();
                }
                if (Metadata.Count > 1)
                {
                    Metadata.Delete();
                    Metadata.Clear();
                }
                try
                {
                    DLLManager = new DLLManager(Metadata[0].PATHS, true);
                }
                catch
                {
                    DefaultLogger.SendToAll(303);
                }
                DefaultSender.SendToAll(102);
            }
            catch
            {
                DefaultSender.SendToAll(103);
            }
        }
        #endregion
        #region Methods/Private
        public static string DetermineArgs(string[] args, out HashSet<string> paths)
        {
            int index = -1;
            for(int i = 0; i < args.Count(); i++)
            {
                if(args[i].Contains("http") || args[i].Contains("localhost"))
                {
                    index = i;
                    string returning = args[i];
                    args.ToList().RemoveAt(index);
                    paths = new HashSet<string>(args);
                    return returning;
                }
            }
            paths = new HashSet<string>(args);
            return "localfiles";
        }
        #endregion
    }
}
