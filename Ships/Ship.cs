using BattleshipsByKarlo.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsByKarlo
{
    public struct Position
    {
        public char Row;
        public char Column;
    };

    public struct Point
    {
        public int Row;
        public int Column;
    }

    public class Ship : IShip
    {
        public int Size { get; set; }
        public bool IsDestroyed { get; set; }
        public Orientations Orientation { get; set; }

        public Position EndPosition;
        public Position StartPosition;

        public string Legend { get; set; }
        public string PlacingString { get; set; }

        // To keep track of the location of ship location
        public char[] LocationRows { get; set; }
        public char[] LocationColumns { get; set; }

        public Ship()
        {
            Orientation = Orientations.Horizontal;
        }

        public enum Orientations
        {
            Vertical,
            Horizontal
        }

        public bool ValidateOrientation(char orientation)
        {
            return orientation == '1' || orientation == '2';
        }
        public string GetInvalidInputText()
        {
            return $"Invalid input. Try again";
        }

        public bool ValidateCellContents(string candidateCellContents)
        {
            bool isValid = false;
             isValid =  !(candidateCellContents == "BS" 
                || candidateCellContents == "DE");
            return isValid;
        }

        public bool ValidateStartPosition(Position position,
            char[] rows, char[] columns, string[,] grid)
        {
            bool isValid = false;
            isValid = rows.Contains(position.Row) 
                && columns.Contains(position.Column);
            if (isValid)
            {
                int rowIndex = Array.IndexOf(rows, position.Row);
                int columnIndex = Array.IndexOf(columns, position.Column);
                string candidateCellContents = grid[columnIndex, rowIndex];

                isValid = ValidateCellContents(candidateCellContents);
            }
            return isValid;
        }

        public bool ValidateEndPosition(Position endPosition, Position startPosition, int size, char[] rows, char[] columns, string[,] grid)
        {
            bool isValid = false;

            // second point must be an empty cell, this scenario is covered in ValidateStartPosition method
            isValid = ValidateStartPosition(endPosition, rows, columns, grid);

            if (isValid)
            {
                Point startPoint = new Point() { Row = Array.IndexOf(rows, startPosition.Row), Column = Array.IndexOf(columns, startPosition.Column) };
                Point endPoint = new Point() { Row = Array.IndexOf(rows, endPosition.Row), Column = Array.IndexOf(columns, endPosition.Column) };

                if (Orientation == Orientations.Horizontal)
                {
                    isValid = Math.Abs(startPoint.Row - endPoint.Row) == 0 && Math.Abs(startPoint.Column - endPoint.Column) == Size - 1;
                }
                else
                {
                    isValid = Math.Abs(startPoint.Row - endPoint.Row) == Size - 1 && Math.Abs(startPoint.Column - endPoint.Column) == 0;
                }
            }

            return isValid;
        }

    }
}
