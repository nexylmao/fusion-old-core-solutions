using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Reflection;

namespace Fusion1911
{
    public abstract class GlobalPointers
    {
        public static object ObjectPointer;

        public static string Input, Output;

        public static Type[] DatabaseTypes;
        public static string[] DatabasePaths;
        public static List<object>  Databases;

        public static Action<Return> PushFunction;

        public static void DefineDBS()
        {
            DatabaseTypes = new Type[]{ typeof(Ability), typeof(Item), typeof(PlayerClass), typeof(NPC), typeof(Character) };
            DatabasePaths = new string[] { STRINGS._DATABASE_PATH_ABILITYLIST, STRINGS._DATABASE_PATH_ITEMLIST, STRINGS._DATABASE_PATH_CLASSLIST, STRINGS._DATABASE_PATH_NPCLIST, STRINGS._DATABASE_PATH_CHARACTERLIST };
            Databases = new List<object>();
            foreach (Type x in DatabaseTypes)
            {
                Type databasetype = typeof(Database<>).MakeGenericType(x);
                int index = DatabaseTypes.ToList().IndexOf(x);
                ConstructorInfo ctor = databasetype.GetConstructor(new Type[] { typeof(string) });
                Databases.Add(ctor.Invoke(new object[] {DatabasePaths[index]}));
            }
        }
    }

    public static class GlobalHandler
    {
        public static bool Admin;

        static Dictionary<Action, string> Methods;
        static Dictionary<Action, string> AdminMethods;

        private static void DefineMethods()
        {
            Methods = new Dictionary<Action, string>();
            AdminMethods = new Dictionary<Action, string>();
        }

        public static void Start()
        {
            GlobalPointers.DefineDBS();
            DefineMethods();
            DatabaseHandler.SwitchToDatabase(typeof(Ability));
        }

        public static void End()
        {

        }

        public static void Do(string Command, bool ForceAdmin = false)
        {

            if(Methods.Values.ToList().Contains(Command.ToLower()))
            {
                Methods.Keys.ToList()[Methods.Values.ToList().IndexOf(Command.ToLower())]();
                return;
            }
            else if(AdminMethods.Values.ToList().Contains(Command.ToLower()) && (Admin || ForceAdmin))
            {
                AdminMethods.Keys.ToList()[AdminMethods.Values.ToList().IndexOf(Command.ToLower())]();
                return;
            }
            else if(Command.ToLower().Contains("db") && (Admin || ForceAdmin))
            {
                DatabaseHandler.Do(Command);
                return;
            }

            GlobalPointers.PushFunction(new Return(STRINGS._NOFUNCTIONS));
        }

        #region Methods
        static void SwitchAdmin()
        {
            Admin = !Admin;
            GlobalPointers.PushFunction(new Return(STRINGS._SWITCHADMIN));
        }
        #endregion
    }

    public class Database<T>
    {
        public LinkedList<T> Data;
        public string Path;

        public Database(string Path = "")
        {
            Data = new LinkedList<T>();
            this.Path = Path;
        }

        public T this[int index]
        {
            get
            {
                try
                {
                    return Data.ElementAt(index);
                }
                catch
                {
                    return default(T);
                }
            }
        }

        #region Methods
        public Return Add(T newValue)
        {
            try
            {
                if (newValue != null)
                {
                    Data.AddLast(newValue);
                    return new Return(string.Format(STRINGS._GAMECREATE_GOOD_ADDTODB, typeof(T).Name), nameof(STRINGS._GAMECREATE_GOOD_ADDTODB));
                }
                return new Return(STRINGS._GAMECREATE_BAD_ADDTODB, nameof(STRINGS._GAMECREATE_BAD_ADDTODB));
            }
            catch
            {
                return new Return(STRINGS._GAMECREATE_BAD_ADDTODB, nameof(STRINGS._GAMECREATE_BAD_ADDTODB));
            }
        }
        public Return Remove(T toRemove)
        {
            try
            {
                if (toRemove != null)
                {
                    Data.Remove(toRemove);
                    return new Return(string.Format(STRINGS._GAMECREATE_GOOD_REMOVEFROMDB, typeof(T).Name), nameof(STRINGS._GAMECREATE_GOOD_REMOVEFROMDB));
                }
                return new Return(STRINGS._GAMECREATE_BAD_REMOVEFROMDB, nameof(STRINGS._GAMECREATE_BAD_REMOVEFROMDB));

            }
            catch
            {
                return new Return(STRINGS._GAMECREATE_BAD_REMOVEFROMDB, nameof(STRINGS._GAMECREATE_BAD_REMOVEFROMDB));
            }
        }
        public Return Find(string input)
        {
            try
            {
                int index = Int32.Parse(input.Substring(input.IndexOf(" ")));
                GlobalPointers.ObjectPointer = this[index];
                return new Return(STRINGS._GAMECREATE_GOOD_SETPOINTER, nameof(STRINGS._GAMECREATE_GOOD_SETPOINTER));
            }
            catch
            {
                return new Return(STRINGS._GAMECREATE_BAD_SETPOINTER, nameof(STRINGS._GAMECREATE_BAD_SETPOINTER));
            }
        }
        public Return Save()
        {
            FileStream Stream = new FileStream(Path, FileMode.Create);
            try
            {
                var Serializer = new DataContractJsonSerializer(typeof(Database<T>));
                Serializer.WriteObject(Stream, this);
                Stream.Close();
                return new Return(string.Format(STRINGS._GAMECREATE_GOOD_SAVED, typeof(T).Name), nameof(STRINGS._GAMECREATE_GOOD_SAVED));
            }
            catch
            {
                Stream.Close();
                return new Return(STRINGS._GAMECREATE_BAD_SAVED, nameof(STRINGS._GAMECREATE_BAD_SAVED));
            }
        }
        public Return Load()
        {
            try
            {
                var Serializer = new DataContractJsonSerializer(typeof(Database<T>));
                var Stream = new FileStream(Path, FileMode.Open);
                Database<T> temp = (Database<T>)Serializer.ReadObject(Stream);
                if (Data == null)
                {
                    Data = new LinkedList<T>();
                }
                foreach (T t in temp.Data)
                {
                    Data.AddLast(t);
                }
                Stream.Close();
                return new Return(string.Format(STRINGS._GAMELOAD_GOOD_LOADED, typeof(T).Name), nameof(STRINGS._GAMELOAD_GOOD_LOADED));
            }
            catch
            {
                return new Return(STRINGS._GAMELOAD_BAD_LOADED, nameof(STRINGS._GAMELOAD_BAD_LOADED));
            }
        }
        #endregion
    }

    public static class DatabaseHandler
    {
        static Type TypeWorking = GlobalPointers.DatabaseTypes[0];

        static Type DB;
        static dynamic Database;

        public static Return SwitchToDatabase(Type newType)
        {
            try
            {
                if(GlobalPointers.DatabaseTypes.Contains(newType))
                {
                    TypeWorking = newType;
                    DB = typeof(Database<>).MakeGenericType(TypeWorking);
                    Database = Convert.ChangeType(GlobalPointers.Databases[(GlobalPointers.DatabaseTypes.ToList().IndexOf(newType))], DB);
                    DefineMethods();
                    return new Return(string.Format(STRINGS._GAMECREATE_GOOD_DBSWITCH, TypeWorking.Name), nameof(STRINGS._GAMECREATE_GOOD_DBSWITCH));
                }
                return new Return(STRINGS._GAMECREATE_BAD_DBSWITCH, nameof(STRINGS._GAMECREATE_BAD_DBSWITCH));
            }
            catch
            {
                return new Return(STRINGS._GAMECREATE_BAD_DBSWITCH, nameof(STRINGS._GAMECREATE_BAD_DBSWITCH));
            }
        }

        public static Dictionary<Action, string> Methods;

        public static Return DefineMethods()
        {
            try
            {
                Methods = new Dictionary<Action, string>();
                Methods.Add(Add, string.Format(STRINGS._COMMAND_ADDDB, TypeWorking.Name).ToLower());
                Methods.Add(Remove, string.Format(STRINGS._COMMAND_REMOVEDB, TypeWorking.Name).ToLower());
                Methods.Add(Find, string.Format(STRINGS._COMMAND_FINDDB, TypeWorking.Name).ToLower());
                Methods.Add(Save, string.Format(STRINGS._COMMAND_SAVEDB, TypeWorking.Name).ToLower());
                Methods.Add(Load, string.Format(STRINGS._COMMAND_LOADDB, TypeWorking.Name).ToLower());
                return new Return(STRINGS._GAMECREATE_GOOD_DBHLOAD, nameof(STRINGS._GAMECREATE_GOOD_DBHLOAD));
            }
            catch
            {
                return new Return(STRINGS._GAMECREATE_BAD_DBHLOAD, nameof(STRINGS._GAMECREATE_BAD_DBHLOAD));
            }
        }

        public static void Do(string Command)
        {
            try
            {
                string stringType = Command.Substring(Command.IndexOf(" ") + 1);
                if(stringType.IndexOf(" ") != -1)
                {
                    stringType = stringType.Substring(stringType.IndexOf(" "));
                }
                // SPECIAL CASE FOR ALL DATABASES!~~
                if(STRINGS._COMMAND_ALLDB.Contains(stringType))
                {
                    int index = Methods.Values.ToList().IndexOf((Command.Substring(0, Command.IndexOf(" ")) + " Ability").ToLower());
                    foreach (Type x in GlobalPointers.DatabaseTypes)
                    {
                        GlobalPointers.PushFunction(SwitchToDatabase(x));
                        Methods.Keys.ToList()[index]();
                    }
                    return;
                }
                Type newType = Type.GetType(Convert.ToString(typeof(Ability).Namespace) + "." + stringType, true);
                if(newType != TypeWorking)
                {
                    GlobalPointers.PushFunction(SwitchToDatabase(newType));
                }
                int Index = Methods.Values.ToList().IndexOf(Command.ToLower());
                if (Index != -1)
                {
                    Methods.Keys.ToList()[Index]();
                }
            }
            catch
            {
                GlobalPointers.PushFunction(new Return(STRINGS._NOFUNCTIONS));
            }
        }

        #region Methods
        public static void Add()
        {
            GlobalPointers.PushFunction(Database.Add(GlobalPointers.ObjectPointer));
        }
        public static void Remove()
        {
            GlobalPointers.PushFunction(Database.Remove(GlobalPointers.ObjectPointer));
        }
        public static void Find()
        {
            GlobalPointers.ObjectPointer = Database[Int32.Parse(GlobalPointers.Input.Substring(GlobalPointers.Input.IndexOf(" ")))];
        }
        public static void Save()
        {
            GlobalPointers.PushFunction(Database.Save());
        }
        public static void Load()
        {
            GlobalPointers.PushFunction(Database.Load());
        }
        public static void DefineDatabases()
        {

        }
        #endregion  
    }

    public class Return
    {
        public string Name, Value;
        public Return(string Value, string Name = "")
        {
            this.Name = Name;
            this.Value = Value;
        }
    }
}
