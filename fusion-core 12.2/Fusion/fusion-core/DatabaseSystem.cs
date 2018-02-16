using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.Core.StoringSystems;
using Fusion.Core.Messaging;
using Newtonsoft.Json;

namespace Fusion.Core.Database
{
    public abstract class StorableObject
    {
        #region Fields
        protected string _id;
        protected uint ID;
        #endregion
        #region Methods
        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
        #endregion
    }

    public class Database<Type> : HashSet<Type>
    {
        #region Fields
        MessageSender Msg;
        DatabaseOutputHandler<Type> OutputHandler;
        #endregion
        #region Property
        public string TypeName
        {
            get
            {
                return typeof(Type).Name;
            }
        }
        public StoringSystems.Database Output
        {
            get
            {
                return OutputHandler.OutputType;
            }
        }
        public string Path
        {
            get
            {
                return OutputHandler.OutputPath;
            }
        }
        public Type this[int index]
        {
            get
            {
                return this.ElementAt(index);
            }
        }
        #endregion
        #region Constructors
        public Database(string Path = "", bool Load = false, string Collection = "") : base()
        {
            if(typeof(Type).IsSubclassOf(typeof(StorableObject)))
            {
                if(Path == "")
                {
                    Path = ApplicationCore.Path;
                }
                Msg = Integrated.DatabaseSender(TypeName + "DB");
                OutputHandler = new DatabaseOutputHandler<Type>(Path);
                if(Load)
                {
                    this.Load();
                }
            }
        }
        #endregion
        #region OutputMethods
        public void Load()
        {
            dynamic x;
            if ((x = OutputHandler.Load()) != null || x.Count != 0)
            {
                foreach(Type y in x)
                {
                    Add(y);
                }
                Msg.SendTo(Msg.PreloadedMessages.ElementAt(0), MessageReceiver.First);
            }
            else
            {
                Msg.SendTo(Msg.PreloadedMessages.ElementAt(1), MessageReceiver.First);
            }
        }
        public void Save()
        {
            if(Delete())
            {
                var result = OutputHandler.Save(this.ToArray());
                if (result.ToLower().Contains("successfully"))
                {
                    Msg.SendTo(Msg.PreloadedMessages.ElementAt(2), MessageReceiver.First);
                }
                else
                {
                    Msg.SendTo(Msg.PreloadedMessages.ElementAt(3), MessageReceiver.First);
                }
            }
        }
        public bool Delete()
        {
            var result = OutputHandler.Delete();
            if (result.ToLower().Contains("successfully"))
            {
                Msg.SendTo(Msg.PreloadedMessages.ElementAt(4), MessageReceiver.First);
                return true;
            }
            else
            {
                Msg.SendTo(Msg.PreloadedMessages.ElementAt(5), MessageReceiver.First);
                return false;
            }
        }
        #endregion
    }
}
