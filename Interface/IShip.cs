using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsByKarlo.Interface
{
    public interface IShip
    {
        bool ValidateStartPosition(Position position,
            char[] rows, char[] columns, string[,] grid);
        bool ValidateOrientation(char orientation);
        bool ValidateCellContents(string candidateCellContents);

        int Size { get; set; }
        bool IsDestroyed { get; set; }

        string Legend { get; set; }
        string PlacingString { get; set; }

        char[] LocationRows { get; set; }
        char[] LocationColumns { get; set; }

    }
}
