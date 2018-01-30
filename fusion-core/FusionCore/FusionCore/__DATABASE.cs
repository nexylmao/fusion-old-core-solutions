using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fusion
{
    public abstract class DatabaseClass
    {
        // _id - is the id that MongoDB provides, and ID is a customID we made, more made for 1-2-3 kinds of id
        public string _id;
        public string ID;
    }

    public class Database<T>
    { 
        public HashSet<T> Data;
        private __MESSAGE_SENDER Msg;

        public T this[int index]
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

        public Database()
        {
            Data = new HashSet<T>();
            Msg = new __MESSAGE_SENDER();
            Msg.DefaultDestination = __MESSAGE_RECEIVER.Default;
        }

        public void Add(T obj)
        {
            Data.Add(obj);
            Msg.SendMessageAsync(new __MESSAGE(true, 0, "Object added to database (" + typeof(T).Name + ")"));
        }

        public void SaveEverything()
        {
            try
            {
                foreach(T x in Data)
                {
                    Msg.SendMessageAsync(new __MESSAGE(true, 0, __RESTCLIENT.Post(x).Result));
                }
            }
            catch (Exception ex)
            {
                Msg.SendMessageAsync(new __MESSAGE(false, 0, ex.Message));
            }
        }

        public void LoadEverything()
        {
            try
            {
                List<T> Loaded = __RESTCLIENT.Get<T>().Result;
                foreach (T x in Loaded)
                {
                    Data.Add(x);
                }
                Msg.SendMessageAsync(new __MESSAGE(true, 0, "Database loaded successfully!"));
            }
            catch (Exception ex)
            {
                Msg.SendMessageAsync(new __MESSAGE(false, 0, ex.Message, "--sd"));
            }
        }

        public void ClearLocalDatabase()
        {
            Data.Clear();
            Msg.SendMessageAsync(new __MESSAGE(true,0,"Local copy of database " + typeof(T).Name));
        }

        public void ClearDatabase()
        {
            MethodInfo method = typeof(T).GetMethod("DeleteCollection").MakeGenericMethod(typeof(T));
            var x = method.Invoke(typeof(T), null);
        }
    }
}
