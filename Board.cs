using BattleshipsByKarlo.Interface;
using BattleshipsByKarlo.Ships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsByKarlo
{
    public class Board : IBoard
    {
        public string[,] Grid { get; set; }

        public readonly char[] Rows = { '0', '1', '2', '3', '4',
            '5', '6', '7', '8', '9' };
        public readonly char[] Columns = { 'A', 'B', 'C', 'D', 'E',
            'F', 'G', 'H', 'I', 'J' };

        public readonly char[] ValidRows = { '+', '+', '+', '+', '+',
            '+', '+', '+', '+', '+' };
        public readonly char[] ValidColumns = { '+', '+', '+', '+', '+',
            '+', '+', '+', '+', '+' };

        public short NumberOfShips { get; set; }
        public Destroyer Destroyer1 { get; set; }
        public Destroyer Destroyer2 { get; set; }
        public Battleship Battleship { get; set; }

        public Board()
        {
            NumberOfShips = 3;
            Destroyer1 = new Destroyer();
            Destroyer2 = new Destroyer();
            Battleship = new Battleship();
            Grid = new string[10, 10];

            for (int column = 0; column < 10; ++column)
                for (int row = 0; row < 10; ++row)
                    Grid[column, row] = ValidColumns[column].ToString() 
                        + ValidRows[row].ToString();
        }
        public void DisplayBoard()
        {
            Console.Write($"{Environment.NewLine}{Environment.NewLine}    ");
            foreach (char c in Columns)
                Console.Write($" {c} ");

            Console.WriteLine();

            for (int row = 0; row < 10; ++row)
            {
                Console.WriteLine();
                Console.Write($" {Rows[row]}   ");
                for (int column = 0; column < 10; ++column)
                    Console.Write($"{Grid[column, row]} ");
            }
        }
    }
}
