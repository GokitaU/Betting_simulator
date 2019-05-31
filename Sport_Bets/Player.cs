using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport_Bets
{
    class Player
    {

        public double money { get; set; } 

        List<Bet> Bets;

        public Player()
        {
            money = 1000;
            Bets = new List<Bet>();
        }

        public void makeBet(Bet bet)
        {
            Bets.Add(bet);
            money -= bet.getSumm();
        }

        public void printBets()
        {
            foreach(Bet b in Bets)
            {
                b.print();
            }
        }

        public void updateBets(string match_id, string result)
        {
           foreach (Bet b in Bets)
            {
                if (b.match_id.Equals(match_id))
                {
                    //Console.WriteLine("Result {0} Prediction {1}", result, b.prediction);
                    //Console.WriteLine(result.Equals(b.prediction));
                    if (result.Equals(b.prediction)){
                        b.setResult(true);
                    }
                    else
                    {
                        b.setResult(false);
                    }
                    break;
                }
            }
        }

        public void cleanBetList()
        {
            Bets.Clear();
        }

        public void calculateBets()
        {
            foreach (Bet b in Bets)
            {

                if (!b.calculated)
                {
                    if (b.result)
                    {
                        money += b.getProfit();
                    }
                    b.setCondition(true);
                }
            }
        }
    }
}
