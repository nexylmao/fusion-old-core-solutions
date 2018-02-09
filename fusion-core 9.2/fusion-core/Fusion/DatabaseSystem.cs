using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion.StoringSystems;
using Fusion.MessageSystem;
using Newtonsoft.Json;

namespace Fusion
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

    public class Database<Type>
    {
        #region Fields
        HashSet<Type> Data;
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
        public Database Output
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
                return Data.ElementAt(index);
            }
        }
        public int Count
        {
            get
            {
                return Data.Count;
            }
        }
        public IEnumerable<Type> GetAllData
        {
            get
            {
                return Data.ToList();
            }
        }
        #endregion
        #region Constructors
        public Database(string Path = "", bool Load = false, string Collection = "")
        {
            if(typeof(Type).IsSubclassOf(typeof(StorableObject)))
            {
                if(Path == "")
                {
                    Path = Core.DefaultPath;
                }
                Data = new HashSet<Type>();
                Msg = new MessageSender(TypeName + "DB",MessageReceiver.ReceiverList);
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
            Data = new HashSet<Type>(OutputHandler.Load());
        }
        public void Save()
        {
            OutputHandler.Save(Data);
        }
        public void Delete()
        {
            OutputHandler.Delete();
        }
        #endregion
        #region LocalMethods
        public void Add(Type newObject)
        {
            Data.Add(newObject);
        }
        public IEnumerable<Type> GetData()
        {
            return Data;
        }
        #endregion
    }
}
