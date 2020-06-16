using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Boris;
using System.Threading;

namespace BattleshipsByKarlo
{
    public class HumanPlayer : Player
    {
        public HumanPlayer()
        {
            Name = "You";
        }

        private void PlaceShip(Ship ship)
        {
            bool isOrientationValid = false;
            while (!isOrientationValid)
            {
                Console.Write($"{Environment.NewLine}{Environment.NewLine} {ship.PlacingString}" +
                $"{Environment.NewLine}{Environment.NewLine} Set Orientation (enter 1 or 2): {Environment.NewLine} " +
                $"1. Horizontal {Environment.NewLine} " +
                $"2. Vertical {Environment.NewLine} " +
                $"==> ");

                char orientation = Console.ReadKey().KeyChar;
                isOrientationValid = ship.ValidateOrientation(orientation);

                if (!isOrientationValid)
                    Console.WriteLine(ship.GetInvalidInputText());
                else
                {
                    ship.Orientation = orientation == '1' ? Ship.Orientations.Horizontal : Ship.Orientations.Vertical;
                }
            }

            bool isStartPositionValid = false;
            while (!isStartPositionValid)
            {
                Console.Write($"{Environment.NewLine} Enter start location: (A0 to J9) ");
                ship.StartPosition.Column = Char.ToUpper(Console.ReadKey().KeyChar);
                ship.StartPosition.Row = Console.ReadKey().KeyChar;
                isStartPositionValid = ship.ValidateStartPosition(ship.StartPosition, Board.Rows, Board.Columns, Board.Grid);
            }

            bool isEndPositionValid = false;
            while (!isEndPositionValid)
            {
                Console.Write($"{Environment.NewLine} Enter end location: ");
                ship.EndPosition.Column = Char.ToUpper(Console.ReadKey().KeyChar);
                ship.EndPosition.Row = Console.ReadKey().KeyChar; 
                isEndPositionValid = ship.ValidateEndPosition(ship.EndPosition, ship.StartPosition, ship.Size, Board.Rows, Board.Columns, Board.Grid);
            }

            if (isStartPositionValid && isEndPositionValid)
            {
                Point startPoint = new Point()
                    { Row = Array.IndexOf(Board.Rows, ship.StartPosition.Row), Column = Array.IndexOf(Board.Columns, ship.StartPosition.Column) };
                Point endPoint = new Point()
                    { Row = Array.IndexOf(Board.Rows, ship.EndPosition.Row), Column = Array.IndexOf(Board.Columns, ship.EndPosition.Column) };

                if (ship.Orientation == Ship.Orientations.Vertical)
                {
                    shipLocationCounter = 0;
                    if (startPoint.Row - endPoint.Row < 0)
                    {
                        for (int i = startPoint.Row; i <= endPoint.Row; i++)
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
                else
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
        }

        public void PlaceAllShips()
        {
            Console.Clear();
            Board.DisplayBoard();
            PlaceShip(Board.Destroyer1);

            Console.Clear();
            Board.DisplayBoard();
            PlaceShip(Board.Destroyer2);

            Console.Clear();
            Board.DisplayBoard();
            PlaceShip(Board.Battleship);
        }
    }
}
