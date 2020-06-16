using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsByKarlo
{
    public struct Input
    {
        public char Row;
        public char Column;
    };

    public class Player
    {
        public string Name { get; set; }

        public int shipLocationCounter { get; set; }
        public int NumberOfShipsDestroyed { get; set; }

        public bool IsWinner { get; set; }
        public bool IsCurrentTurn { get; set; }

        public Input Input;
        public Board Board;

        public Player()
        {
            Input.Row = ' ';
            Input.Column = ' ';
            Board = new Board();
            shipLocationCounter = 0;
            NumberOfShipsDestroyed = 0;
        }
    }
}
