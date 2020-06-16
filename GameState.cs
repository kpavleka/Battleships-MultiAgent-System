using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Boris;

namespace BattleshipsByKarlo
{

    public class GameState
    {
        public HumanPlayer Human { get; set; }
        public ComputerPlayer Computer { get; set; }
        public Board ShadowBoard { get; set; }
        public bool IsAttemptSuccessful { get; set; }
        public bool IsGameOver { get; set; }
        public bool IsHumanTurn { get; set; }
        public bool IsInputValid { get; set; }
        public bool IsShipPlacementComplete { get; set; }
        public Point SuccessfulPoint { get; set; }

        public GameState()
        {
            Human = new HumanPlayer();
            ShadowBoard = new Board();
            Computer = new ComputerPlayer();
            IsAttemptSuccessful = false;
            IsGameOver = false;
            IsHumanTurn = false;
            IsShipPlacementComplete = false;
        }

        private void DisplayBoards()
        {
            Console.Write($"{Environment.NewLine}\t\t YOU");
            Human.Board.DisplayBoard();

            Console.Write($"{Environment.NewLine}{Environment.NewLine}\t       COMPUTER");
            ShadowBoard.DisplayBoard();

            Console.WriteLine($"\t{Environment.NewLine}{Environment.NewLine} LEGEND: {Environment.NewLine} " +
               $"DE: Destroyer,  " +
               $"BS: Battleship,  " +
               $"HM: Hit Missed,  " +
               $"HS: Hit Successful,  " +
               $"++: Empty Cell");
        }

        private void PlaceShips()
        {
            Console.Clear();
            Human.PlaceAllShips();
            Computer.PlaceAllShips();
            Console.Clear();
        }

        public bool ValidateInput(Input input)
        {
            return Human.Board.Rows.Contains(Char.ToUpper(input.Row)) && Computer.Board.Columns.Contains(input.Column);
        }

        public void Start()
        {
            Console.Clear();
            PlaceShips();
            DisplayBoards();
            Player currentPlayer = new Player();

            while (!IsGameOver)
            {
                Console.Clear();
                DisplayBoards();
                IsHumanTurn = !IsHumanTurn;
                Human.IsCurrentTurn = IsHumanTurn;
                Computer.IsCurrentTurn = !IsHumanTurn;

                currentPlayer = IsHumanTurn ? (Player)Human : (Player)Computer;

                if (IsHumanTurn)
                {
                    //////////////////////////AGENTS///////////////////////
                    Agents agents = Agents.GetInstance();
                    Console.Write($"{Environment.NewLine}");
                    Agents.SendRequest_Location(agents.GetAgentReferee(), 
                        agents.GetAgentPlayer());
                    IsInputValid = false;
                    while (!IsInputValid)
                    {
                        Human.Input.Column = Char.ToUpper(Console.ReadKey().KeyChar);
                        Human.Input.Row = Console.ReadKey().KeyChar;

                        IsInputValid = ValidateInput(Human.Input);
                        if (!IsInputValid)
                        {
                            Console.Write($"{Environment.NewLine} {Human.Board.Destroyer1.GetInvalidInputText()} {Environment.NewLine} [Referee] Choose the location: ");
                            continue;
                        }

                        Point aimPoint = new Point
                        {
                            Column = Array.IndexOf(Computer.Board.Columns, Human.Input.Column),
                            Row = Array.IndexOf(Computer.Board.Rows, Human.Input.Row)
                        };

                        string cellContents = Computer.Board.Grid[aimPoint.Column, aimPoint.Row];

                        if (cellContents == "HM" || cellContents == "HS")
                        {
                            IsInputValid = false;
                            Console.Write($"{Environment.NewLine} {Human.Board.Destroyer1.GetInvalidInputText()} {Environment.NewLine} [Referee] Choose the location: ");
                            continue;
                        }
                        else if (cellContents == "++")
                        {
                            Computer.Board.Grid[aimPoint.Column, aimPoint.Row] = "HM";
                            ShadowBoard.Grid[aimPoint.Column, aimPoint.Row] = Computer.Board.Grid[aimPoint.Column, aimPoint.Row];

                            Console.WriteLine($"{Environment.NewLine} You: Hit Missed");
                        }
                        else if (cellContents == "BS")
                        {
                            Computer.Board.Grid[aimPoint.Column, aimPoint.Row] = "HS";
                            ShadowBoard.Grid[aimPoint.Column, aimPoint.Row] = Computer.Board.Grid[aimPoint.Column, aimPoint.Row];
                            Console.WriteLine($"{Environment.NewLine} You: Hit Successful");

                            int bsLocationColIndex = Array.IndexOf(Computer.Board.Battleship.LocationColumns, Human.Input.Column);
                            int bsLocationRowIndex = Array.IndexOf(Computer.Board.Battleship.LocationRows, Human.Input.Row);

                            if (bsLocationColIndex > -1 && bsLocationRowIndex > -1)
                            {
                                Computer.Board.Battleship.LocationColumns[bsLocationColIndex] = 'x';
                                Computer.Board.Battleship.LocationRows[bsLocationRowIndex] = 'x';
                                ShadowBoard.Battleship.LocationColumns[bsLocationColIndex] = Computer.Board.Battleship.LocationColumns[bsLocationColIndex];
                                ShadowBoard.Battleship.LocationRows[bsLocationRowIndex] = Computer.Board.Battleship.LocationColumns[bsLocationRowIndex];

                                if (Computer.Board.Battleship.LocationRows.Where(num => num == 'x').Count() == Computer.Board.Battleship.Size)
                                {
                                    Console.WriteLine($" Battleship Eliminated {Environment.NewLine} {Environment.NewLine} Press any key to continue ... ");
                                    Computer.NumberOfShipsDestroyed++;
                                    Console.ReadKey();
                                }
                            }
                        }
                        else if (cellContents == "DE")
                        {
                            Computer.Board.Grid[aimPoint.Column, aimPoint.Row] = "HS";
                            ShadowBoard.Grid[aimPoint.Column, aimPoint.Row] = Computer.Board.Grid[aimPoint.Column, aimPoint.Row];

                            Console.WriteLine($"{Environment.NewLine} You: Hit Successful");

                            int de1LocationColIndex = Array.IndexOf(Computer.Board.Destroyer1.LocationColumns, Human.Input.Column);
                            int de1LocationRowIndex = Array.IndexOf(Computer.Board.Destroyer1.LocationRows, Human.Input.Row);

                            int de2LocationColIndex = Array.IndexOf(Computer.Board.Destroyer2.LocationColumns, Human.Input.Column);
                            int de2LocationRowIndex = Array.IndexOf(Computer.Board.Destroyer2.LocationRows, Human.Input.Row);

                            if (de1LocationColIndex > -1 && de1LocationRowIndex > -1)
                            {
                                Computer.Board.Destroyer1.LocationColumns[de1LocationColIndex] = 'x';
                                Computer.Board.Destroyer1.LocationRows[de1LocationRowIndex] = 'x';
                                ShadowBoard.Destroyer1.LocationColumns[de1LocationColIndex] = Computer.Board.Destroyer1.LocationColumns[de1LocationColIndex];
                                ShadowBoard.Destroyer1.LocationRows[de1LocationRowIndex] = Computer.Board.Destroyer1.LocationRows[de1LocationRowIndex];


                                if (Computer.Board.Destroyer1.LocationRows.Where(num => num == 'x').Count() == Computer.Board.Destroyer1.Size)
                                {
                                    Console.WriteLine($" Destroyer Eliminated {Environment.NewLine} {Environment.NewLine} Press any key to continue ... ");
                                    Computer.NumberOfShipsDestroyed++;
                                    Console.ReadKey();
                                }
                            }
                            else if (de2LocationColIndex > -1 && de2LocationRowIndex > -1)
                            {
                                Computer.Board.Destroyer2.LocationColumns[de2LocationColIndex] = 'x';
                                Computer.Board.Destroyer2.LocationRows[de2LocationRowIndex] = 'x';
                                ShadowBoard.Destroyer2.LocationColumns[de2LocationColIndex] = Computer.Board.Destroyer2.LocationColumns[de2LocationColIndex];
                                ShadowBoard.Destroyer2.LocationRows[de2LocationRowIndex] = Computer.Board.Destroyer2.LocationRows[de2LocationRowIndex];

                                if (Computer.Board.Destroyer2.LocationRows.Where(num => num == 'x').Count() == Computer.Board.Destroyer2.Size)
                                {
                                    Console.WriteLine($" Destroyer Eliminated {Environment.NewLine} {Environment.NewLine} Press any key to continue ... ");
                                    Computer.NumberOfShipsDestroyed++;
                                    Console.ReadKey();
                                }
                            }
                        }

                        /////////////////////////////AGENTS/////////////////////////////
                        Agents.Agent_RefereeDecideWinner(Human, Computer, this);
                        if (Agents.Agent_RefereeDecideWinner(Human, Computer, this) == true)
                        {
                            break;
                        }
                    }
                }
                else // Computer's turn
                {
                    Point aimPoint = new Point();               

                    if (IsAttemptSuccessful == false)
                    {
                        aimPoint = Agents.Agent_RandomPointSelect(Computer, Human);
                    }
                    else
                    {
                        aimPoint = Agents.Agent_SmartDecisionSelect(Computer, Human, SuccessfulPoint);                    
                    }


                    string cellContents = Human.Board.Grid[aimPoint.Column, aimPoint.Row];

                    if (cellContents == "Hit Missed" || cellContents == "Hit Successful")
                    {
                        IsInputValid = false;
                        Console.Write($"{Environment.NewLine} {Human.Board.Destroyer1.GetInvalidInputText()} {Environment.NewLine} [Referee] Choose the location: ");
                        continue;
                    }
                    else if (cellContents == "++")
                    {
                        IsAttemptSuccessful = false;
                        Human.Board.Grid[aimPoint.Column, aimPoint.Row] = "HM";
                        Console.WriteLine($"{Environment.NewLine} You: Hit Missed");
                    }
                    else if (cellContents == "BS")
                    {
                        IsAttemptSuccessful = true;
                        SuccessfulPoint = aimPoint;
                        Human.Board.Grid[aimPoint.Column, aimPoint.Row] = "HS";
                        Console.WriteLine($"{Environment.NewLine} You: Hit Successful");

                        int bsLocationColIndex = Array.IndexOf(Human.Board.Battleship.LocationColumns, Computer.Input.Column);
                        int bsLocationRowIndex = Array.IndexOf(Human.Board.Battleship.LocationRows, Computer.Input.Row);

                        if (bsLocationColIndex > -1 && bsLocationRowIndex > -1)
                        {
                            Human.Board.Battleship.LocationColumns[bsLocationColIndex] = 'x';
                            Human.Board.Battleship.LocationRows[bsLocationRowIndex] = 'x';

                            if (Human.Board.Battleship.LocationRows.Where(num => num == 'x').Count() == Human.Board.Battleship.Size)
                            {
                                Console.WriteLine($" Battleship Eliminated! {Environment.NewLine} {Environment.NewLine} Press any key to continue ... ");
                                Human.NumberOfShipsDestroyed++;
                                Console.ReadKey();
                            }
                        }
                    }
                    else if (cellContents == "DE") 
                    {
                        IsAttemptSuccessful = true;
                        SuccessfulPoint = aimPoint;
                        Human.Board.Grid[aimPoint.Column, aimPoint.Row] = "HS";
                        Console.WriteLine($"{Environment.NewLine} You: Hit Successful");

                        int de1LocationColIndex = Array.IndexOf(Human.Board.Destroyer1.LocationColumns, Computer.Input.Column);
                        int de1LocationRowIndex = Array.IndexOf(Human.Board.Destroyer1.LocationRows, Computer.Input.Row);

                        int de2LocationColIndex = Array.IndexOf(Human.Board.Destroyer2.LocationColumns, Computer.Input.Column);
                        int de2LocationRowIndex = Array.IndexOf(Human.Board.Destroyer2.LocationRows, Computer.Input.Row);

                        if (de1LocationColIndex > -1 && de1LocationRowIndex > -1)
                        {
                            Human.Board.Destroyer1.LocationColumns[de1LocationColIndex] = 'x';
                            Human.Board.Destroyer1.LocationRows[de1LocationRowIndex] = 'x';

                            if (Human.Board.Destroyer1.LocationRows.Where(num => num == 'x').Count() == Human.Board.Destroyer1.Size)
                            {
                                Console.WriteLine($" Destroyer Eliminated {Environment.NewLine} {Environment.NewLine} Press any key to continue ... ");
                                Human.NumberOfShipsDestroyed++;
                                Console.ReadKey();
                            }
                        }
                        else if (de2LocationColIndex > -1 && de2LocationRowIndex > -1)
                        {
                            Human.Board.Destroyer2.LocationColumns[de2LocationColIndex] = 'x';
                            Human.Board.Destroyer2.LocationRows[de2LocationRowIndex] = 'x';

                            if (Human.Board.Destroyer2.LocationRows.Where(num => num == 'x').Count() == Human.Board.Destroyer2.Size)
                            {
                                Console.WriteLine($" Destroyer Eliminated {Environment.NewLine} {Environment.NewLine} Press any key to continue ... ");
                                Human.NumberOfShipsDestroyed++;
                                Console.ReadKey();
                            }
                        }
                    }

                    /////////////////////////////AGENTS/////////////////////////////
                    Agents.Agent_RefereeDecideWinner(Human, Computer, this);
                    if (Agents.Agent_RefereeDecideWinner(Human, Computer, this) == true)
                    {
                        break;
                    }
                }
            }
            if (currentPlayer.IsWinner)
            {
                /////////////////////////////AGENTS/////////////////////////////
                Agents agents = Agents.GetInstance();
                Agents.SendRequest_AnnounceWinner(agents.GetAgentReferee(), agents.GetAgentPlayer(), currentPlayer);
                Console.ReadKey();
            }
        }

    }
}
