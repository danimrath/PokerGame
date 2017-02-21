using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using GameEvaluator;

namespace GameTester
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] players = new String[3];
            players[0] = ("Joe,H,H,H,H,H");
            players[1] = ("JimBob");
            players[2] = ("Joeb,3H, 4H, 5H, 6H");

            PokerHandEvaluator Evalutator = new PokerHandEvaluator(players);

            Evalutator.AddPlayer("Bobby, 3C, 3D, 3S, 8C, 10D");
            Debug.WriteLine(Evalutator.ToString());
            Evalutator.EvalHands();
            Evalutator.ClearPlayers();
            Evalutator.AddPlayer("Sallyann,AC, 10C, 5c, 2C, 8C");
            Evalutator.AddPlayer("Bob", "3C,3D,4S,4C,4D");
            Evalutator.AddPlayer("Sally", "AC,10C,5C,3S,2C");
            Debug.WriteLine(Evalutator.ToString());
            Evalutator.EvalHands();
        }
    }
}
