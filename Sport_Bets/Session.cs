using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sport_Bets
{
    class Session
    {
        Player player;
        DateTime current_date;
        Match_Manager manager;
        internal void run()
        {
            player = new Player();
            current_date = DateTime.Now;
            manager = new Match_Manager(current_date);
            // manager.UpdateLine(current_date, player);

            Console.WriteLine("♦♦♦♦♦♦♦♦♦♦♦♦♦♦♦_Betting Simulator 0.01_♦♦♦♦♦♦♦♦♦♦♦♦♦♦♦");

            int answer;

            bool trigger = true;
            while (trigger)
            {
                manager.UpdateLine(current_date, player);
                manager.PrintLine();

                Console.WriteLine("Дата: {0}", current_date.ToString("dd.MM.yyyy"));
                Console.WriteLine("Ваш счет: {0}", player.money);
                if (player.money <= 0)
                {
                    Console.WriteLine("Увы, но вы банкрот.");
                    Console.WriteLine("Завершение...");
                    trigger = false;
                    break;
                }
                Console.WriteLine("Ваши ставки:");
                player.printBets();
                Console.WriteLine("Что дальше? \n" +
                    "   1 - делать ставки \n" +
                    "   2 - завершить работу с программой \n" +
                    "   3 - очистить список ставок \n" +
                    "   4 - продолжить/запустить скрипт ");
                answer = int.Parse(Console.ReadLine());

                switch (answer)
                {
                    case 1:
                        doBets(player);
                        break;
                    case 2:
                        trigger = false;
                        Console.WriteLine("Завершение...");
                        break;
                    case 3:
                        player.cleanBetList();
                        break;
                    case 4:
                        Console.WriteLine("Work in progress...");
                        break;
                }
                current_date = current_date.AddDays(1);
            }
        }

        private void doBets(Player player)
        {

            bool trigger1 = true;
            string answ = "";

            while (trigger1)
            {
                Console.WriteLine("Введите информацию о пари в таком формате: ID_МАТЧА-ИСХОД-РАЗМЕР_СТАВКИ");
                string bet_info = Console.ReadLine();

                player.makeBet(parseBetInfo(bet_info));

                Console.WriteLine("Продолжить ставить? Y/N");
                answ = Console.ReadLine();
                if (answ == "N")
                {
                    trigger1 = false;
                }
            }

               
        }

        private Bet parseBetInfo(string bet_info)
        {
            string[] data = bet_info.Split(new char[] { '-' });
            Bet bet = new Bet(Double.Parse(data[2]), manager.getCoeff(data[0], data[1]), data[1], data[0]);
            return bet;
        }


    }
}


