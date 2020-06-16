using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Boris;
using System.Threading;

namespace BattleshipsByKarlo
{
    public class Agents
    {
        private static Agents instance;

        //agents
        private Portal PortalReferee;
        private Portal PortalPlayer;
        private Portal PortalComputer;
        private MetaAgent AgentReferee;
        private MetaAgent AgentPlayer;
        private MetaAgent AgentComputer;

        protected Agents()
        {
            PortalReferee = new Portal("referee");
            PortalPlayer = new Portal("player");
            PortalComputer = new Portal("computer");
            AgentReferee = new MetaAgent(" [Referee] ");
            AgentPlayer = new MetaAgent("Player");
            AgentComputer = new MetaAgent("Computer");

            AddingConnecting();
        }

        public MetaAgent GetAgentReferee()
        {
            return this.AgentReferee;
        }
        public MetaAgent GetAgentPlayer()
        {
            return this.AgentPlayer;
        }
        public MetaAgent GetAgentComputer()
        {
            return this.AgentComputer;
        }

        public static Agents GetInstance()
        {
            if (instance == null)
            {
                instance = new Agents();
            }
                return instance;
        }
        
        private void AddingConnecting()
        {
            PortalReferee.AddAgent(AgentReferee);
            PortalPlayer.AddAgent(AgentPlayer);
            PortalComputer.AddAgent(AgentComputer);
            PortalReferee.Connect(PortalPlayer);
        }

        public static void ReceivedMessageAgent(Communication message)
        {
            Console.Write("{0}{1}", message.Sender, message.Body);
            message.Body = string.Empty;
            message.Sender = string.Empty;
        }

        public static void SendRequest_Location(MetaAgent refereeA, MetaAgent playerA)
        {
            string message = "Choose the location: ";
            refereeA.SendMessage(playerA.Name, message);
            playerA.MessageReceived += new MetaAgent.MessageReceivedHandler(ReceivedMessageAgent);
            Thread.Sleep(1000);
        }

        public static void SendRequest_AnnounceWinner(MetaAgent refereeA, MetaAgent playerA, Player currentPlayer)
        {
            string message = $"{Environment.NewLine}{Environment.NewLine} {currentPlayer.Name} Won! Congratulations! :)";
            refereeA.SendMessage(playerA.Name, message);
            playerA.MessageReceived += new MetaAgent.MessageReceivedHandler(ReceivedMessageAgent);
            Thread.Sleep(1000);
        }

        public static bool Agent_RefereeDecideWinner(HumanPlayer human, ComputerPlayer computer, GameState gamestate)
        {
            if (computer.NumberOfShipsDestroyed == computer.Board.NumberOfShips)
            {
                human.IsWinner = true;
                gamestate.IsGameOver = true;
                return true;
            }
            else if (human.NumberOfShipsDestroyed == human.Board.NumberOfShips)
            {
                computer.IsWinner = true;
                gamestate.IsGameOver = true;
                return true;
            }
            return false;
        }

        public static Point Agent_RandomPointSelect(ComputerPlayer computer, HumanPlayer human)
        {
            Position hitPos = new Position();      
            hitPos = computer.GetRandomHitPosition(human.Board);
            computer.Input.Row = hitPos.Row;
            computer.Input.Column = hitPos.Column;

            Point aimPoint = new Point
            {
                Column = Array.IndexOf(human.Board.Columns, computer.Input.Column),
                Row = Array.IndexOf(human.Board.Rows, computer.Input.Row)
            };

            return aimPoint;
        }

        public static Point Agent_SmartDecisionSelect(ComputerPlayer computer, HumanPlayer human, Point sucPoint)
        {
            string column = "";
            int newRowInt;

            switch (sucPoint.Column)
            {
                case 0:
                    column = "A";
                    break;
                case 1:
                    column = "B";
                    break;
                case 2:
                    column = "C";
                    break;
                case 3:
                    column = "D";
                    break;
                case 4:
                    column = "E";
                    break;
                case 5:
                    column = "F";
                    break;
                case 6:
                    column = "G";
                    break;
                case 7:
                    column = "H";
                    break;
                case 8:
                    column = "I";
                    break;
                case 9:
                    column = "J";
                    break;
            }

            if (sucPoint.Row == 9)
            {
                newRowInt = 0;
            }

            newRowInt = sucPoint.Row + 1;
            string row = newRowInt.ToString();

            Position newAttemptPosition = new Position
            {
                Column = column[0],
                Row = row[0]
            };

            Point aimPoint = new Point
            {
                Column = Array.IndexOf(human.Board.Columns, newAttemptPosition.Column),
                Row = Array.IndexOf(human.Board.Rows, newAttemptPosition.Row)
            };

            return aimPoint;
        }
    }
}
