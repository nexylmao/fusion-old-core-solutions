using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Serialization;

namespace Fusion
{
    public class Character
    {
        public InCombat CharacterInCombat;
        public uint ID;
        public Levels Level;
        public string Name, Title;
        public ConsoleColor TitleColor;

        public Character()
        {
            CharacterInCombat = null;
            Level = new Levels();
            Name = string.Empty;
            Title = string.Empty;
            TitleColor = ConsoleColor.Black;
        }

        #region Character_Functions
        public void SetName(string newName)
        {
            Name = newName;
        }
        public void SetTitle(string newTitle)
        {
            Title = newTitle;
        }
        public void SetTitleColor(ConsoleColor newColor)
        {
            TitleColor = newColor;
        }
        public void ToCombat()
        {
            CharacterInCombat = new InCombat();
            CharacterInCombat.AssignCharacter(this);
        }
        #endregion
        #region LEVEL_FUNCTIONS
        public ReturnValue CheckLevelUp()
        {
            return Level.CheckLevelUp();
        }
        public ReturnValue RewardExperience(UInt64 a)
        {
            return Level.RewardExperience(a);
        }
        public void AddToMultiplier(double x)
        {
            Level.AddToMultiplier(x);
        }
        #endregion
        #region ADMIN_LEVEL_FUNCTIONS
        public void SetLevel(uint a)
        {
            Level.SetLevel(a);
        }
        public void AddLevel(uint a)
        {
            Level.AddLevel(a);
        }
        public void SetExperience(uint a)
        {
            Level.SetExperience(a);
        }
        public void AddExperience(uint a)
        {
            Level.AddExperience(a);
        }
        public void SetMultiplier(uint a)
        {
            Level.SetMultiplier(a);
        }
        #endregion
    }

    public class CharacterController
    {

    }
}
