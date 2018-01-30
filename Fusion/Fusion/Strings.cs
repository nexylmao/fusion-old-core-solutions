using System.IO;
using System.Xml.Serialization;

namespace Fusion
{
    public static class STRINGS
    {
        public const string _LANGUAGEINFO = "enUS";
        public const string _GOOD = "Everything's fine!";

        public const string _SWITCHADMIN = "Admin mode switched! (AdminMode = {0})";
        public const string _NOFUNCTIONS = "No command like that!";
        public const string _EXITMESSAGE = "Bye :)";

        // databases
        public const string _DATABASE_PATH_CLASSLIST = "classlist.xyz";
        public const string _DATABASE_PATH_ITEMLIST = "itemlist.xyz";
        public const string _DATABASE_PATH_ABILITYLIST = "abilitylist.xyz";
        public const string _DATABASE_PATH_NPCLIST = "npclist.xyz";
        public const string _DATABASE_PATH_CHARACTERLIST = "character.xyz";
        public const string _DATABASE_PATH_PLAYERSAVES = "playersaves.xyz";

        // database returns
        public const string _GAMECREATE_GOOD_ADDTODB = "Successfully added element to {0} DB";
        public const string _GAMECREATE_BAD_ADDTODB = "Couldn't add element!";
        public const string _GAMECREATE_GOOD_REMOVEFROMDB = "Successfully removed element from {0} DB";
        public const string _GAMECREATE_BAD_REMOVEFROMDB = "Couldn't remove element!";
        public const string _GAMELOAD_GOOD_LOADED = "Successfully loaded {0} DB!";
        public const string _GAMELOAD_BAD_LOADED = "Couldn't load the DB!";
        public const string _GAMECREATE_GOOD_SAVED = "Successfully saved {0} DB!";
        public const string _GAMECREATE_BAD_SAVED = "Couldn't save the DB!";
        public const string _GAMECREATE_GOOD_SETPOINTER = "Successfully set to pointer!";
        public const string _GAMECREATE_BAD_SETPOINTER = "Couldn't set to pointer!";
        public const string _GAMECREATE_GOOD_EDITONINDEX = "Successfully set pointer to index!";
        public const string _GAMECREATE_BAD_EDITONINDEX = "Couldn't set pointer to index!";

        // database commands
        public const string _COMMAND_SAVEDB = "/Write{0}DB";
        public const string _COMMAND_LOADDB = "/Read{0}DB";
        public const string _COMMAND_ADD = "/AddTo{0}DB";
        public const string _COMMAND_REMOVE = "/RemoveFrom{0}DB";
        public const string _COMMAND_REMOVEAT = "/RemoveAt{0}DB";
        public const string _COMMAND_FIND = "/FindIn{0}DB";
        public const string _COMMAND_SET = "/SetOn{0}DB";

        public const string _GAMECREATE_GOOD_STATCHANGED = "Changed {0} to {1}!";
        public const string _GAMECREATE_BAD_COULDNTCHANGESTAT = "Stat couldn't be changed!";

        // Level messages
        public const string _MESSAGE_LEVELUP = "You've gained a level up!";
        public const string _MESSAGE_EXP = "You've gained {0} experience.";

        // Roll message
        public const string _MESSAGE_ROLL = "{0} just rolled a {1}!";

        // Damage messages
        public const string _COMBATLOG_DEALTDAMAGE = "{0} hit {3} for {1} damage ({2} absorbed).";
        public const string _COMBATLOG_HEALED = "{0} healed {1} for {2}.";
        public const string _COMBATLOG_CRITICALSTRIKE = "{0} lands a critical strike on {1}!";
        public const string _COMBATLOG_MISSED = "{0} has missed his target.";
        public const string _COMBATLOG_CONSUMABLE = "{0} has used {1}.";

        public const string _GAMECREATE_GOOD_CREATELOOTTABLE = "[Success] New loottable created";
        public const string _GAMECREATE_GOOD_ADDITEMLOOT = "Added item to loottable (with chance {0}%)!";
        public const string _GAMECREATE_BAD_ADDITEMLOOT = "Couldn't add item to loottable";
        public const string _GAMECREATE_GOOD_ITEMLOOTCHANCECHANGE = "Changed {0} chance to {1}%!";
        public const string _GAMECREATE_BAD_ITEMLOOTCHANCECHANGE = "Couldn't change item chance!";
        public const string _GAMECREATE_GOOD_DELETEITEMLOOT = "Deleted {0} from current LootTable";
        public const string _GAMECREATE_BAD_DELETEITEMLOOT = "Couldn't delete item!";
        public const string _GAMECREATE_GOOD_ADDEDCHARACTER = "Character successfully added to list!";
        public const string _GAMECREATE_BAD_ADDEDCHARACTER = "Couldn't add the character to the list!";

        public const string _GAMECREATE_ADDABILITY = "[Success] Added ability to database!";
        public const string _GAMECREATE_DIDNTADDABILITY = "[Failed] Couldn't add ability!";
        public const string _GAMECREATE_REMOVEABILITY = "[Success] Removed ability from list!";
        public const string _GAMECREATE_DIDNTREMOVEABILITY = "[Failed] Couldn't remove ability!";

        public const string _GAMECREATE_BAD_NPCNOABILITY = "[Error] The NPC doesn't have that ability!";

        public const string _GAMECREATE_GOOD_ADDABILITYTOSEQ = "[Success] Added ability to sequence!";
        public const string _GAMECREATE_BAD_ADDABILITYTOSEQ = "[Error] Couldn't add ability to sequence!";

        public const string _GAMECREATE_GOOD_REMOVEABILITY = "[Success] Removed {0} from sequence!";
        public const string _GAMECREATE_BAD_REMOVEABILITY = "[Error] Couldn't remove ability from sequence!";

        // Bags
        public const string _MESSAGE_BAD_EQUIPNULLITEM = "You're trying to equip an item you don't have.";
        public const string _MESSAGE_BAD_INTERNALBAGERROR = "Internal bag error.";
        public const string _MESSAGE_BAD_NOTCONSUMABLE = "Not a consumable";
        

        // Ability messages
        public const string _MESSAGE_BAD_NOPERMISSION = "You don't have permission to do that";
        public const string _MESSAGE_BAD_DEAD = "You're dead";
        public const string _MESSAGE_BAD_NOENERGY = "You don't have enough resources to cast the ability";
        public const string _MESSAGE_BAD_NOSELECTEDABILITY = "No ability to cast";
        public const string _MESSAGE_BAD_YOUDIED = "*knock knock* - who is it? DEATH LUL";
        public const string _MESSAGE_GOOD_YOUGOTRESS = "Come back you f*****!";

        // COMMANDS
        

        public const string _COMMAND_ADDITEMTOLIST = "/AddItemToList";
        public const string _COMMAND_ADDCLASSTOLIST = "/AddClassToList";
        public const string _COMMAND_ADDABILITYTOLIST = "/AddAbilityToList";
        public const string _COMMAND_FINDITEM = "/FindItem";
        public const string _COMMAND_FINDABILITY = "/FindAbility {0}";
        public const string _COMMAND_GETSTATNAME = "/GetStatName {0}";
        public const string _COMMAND_ROLL = "/Roll";
        public const string _COMMAND_ADDNPCTOLIST = "/AddNPCToList";
        public const string _COMMAND_WRITE_CHARACTERLIST = "/WriteCharacterList";
        public const string _COMMAND_READ_CHARACTERLIST = "/ReadCharacterList";
        public const string _COMMAND_ADDCHARACTERTOLIST = "/AddCharacterToList";
        public const string _COMMAND_EXIT = "/Quit";
        public const string _COMMAND_ADMIN = "/Admin";
        public const string _COMMAND_HELP = "/Help";
        public const string _COMMAND_CLEAR = "/Clear";

        public static void LoadStringPack()
        {

        }
        public static void SaveStringPack()
        {

        }
    }
}
