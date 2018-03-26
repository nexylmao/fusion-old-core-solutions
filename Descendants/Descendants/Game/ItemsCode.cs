using System;
using System.Collections.Generic;
using System.Text;

namespace Descendants.Game
{
    public struct Resource
    {
        public string Name;
        public ConsoleColor Color;
        public double RegenRate;
        public uint Cap;
    }

    public class CombatStats
    {
        #region Static-part
        public static readonly Dictionary<string, Resource> ResourceInfos = new Dictionary<string, Resource>()
        {
            // 1st faction
            {"Knight",new Resource()
            {
                Name = "Energy",
                Color = ConsoleColor.Cyan,
                RegenRate = 0,
                Cap = 100
            } },
            {"Gunner",new Resource()
            {
                Name = "Focus",
                Color = ConsoleColor.Red,
                RegenRate = 0.1,
                Cap = 100
            } },
            {"Assassin",new Resource()
            {
                Name = "Energy",
                Color = ConsoleColor.Yellow,
                RegenRate = 0.1,
                Cap = 100
            } },
            {"Engineer",new Resource()
            {
                Name = "Scrap",
                Color = ConsoleColor.Blue,
                RegenRate = 0.25,
                Cap = 500
            } },
            {"Medic",new Resource()
            {
                Name = "Mana",
                Color = ConsoleColor.Blue,
                RegenRate = 0.05,
                Cap = 0
            } },
            // 2nd faction
            {"Druid",new Resource()
            {
                Name = "Mana",
                Color = ConsoleColor.Blue,
                RegenRate = 0.05,
                Cap = 0
            } },
            {"Elementalist",new Resource()
            {
                Name = "Sparks",
                Color = ConsoleColor.Green,
                RegenRate = 1,
                Cap = 1000
            } },
            {"Mage",new Resource()
            {
                Name = "Mana",
                Color = ConsoleColor.Blue,
                RegenRate = 0.05,
                Cap = 0
            } },
            {"Feral",new Resource()
            {
                Name = "Combo",
                Color = ConsoleColor.Red,
                RegenRate = 0,
                Cap = 2
            } }
        };
        public int ClassCount
        {
            get
            {
                return ResourceInfos.Count;
            }
        }
        public static UInt64 BaseHP(UInt64 survratin, UInt64 level)
        {
            UInt64 basehp = (UInt64)Math.Round(survratin * 1.5 * (level / 100 + 1) * ((level / 2) + 1) * ((level / 1000) + 1), 0);
            return basehp;
        }
        public static UInt64 LevelHP(UInt64 survratin, UInt64 level)
        {
            UInt64 levelhp = ((level - 1) * ((level % 65536) / 5 + 15 + ((level / 1000) * 8 * (level / 10000 + 1)) + ((level / 100) * 8)));
            return levelhp;
        }
        public static UInt64 CalculateHP(UInt64 survratin, UInt64 level)
        {
            UInt64 basehp = BaseHP(survratin, level);
            UInt64 levelhp = LevelHP(survratin, level);
            UInt64 returer = (100 + basehp) + levelhp;
            return returer;
        }
        public static UInt64 BaseEnergy(UInt64 energystat, UInt64 level)
        {
            UInt64 basehp = (energystat * 2 * (level / 100 + 1) * ((level / 2) + 1) * ((level / 1000) + 1));
            return basehp;
        }
        public static UInt64 LevelEnergy(UInt64 energystat, UInt64 level)
        {
            UInt64 levelhp = ((level - 1) * ((level % 65536) / 5 + 9 + ((level / 1000) * 4 * (level / 10000 + 1)) + ((level / 100) * 5)));
            return levelhp;
        }
        public static UInt64 CalculateEnergy(UInt64 energystat, UInt64 level, UInt64 cap = 0)
        {
            if(cap == 0)
            {
                return cap;
            }
            else
            {
                return (100 + BaseEnergy(energystat, level)) + LevelEnergy(energystat, level);
            }
        }
        #endregion
        #region Properties
        public string ClassName
        {
            get
            {
                return PickedClassName;
            }
        }
        public Resource ClassResource
        {
            get
            {
                return ClassResourceInfo;
            }
        }
        public double HealthPercentage
        {
            get
            {
                return Math.Round((double)CurrHealth / MaxHealth, 2);
            }
        }
        public double EnergyPercentage
        {
            get
            {
                return MaxEnergy / CurrEnergy;
            }
        }
        #endregion
        #region Fields
        private string PickedClassName;
        private Resource ClassResourceInfo;
        public UInt16 Level;
        public UInt32 Experience;
        public UInt64 MaxHealth, CurrHealth;
        public UInt32 MaxEnergy, CurrEnergy;
        public Stats Stats;
        #endregion
        public CombatStats()
        {
            PickedClassName = ((ResourceInfos.Keys as ICollection<string>) as string[])[0];
            ClassResourceInfo = ResourceInfos[PickedClassName];
            Level = 1;
            Experience = 0;
            Stats = new Stats(new int[] {0,0,0,0,0,0});
            MaxHealth = CalculateHP((ulong)Stats[5], Level);
            MaxEnergy = CalculateEnergy((ulong)Stats[4], Level, Convert.ToUInt64(ClassResourceInfo.Cap));
        }
    }

    public struct StatInfo
    {
        public string FullName, ShortName;
        public int Cap;
    }

    public class Stats
    {
        #region Static-part
        public static readonly Dictionary<string, StatInfo> StatInfos = new Dictionary<string, StatInfo>()
        {
            {"Strength", new StatInfo
            {
                FullName = "Strength",
                ShortName = "STR",
                Cap = 0
            } },
            {"Steadiness", new StatInfo
            {
                FullName = "Steadiness",
                ShortName = "STED",
                Cap = 0
            } },
            {"Dexterity", new StatInfo
            {
                FullName = "Dexterity",
                ShortName = "DEXT",
                Cap = 0
            } },
            {"Intelligence", new StatInfo
            {
                FullName = "Intelligence",
                ShortName = "INTL",
                Cap = 0
            } },
            {"Survivability", new StatInfo
            {
                FullName = "Survivability",
                ShortName = "SURV",
                Cap = 100
            } },
            {"Luck", new StatInfo
            {
                FullName = "Luck",
                ShortName = "LUCK",
                Cap = 100
            } }
        };
        public static int StatCount
        {
            get
            {
                return StatInfos.Count;
            }
        }
        #endregion
        #region Operators
        public static Stats operator+(Stats a, Stats b)
        {
            int[] values = new int[StatCount];
            for(int i = 0; i < StatCount; i++)
            {
                values[i] = a[i] + b[i];
            }
            return new Stats(values);
        }
        public static Stats operator*(Stats a, int b)
        {
            int[] values = new int[StatCount];
            for(int i = 0; i < StatCount; i++)
            {
                values[i] = a[i] * b;
            }
            return new Stats(values);
        }
        public override bool Equals(object obj)
        {
            return this == obj;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public static bool operator==(Stats a, Stats b)
        {
            for(int i = 0; i < StatCount; i++)
            {
                if(a[i] != b[i])
                {
                    return false;
                }
            }
            return true;
        }
        public static bool operator!=(Stats a, Stats b)
        {
            return !(a == b);
        }
        #endregion

        #region Fields
        int[] values;
        bool readOnly;
        #endregion
        #region Properties
        public int this[int index]
        {
            get
            {
                if(values[index] < 0)
                {
                    return 0;
                }
                return values[index];
            }
            set
            {
                if(!readOnly)
                {
                    values[index] = value;
                }
            }
        }
        public bool isReadOnly
        {
            get
            {
                return readOnly;
            }
            set
            {
                readOnly = value;
            }
        }
        #endregion
        #region Constructors
        public Stats()
        {
            readOnly = false;
            values = new int[StatCount];
        }
        public Stats(bool readOnly)
        {
            this.readOnly = readOnly;
            values = new int[StatCount];
        }
        public Stats(int[] values)
        {
            readOnly = true;
            this.values = new int[StatCount];
            values.CopyTo(this.values, Math.Min(values.Length, StatCount));
        }
        public Stats(int[] values, bool readOnly)
        {
            this.readOnly = readOnly;
            this.values = new int[StatCount];
            values.CopyTo(this.values, Math.Min(values.Length, StatCount));
        }
        #endregion
    }

    
}
