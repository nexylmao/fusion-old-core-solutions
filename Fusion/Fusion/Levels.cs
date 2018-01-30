using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusion
{
    public class Levels
    {
        public const uint DefaultMaxLevel = CONSTANTS.LevelCap;
        public uint LevelValue;
        public UInt64 Experience;
        public double ExperienceMultiplier;
        public static uint MaxLevel;
        public Levels(uint Max = DefaultMaxLevel)
        {
            LevelValue = CONSTANTS.LevelStart;
            Experience = CONSTANTS.StartExp;
            ExperienceMultiplier = CONSTANTS.ExperienceMultiplier;
            MaxLevel = Max;
        }
        #region GENERAL_PURPOSE_FUNCTIONS
        public Levels()
        {
            LevelValue = 1;
            Experience = 0;
            ExperienceMultiplier = 1;
        }
        public ReturnValue CheckLevelUp()
        {
            UInt64 expforlvlup = (UInt64)Math.Round(LevelValue * 100 * (100 / (float)LevelValue) * (1000 / (float)LevelValue));
            if (Experience >= expforlvlup && LevelValue < MaxLevel)
            {
                LevelValue++;
                Experience -= expforlvlup;
                return new ReturnValue(STRINGS._MESSAGE_LEVELUP,nameof(STRINGS._MESSAGE_LEVELUP));
            }
            if(LevelValue == MaxLevel)
            {
                LevelValue = MaxLevel;
                Experience = 0;
            }
            return new ReturnValue(STRINGS._GOOD,nameof(STRINGS._GOOD));
        }
        public ReturnValue RewardExperience(UInt64 a)
        {
            if(a != 0 && LevelValue < MaxLevel)
            {
                Experience += (UInt64)Math.Round(a * ExperienceMultiplier);
                CheckLevelUp();
                string _MESSAGE_EXP = string.Format(STRINGS._MESSAGE_EXP, Math.Round(a * ExperienceMultiplier));
                return new ReturnValue(_MESSAGE_EXP,nameof(_MESSAGE_EXP));
            }
            if(LevelValue == MaxLevel)
            {
                Experience = 0;
            }
            return new ReturnValue(STRINGS._GOOD,nameof(STRINGS._GOOD));
        }
        public void AddToMultiplier(double x)
        {
            ExperienceMultiplier += x;
        }
        #endregion 
        #region ADMIN_LEVEL_FUNCTIONS
        public void SetLevel(uint a)
        {
            LevelValue = a;
        }
        public void AddLevel(uint a)
        {
            LevelValue += a;
        }
        public void SetExperience(uint a)
        {
            Experience = a;
        }
        public void AddExperience(uint a)
        {
            Experience += a;
        }
        public void SetMultiplier(uint a)
        {
            ExperienceMultiplier = (double)a / 100;
        }
        public void SetMaximumLevel(uint a)
        {
            MaxLevel = a;
        }
        #endregion
    }
}
