using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using Fusion.MessageSystem;

namespace Fusion.AssemblyLoading
{
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
            Receiver = new MessageReceiver("DLLManager");
            Sender = new MessageSender("DLLManager", new MessageReceiver[] { MessageReceiver.First });
            if(LoadNow)
            {
                Load();
            }
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
            foreach (string one in Path)
            {
                Assembly.AddLast(Domain.Load(System.Reflection.Assembly.LoadFile(Directory.GetCurrentDirectory() + one).FullName));
                foreach(Type x in Assembly.Last.Value.ExportedTypes)
                {
                    if(x.IsSubclassOf(typeof(IScript)))
                    {
                        IScript scriptClass = (IScript)Activator.CreateInstance(x);
                        scriptClass.Start();
                    }
                }
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
                AppDomain.Unload(Domain);
            }
        }
        #endregion
    }
}
