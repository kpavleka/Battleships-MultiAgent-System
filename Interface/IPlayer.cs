using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsByKarlo.Interface
{
    interface IPlayer
    {
        string Name { get; set; }

        int shipLocationCounter { get; set; }
        int NumberOfShipsDestroyed { get; set; }

        bool IsWinner { get; set; }
        bool IsCurrentTurn { get; set; }

    }
}
