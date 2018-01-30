using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusion
{
    public class Player: Character
    {
        public Stats Stats;
        public Bag Bag;
        public GearSlots Gear;
        public List<Ability> Abilities;
        public PlayerClass Class;
        public playerSpec Spec;
        #region ABILITY_FUNCTIONS
        public List<Ability> Castables()
        {
            List<Ability> x = new List<Ability>();
            foreach (Ability y in Abilities)
            {
                if (y.LevelRequirment >= Level.LevelValue)
                {
                    Abilities.Add(y);
                }
            }
            return x;
        }
        #endregion
        #region SPEC_FUNCTIONS
        public void ChangeSpec(playerSpec newSpec)
        {
            // remove all abilities
            foreach (Ability x in Abilities)
            {
                if (Spec.AbilityList.Contains(x))
                {
                    Abilities.Remove(x);
                }
            }
            // add all new abilities
            foreach (Ability x in newSpec.AbilityList)
            {
                if (!Abilities.Contains(x))
                {
                    Abilities.Add(x);
                }
            }
            Spec = newSpec;
            Gear.ChangingSpecs(newSpec, Bag);
        }
        private void SetSpec(playerSpec firstSpec)
        {
            foreach (Ability x in firstSpec.AbilityList)
            {
                if (!Abilities.Contains(x))
                {
                    Abilities.Add(x);
                }
            }
            Spec = firstSpec;
            Gear.ChangeWeaponSlot((uint)firstSpec.gearSlots[CONSTANTS.GearSlots - 1]);
        }
        public void AssignClass(PlayerClass newClass)
        {
            Class = newClass;
            SetSpec(Class.Specs[0]);
        }
        // more like assign gender amirite?
        // fucking PC culture
        #endregion
        #region BAG_OR_GEAR_FUNCTIONS
        public ReturnValue EquipItem(Item i)
        {
            return Bag.EquipItem(i);
        }
        public ReturnValue DeequipItem(Item i)
        {
            return Bag.EquippedGear.DeequipItem(i);
        }
        public ReturnValue LootItem(Item i)
        {
            return Bag.LootItem(i);
        }
        public ReturnValue DestroyItem(Item i)
        {
            return Bag.DestroyItem(i);
        }
        public List<Item> SeeBag()
        {
            return Bag.ItemList;
        }
        #endregion
    }

    public class Player_Controller
    {
        public EventHandler EventHandler;
        public Player Player;

        public Dictionary<Action, string> Functions;

        public Player_Controller(EventHandler eh, Player p)
        {
            EventHandler = eh;
            Player = p;

            Functions = new Dictionary<Action, string>();         
        }

        #region Functions

        #endregion
    }

    public class PlayerSaves
    {
        public List<Player> Player_DB;
        public const uint MaxSaves = CONSTANTS.MaxPlayerSaves;

        public PlayerSaves()
        {
            Player_DB = new List<Player>((int)MaxSaves);
        }

        public ReturnValue LoadUp(uint x)
        {
            return null;
        }

        public ReturnValue SaveCurrent(uint x)
        {
            return null;
        }
    }

    public class PlayerSaves_EventHandler
    {
        public EventHandler EventHandler;
        public PlayerSaves PlayerSaves;

        public Dictionary<Action, string> Functions;

        public PlayerSaves_EventHandler(EventHandler eh, PlayerSaves ps)
        {
            EventHandler = eh;
            PlayerSaves = ps;

            Functions = new Dictionary<Action, string>();
        }

        #region Functions
        #endregion
    }
}
