﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace Fusion
{
    public class NPC : Character
    {
        public new uint ID;
        public Stats Stats;
        public List<Ability> Abilities;
        public LootTable LootTable;
        public AbilitySequencer AbilitySequencer;

        public NPC()
        {
            Stats = new Stats();
            Abilities = new List<Ability>();
            LootTable = new LootTable();
            AbilitySequencer = new AbilitySequencer();
            AbilitySequencer.AssignNPC(this);
        }

        #region ADMIN_FUNCTIONS
        // Stats
        public ReturnValue ChangeStat(byte stat, UInt64 value)
        {
            return Stats.ChangeStat(value, stat);
        }

        // Abilities
        public ReturnValue AbilityAdd(Ability a)
        {
            try
            {
                Abilities.Add(a);
                return new ReturnValue(STRINGS._GAMECREATE_ADDABILITY, nameof(STRINGS._GAMECREATE_ADDABILITY));
            }
            catch
            {
                return new ReturnValue(STRINGS._GAMECREATE_DIDNTADDABILITY, nameof(STRINGS._GAMECREATE_DIDNTADDABILITY));
            }
        }
        public ReturnValue AbilityRemove(Ability a)
        {
            try
            {
                Abilities.Remove(a);
                return new ReturnValue(STRINGS._GAMECREATE_REMOVEABILITY, nameof(STRINGS._GAMECREATE_REMOVEABILITY));
            }
            catch
            {
                return new ReturnValue(STRINGS._GAMECREATE_DIDNTREMOVEABILITY, nameof(STRINGS._GAMECREATE_DIDNTREMOVEABILITY));
            }
        }

        // AbilitySequencer
        public ReturnValue AbilityAddSequence(Ability a)
        {
            if(Abilities.Contains(a))
            {
                return AbilitySequencer.AddAbility(a);
            }
            else
            {
                return new ReturnValue(STRINGS._GAMECREATE_BAD_NPCNOABILITY, nameof(STRINGS._GAMECREATE_BAD_NPCNOABILITY));
            }
        }
        public ReturnValue RemoveAbilitySequence(int index)
        {
            return AbilitySequencer.RemoveAbility(index);
        }
        public Ability GetNextAbility()
        {
            return AbilitySequencer.NextForCast();
        }

        // LootTable
        public List<Item> DropLoot(uint num = 0)
        {
            return LootTable.DropLoot(num);
        }
        public ReturnValue AddItem(Item item, float chance)
        {
            return LootTable.AddItem(item, chance);
        }
        public ReturnValue ChangeItemChance(Item item, float newchance)
        {
            return LootTable.ChangeItemChance(item, newchance);
        }
        public ReturnValue DeleteItem(Item item)
        {
            return LootTable.DeleteItem(item);
        }
        #endregion
    }

    // empty temp, finish first up
    public class NPC_Controller
    {
        public EventHandler EventHandler;
        public NPC NPC;

        public Dictionary<Action, string> Functions;

        public NPC_Controller(EventHandler eh, NPC np)
        {
            EventHandler = eh;
            NPC = np;

            Functions = new Dictionary<Action, string>();
        }

        #region Functions

        #endregion
    }

    public class AbilitySequencer
    {
        private NPC Npc;
        private List<Ability> Abilities;
        private int Counter;

        public AbilitySequencer()
        {
            Abilities = new List<Ability>();
            Counter = 0;
        }

        public void AssignNPC(NPC n)
        {
            Npc = n;
        }

        public ReturnValue AddAbility(Ability XD)
        {
            try
            {
                Abilities.Add(XD);
                return new ReturnValue(STRINGS._GAMECREATE_GOOD_ADDABILITYTOSEQ, nameof(STRINGS._GAMECREATE_GOOD_ADDABILITYTOSEQ));
            }
            catch
            {
                return new ReturnValue(STRINGS._GAMECREATE_BAD_ADDABILITYTOSEQ, nameof(STRINGS._GAMECREATE_BAD_ADDABILITYTOSEQ));
            }
        }

        public ReturnValue RemoveAbility(int index)
        {
            try
            {
                Ability x = Abilities[index];
                string _GAMECREATE_GOOD_REMOVEABILITY = string.Format(STRINGS._GAMECREATE_GOOD_REMOVEABILITY, x.Name);
                Abilities.Remove(x);
                return new ReturnValue(_GAMECREATE_GOOD_REMOVEABILITY, nameof(_GAMECREATE_GOOD_REMOVEABILITY));
            }
            catch
            {
                return new ReturnValue(STRINGS._GAMECREATE_BAD_REMOVEABILITY, nameof(STRINGS._GAMECREATE_BAD_REMOVEABILITY));
            }
        }

        public Ability NextForCast()
        {
            try
            {
                if(Counter + 1 == Abilities.Count)
                {
                    Counter = 0;
                }
                return Abilities[Counter++];
            }
            catch
            {
                return null;
            }
        }
    }
}
