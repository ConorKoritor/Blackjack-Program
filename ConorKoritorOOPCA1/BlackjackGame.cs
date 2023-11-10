using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Decks;
using Players;

namespace BlackJackProgram
{
    internal class BlackjackGame
    {
        private List<Player> _playerList = new List<Player>();
        private Dealer _dealer;
        private Shoe _shoe;

        

        public BlackjackGame(int numberOfPlayers, int numberOfDecksInShoe)
        {
            _shoe = new Shoe(numberOfDecksInShoe);
            CreatePlayers(numberOfPlayers);
            _dealer = new Dealer(_shoe, _playerList);
            _dealer.ShuffleShoe();
            PlayerBets();
            DealerActions();
        }

        public void CreatePlayers(int n)
        {
            string name;
            int money = 0;
            bool incorrectEntry;

            for(int i = 0; i < n; i++)
            {
                Console.WriteLine($"Player {i + 1}");
                Console.Write("Enter Name: ");
                name = Console.ReadLine();

                do
                {
                    incorrectEntry = false;
                    Console.Write($"\nEnter Starting Money: ");

                    try
                    {
                        money = Convert.ToInt32(Console.ReadLine());
                    }

                    catch (FormatException)
                    {
                        // Tells the user what they did wrong
                        Console.WriteLine("You Entered your Starting Money Incorrectly. Don't use Letters or Special Characters.");
                        incorrectEntry = true;

                    }
                    catch (OverflowException)
                    {
                        Console.WriteLine("You Entered your Starting Money Incorrectly. Use numbers between 1 and 1,000,000.");
                        incorrectEntry = true;
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        Console.WriteLine("You Entered your Starting Money Incorrectly. Use numbers between 1 and 1,000,000.");
                        incorrectEntry = true;
                    }

                    if (money < 0)
                    {
                        Console.WriteLine("You Entered your Starting Money Incorrectly. Use numbers between 1 and 1,000,000.");
                        incorrectEntry = true;
                    }

                } while (incorrectEntry == true);

                Console.WriteLine("\n\n");
                Player p = new Player(money, name);
                _playerList.Add(p);

                Console.Clear();
            }

            Console.Clear();
        }

        public void PlayerBets()
        {
            foreach(Player p in _playerList)
            {
                p.MakeBet();
            }
        }

        public void DealerActions()
        {

            _dealer.InitialDeal();

            if(_dealer.IsBlackjack == true)
            {
                Console.WriteLine("Dealer Hit Blackjack.");
                foreach(Player p in _playerList)
                {
                    p.CheckBlackjack();
                }
                CheckWinners();
            }

            PlayerActions();

            Console.WriteLine();
            Console.WriteLine("\nDEALERS TURN");
            Thread.Sleep(3000);

            _dealer.DealerAI();

            Thread.Sleep(3000);

            CheckWinners();
        }

        public void PlayerActions()
        {
            foreach(Player p in _playerList)
            {
               _dealer = p.GameActions(_dealer);
            }
        }

        public void CheckWinners()
        {
            string resetKey;
            int count = 0;

            Console.Clear();
            foreach (Player p in _playerList)
            {
                count++;

                Thread.Sleep(1500);
                if (p.IsBust == true)
                {
                    Console.WriteLine(p.GetName() + " Lost From Bust");
                    p.Lose();
                }
                else if (_dealer.IsBust == true)
                {
                    Console.WriteLine(p.GetName() + " Won From Dealer Bust");
                    p.Win();
                }
                else if(p.GetCardTotal() < _dealer.GetCardTotal())
                {
                    Console.WriteLine(p.GetName() + " Lost");
                    p.Lose();
                }
                else if (p.GetCardTotal() > _dealer.GetCardTotal())
                {
                    Console.WriteLine(p.GetName() + " Won");
                    p.Win();
                }
                else if(p.GetCardTotal() == _dealer.GetCardTotal())
                {
                    p.Push();
                }
                else if(p.IsBlackjack == true)
                {
                    p.BlackJack();
                }
                

                if (_playerList.Count > 1 && count < _playerList.Count)
                {
                    Console.WriteLine("\nPress Enter to see next players Result");
                    Console.ReadKey();
                    Console.Clear();
                }
                
            }

            foreach (Player p in _playerList)
            {
                if (p.GetMoney() <= 0)
                {
                    Console.WriteLine($"{p.GetName()} Is out of Money");
                    p.CashOut();
                    _playerList.Remove(p);
                }

                else
                {
                    do
                    {
                        Console.Write($"{p.GetName()} Would You Like to Play Another Hand? [y,n]: ");
                        resetKey = Console.ReadLine();


                    } while (resetKey != "y" && resetKey != "n");

                    if (resetKey == "y")
                    {
                        p.Reset();
                    }
                    else if (resetKey == "n")
                    {
                        p.CashOut();
                        _playerList.Remove(p);
                    }
                }

                if (_playerList.Count > 1 && count < _playerList.Count)
                {
                    Console.WriteLine("\nPress Enter to go to Next Player");
                    Console.ReadKey();
                    Console.Clear();
                }
            }

            Reset();
        }

        public void Reset()
        {
            foreach(Player p in _playerList)
            {
                p.Reset();
            }
            _dealer.Reset();

            Console.Clear();

            if (_playerList.Count > 0)
            {
                DealerActions();
            }
        }
    }
}
