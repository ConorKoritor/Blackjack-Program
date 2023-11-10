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
            /*This Method defines the actions the player is allowed to take
             * It takes in a dealer and then returns the same dealer to maintain the continuity
             * of the shoe */

            string playerAction = "";
            string doubleDown = "";
            bool incorrectAction;
            bool incorrectDoubleDown;
            bool continueActions;

            //Displays the Hand and then checks for a Blackjack before the player is allowed to take actions
            DisplayHand();
            DisplayBet();
            CheckBlackjack();

            /*Is Blackjack is a bool value that CheckBlackjack() sets to true if the player has a Blackjack
             * The Player is only allowed to take actions if the IsBlackjack false because if it is true
             * The player already has 21 and doesn't need to take any actions*/
            if (IsBlackjack == false)
            {
                /*This Do/While loop checks if the player has stayed or doubled down and exits the loop if they have.
                 * If the player has Hit they are still allowed to take game actions so the loop runs another time
                 * until they have stayed. If they doubled down then the loop doesnt run again*/
                do
                {
                    /*These Do/While Loops are just checking if the player inputted anything incorrectly. 
                     * The loop runs again if they have*/
                    do
                    {
                        incorrectDoubleDown = false;

                        //Asks the player if they want to double down after the initial cards have been dealt
                        Console.Write($"\n\n{Name} Would You Like to Double Down [y,n]?: ");
                        doubleDown = Console.ReadLine();
                        doubleDown.ToLower();

                        //Checks if the player has entered anything
                        if (doubleDown == string.Empty)
                        {
                            Console.Write("\nYou Didn't Enter an Answer. Please type [y/n].\n");
                            Console.Write("Press Enter to try again.");
                            Console.ReadKey();
                            incorrectDoubleDown = true;
                        }
                        //checks if the player entered in the correct format
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
                                //If the player has not Doubled Down, Asks they player to enter their action
                                Console.Write($"\n\n{Name} What Would You Like to do?");
                                Console.Write("\n[h] to hit or [s] to stay: ");
                                playerAction = Console.ReadLine();
                                playerAction.ToLower();

                                //Checks if the player has entered anything
                                if (playerAction == string.Empty)
                                {
                                    Console.Write("\nYou Didn't Enter an Answer. Please type [h] [s].\n");
                                    Console.Write("Press Enter to try again.");
                                    Console.ReadKey();
                                    incorrectAction = true;
                                }
                                //checks if the player entered in the correct format
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
                                //If the player has Doubled Down this sets the player action to "d"
                                playerAction = "d";
                            }

                        } while (incorrectAction == true);

                    } while (incorrectDoubleDown == true);

                    Console.WriteLine();

                    //takes in player action which can be either h, s, or d
                    switch (playerAction)
                    {
                        case "h":
                            //If the player has hit it runs the dealer Hit method and passes in itself
                            Console.Write("\nHit\n");
                            dealer.HitPlayer(this);
                            continueActions = true;
                            Thread.Sleep(1000);
                            break;
                        case "s":
                            //If the player stands It sets continue actions to false so we fall out of the loop
                            //No action is taken as standing means that nothing else has to be donw
                            Console.Write("\nStand\n");
                            continueActions = false;
                            Thread.Sleep(1000);
                            break;
                        case "d":
                            /*If the player has doubled down it runs the dealer Hit method and passes in itself
                            It then sets contiuous actions to false so we fall out of the loop
                            as players can't take anymore actions after doubling down
                            It then calls the DOuble down method to ask the player for an additional bet*/
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
                        /*Checks if the card total is 21 and then sets Continuous Action to false
                         * Because if the player is at 21 no more game actions can be taken*/
                        Console.WriteLine("\nYou Hit 21!");
                        continueActions = false;
                        Thread.Sleep(1000);
                    }
                    else if (CardTotal > 21)
                    {
                        /*Checks if the player has gone over 21 and then sets Continuous Actions to false
                         * because the player is over 21 no more game actions can be taken
                         * It also sets IsBust to true as the player has Bust*/                         
                        Console.WriteLine("\nBust");
                        IsBust = true;
                        continueActions = false;
                        Thread.Sleep(1000);
                    }
                } while (continueActions == true);
            }

            else
            {
                //If IsBlackJack is true then it displays this
                Console.WriteLine("Blackjack!!!");
                Console.WriteLine("You Won 1.5x Your Bet!!!");
                Thread.Sleep(1500);
            }

            return dealer;
            
        }

        /*This method displays the hand to the screen
         * Each Card has a visual component
         * That component is an array of strings that has a length of 5
         * The array represents every line of the card visual from top to bottom
         * Because the display has to display multiple cards in a row
         * It initially loops 5 times for the length of the card visual array
         * It then loops again for each card in hand and displays each line
         * for each card in hand in a row with a space between*/
        public void DisplayHand()
        {
            //Calculates hand total before displaying so it can then display the Hand total beneath the cards
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
            //Adds each visible card value to the total value of the hand
            //This is so the dealers hand total only shows the value of the visible card
            foreach (Card card in Hand)
            {
                if (card.IsVisible == true)
                {
                    CardTotal += card.Value;
                }
            }

            /*Loops through each card in hand and checks for aces
            If the ace puts the value over 21 then it will reduce the total by 10
            as aces can either be 11 or 1

            I put them in seperate loops because if Hand[0] is the ace but Hand[3] is the 
            card that puts the total over 21 then the change would never be made so I have
            to add everything seperately first and then go through and check for aces
            */
            foreach (Card card in Hand)
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
                //Asks player for their initial bet
                incorrectEntry = false;
                Console.WriteLine(String.Format("{0} Has {1:C2} left to bet.", Name, Money));
                Console.Write($"{Name}, How Much Would You Like to Bet on this Hand: ");

                /*Tries to convert the players entry to Int32
                //It makes the player reenter if it can't convert or if the Bet is Less than 0 or if the bet
                Is greater than the amount of money they have left
                It then removes the bet from the total money*/
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

            Bet += bet;
            Money -= bet;
            Console.Clear();
        }

        public void DoubleDown()
        {
            //same as Make Bet() except it doesn't clear the console
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

        //Just methods to read private variables
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


        /*Win() and Lose() are called when the game is checking for winners
         * Win() adds back the full bet and winnings which is equal to the bet
         * It then displays that the player has won and what their winnings are
         * Lose() Doesn't make any changes as the bet has already been taken from Money
         * It then displays that the player has lost and how much they lost
         */ 
        public void Win()
        {
            Money += Bet * 2;
            Console.WriteLine();
            Console.WriteLine(String.Format("{0} Won {1:C2}!", Name, Bet));
            Console.WriteLine(String.Format("{0} Total Money is now: {1:C2}", Name, Money));
        }

        public void Lose()
        {
            Console.WriteLine();
            Console.WriteLine(String.Format("{0} Lost {1:C2}!", Name, Bet));
            Console.WriteLine(String.Format("{0} Total Money is now: {1:C2}", Name, Money));
        }

        /*Push is called when the Player and dealer have Stayed on the same hand total
         * Push adds the bet back to the players money and gives no winnings
         */
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

        //Check blackjack is only called after initial deals so if the Card value equals 21 after initial deal it sets
        //Is blackjack to true
        public void CheckBlackjack()
        {
            if(CardTotal == 21)
            {
                IsBlackjack = true;
            }
        }

        /*Black Jack is called when the Blackjack game is checking winner
         * It then adds back the bet to money and the winnings
         * which are 1.5 times the bet
         * It then displays a message saying that the player has got blacjack
         * and how much was won
         */
        public void BlackJack()
        {
            Money += (Bet * (decimal)2.5);
            Console.WriteLine();
            Console.WriteLine("Blackjack!");
            Console.WriteLine(String.Format("{0} Won {1:C2}!", Name, Bet * (decimal)1.5));
            Console.WriteLine(String.Format("{0} Total Money is now: {1:C2}", Name, Money));
        }

        /*Reset is called if the player wants to play again
         * It resets all the values that were changed during the course of the program
         */
        public void Reset()
        {
            Hand.Clear();
            SplitHand.Clear();
            CardTotal = 0;
            IsBust = false;
            Bet = 0;
            IsBlackjack = false;
        }

        /*Cash Out is called if the player doesn't want to play again or the player runs out of money
         * It displays a thank you message and the final total of money the player leaves with
         */
        public void CashOut()
        {
            Console.WriteLine($"Thank You For Playing {Name}!");
            Console.WriteLine(String.Format("You are Cashing Out with {0:C2}", Money));
            Console.WriteLine("We Hope to see you again soon!");
        }
    }

    
}
