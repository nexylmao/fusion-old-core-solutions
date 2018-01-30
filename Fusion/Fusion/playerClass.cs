using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace Fusion
{
    public class playerSpec
    {
        public uint ID;
        public string Name;
        public ConsoleColor Color;
        public byte mainStatID;
        public float mainStatMod;
        public byte offStatID;
        public float offStatMod;
        public List<GearSlot> gearSlots;
        public UInt64 MaxEnergy;
        public double EnergyRegen;
        public List<Ability> AbilityList;
        public PlayerClass Class;
    }

    public class PlayerClass
    {
        public uint ID;
        public string Name;
        public List<playerSpec> Specs;
    }
}
