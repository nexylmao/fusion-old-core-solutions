using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using Fusion.Core.Messaging;

namespace Fusion.Core.AssemblyLoading
{
    public interface IScript
    {
        #region Methods
        void Start();
        void End();
        #endregion
    }

    public class DLLManager
    { 
        #region Fields
        string[] Path;
        LinkedList<Assembly> Assembly;
        MessageSender Sender;
        MessageReceiver Receiver;
        #endregion
        #region Static Fields
        static AppDomain Domain;
        #endregion
        #region Property
        public IEnumerable<Type> Types
        {
            get
            {
                LinkedList<Type> list = new LinkedList<Type>();
                foreach(Assembly x in Assembly)
                {
                    foreach(Type y in x.GetTypes())
                    {
                        list.AddLast(y);
                    }
                }
                return list;
            }
        }
        #endregion
        #region Constructors
        public DLLManager(IEnumerable<string> Paths, bool LoadNow = true)
        {
            Path = new string[Paths.Count()];
            for(int i = 0; i < Paths.Count(); i++)
            {
                Path[i] = Paths.ElementAt(i);
            }
            Receiver = Integrated.AssemblyLoaderReceiver();
            Sender = Integrated.AssemblyLoaderSender();
            if(LoadNow)
            {
                Load();
            }
        }
        ~DLLManager()
        {
            Unload();
        }
        #endregion
        #region Methods
        public void Load()
        {
            if (Domain == null)
            {
                Domain = AppDomain.CreateDomain("DLLLoader");
            }
            if (Assembly == null)
            {
                Assembly = new LinkedList<Assembly>();
            }
            if(Path.Count() > 0)
            {
                foreach (string one in Path)
                {
                    try
                    {
                        Assembly.AddLast(Domain.Load(System.Reflection.Assembly.LoadFile(Directory.GetCurrentDirectory() + @"\" + one).FullName));
                        foreach (Type x in Assembly.Last.Value.ExportedTypes)
                        {
                            if (x.IsSubclassOf(typeof(IScript)))
                            {
                                IScript scriptClass = (IScript)Activator.CreateInstance(x);
                                scriptClass.Start();
                            }
                        }
                        Sender.SendTo(Sender.PreloadedMessages.ElementAt(1), MessageReceiver.First);
                    }
                    catch (Exception ex)
                    {
                        Sender.PreloadedMessages.ElementAt(0).AddExceptionMessage(ex);
                        Sender.SendTo(Sender.PreloadedMessages.ElementAt(0), MessageReceiver.First);
                    }
                }
            }
            else
            {
                Sender.SendTo(Sender.PreloadedMessages.ElementAt(2), MessageReceiver.First);
            }
        }
        public void Unload()
        {
            foreach (Type x in Assembly.Last.Value.ExportedTypes)
            {
                if (x.IsSubclassOf(typeof(IScript)))
                {
                    IScript scriptClass = (IScript)Activator.CreateInstance(x);
                    scriptClass.End();
                }
            }
            if (Domain != null)
            {
                try
                {
                    AppDomain.Unload(Domain);
                }
                catch
                {
                    Sender.SendToAll(304);
                }
            }
        }
        #endregion
    }
}
