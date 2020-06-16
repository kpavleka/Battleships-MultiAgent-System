using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsByKarlo
{
    public class ComputerPlayer : Player
    {
        private Position InvalidPosition;
        public List<string> HitPositions;

        public ComputerPlayer()
        {
            InvalidPosition = new Position
            {
                Row = 'x',
                Column = 'x'
            };

            Name = "Computer";
            HitPositions = new List<string>();
            for (int i = 0; i < Board.Rows.Length; i++)
                for (int j = 0; j < Board.Columns.Length; j++)
                    HitPositions.Add($"{Board.Columns[j]}{Board.Rows[i]}");

        }

        private int GetRandomNumberInExludedRange(int start, int end, HashSet<int> exclude)
        {
            end++;
            var range = Enumerable.Range(start, end).Where(i => !exclude.Contains(i));
            Random rand = new Random();
            int index = rand.Next(start, end - exclude.Count);
            return range.ElementAt(index);
        }


        private int GetRandomNumberInRange(int start, int end)
        {
            end++;
            Random rand = new Random();
            int number = rand.Next(start, end);
            return number;
        }

        private int GetRandomOrientation()
        {
            return GetRandomNumberInRange(1, 2);
        }

        public Position GetRandomGridPosition(Ship ship)
        {
            Position randPosition = new Position();
            Point randPoint = new Point();
            Random rand = new Random();
            bool isValidPosition = false;

            while (!isValidPosition)
            {
                randPoint.Column = rand.Next(Board.Grid.GetLength(0));
                randPoint.Row = rand.Next(Board.Grid.GetLength(1));

                string cellContents = Board.Grid[randPoint.Column, randPoint.Row];
                randPosition.Column = cellContents[0];
                randPosition.Row = cellContents[1];
                isValidPosition = ship.ValidateCellContents(cellContents);

                randPosition.Column = Board.Columns[randPoint.Column];
                randPosition.Row = Board.Rows[randPoint.Row];
            }
            return randPosition;
        }

        private Position GetRandomEndPosition(Ship ship)
        {
            Position endPosition = new Position();
            Point endPoint = new Point();

            bool isValidPosition = false;

            while (!isValidPosition)
            {
                if (ship.Orientation == Ship.Orientations.Horizontal)
                {
                    endPoint.Column = Array.IndexOf(Board.Columns, ship.StartPosition.Column) + (ship.Size - 1);
                    endPoint.Row = Array.IndexOf(Board.Rows, ship.StartPosition.Row);

                    string cellContents = "";
                    isValidPosition = endPoint.Column < Board.Columns.Length && endPoint.Row < Board.Rows.Length;
                    if (isValidPosition)
                    {
                        cellContents = Board.Grid[endPoint.Column, endPoint.Row];
                        endPosition.Column = cellContents[0];
                        endPosition.Row = cellContents[1];
                        isValidPosition = ship.ValidateCellContents(cellContents);
                    }

                    if (!isValidPosition)
                    {
                        endPoint.Column = Array.IndexOf(Board.Columns, ship.StartPosition.Column) - (ship.Size - 1);
                        endPoint.Row = Array.IndexOf(Board.Rows, ship.StartPosition.Row);
                        cellContents = Board.Grid[endPoint.Column, endPoint.Row];
                        endPosition.Column = cellContents[0];
                        endPosition.Row = cellContents[1];
                        isValidPosition = ship.ValidateCellContents(cellContents);
                        return InvalidPosition;
                    }
                }
                else // Ship orientation is vertical
                {
                    endPoint.Column = Array.IndexOf(Board.Columns, ship.StartPosition.Column);
                    endPoint.Row = Array.IndexOf(Board.Rows, ship.StartPosition.Row) + (ship.Size - 1);

                    string cellContents = "";
                    isValidPosition = endPoint.Column < Board.Columns.Length && endPoint.Row < Board.Rows.Length;

                    if (isValidPosition)
                    {
                        cellContents = Board.Grid[endPoint.Column, endPoint.Row];
                        endPosition.Column = cellContents[0];
                        endPosition.Row = cellContents[1];
                        isValidPosition = ship.ValidateCellContents(cellContents);
                    }
                    if (!isValidPosition)
                    {
                        endPoint.Column = Array.IndexOf(Board.Columns, ship.StartPosition.Column);
                        endPoint.Row = Array.IndexOf(Board.Rows, ship.StartPosition.Row) - (ship.Size - 1);
                        cellContents = Board.Grid[endPoint.Column, endPoint.Row];
                        endPosition.Column = cellContents[0];
                        endPosition.Row = cellContents[1];
                        isValidPosition = ship.ValidateCellContents(cellContents);
                        return InvalidPosition;
                    }
                }
            }

            if (isValidPosition)
            {
                endPosition.Column = Board.Columns[endPoint.Column];
                endPosition.Row = Board.Rows[endPoint.Row];
            }
            return endPosition;
        }

        private void PlaceShip(Ship ship)
        {
            ship.Orientation = GetRandomOrientation() == 1 ? Ship.Orientations.Horizontal : Ship.Orientations.Vertical;

            bool isShipPlaced = false;

            while (!isShipPlaced)
            {
                ship.StartPosition = GetRandomGridPosition(ship);
                ship.EndPosition = GetRandomEndPosition(ship);
                isShipPlaced = !ship.EndPosition.Equals(InvalidPosition);
            }

            Point startPoint = new Point()
                { Row = Array.IndexOf(Board.Rows, ship.StartPosition.Row), Column = Array.IndexOf(Board.Columns, ship.StartPosition.Column) };
            Point endPoint = new Point()
                { Row = Array.IndexOf(Board.Rows, ship.EndPosition.Row), Column = Array.IndexOf(Board.Columns, ship.EndPosition.Column) };

            if (ship.Orientation == Ship.Orientations.Vertical)
            {
                shipLocationCounter = 0;
                if (startPoint.Row - endPoint.Row < 0)
                {
                    for (var i = startPoint.Row; i <= endPoint.Row; i++)
                    {
                        ship.LocationRows[shipLocationCounter] = Board.Rows[i];
                        ship.LocationColumns[shipLocationCounter] = Board.Columns[startPoint.Column];
                        shipLocationCounter++;
                        Board.Grid[startPoint.Column, i] = ship.Legend;
                    }
                }
                else
                {
                    for (var i = endPoint.Row; i <= startPoint.Row; i++)
                    {
                        ship.LocationRows[shipLocationCounter] = Board.Rows[i];
                        ship.LocationColumns[shipLocationCounter] = Board.Columns[startPoint.Column];
                        shipLocationCounter++;
                        Board.Grid[startPoint.Column, i] = ship.Legend;
                    }
                }
            }
            else // Horizontal
            {
                shipLocationCounter = 0;
                if (startPoint.Column - endPoint.Column < 0)
                {
                    for (var i = startPoint.Column; i <= endPoint.Column; i++)
                    {
                        ship.LocationRows[shipLocationCounter] = Board.Rows[startPoint.Row];
                        ship.LocationColumns[shipLocationCounter] = Board.Columns[i];
                        shipLocationCounter++;
                        Board.Grid[i, startPoint.Row] = ship.Legend;
                    }
                }
                else
                {
                    for (var i = endPoint.Column; i <= startPoint.Column; i++)
                    {
                        ship.LocationRows[shipLocationCounter] = Board.Rows[startPoint.Row];
                        ship.LocationColumns[shipLocationCounter] = Board.Columns[i];
                        shipLocationCounter++;
                        Board.Grid[i, startPoint.Row] = ship.Legend;
                    }
                }
            }
        }

        public void PlaceAllShips()
        {
            PlaceShip(Board.Destroyer1);
            PlaceShip(Board.Destroyer2);
            PlaceShip(Board.Battleship);
        }

        internal Position GetRandomHitPosition(Board humanBoard)
        {
            Position randPosition = new Position();
            Random rand = new Random();

            int hitPositionIndex = rand.Next(HitPositions.Count);

            string positionContents = HitPositions.ElementAt(hitPositionIndex);
            HitPositions.RemoveAt(hitPositionIndex);

            randPosition.Column = positionContents[0];
            randPosition.Row = positionContents[1];

            return randPosition;
        }

    }
}
