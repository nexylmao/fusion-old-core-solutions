using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fusion
{
    public class CombatField
    {
        public Fight Fight;
        public List<InCombat> Side_1;
        public List<InCombat> Side_2;
        public ConsoleColor Color_Side1, Color_Side2;
    }

    public class Fight
    {
        public CombatField Characters;
        public TurnHandler TurnHandler;
        public GameStarter GameStarter;
    }

    public class TurnHandler
    {

    }
}
