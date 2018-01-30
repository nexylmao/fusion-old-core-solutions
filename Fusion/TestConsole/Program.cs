using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion;
using System.IO;
using System.Xml.Serialization;

namespace TestConsole
{
    class IOHandler
    {
        public static GameStarter Game;

        public static void DefineMethods()
        {
            Game.GEH.AdminFunctions.Add(CreateEmptyObject, "/CreateEmpty");
        }

        public static void Push(ReturnValue returnValue)
        {
            Console.Write(returnValue.Value);
            //Console.Write(" ");
            //Console.Write(returnValue.Name);
            Console.Write("\n");
        }

        public static void CheckAllDatabases()
        {
            if(Pointers.GADB == null)
            {
                Push(new ReturnValue("No ability database!", "lul"));
            }
            else
            {
                Push(new ReturnValue("Abilitydatabase OK!", "lul"));
            }
            if (Pointers.GCDB == null)
            {
                Push(new ReturnValue("No character database!", "lul"));
            }
            else
            {
                Push(new ReturnValue("Characterdatabase OK!", "lul"));
            }
            if (Pointers.GIDB == null)
            {
                Push(new ReturnValue("No item database!", "lul"));
            }
            else
            {
                Push(new ReturnValue("Itemdatabase OK!", "lul"));
            }
            if (Pointers.GNDB == null)
            {
                Push(new ReturnValue("No NPC database!", "lul"));
            }
            else
            {
                Push(new ReturnValue("NPCdatabase OK!", "lul"));
            }
            if (Pointers.GPCDB == null)
            {
                Push(new ReturnValue("No playerclass database!", "lul"));
            }
            else
            {
                Push(new ReturnValue("Playerclassdatabase OK!", "lul"));
            }
        }

        public static void LoadAll()
        {
            foreach (Type t in Pointers.DatabaseTypes)
            {
                Console.WriteLine("Trying to read : {0}DB", t.Name);
                Game.GEH.Do(string.Format(STRINGS._COMMAND_LOADDB, t.Name), true);
            }
        }

        public static void SaveAll()
        {
            foreach(Type t in Pointers.DatabaseTypes)
            {
                Console.WriteLine("Trying to save : {0}DB", t.Name);
                Game.GEH.Do(string.Format(STRINGS._COMMAND_SAVEDB, t.Name), true);
            }
        }

        public static void RunAutoStart()
        {
            CheckAllDatabases();
            LoadAll();
            DefineMethods();
        }

        public static void RunAutoEnd()
        {
            SaveAll();
        }

        public static void CreateEmptyObject()
        {
            int x = 0, ispis;
            do
            {
                ispis = 1;
                Console.WriteLine("Which type do you want?");
                foreach(Type t in Pointers.DatabaseTypes)
                {
                    Console.WriteLine("{0}. - {1}",ispis++, t.Name);
                }
                Console.WriteLine("6. - None");
                Console.Write("> ");
                x = Convert.ToInt32(Console.ReadLine());
                if(x == 6)
                {
                    return;
                }
                else if (x < 1 || x > 5)
                {
                    Console.WriteLine("You've entered an non existing type, try again!");
                }
                else
                {
                    Pointers.ObjectPointer = Activator.CreateInstance(Pointers.DatabaseTypes[x-1]);
                    Game.GEH.Do(string.Format(STRINGS._COMMAND_ADD, Pointers.DatabaseTypes[x-1].Name), Game.GEH.AdminEnabler);
                }
            }
            while (x < 1 || x > 6);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            IOHandler IOH = new IOHandler();
            GameStarter Game = new GameStarter(IOH, IOHandler.Push);
            IOHandler.Game = Game;
            IOHandler.RunAutoStart();
            Console.ReadKey();
            Console.Clear();
            var Stream = new FileStream(STRINGS._DATABASE_PATH_NPCLIST, FileMode.Create);
            Stream.Close();
            do
            {
                Console.Write("> ");
                Pointers.Input = Console.ReadLine();
                Game.GEH.Do(Pointers.Input, Game.GEH.AdminEnabler);
            }
            while (Pointers.Input != STRINGS._COMMAND_EXIT);
            IOHandler.RunAutoEnd();
            Console.ReadKey();
        }
    }
}
