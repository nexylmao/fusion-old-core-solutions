using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace Fusion1911
{
    public enum GearSlot { Head, Neck, Shoulder, Chest, Hands, Legs, Boots, Ring, Trinket, Relic, Bow, Wand, Staff, Sword, Axe, Dagger, Gun };

    // REPLACE STRING LIST
    public class Stats
    {
        public static byte ValuesCount = CONSTANTS.StatsCount;
        public List<UInt64> Values;
        public Stats NewStats(List<UInt64> lista)
        {
            if(lista.Count != ValuesCount)
            {
                return null;
            }
            Stats x = new Stats
            {
                Values = lista
            };
            return x;
        }
        public Return ChangeStat(UInt64 value, byte stat)
        {
            try
            {
                Values[stat] = value;
                string _GAMECREATE_GOOD_STATCHANGED = string.Format(STRINGS._GAMECREATE_GOOD_STATCHANGED, GetStatName(stat), value);
                return new Return(_GAMECREATE_GOOD_STATCHANGED, nameof(_GAMECREATE_GOOD_STATCHANGED));
            }
            catch
            {
                return new Return(STRINGS._GAMECREATE_BAD_COULDNTCHANGESTAT, nameof(STRINGS._GAMECREATE_BAD_COULDNTCHANGESTAT));
            }
        }
        public static string GetStatName(byte x)
        {
            if(x < ValuesCount)
            {
                return null;
            }
            else
            {
                List<string> StatNames = new List<string> { "Muscularity", "Steadiness", "Dexterity", "Intelligence", "Luckiness", "Survivability", "Maximum Health", "Maximum Energy" };
                return StatNames[x];
            }
        }
    }

    public class Item
    {
        public uint ID;
        public string Name, Description;
        public ConsoleColor Rarity;
        public bool IsGear, IsConsumable;
        public GearSlot Slot;
        public Stats Stats;
    }

    // port functions to player 

    public class GearSlots
    {
        public List<GearSlot> SlotTypes;
        public List<Item> Items;
        public static uint ItemsCount = CONSTANTS.GearSlots;
        public GearSlots()
        {
            SlotTypes = new List<GearSlot>((int)ItemsCount);
            Items = new List<Item>((int)ItemsCount);
        }
        #region GEARSLOT_ITEMING
        public Return EquipItem(Item i)
        {
            try
            {
                Items[SlotTypes.FindIndex(a => a == i.Slot)] = i;
                return new Return(STRINGS._GOOD, nameof(STRINGS._GOOD));
            }
            catch
            {
                return new Return(STRINGS._MESSAGE_BAD_INTERNALBAGERROR, nameof(STRINGS._MESSAGE_BAD_INTERNALBAGERROR));
            }
        }
        public Return DeequipItem(Item i)
        {
            try
            {
                Items[Items.FindIndex(a => a == i)] = new Item();
                return new Return(STRINGS._GOOD, nameof(STRINGS._GOOD));
            }
            catch
            {
                return new Return(STRINGS._MESSAGE_BAD_INTERNALBAGERROR, nameof(STRINGS._MESSAGE_BAD_INTERNALBAGERROR));
            }
        }
        public void ChangingSpecs(playerSpec ps, Bag b)
        {
            SlotTypes[CONSTANTS.GearSlots] = ps.gearSlots[CONSTANTS.GearSlots];
            if (Items[CONSTANTS.GearSlots - 1].Slot != SlotTypes[CONSTANTS.GearSlots - 1])
            {
                b.LootItem(Items[CONSTANTS.GearSlots]);
                DeequipItem(Items[CONSTANTS.GearSlots]);
            }
        }
        #endregion
        #region GEARSLOT_ADMIN_FUNCTIONS
        public void ChangeWeaponSlot(uint i)
        {
            if(i >= 10 && i <= 16)
            {
                SlotTypes[(int)ItemsCount - 1] = (GearSlot)i;
            }
        }
        public void CreateAndEquipItem(Item i)
        {
            EquipItem(i);
        }
        #endregion
    }

    public class Bag
    {
        public GearSlots EquippedGear;
        public List<Item> ItemList;
        #region BAG_FUNCTIONS
        public Return LootItem(Item i)
        {
            try
            {
                ItemList.Add(i);
                return new Return(STRINGS._GOOD, nameof(STRINGS._GOOD));
            }
            catch
            {
                return new Return(STRINGS._MESSAGE_BAD_INTERNALBAGERROR, nameof(STRINGS._MESSAGE_BAD_INTERNALBAGERROR));
            }
        }
        public Return DestroyItem(Item i)
        {
            try
            {
                ItemList.Remove(i);
                return new Return(STRINGS._GOOD, nameof(STRINGS._GOOD));
            }
            catch
            {
                return new Return(STRINGS._MESSAGE_BAD_INTERNALBAGERROR, nameof(STRINGS._MESSAGE_BAD_INTERNALBAGERROR));
            }
        }
        public Return EquipItem(Item i)
        {
            if (ItemList.Contains(i))
            {
                if (EquippedGear.Items.Any(x => x.Slot == i.Slot))
                {
                    for (int j = 0; j < EquippedGear.Items.Count; j++)
                    {
                        if (EquippedGear.Items[j].Slot == i.Slot)
                        {
                            LootItem(EquippedGear.Items[j]);
                            EquippedGear.DeequipItem(EquippedGear.Items[j]);
                            EquippedGear.EquipItem(i);
                            DestroyItem(i);
                            return new Return(STRINGS._GOOD, nameof(STRINGS._GOOD));
                        }
                    }
                    return new Return(STRINGS._MESSAGE_BAD_INTERNALBAGERROR, nameof(STRINGS._MESSAGE_BAD_INTERNALBAGERROR));
                }
                else
                {
                    EquippedGear.EquipItem(i);
                    DestroyItem(i);
                    return new Return(STRINGS._GOOD, nameof(STRINGS._GOOD));
                }
            }
            else
            {
                return new Return(STRINGS._MESSAGE_BAD_EQUIPNULLITEM, nameof(STRINGS._MESSAGE_BAD_EQUIPNULLITEM));
            }
        }
        #endregion
        #region ADMIN_BAG_FUNCTIONS
        public void AddItemToBag(Item i)
        {
            LootItem(i);
        }
        #endregion
    }

    public class LootTable
    {
        public List<Item> Items;
        public List<float> Chances;
        public LootTable()
        {
            Items = new List<Item>();
            Chances = new List<float>();
        }
        public Return AddItem(Item item, float chance)
        {
            try
            {
                Items.Add(item);
                Chances.Add(chance);
                string _GAMECREATE_GOOD_ADDITEMLOOT = string.Format(STRINGS._GAMECREATE_GOOD_ADDITEMLOOT, chance);
                return new Return(_GAMECREATE_GOOD_ADDITEMLOOT,nameof(_GAMECREATE_GOOD_ADDITEMLOOT));
            }
            catch
            {
                return new Return(STRINGS._GAMECREATE_BAD_ADDITEMLOOT, nameof(STRINGS._GAMECREATE_BAD_ADDITEMLOOT));
            }
        }
        public Return ChangeItemChance(Item item, float newchance)
        {
            try
            {
                int l = Items.IndexOf(item);
                Chances[l] = newchance;
                string _GAMECREATE_BAD_ITEMLOOTCHANCECHANGE = string.Format(STRINGS._GAMECREATE_BAD_ITEMLOOTCHANCECHANGE, item.Name, newchance);
                return new Return(_GAMECREATE_BAD_ITEMLOOTCHANCECHANGE, nameof(_GAMECREATE_BAD_ITEMLOOTCHANCECHANGE));
            }
            catch
            {
                return new Return(STRINGS._GAMECREATE_BAD_ITEMLOOTCHANCECHANGE, nameof(STRINGS._GAMECREATE_BAD_ITEMLOOTCHANCECHANGE));
            }
        }
        public Return DeleteItem(Item item)
        {
            try
            {
                int index = Items.IndexOf(item);
                Items.RemoveAt(index);
                Chances.RemoveAt(index);
                string _GAMECREATE_GOOD_DELETEITEMLOOT = string.Format(STRINGS._GAMECREATE_GOOD_DELETEITEMLOOT, item.Name);
                return new Return(_GAMECREATE_GOOD_DELETEITEMLOOT, nameof(_GAMECREATE_GOOD_DELETEITEMLOOT));
            }
            catch
            {
                return new Return(STRINGS._GAMECREATE_BAD_DELETEITEMLOOT, nameof(STRINGS._GAMECREATE_BAD_DELETEITEMLOOT));
            }
        }
        public List<Item> DropLoot(uint num = 0)
        {
            List<Item> DroppedItem = new List<Item>();
            if(num != 0)
            {
                uint x = 0;
                while(x <= num)
                {
                    foreach(Item i in Items)
                    {
                        int index = Items.IndexOf(i);
                        if(Chances[index] > DamageDice.NumberRoll())
                        {
                            DroppedItem.Add(i);
                            x++;
                        }
                    }
                }
            }
            else
            {
                foreach(Item i in Items)
                {
                    int index = Items.IndexOf(i);
                    if (Chances[index] > DamageDice.NumberRoll())
                    {
                        DroppedItem.Add(i);
                    }
                }
            }
            return DroppedItem;
        }
    }

    // 
}
