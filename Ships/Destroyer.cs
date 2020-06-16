using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsByKarlo.Ships
{
    public class Destroyer : Ship
    {
        public Destroyer()
        {
            Size = 4;
            LocationRows = new char[Size];
            LocationColumns = new char[Size];
            Legend = "DE";
            PlacingString = "Place destroyer (4 cells)";
        }
    }
}
