using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsByKarlo.Ships
{
    public class Battleship : Ship
    {
        public Battleship()
        {
            Size = 5;
            Legend = "BS";
            PlacingString = "Place battleship (5 cells)";
            LocationRows = new char[Size];
            LocationColumns = new char[Size];
        }
    }
}
