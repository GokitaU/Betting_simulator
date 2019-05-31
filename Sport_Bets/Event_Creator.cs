using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport_Bets
{
    class Event_Creator
    {
        const int BETTING_MARGIN = 2; //% - букмекерская маржа (данные для Pinnacle)
        
        // вычислено на основе данных о 190000 футбольных матчей с 2007 по 2019 год:
        const double PROBABILITY_ALPHA = 3.99; //% - поправка для букмекерской вероятности победы хозяев
        const double PROBABILITY_BETA = 7.6; //% - поправка для букмекерской вероятности ничьей
        const double PROBABILITY_PHI = 4.1; //% - поправка для букмекерской вероятности победы гостей

        // параметры нормального распределения вероятностей, вычеслено не основе данных 
        // по оси x отложены вероятности, по оси y - количество событий (матчей) с данной вероятностью на данный исход 
        const double PH_NORMAL_CENTER = 0.46 ; // - вершина кривой для вероятностей победы хозяев
        const double PA_NORMAL_CENTER = 0.3; // - вершина кривой для вероятностей победы гостей
        const double PD_NORMAL_CENTER = 0.25; // - вершина кривой для вероятностей ничейного результата
        
        // параметры отклонения
        const double PH_NORMAL_SIGMA = 0.45;
        const double PA_NORMAL_SIGMA = 0.35;
        const double PD_NORMAL_SIGMA = 0.15;



        Random r;

        public Event_Creator()
        {
            r = new Random();
        }

        public Random GetRandom()
        {
            return r;
        }

        public Match GetMatch(DateTime currentDate)
        {
            double Ph_real = Gauss(PH_NORMAL_CENTER, PH_NORMAL_SIGMA);
            double Pd_real = Gauss(PD_NORMAL_CENTER, PD_NORMAL_SIGMA);
            //Console.WriteLine("_++++_{0}=={1}==", Ph_real, Pd_real);
            if (Ph_real + Pd_real >= 1)
            {
                Console.WriteLine("ALARM!___{}{}{}");
                double delta, delta_h, delta_d;
                delta = (Ph_real + Pd_real) - 1;
                delta_h = delta * Ph_real;
                delta_d = delta * Pd_real;
                while ((Ph_real + Pd_real) > (0.98 + (r.NextDouble() * 0.01 - 0.02))) //приведение показателей к корректному виду, 
                    //0.02 - наименьшая вероятность победы команды гостей (для выборки)
                {
                    Ph_real -= delta_h;
                    Pd_real -= delta_d;
                }
                    
                
            }
            
            double Pa_real = 1 - Ph_real - Pd_real;
            //Console.WriteLine("___{0}=={1}=={2}", Ph_real, Pd_real, Pa_real);

            double current_alpha = r.NextDouble() * PROBABILITY_ALPHA / 100 - 2 * PROBABILITY_ALPHA / 100;
            double Ph_bookie = Math.Abs(Ph_real + current_alpha);

            double current_beta = r.NextDouble() * PROBABILITY_BETA / 100 - 2 * PROBABILITY_BETA / 100; ;
            double Pd_bookie = Math.Abs(Pd_real - current_beta);

            double current_phi = (Ph_bookie + Pd_bookie + Pa_real) - 1;
            double Pa_bookie = Math.Abs(Pa_real - current_phi);

            double Kh = 1 / Ph_bookie - ((1 / Ph_bookie) * (BETTING_MARGIN / 100));
            double Kd = 1 / Pd_bookie - ((1 / Pd_bookie) * (BETTING_MARGIN / 100));
            double Ka = 1 / Pa_bookie - ((1 / Pa_bookie) * (BETTING_MARGIN / 100));


            //Console.WriteLine("LOG:  {0}, {1}, {2}, {3}, {4}, {5}, {6}", Ph_real, Pd_real, Pa_real, 
            //Ph_bookie, Pd_bookie, Pa_bookie, currentDate);

            //WriteToFile(Ph_real, Pd_real, Pa_real);

            return new Match(Ph_real, Pd_real, Pa_real, Kh, Kd, Ka, currentDate, "TEAM1", "TEAM2", r);
        }

        public double Gauss(double center, double sigma)
        {
            return (center + sigma * (r.NextDouble() + r.NextDouble() + r.NextDouble() + r.NextDouble() - 2) / 2);
        }

        private void WriteToFile(double Ph_real, double Pd_real, double Pa_real)
        {
            string text = Math.Round(Ph_real, 2).ToString() + " \t" +
                Math.Round(Pd_real, 2).ToString() + " \t" +
                 Math.Round(Pa_real, 2).ToString() + "\n";

            StreamWriter sw = new StreamWriter("note.txt", true);
            sw.WriteLine(text);
            sw.Close();
        }

    }
}
