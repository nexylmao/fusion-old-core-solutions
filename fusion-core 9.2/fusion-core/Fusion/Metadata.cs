using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Fusion.MessageSystem;
using Fusion.StoringSystems;
using Fusion.LoggingSystems;
using Fusion.AssemblyLoading;

namespace Fusion
{
    public interface IScript
    {
        #region Methods
        void Start();
        void End();
        #endregion
    }

    public class MetadataFile : StorableObject
    {
        #region Fields
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
            foreach(string x in args)
            {
                PATHS.Add(x);
            }
        }
        #endregion
    }

    public static class Core
    {
        #region Fields
        static Database<MetadataFile> File;
        static Database<Message> Messages;
        static string Path;
        static MessageSender DefaultSender;
        static MessageReceiver DefaultReceiver;
        static ILogger DefaultLogger;
        static DLLManager AssemblyManager;
        static bool isStarted = false;
        #endregion
        #region Properties
        public static string DefaultPath
        {
            get
            {
                return Path;
            }
        }
        public static MessageReceiver EditReceiver
        {
            get
            {
                return DefaultReceiver;
            }
        }
        public static MessageSender EditSender
        {
            get
            {
                return DefaultSender;
            }
        }
        #endregion
        #region Methods
        public static void Start(string[] args)
        {
            DefaultLogger = new LocalLogger();
            if(args.Count() > 0)
            {
                Path = args[0];
            }
            LoadIntegratedMessages();
            if (Messages == null)
            {
                MakeIntegratedMessages();
            }
            LoadDefaultReceiver();
            if (DefaultReceiver == null)
            {
                MakeDefaultReceiver();
            }
            LoadDefaultSender();
            File = new Database<MetadataFile>(Path, true);
            if (DefaultSender == null)
            {
                MakeDefaultSender();
            }
            DefaultSender.Preload(Messages);
            AssemblyManager = new DLLManager(((List<MetadataFile>)File.GetAllData).ElementAt(0).PATHS);
            isStarted = true;
        }
        public static void InitStart(string[] args)
        {
            if (args.Count() > 0)
            {
                Path = args[0];
            }
            File = new Database<MetadataFile>(Path, true);
            MakeIntegratedMessages();
            MakeDefaultReceiver();
            MakeDefaultSender();
            DefaultSender.Preload(Messages);
            isStarted = true;
        }
        public static void MakeMetadata(string[] args)
        {
            if (isStarted)
            {
                File.Delete();
                File.Add(new MetadataFile(args));
                File.Save();
            }
        }
        public static void DeleteMakedata()
        {
            File.Delete();
        }
        public static void AddReceiver(MessageReceiver Receiver)
        {
            DefaultSender.AddReceiver(Receiver);
        }
        public static void Init(string[] args)
        {
            Database<MessageReceiver> Receivers = new Database<MessageReceiver>(args[0], false);
            Receivers.Delete();
            Receivers.Add(DefaultReceiver);
            Receivers.Save();
        }
        public static void End()
        {
            SaveDefaultReceiver();
            SaveDefaultSender();
            SaveIntegratedMessages();
            AssemblyManager.Unload();
        }

        // INTEGRATED RECEIVER COMMANDS
        public static void WriteMessageToConsole(Message m)
        {
            Console.WriteLine(m.ToJSON(Formatting.Indented));
        }
        public static void ExitConsoleApplication()
        {
            Environment.Exit(69);
        }

        // DEFAULT RECEIVER/SENDER COMMANDS
        public static void MakeDefaultReceiver()
        {
            DefaultReceiver = new MessageReceiver("Core", WriteMessageToConsole);
            DefaultReceiver.AddCommand("--sd", ExitConsoleApplication);
        }
        public static void MakeDefaultSender()
        {
            DefaultSender = new MessageSender("Core", new MessageReceiver[] { DefaultReceiver });
        }
        public static void SaveDefaultSender()
        {
            DefaultSender.SendToAll(new Message(RestClient<MessageSender>.DeleteCollection(Path, "CoreSender").Result));
            if(DefaultSender != null)
            {
                RestClient<MessageSender>.Post(new MessageSender[] { DefaultSender }, Path, "CoreSender").Wait();
            }
        }
        public static void LoadDefaultSender()
        {
            try
            {
                DefaultSender = RestClient<MessageSender>.Get(Path, "CoreSender").Result[0];
            }
            catch
            {
                DefaultSender = null;
            }
        }
        public static void SaveDefaultReceiver()
        {
            DefaultSender.SendToAll(new Message(RestClient<MessageReceiver>.DeleteCollection(Path, "CoreReceiver").Result));
            if(DefaultReceiver != null)
            {
                RestClient<MessageReceiver>.Post(new MessageReceiver[] { DefaultReceiver }, Path, "CoreReceiver").Wait();
            }
        }
        public static void LoadDefaultReceiver()
        {
            try
            {
                DefaultReceiver = RestClient<MessageReceiver>.Get(Path, "CoreReceiver").Result[0];
            }
            catch
            {
                DefaultReceiver = null;
            }
        }
        public static void DeleteDefaultReceiver()
        {
            DefaultReceiver = null;
        }
        public static void DeleteDefaultSender()
        {
            DefaultSender = null;
        }
        // INTEGRATED MESSAGES
        public static void MakeIntegratedMessages()
        {
            Messages = new Database<Message>();
            Messages.Add(new Message(true, 200, "Everything is working good!"));
            Messages.Add(new Message(false, 101, "The task couldn't be finished!"));
            Messages.Add(new Message(false, 1, "Critical app error! (Shutting down...)", "--sd"));
        }
        public static void SaveIntegratedMessages()
        {
            Messages.Save();
        }
        public static void LoadIntegratedMessages()
        {
            try
            {
                Messages.Load();
            }
            catch
            {
                Messages = null;
            }
        }
        public static void DeleteIntegratedMessages()
        {
            Messages.Delete();
        }
        #endregion
    }
}
