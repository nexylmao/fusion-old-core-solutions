using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization.Json;

namespace Fusion
{
    public abstract class Pointers
    {
        public static object ObjectPointer;
        public static string Input, Output;

        public static List<Type> DatabaseTypes = new List<Type>() { typeof(Ability), typeof(Item), typeof(PlayerClass), typeof(NPC), typeof(Character)};

        public static Database<Ability> GADB;
        public static DatabaseHandler<Ability> ADBH;
        public static Database<Item> GIDB;
        public static DatabaseHandler<Item> IDBH;
        public static Database<PlayerClass> GPCDB;
        public static DatabaseHandler<PlayerClass> PCDBH;
        public static Database<NPC> GNDB;
        public static DatabaseHandler<NPC> NDBH;
        public static Database<Character> GCDB;
        public static DatabaseHandler<Character> CDBH;
    }

    public class EventHandler : Pointers
    {
        public EventHandler()
        {
            Functions = new Dictionary<Action, string>();
            AdminFunctions = new Dictionary<Action, string>();
            Functions.Add(AdminMode, STRINGS._COMMAND_ADMIN);
            Functions.Add(Help, STRINGS._COMMAND_HELP);
            Functions.Add(Clear, STRINGS._COMMAND_CLEAR);
            Functions.Add(Quit, STRINGS._COMMAND_EXIT);
        }

        private void AdminMode()
        {
            AdminEnabler = !AdminEnabler;
            PushFunction(new ReturnValue(string.Format(STRINGS._SWITCHADMIN, AdminEnabler), nameof(STRINGS._SWITCHADMIN)));
        }

        private void Quit()
        {
            PushFunction(new ReturnValue(STRINGS._EXITMESSAGE, nameof(STRINGS._EXITMESSAGE)));
        }

        private void Help()
        {
            WriteAllCommands(AdminEnabler);
        }

        private void Clear()
        {
            Console.Clear();
        }

        public void AddDatabaseFunctions()
        {
            foreach (Action Method in ADBH.Methods.Keys.ToList())
            {
                Functions.Add(Method, ADBH.Methods.Values.ToList()[ADBH.Methods.Keys.ToList().IndexOf(Method)]);
            }
            foreach (Action Method in IDBH.Methods.Keys.ToList())
            {
                Functions.Add(Method, IDBH.Methods.Values.ToList()[IDBH.Methods.Keys.ToList().IndexOf(Method)]);
            }
            foreach (Action Method in CDBH.Methods.Keys.ToList())
            {
                Functions.Add(Method, CDBH.Methods.Values.ToList()[CDBH.Methods.Keys.ToList().IndexOf(Method)]);
            }
            foreach (Action Method in PCDBH.Methods.Keys.ToList())
            {
                Functions.Add(Method, PCDBH.Methods.Values.ToList()[PCDBH.Methods.Keys.ToList().IndexOf(Method)]);
            }
            foreach (Action Method in NDBH.Methods.Keys.ToList())
            {
                Functions.Add(Method, NDBH.Methods.Values.ToList()[NDBH.Methods.Keys.ToList().IndexOf(Method)]);
            }

            foreach (Action Method in ADBH.AdminMethods.Keys.ToList())
            {
                AdminFunctions.Add(Method, ADBH.AdminMethods.Values.ToList()[ADBH.AdminMethods.Keys.ToList().IndexOf(Method)]);
            }
            foreach (Action Method in IDBH.AdminMethods.Keys.ToList())
            {
                AdminFunctions.Add(Method, IDBH.AdminMethods.Values.ToList()[IDBH.AdminMethods.Keys.ToList().IndexOf(Method)]);
            }
            foreach (Action Method in CDBH.AdminMethods.Keys.ToList())
            {
                AdminFunctions.Add(Method, CDBH.AdminMethods.Values.ToList()[CDBH.AdminMethods.Keys.ToList().IndexOf(Method)]);
            }
            foreach (Action Method in PCDBH.AdminMethods.Keys.ToList())
            {
                AdminFunctions.Add(Method, PCDBH.AdminMethods.Values.ToList()[PCDBH.AdminMethods.Keys.ToList().IndexOf(Method)]);
            }
            foreach (Action Method in NDBH.AdminMethods.Keys.ToList())
            {
                AdminFunctions.Add(Method, NDBH.AdminMethods.Values.ToList()[NDBH.AdminMethods.Keys.ToList().IndexOf(Method)]);
            }
        }

        public Dictionary<Action, string> Functions;
        public Dictionary<Action, string> AdminFunctions; 
        public Action<ReturnValue> PushFunction;
        public GameStarter GameStarter;

        // ADMINMDOE ALLOWER
        public bool AdminEnabler;

        public void Do(string command, bool forceAdmin = false)
        {
            if(command.IndexOf(" ") != -1)
            {
                command = command.Substring(0, command.IndexOf(" "));
            }
            if (Functions.Values.Contains(command))
            {
                Functions.Keys.ToList()[Functions.Values.ToList().IndexOf(command)]();
                return;
            }
            else if (AdminFunctions.Values.Contains(command) && (forceAdmin || AdminEnabler))
            {
                AdminFunctions.Keys.ToList()[AdminFunctions.Values.ToList().IndexOf(command)]();
                return;
            }
            PushFunction(new ReturnValue(STRINGS._NOFUNCTIONS, nameof(STRINGS._NOFUNCTIONS)));
        }

        public void WriteAllCommands(bool forceAdmin = false)
        {
            PushFunction(new ReturnValue("Commands : ", ""));
            string AllCommands = "";
            foreach(string Command in Functions.Values.ToList())
            {
                AllCommands += (Command + " ");
            }
            PushFunction(new ReturnValue(AllCommands, ""));

            if(AdminEnabler || forceAdmin)
            {
                PushFunction(new ReturnValue("Admincommands : ", ""));
                AllCommands = "";
                foreach (string Command in AdminFunctions.Values.ToList())
                {
                    AllCommands += (Command + " ");
                }
                PushFunction(new ReturnValue(AllCommands, ""));
            }
        }
    }

    public class GameStarter : Pointers
    {
        public EventHandler GEH;
        public object GIOH;

        public GameStarter(object newIOH, Action<ReturnValue> push)
        {
            Start(newIOH, push);
        }

        // PLAY THE GAME
        public int Start(object newIOH, Action<ReturnValue> push)
        {
            GEH = new EventHandler
            {
                PushFunction = push
            };

            // to load databases and all handlers
            GADB = Database<Ability>.CreateNewDatabase(STRINGS._DATABASE_PATH_ABILITYLIST);
            ADBH = new DatabaseHandler<Ability>(GEH, GADB);
            GIDB = Database<Item>.CreateNewDatabase(STRINGS._DATABASE_PATH_ITEMLIST);
            IDBH = new DatabaseHandler<Item>(GEH, GIDB);
            GPCDB = Database<PlayerClass>.CreateNewDatabase(STRINGS._DATABASE_PATH_CLASSLIST);
            PCDBH = new DatabaseHandler<PlayerClass>(GEH, GPCDB);
            GCDB = Database<Character>.CreateNewDatabase(STRINGS._DATABASE_PATH_CHARACTERLIST);
            CDBH = new DatabaseHandler<Character>(GEH, GCDB);
            GNDB = Database<NPC>.CreateNewDatabase(STRINGS._DATABASE_PATH_NPCLIST);
            NDBH = new DatabaseHandler<NPC>(GEH, GNDB);
            GIOH = newIOH;

            GEH.AddDatabaseFunctions();

            // to start actually doing something
            return 0;
        }
    }

    public class ReturnValue
    {
        public string Value, Name;
        public ReturnValue(string Value, string Name)
        {
            this.Value = Value;
            this.Name = Name;
        }
    }

    public class Database<T>
    {
        public List<T> Data;
        public string Path;
        
        public static Database<T> CreateNewDatabase(string Path = "")
        {
            Database<T> Returning = new Database<T>();
            Returning.Data = new List<T>();
            Returning.Path = Path;
            return Returning;
        }

        public T this[int index]
        {
            get
            {
                try
                {
                    return Data[index];
                }
                catch
                {
                    return default(T);
                }
            }
            set
            {
                try
                {
                    Data[index] = value;
                }
                catch
                {
                    
                }
            }
        }

        public ReturnValue Add(T newValue)
        {
            try
            {
                if(newValue != null)
                {
                    Data.Add(newValue);
                    return new ReturnValue(string.Format(STRINGS._GAMECREATE_GOOD_ADDTODB, typeof(T).Name), nameof(STRINGS._GAMECREATE_GOOD_ADDTODB));
                }
                return new ReturnValue(STRINGS._GAMECREATE_BAD_ADDTODB, nameof(STRINGS._GAMECREATE_BAD_ADDTODB));
            }
            catch
            {
                return new ReturnValue(STRINGS._GAMECREATE_BAD_ADDTODB, nameof(STRINGS._GAMECREATE_BAD_ADDTODB));
            }
        }
        public ReturnValue Remove(T toRemove)
        {
            try
            {
                if(toRemove != null)
                {
                    Data.Remove(toRemove);
                    return new ReturnValue(string.Format(STRINGS._GAMECREATE_GOOD_REMOVEFROMDB, typeof(T).Name), nameof(STRINGS._GAMECREATE_GOOD_REMOVEFROMDB));
                }
                return new ReturnValue(STRINGS._GAMECREATE_BAD_REMOVEFROMDB, nameof(STRINGS._GAMECREATE_BAD_REMOVEFROMDB));

            }
            catch
            {
                return new ReturnValue(STRINGS._GAMECREATE_BAD_REMOVEFROMDB, nameof(STRINGS._GAMECREATE_BAD_REMOVEFROMDB));
            }
        }
        public ReturnValue RemoveAtIndex(int index)
        {
            try
            {
                Data.RemoveAt(index);
                return new ReturnValue(string.Format(STRINGS._GAMECREATE_GOOD_REMOVEFROMDB, typeof(T).Name), nameof(STRINGS._GAMECREATE_GOOD_REMOVEFROMDB));
            }
            catch
            {
                return new ReturnValue(STRINGS._GAMECREATE_BAD_REMOVEFROMDB, nameof(STRINGS._GAMECREATE_BAD_REMOVEFROMDB));
            }
        }
        public ReturnValue Save()
        {
            FileStream Stream = new FileStream(Path, FileMode.Create);
            try
            {
                var Serializer = new DataContractJsonSerializer(typeof(Database<T>));
                Serializer.WriteObject(Stream, this);
                Stream.Close();
                return new ReturnValue(string.Format(STRINGS._GAMECREATE_GOOD_SAVED, typeof(T).Name), nameof(STRINGS._GAMECREATE_GOOD_SAVED));
            }
            catch
            {
                Stream.Close();
                return new ReturnValue(STRINGS._GAMECREATE_BAD_SAVED, nameof(STRINGS._GAMECREATE_BAD_SAVED));
            }
        }
        public ReturnValue Load()
        {
            try
            {
                var Serializer = new DataContractJsonSerializer(typeof(Database<T>));
                var Stream = new FileStream(Path, FileMode.Open);
                Database<T> temp = (Database<T>)Serializer.ReadObject(Stream);
                if(Data == null)
                {
                    Data = new List<T>();
                }
                foreach(T t in temp.Data)
                {
                    Data.Add(t);
                }
                Stream.Close();
                return new ReturnValue(string.Format(STRINGS._GAMELOAD_GOOD_LOADED, typeof(T).Name),nameof(STRINGS._GAMELOAD_GOOD_LOADED));
            }
            catch
            {
                return new ReturnValue(STRINGS._GAMELOAD_BAD_LOADED, nameof(STRINGS._GAMELOAD_BAD_LOADED));
            }
        }
    }

    public class DatabaseHandler<T>
    {
        public EventHandler EventHandler;
        public Database<T> Database;
        public Dictionary<Action, string> Methods;
        public Dictionary<Action, string> AdminMethods;

        public DatabaseHandler(EventHandler EH, Database<T> DB)
        {
            EventHandler = EH;
            Database = DB;
            Methods = new Dictionary<Action, string>();
            AdminMethods = new Dictionary<Action, string>();
            string type = typeof(T).Name;
            AdminMethods.Add(Save, string.Format(STRINGS._COMMAND_SAVEDB, type));
            AdminMethods.Add(Load, string.Format(STRINGS._COMMAND_LOADDB, type));
            AdminMethods.Add(Add, string.Format(STRINGS._COMMAND_ADD, type));
            AdminMethods.Add(Remove, string.Format(STRINGS._COMMAND_REMOVE, type));
            AdminMethods.Add(RemoveAt, string.Format(STRINGS._COMMAND_REMOVEAT, type));
            AdminMethods.Add(EditIndex, string.Format(STRINGS._COMMAND_SET, type));
            Methods.Add(SetPointer, string.Format(STRINGS._COMMAND_FIND, type));
        }

        #region Methods
        public void Add()
        {
            EventHandler.PushFunction(Database.Add((T)Pointers.ObjectPointer));
        }
        public void Remove()
        {
            EventHandler.PushFunction(Database.Remove((T)EventHandler.ObjectPointer));
        }
        public void RemoveAt()
        {
            EventHandler.PushFunction(Database.RemoveAtIndex(int.Parse(EventHandler.Input.Substring(EventHandler.Input.IndexOf(" ") + 1))));
        }
        public void SetPointer()
        {
            try
            {
                Pointers.ObjectPointer = Database[int.Parse(Pointers.Input)];
                EventHandler.PushFunction(new ReturnValue(STRINGS._GAMECREATE_GOOD_SETPOINTER, nameof(STRINGS._GAMECREATE_GOOD_SETPOINTER)));
            }
            catch
            {
                EventHandler.PushFunction(new ReturnValue(STRINGS._GAMECREATE_BAD_SETPOINTER, nameof(STRINGS._GAMECREATE_BAD_SETPOINTER)));
            }
        }
        public void EditIndex()
        {
            try
            {
                Database.Data[int.Parse(Pointers.Input.Substring(Pointers.Input.IndexOf(" ") + 1))] = (T)Pointers.ObjectPointer;
                EventHandler.PushFunction(new ReturnValue(STRINGS._GAMECREATE_GOOD_EDITONINDEX, nameof(STRINGS._GAMECREATE_GOOD_EDITONINDEX)));
            }
            catch
            {
                EventHandler.PushFunction(new ReturnValue(STRINGS._GAMECREATE_BAD_EDITONINDEX, nameof(STRINGS._GAMECREATE_BAD_EDITONINDEX)));
            }
        }
        public void Save()
        {
            EventHandler.PushFunction(Database.Save());
        }
        public void Load()
        {
            EventHandler.PushFunction(Database.Load());
        }
        #endregion
    }

    // TURN TO OTHER FILE
    // pretty useless now, might use it in future tho
    // reference to push put up in the EH
    /*public class IOHandler
    {
        public List<Item> LootingItems;
        public Bag PlayerBag;
        public List<Ability> AbilityBar;
        public AbilityList Castables;
        public Player Player;
        public EventHandler EventHandler;

        public static Action<ReturnValue> PushFunction = OldPush;

        public void Push(ReturnValue r)
        {
            PushFunction(r);
        }

        public static void OldPush(ReturnValue r)
        {
            Console.WriteLine(r.Value);
            // Console.WriteLine(r.Name);
            // analyze r.Name !
        }
    }*/
}