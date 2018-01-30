using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Item
{
    public class Item
    {
        public string ItemName;
        public int ItemID;
        public string ItemDesc;

        public Item()
        {

        }

        public Item(string Name, string Desc, int ID)
        {
            ItemName = Name;
            ItemDesc = Desc;
            ItemID = ID;
        }
    }
}
