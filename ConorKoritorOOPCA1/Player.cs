using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Decks;
using BlackJackProgram;

namespace Players
{
    internal class Player
    {
        private string Name {  get; set; }
        private decimal Money {  get; set; }
        public List<Card> Hand = new List<Card>();
        public List<Card> SplitHand = new List<Card>();
        protected int CardTotal {  get; set; }
        protected int ActualCardTotal {  get; set; }
        public bool IsBust = false;
        public bool IsBlackjack = false;
        private decimal Bet;
         
        public Player(int money, string name)
        {
            Money = money;
            Name = name;
        }
        
        public virtual Dealer GameActions(Dealer dealer)
        {
            string playerAction = "";
            string doubleDown = "";
            bool incorrectAction;
            bool incorrectDoubleDown;
            bool continueActions;

            DisplayHand();
            DisplayBet();
            CheckBlackjack();

            if (IsBlackjack == false)
            {
                do
                {
                    do
                    {
                        incorrectDoubleDown = false;

                        Console.Write($"\n\n{Name} Would You Like to Double Down [y,n]?: ");
                        doubleDown = Console.ReadLine();
                        doubleDown.ToLower();

                        if (doubleDown == string.Empty)
                        {
                            Console.Write("\nYou Didn't Enter an Answer. Please type [y/n].\n");
                            Console.Write("Press Enter to try again.");
                            Console.ReadKey();
                            incorrectDoubleDown = true;
                        }
                        else if (doubleDown != "y" && doubleDown != "n")
                        {
                            Console.Write("\nYou Entered Your Answer Incorrectly. Please type [y/n].\n");
                            Console.Write("Press Enter to try again.");
                            Console.ReadKey();
                            incorrectDoubleDown = true;
                        }
                        do
                        {
                            incorrectAction = false;
                            if (doubleDown == "n")
                            {
                                Console.Write($"\n\n{Name} What Would You Like to do?");
                                Console.Write("\n[h] to hit or [s] to stay: ");
                                playerAction = Console.ReadLine();
                                playerAction.ToLower();

                                if (playerAction == string.Empty)
                                {
                                    Console.Write("\nYou Didn't Enter an Answer. Please type [h] [s].\n");
                                    Console.Write("Press Enter to try again.");
                                    Console.ReadKey();
                                    incorrectAction = true;
                                }
                                else if (playerAction != "h" && playerAction != "s")
                                {
                                    Console.Write("\nYou Entered Your Answer Incorrectly. Please type [h] or [s].\n");
                                    Console.Write("Press Enter to try again.");
                                    Console.ReadKey();
                                    incorrectAction = true;
                                }
                            }
                            else if(doubleDown == "y")
                            {
                                playerAction = "d";
                            }

                        } while (incorrectAction == true);

                    } while (incorrectDoubleDown == true);

                    Console.WriteLine();

                    switch (playerAction)
                    {
                        case "h":
                            Console.Write("\nHit\n");
                            dealer.HitPlayer(this);
                            continueActions = true;
                            Thread.Sleep(1000);
                            break;
                        case "s":
                            Console.Write("\nStand\n");
                            continueActions = false;
                            Thread.Sleep(1000);
                            break;
                        case "d":
                            Console.Write("\nDouble Down\n");
                            DoubleDown();
                            dealer.HitPlayer(this);
                            continueActions = false;
                            Thread.Sleep(1000);
                            break;
                        default:
                            Console.Write("\nSomething went wrong. Try Again\n");
                            continueActions = true;
                            Thread.Sleep(1000);
                            break;
                    }

                    if (CardTotal == 21)
                    {
                        Console.WriteLine("\nYou Hit 21!");
                        continueActions = false;
                        Thread.Sleep(1000);
                    }
                    else if (CardTotal > 21)
                    {
                        Console.WriteLine("\nBust");
                        IsBust = true;
                        continueActions = false;
                        Thread.Sleep(1000);
                    }
                } while (continueActions == true);
            }

            else
            {
                Console.WriteLine("Blackjack!!!");
                Console.WriteLine("You Won 1.5x Your Bet!!!");
                Thread.Sleep(1500);
            }

            return dealer;
            
        }

        public void DisplayHand()
        {
            CalculateHandTotal();
            Console.WriteLine($"----------------- {Name} -----------------\n");
            for (int i=0; i<5; i++)
            {
                foreach (Card card in Hand)
                {
                    Console.Write(card.Visual[i] + "  ");
                }
                
            }
            Console.WriteLine($"\nTotal Value of Hand: {CardTotal}\n");
        }

        public void DisplayBet()
        {
            Console.WriteLine(String.Format("\nCurrent Bet: {0:C2}", Bet));
        }

        public void CalculateHandTotal()
        {
            CardTotal = 0;
            ActualCardTotal = 0;
            //Adds each card value to the total value of the hand
            foreach (Card card in Hand)
            {
                if (card.IsVisible == true)
                {
                    CardTotal += card.Value;
                }

                ActualCardTotal += card.Value;
            }

            /*Loops through each card in hand and checks for aces
            If the ace puts the value over 21 then it will reduce the total by 10
            as aces can either be 11 or 1

            I put them in seperate loops because if Hand[0] is the ace but Hand[3] is the 
            card that puts the total over 21 then the change would never be made so I have
            to add everything seperately first and then go through and check for aces
            */
            foreach(Card card in Hand)
            {
                if(card.Value == 11 && CardTotal > 21)
                {
                    CardTotal -= 10;
                }
            }
        }

        public void MakeBet()
        {
            Console.Clear();


            decimal bet = 0;
            bool incorrectEntry;

            do
            {
                incorrectEntry = false;
                Console.WriteLine(String.Format("{0} Has {1:C2} left to bet.", Name, Money));
                Console.Write($"{Name}, How Much Would You Like to Bet on this Hand: ");

                try
                {
                    bet = Convert.ToInt32(Console.ReadLine());
                }

                catch (FormatException)
                {
                    // Tells the user what they did wrong
                    Console.WriteLine("You Entered Your Bet Incorrectly. Don't use Letters or Special Characters.\n");
                    incorrectEntry = true;

                }
                catch (OverflowException)
                {
                    Console.WriteLine("You Entered Your Bet Incorrectly. Use numbers between 1 and the Amount of Money You Have Left.\n");
                    incorrectEntry = true;
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("You Entered Your Bet Incorrectly. Use numbers between 1 and the Amount of Money You Have Left.\n");
                    incorrectEntry = true;
                }

                if (bet < 1 || bet > Money)
                {
                    Console.WriteLine("You Entered the Your Bet Incorrectly. Use numbers between 1 and the Amount of Money You Have Left.\n");
                    incorrectEntry = true;
                }

            } while (incorrectEntry == true);

            Bet = bet;
            Money -= bet;
            Console.Clear();
        }

        public void DoubleDown()
        {
            decimal bet = 0;
            bool incorrectEntry;

            do
            {
                incorrectEntry = false;
                Console.WriteLine(String.Format("{0} Has {1:C2} left to bet.", Name, Money));
                Console.WriteLine(String.Format("{0} Can Bet up to {1:C2}.", Name, Bet));
                Console.Write($"{Name}, How Much Would You Like to add to your Bet: ");

                try
                {
                    bet = Convert.ToInt32(Console.ReadLine());
                }

                catch (FormatException)
                {
                    // Tells the user what they did wrong
                    Console.WriteLine("You Entered Your Bet Incorrectly. Don't use Letters or Special Characters.\n");
                    incorrectEntry = true;

                }
                catch (OverflowException)
                {
                    Console.WriteLine("You Entered Your Bet Incorrectly. Use numbers between 1 and the Amount of your initial Bet.\n");
                    Console.WriteLine("You must have enough Money to cover the Bet");
                    incorrectEntry = true;
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("You Entered Your Bet Incorrectly. Use numbers between 1 and the Amount of your initial Bet.\n");
                    Console.WriteLine("You must have enough Money to cover the Bet");
                    incorrectEntry = true;
                }

                if (bet < 1 || bet > Bet || bet > Money)
                {
                    Console.WriteLine("You Entered the Your Bet Incorrectly. Use numbers between 1 and the Amount of your initial Bet.\n");
                    Console.WriteLine("You must have enough Money to cover the Bet");
                    incorrectEntry = true;
                }

            } while (incorrectEntry == true);

            Bet += bet;
            Money -= bet;
        }

        public string GetName()
        {
            return Name;
        }

        public int GetCardTotal()
        {
            return CardTotal;
        }

        public decimal GetMoney()
        {
            return Money;
        }

        public void Win()
        {
            Money += Bet * 2;
            Console.WriteLine();
            Console.WriteLine(String.Format("{0} Won {1:C2}!", Name, Bet));
            Console.WriteLine(String.Format("{0} Total Money is now: {1:C2}", Name, Money));
        }

        public void Lose()
        {
            Money -= Bet;
            Console.WriteLine();
            Console.WriteLine(String.Format("{0} Lost {1:C2}!", Name, Bet));
            Console.WriteLine(String.Format("{0} Total Money is now: {1:C2}", Name, Money));
        }

        public void Push()
        {
            if(IsBlackjack == true)
            {
                Money += Bet;
                Console.WriteLine();
                Console.WriteLine($"{Name} and the Dealer both hit Blackjack");
                Console.WriteLine($"{Name}'s Bet was Returned");
                Console.WriteLine(String.Format("{0} Total Money is now: {1:C2}", Name, Money));
            }
            else
            {
                Money += Bet;
                Console.WriteLine();
                Console.WriteLine($"{Name} and the Dealer both stayed on the Same Card Total");
                Console.WriteLine($"{Name}'s Bet was Returned");
                Console.WriteLine(String.Format("{0} Total Money is now: {1:C2}", Name, Money));
            }
        }

        public void CheckBlackjack()
        {
            if(CardTotal == 21)
            {
                IsBlackjack = true;
            }
        }

        public void BlackJack()
        {
            Money += (Bet * (decimal)2.5);
            Console.WriteLine();
            Console.WriteLine("Blackjack!");
            Console.WriteLine(String.Format("{0} Won {1:C2}!", Name, Bet * (decimal)1.5));
            Console.WriteLine(String.Format("{0} Total Money is now: {1:C2}", Name, Money));
        }

        public void Reset()
        {
            Hand.Clear();
            SplitHand.Clear();
            CardTotal = 0;
            IsBust = false;
            Bet = 0;
            IsBlackjack = false;
        }

        public void CashOut()
        {
            Console.WriteLine($"Thank You For Playing {Name}!");
            Console.WriteLine(String.Format("You are Cashing Out with {0:C2}", Money));
            Console.WriteLine("We Hope to see you again soon!");
        }
    }

    
}
