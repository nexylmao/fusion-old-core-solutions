using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusion
{
    public class InCombat
    {
        public Player PPointer;
        public NPC NPointer;
        public Character CPointer;
        public InCombat targetPointer;
        public Stats combatStats;
        public Buffs Buffs;
        public Debuffs Debuffs;
        public CombatField CombatField;
        public UInt64 Health, Energy;
        public bool Dead;
        public string Name;

        public InCombat()
        {
            targetPointer = null;
            CombatField = null;
        }

        public void AssignCharacter(object a)
        {
            if (a.GetType() == typeof(Character))
            {
                CPointer = (Character)a;
                CPointer.CharacterInCombat = this;
                Name = CPointer.Name;
            }
            else if (a.GetType() == typeof(Player))
            {
                PPointer = (Player)a;
                PPointer.CharacterInCombat = this;
                Name = PPointer.Name;
            }
            else if (a.GetType() == typeof(NPC))
            {
                NPointer = (NPC)a;
                NPointer.CharacterInCombat = this;
                Name = NPointer.Name;
            }
            targetPointer = null;
            CalculateStats();
            Health = combatStats.Values[6];
            Energy = combatStats.Values[7];
            Buffs = new Buffs();
            Debuffs = new Debuffs();
            Dead = false;
        }

        #region Functions
        public void CalculateStats()
        {
            if (PPointer == null)
            {
                if (combatStats == null)
                {
                    combatStats = new Stats();
                }
                NPointer.Stats.Values.CopyTo(combatStats.Values.ToArray());
                Health = NPointer.Stats.Values[6];
                Energy = NPointer.Stats.Values[7];
            }
            if (PPointer != null)
            {
                if (combatStats == null)
                {
                    combatStats = new Stats();
                }
                UInt64 sum;
                byte x = PPointer.Spec.mainStatID, y = PPointer.Spec.offStatID;
                float xm = PPointer.Spec.mainStatMod, ym = PPointer.Spec.offStatMod;
                for (int i = 0; i < Stats.ValuesCount; i++)
                {
                    // ITEMS AND CLASS BONUSES
                    sum = 0;
                    if (x == i)
                    {
                        sum += (uint)Math.Round(PPointer.Stats.Values[i] * xm);
                    }
                    else if (y == i)
                    {
                        sum += (uint)Math.Round(PPointer.Stats.Values[i] * ym);
                    }
                    else
                    {
                        sum += PPointer.Stats.Values[i];
                    }
                    //BUFFS
                    for (int k = 0; k < Buffs.BuffMax; k++)
                    {
                        sum += Buffs.__Buffs[k].StatMod.Values[i];
                    }
                    //DEBUFFS
                    for (int k = 0; k < Debuffs.DebuffMax; k++)
                    {
                        sum -= Debuffs.__Debuffs[k].StatMod.Values[i];
                    }
                    Buffs.MinusOne();
                    Debuffs.MinusOne();
                    combatStats.Values[i] = sum;
                }
                combatStats.Values[6] += CONSTANTS.Health(combatStats.Values[5], PPointer.Level.LevelValue);
                if (PPointer.Spec.MaxEnergy != 0)
                {
                    combatStats.Values[7] = PPointer.Spec.MaxEnergy;
                }
            }
        }
        public ReturnValue ChangeHealth(UInt64 amount, bool damage)
        {
            if (amount != 0)
            {
                if (damage)
                {
                    UInt64 newamount = amount;
                    newamount -= amount * (ulong)((combatStats.Values[5] / 2) * 0.01);
                    if (newamount > Health)
                    {
                        Health = 0;
                        CheckDeath();
                        return new ReturnValue(string.Format(STRINGS._COMBATLOG_DEALTDAMAGE, Name, newamount, amount - newamount, targetPointer.Name), STRINGS._COMBATLOG_DEALTDAMAGE);
                    }
                    else
                    {
                        Health -= newamount;
                        CheckDeath();
                        return new ReturnValue(string.Format(STRINGS._COMBATLOG_DEALTDAMAGE, Name, newamount, amount - newamount, targetPointer.Name), nameof(STRINGS._COMBATLOG_DEALTDAMAGE));
                    }
                }
                else
                {
                    if (amount > (combatStats.Values[6] - Health))
                    {
                        Health = combatStats.Values[6];
                        return new ReturnValue(string.Format(STRINGS._COMBATLOG_HEALED, Name, targetPointer.Name, amount), nameof(STRINGS._COMBATLOG_HEALED));
                    }
                    else
                    {
                        Health += amount;
                        return new ReturnValue(string.Format(STRINGS._COMBATLOG_HEALED, Name, targetPointer.Name, amount), nameof(STRINGS._COMBATLOG_HEALED));
                    }
                }
            }
            return new ReturnValue(string.Format(STRINGS._COMBATLOG_MISSED, Name), nameof(STRINGS._COMBATLOG_MISSED));
        }
        public bool ChangeEnergy(UInt64 amount, bool spending)
        {
            if (amount != 0 && amount <= Energy)
            {
                if (spending)
                {
                    Energy -= amount;
                }
                else
                {
                    if (amount > (combatStats.Values[7] - Energy))
                    {
                        Energy = combatStats.Values[7];
                    }
                    else
                    {
                        Energy += amount;
                    }
                }
                return true;
            }
            if (amount > Energy)
            {
                return false;
            }
            return true;
        }
        public void RegenEnergy()
        {
            if (PPointer != null)
            {
                if (PPointer.Spec.EnergyRegen < 0)
                {
                    Energy += (UInt64)Math.Round(combatStats.Values[7] * PPointer.Spec.EnergyRegen);
                }
                else
                {
                    Energy += (UInt64)Math.Round(PPointer.Spec.EnergyRegen);
                }
            }
        } // WORKS ONLY IF YOUR CHARACTER IS PLAYER, NOT AS NPC
        public ReturnValue CastAbility(Ability AbilityPointer)
        {
            if (!Dead && AbilityPointer.EnergyCost <= Energy)
            {
                ChangeEnergy(AbilityPointer.EnergyCost, true);
                return AbilityPointer.DoAbility(CombatField, this);
            }
            if (Dead)
            {
                return new ReturnValue(STRINGS._MESSAGE_BAD_DEAD, nameof(STRINGS._MESSAGE_BAD_DEAD));
            }
            if (!(AbilityPointer.EnergyCost <= Energy))
            {
                return new ReturnValue(STRINGS._MESSAGE_BAD_NOENERGY, nameof(STRINGS._MESSAGE_BAD_NOENERGY));
            }
            return new ReturnValue(STRINGS._MESSAGE_BAD_NOSELECTEDABILITY, nameof(STRINGS._MESSAGE_BAD_NOSELECTEDABILITY));
        }
        public void ChangeTarget(InCombat c)
        {
            targetPointer = c;
        }
        public ReturnValue UseConsumable(Item i)
        {
            if (i.IsConsumable && !Dead)
            {
                Ability x = new Ability();
                for (int j = 0; j < Stats.ValuesCount; j++)
                {
                    if (i.Stats.Values[j] != 0)
                    {
                        x.Stat = (byte)j;
                        x.Multiplier = i.Stats.Values[j];
                        x.Turns = 255;
                        break;
                    }
                }
                if (x.Turns != 255)
                {
                    return new ReturnValue(STRINGS._MESSAGE_BAD_NOTCONSUMABLE, nameof(STRINGS._MESSAGE_BAD_NOTCONSUMABLE));
                }
                Buffs.GiveBuff(x, new Stats { Values = new List<ulong> { 1, 1, 1, 1, 1, 1, 1, 1 } });
                return new ReturnValue(string.Format(STRINGS._COMBATLOG_CONSUMABLE, Name, i.Name), nameof(STRINGS._COMBATLOG_CONSUMABLE));
            }
            else if (Dead)
            {
                return new ReturnValue(STRINGS._MESSAGE_BAD_DEAD, nameof(STRINGS._MESSAGE_BAD_DEAD));
            }
            else
            {
                return new ReturnValue(STRINGS._MESSAGE_BAD_INTERNALBAGERROR, nameof(STRINGS._MESSAGE_BAD_INTERNALBAGERROR));
            }
        }
        public ReturnValue CheckDeath()
        {
            if (Health < 1)
            {
                Dead = true;
                Buffs = new Buffs();
                Debuffs = new Debuffs();

                return new ReturnValue(STRINGS._MESSAGE_BAD_YOUDIED, nameof(STRINGS._MESSAGE_BAD_YOUDIED));
            }
            return new ReturnValue(STRINGS._GOOD, nameof(STRINGS._GOOD));
        }
        public ReturnValue Resurrect()
        {
            if (Dead)
            {
                CalculateStats();
                Dead = false;
                return new ReturnValue(STRINGS._MESSAGE_GOOD_YOUGOTRESS, nameof(STRINGS._MESSAGE_GOOD_YOUGOTRESS));
            }
            return new ReturnValue(STRINGS._GOOD, nameof(STRINGS._GOOD));
        }
        #endregion
    }
}
