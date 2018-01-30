using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace Fusion
{    
    public enum Ability_Type { ST_D, AOE_D, ST_DOT, ST_HEAL, AOE_HEAL, BUFF, DEBUFF, RESS };
    public enum Rolled { Miss = 0, Normal = 1, HalfCrit = 2, FullCrit = 3};
    public class Ability
    {
        public uint ID, LevelRequirment;
        public string Name;
        public Ability_Type Type;
        public UInt64 EnergyCost;
        public byte Stat, Turns, Cooldown;
        public double Multiplier;
        public ReturnValue DoAbility(CombatField field, InCombat caster)
        {
            if(this == null)
            {
                return new ReturnValue(STRINGS._MESSAGE_BAD_NOSELECTEDABILITY, nameof(STRINGS._MESSAGE_BAD_NOSELECTEDABILITY));
            }
            if(caster.Dead)
            {
                return new ReturnValue(STRINGS._MESSAGE_BAD_DEAD, nameof(STRINGS._MESSAGE_BAD_DEAD));
            }
            UInt64 x = (UInt64)Math.Round(caster.combatStats.Values[Stat] * Multiplier);
            if(!(caster.ChangeEnergy(EnergyCost,true)))
            {
                return new ReturnValue(STRINGS._MESSAGE_BAD_NOENERGY, nameof(STRINGS._MESSAGE_BAD_NOENERGY));
            }
            DamageDice d;
            if (Type == Ability_Type.AOE_HEAL || Type == Ability_Type.ST_HEAL)
            {
                d = new DamageDice(-1, (uint)(caster.combatStats.Values[4]) % 101, (uint)(caster.combatStats.Values[4]) % 101);
            }
            else
            {
                d = new DamageDice(5, (uint)(caster.combatStats.Values[4]) % 51, (uint)(caster.combatStats.Values[4]) % 101);
            }
            x *= (uint)d.Roll();
            switch (Type)
            {
                case Ability_Type.ST_D: caster.targetPointer.ChangeHealth(x, true);
                    break;
                case Ability_Type.AOE_D:
                    if(field.Side_1.Contains(caster.targetPointer))
                    {
                        for(int i = 0; i < field.Side_1.Count; i++)
                        {
                            field.Side_1[i].ChangeHealth(x, true);
                        }
                    }
                    else if(field.Side_2.Contains(caster.targetPointer))
                    {
                        for(int i = 0; i < field.Side_2.Count; i++)
                        {
                            field.Side_2[i].ChangeHealth(x, true);
                        }
                    }
                    break;
                case Ability_Type.ST_DOT:caster.targetPointer.Debuffs.GiveDebuff(this, caster.combatStats);
                    break;
                case Ability_Type.ST_HEAL: caster.targetPointer.ChangeHealth(x, false);
                    break;
                case Ability_Type.AOE_HEAL:
                    if (field.Side_1.Contains(caster.targetPointer))
                    {
                        for (int i = 0; i < field.Side_1.Count; i++)
                        {
                            field.Side_1[i].ChangeHealth(x, false);
                        }
                    }
                    else if (field.Side_2.Contains(caster.targetPointer))
                    {
                        for (int i = 0; i < field.Side_2.Count; i++)
                        {
                            field.Side_2[i].ChangeHealth(x, false);
                        }
                    }
                    break;
                case Ability_Type.BUFF:
                    caster.targetPointer.Buffs.GiveBuff(this, caster.combatStats);
                    break;
                case Ability_Type.DEBUFF:
                    caster.targetPointer.Debuffs.GiveDebuff(this, caster.combatStats);
                    break;
                case Ability_Type.RESS:
                    caster.targetPointer.Resurrect();
                    break;
            }
            for(int i = 0; i < field.Side_1.Count; i++)
            {
                field.Side_1[i].CheckDeath();
            }
            for (int i = 0; i < field.Side_2.Count; i++)
            {
                field.Side_2[i].CheckDeath();
            }
            return new ReturnValue(STRINGS._GOOD, nameof(STRINGS._GOOD));
        }
    }

    public class Buff
    {
        public Stats StatMod;
        public byte BuffLasts;
    }

    public class Buffs
    {
        public static int BuffMax;
        public List<Buff> __Buffs;
        public void GiveBuff(Ability a, Stats c)
        {
            if (a.Type == Ability_Type.BUFF && __Buffs.Count < BuffMax)
            {
                Buff d = new Buff();
                d.StatMod.Values[a.Stat] = (uint)Math.Round(a.Multiplier * c.Values[a.Stat]);
                d.BuffLasts = a.Turns;
                __Buffs.Add(d);
            }
        }
        public void MinusOne()
        {
            for (int i = 0; i < __Buffs.Count; i++)
            {
                __Buffs[i].BuffLasts--;
            }
            for (int i = 0; i < __Buffs.Count; i++)
            {
                if (__Buffs[i].BuffLasts < 1)
                {
                    __Buffs.Remove(__Buffs[i]);
                }
            }
        }
    }

    public class Debuff
    {
        public Stats StatMod;
        public byte DebuffLasts;
    }

    public class Debuffs
    {
        public static int DebuffMax;
        public List<Debuff> __Debuffs;
        public void GiveDebuff(Ability y, Stats c)
        {
            if (y.Type == Ability_Type.DEBUFF)
            {
                Debuff d = new Debuff();
                d.StatMod.Values[y.Stat] = (uint)Math.Round(y.Multiplier * c.Values[y.Stat]);
                d.DebuffLasts = y.Turns;
                __Debuffs.Add(d);
            }
            if(y.Type == Ability_Type.ST_DOT)
            {
                Debuff d = new Debuff();
                d.StatMod.Values[6] = (uint)Math.Round(y.Multiplier * c.Values[y.Stat]);
                d.DebuffLasts = y.Turns;
                __Debuffs.Add(d);
            }
        }
        public void MinusOne()
        {
            for (int i = 0; i < __Debuffs.Count; i++)
            {
                __Debuffs[i].DebuffLasts--;
            }
            for (int i = 0; i < __Debuffs.Count; i++)
            {
                if (__Debuffs[i].DebuffLasts < 1)
                {
                    __Debuffs.Remove(__Debuffs[i]);
                }
            }
        }
    }

    public class DamageDice
    {
        public int MissValue;
        public uint HalfCritValue;
        public uint FullCritValue;
        private static int min = 0, max = 100;
        public Random Randomizer;

        public DamageDice(int MissValue = -1, uint HalfCritValue = 0, uint FullCritValue = 0)
        {
            this.MissValue = MissValue;
            this.HalfCritValue = HalfCritValue;
            this.FullCritValue = FullCritValue;
            if(this.HalfCritValue + this.FullCritValue > max)
            {
                this.HalfCritValue = (uint)max - this.FullCritValue;
            }
            if((this.MissValue + this.HalfCritValue + this.FullCritValue) > max)
            {
                this.MissValue = max - (int)(this.HalfCritValue + this.FullCritValue);
            }
        }

        public Rolled Roll()
        {
            Randomizer = new Random(DateTime.Now.Millisecond);
            int rolled = Randomizer.Next(min, max);
            if(rolled <= MissValue)
            {
                return Rolled.Miss;
            }
            if(rolled >= max - HalfCritValue && rolled < max - FullCritValue)
            {
                return Rolled.HalfCrit;
            }
            if(rolled >= max - FullCritValue)
            {
                return Rolled.FullCrit;
            }
            return Rolled.Normal;
        }

        public static int NumberRoll()
        {
            Random Randomizer = new Random(DateTime.Now.Millisecond);
            Randomizer = new Random(DateTime.Now.Millisecond);
            return Randomizer.Next(min, max);
        }
    }
}
