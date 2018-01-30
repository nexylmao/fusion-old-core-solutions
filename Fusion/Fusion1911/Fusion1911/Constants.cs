using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusion1911
{
    public static class CONSTANTS
    {
        public const uint LevelStart = 1;
        public const uint StartExp = 0;
        public const double ExperienceMultiplier = 1.0f;
        public const uint LevelCap = 60;
        public const byte StatsCount = 8;
        public const uint BaseHealth = 100;
        public const int GearSlots = 11;
        public const uint MaxPlayerSaves = 10;

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

        public static UInt64 Health(UInt64 survratin, UInt64 level)
        {
            UInt64 basehp = (UInt64)Math.Round(survratin * 1.5 * (level / 100 + 1) * ((level / 2) + 1) * ((level / 1000) + 1), 0);
            UInt64 levelhp = ((level - 1) * ((level % 65536) / 5 + 15 + ((level / 1000) * 8 * (level / 10000 + 1)) + ((level / 100) * 8)));
            UInt64 returer = (100 + basehp) + levelhp;
            return returer;
        }

        // this has to be done luul
        public static UInt64 BaseAttack()
        {
            return new ulong();
        }

        public static string HealthToString(UInt64 health)
        {
            string xd = "";

            if (health >= 0 && health <= 99999)
            {
                xd = Convert.ToString(health);
            }
            else if (health >= 100000 && health <= 99999999)
            {
                xd = Convert.ToString(health / 1000);
                xd += 'K';
            }
            else if (health >= 100000000 && health <= 99999999999)
            {
                xd = Convert.ToString(health / 1000000);
                xd += 'M';
            }
            //                 1 short of T      99.9999 of T       
            else if (health >= 100000000000 && health <= 99999999999999)
            {
                xd = Convert.ToString(health / 100000000000);
                if (health / 100000000000 < 10)
                {
                    if (((health % 100000000000) / 1000000000) < 10)
                    {
                        xd += '0';
                    }
                    xd += Convert.ToString((health % 100000000000) / 1000000000);
                }
                else if (health / 100000000000 < 100)
                {
                    xd += Convert.ToString((health % 100000000000) / 10000000000);
                }
                xd += 'B';
            }
            return xd;
        }
    }
}
