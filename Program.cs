using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleshipsByKarlo
{
    class Program
    {
        static void Main(string[] args)
        {
            Agents agents = Agents.GetInstance();
            GameState Game = new GameState();
            Game.Start();

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(Environment.NewLine);
        }
    }
}
