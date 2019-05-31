using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport_Bets
{
    class Match_Manager
    {
        Event_Creator Event_Creator;
        List<Match> Line;
        Random r;

        public Match_Manager(DateTime current_time)
        {
            Event_Creator = new Event_Creator();
            Line = new List<Match>();
            r = new Random();
            
        }


        public void UpdateLine(DateTime  current_time, Player player, int minimum = 1 )
        {
            int number = r.Next(minimum, 10);
            current_time = current_time.AddDays(1);

            bool trigger = true;
            //Line.Insert(0, Event_Creator.GetMatch(current_time));

            foreach(Match Buf in Line)
            {
                if (Buf.date < current_time)
                {
                    Buf.CalculateResult(Event_Creator.GetRandom());
                    Console.WriteLine("Результат для матча *{0}*: {1}||{2}||{3} - {4}", 
                        Buf.match_id, Buf.homewin_factor, Buf.draw_factor, Buf.awaywin_factor, Buf.result);
                    player.updateBets(Buf.match_id, Buf.result); 
                }
            }

            player.calculateBets();

            for (int i = 0; i < number; i++)
            {
                Line.Insert(0, Event_Creator.GetMatch(current_time));
            }

            while (trigger)
            {
                Match Buf = Line.Last();
                if (Buf.date < current_time)
                {
                    Line.RemoveAt(Line.Count - 1);
                }
                else
                {
                    trigger = false;
                }
            }

  
            
        }

        public void PrintLine()
        {
            Console.WriteLine("  ID  >Команда 1  ||  Команда 2  ||     Дата   || W1 || X || W2 ||");
            foreach (Match m in Line) 
            {
                Console.WriteLine("*{0}*>   {1}   ||    {2}    || {3} || {4} || {5} || {6} ||", 
                    m.match_id, m.team_home, m.team_away, m.date.ToString("dd.MM.yyyy"), m.homewin_factor, m.draw_factor, m.awaywin_factor);

            }
        }

        public double getCoeff(string match_id, string prediction)
        {

            //Console.WriteLine("ID{0}   PR{1}", match_id, prediction);
            foreach (Match m in Line)
            {
                if (m.match_id.Equals(match_id)){
                    if (prediction.Equals("W1"))
                    {
                        return m.homewin_factor;
                    }else if (prediction.Equals("W2")){
                        return m.awaywin_factor;
                    }else if (prediction.Equals("X")){
                        return m.draw_factor;
                    }
                }
            }

            return -999;
        }
    }
}
