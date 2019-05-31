using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport_Bets
{
    class Bet
    {
        double summ;
        double coeff;

        public string prediction;
        public string match_id;

        public bool result;
        public bool calculated { get; set; }

        public Bet(double sum, double coeff, string prediction, string match_id) {
            this.summ = sum;
            this.coeff = coeff;
            this.prediction = prediction;
            this.match_id = match_id;
            calculated = false;
        }

        public double getProfit()
        {
            return Math.Round(summ * coeff, 2); 
        }

        public void print()
        {
            Console.WriteLine("{0} - {1} - {2} - {3} - |{4}|", match_id, prediction, coeff, summ, (result ? "V" : "X"));
        }

        public double getSumm()
        {
            return summ;
        }

        public void setResult(bool res)
        {
            result = res;
            
        }

        internal void setCondition(bool v)
        {
            calculated = v;
        }
    }
}
