using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Sport_Bets
{
    class Match
    {
        public string match_id { set; get; }
        private const int ID_LENGH = 4;  

        double probability_homewin_real { set; get; }
        double probability_draw_real { set; get; }
        double probability_awaywin_real { set; get; }

        //double probability_homewin_bookie { set; get; }
        //double probability_draw_bookie { set; get; }
        //double probability_awaywin_bookie { set; get; }

        public double homewin_factor { set; get; }
        public double draw_factor { set; get; }
        public double awaywin_factor { set; get; }

        public DateTime date;
        public string team_home;
        public string team_away;
        public string result;

        public Match(double ph_r, double pd_r, double pa_r, double hf, double df, double af,
            DateTime d, string th, string ta, Random r)
        {
            match_id = getID(r);
            probability_homewin_real = ph_r;
            probability_awaywin_real = pa_r;
            probability_draw_real = pd_r;

            homewin_factor = Math.Round(hf, 2);
            draw_factor = Math.Round(df, 2);
            awaywin_factor = Math.Round(af, 2);

            date = d;

            team_away = ta;
            team_home = th;

            result = "NONE";

        }

        private string getID(Random r)
        {
            string id = "";
            for (int i = 0; i < ID_LENGH; i++)
            {
                id += (char)(r.Next(65, 122));
            }

            return id;

        }

        public void CalculateResult(Random r)
        {
            int[] prob = new int[1000];
            int h_pos = (int)(probability_homewin_real * 1000);
            int d_pos = (int)(probability_draw_real * 1000);
            int a_pos = Math.Abs(1000 - h_pos - d_pos);

            //Console.WriteLine("{0}__{1}__{2}", h_pos, d_pos, a_pos);

            for (int i = 0; i < h_pos; i++)
            {
                prob[i] = 1;
            }

            for (int i = h_pos; i < h_pos + d_pos; i++)
            {
                prob[i] = 2;
            }

            for (int i = h_pos + d_pos; i < 1000; i++)
            {
                prob[i] = 3;
            }
            
            int T = r.Next(0, 999);
            switch (prob[T])
            {
                case 1:
                    result =  "W1";
                    break;
                case 2:
                    result =  "X";
                    break;
                case 3:
                    result =  "W2";
                    break;
            }
            
        }

        private void Shuffle(int[] prob, Random r)
        {
            for (int i = prob.Length - 1; i > 0; i--)
            {
                int j = r.Next(i);
                var t = prob[i];
                prob[i] = prob[j];
                prob[j] = t;
            }
        }
    }
}
