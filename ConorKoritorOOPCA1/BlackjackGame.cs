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

        
        //Takes in A number of Players and Decks from Program.cs
        //Sot it can create the right amount of players and decks
        public BlackjackGame(int numberOfPlayers, int numberOfDecksInShoe)
        {
            //Creates a shoe with the amount of decks specified by the player
            _shoe = new Shoe(numberOfDecksInShoe);

            //Calls create players and passes in the amount of players
            CreatePlayers(numberOfPlayers);

            //Creates a new dealer passing in the shoe and the list of players
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

            /*This method takes in an integer which is equal to the number of 
             * players specified by the user.
             * it then loops that amount of times and asks for the 
             * player name and amount of starting money.
             * The money entry uses the same try catch as found in other parts of the code
             * It then creates a new player passing in the variables the user entered
             * and adds that new player to the player list that will be passed in
             * to the dealer
             * It then clears the console for the game
             */
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

        //Runs through each player in the list and calls the Make bet function
        //The logic of the make bet function is explained in Player.cs
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

            //In initial deal there is a call to check blackjack for the dealer
            //This checks if that function returned true
            if (_dealer.IsBlackjack == true)
            {
                //If is black jack is true it displayes that the dealer hit blackjack
                //and then checks if any of the players hit Blackjack
                //This is so when player.Push() is called it displays 
                //A message that the payer also hit blacjack
                Console.WriteLine("Dealer Hit Blackjack.");
                foreach (Player p in _playerList)
                {
                    p.CheckBlackjack();
                }
                //If the dealer hit blackjack it immediately goes to
                //Check for winners as no game actions have to be taken
                Thread.Sleep(2500);
                CheckWinners();
            }

            else
            {
                //This calls player actions so that after the initial deal
                //the players can take their actions
                PlayerActions();

                //Displays that the dealer will start playing and sleeps so that the player can read whats happening
                Console.WriteLine();
                Console.WriteLine("\nDEALERS TURN");
                Thread.Sleep(3000);

                //calls the dealer AI so the dealer can take their actions
                _dealer.DealerAI();

                Thread.Sleep(3000);

                //calls check winners when all actions have been taken
                CheckWinners();
            }
        }

        public void PlayerActions()
        {
            //loops through each player and calls the game action method so players can take their actions
            foreach(Player p in _playerList)
            {
               _dealer = p.GameActions(_dealer);
            }
        }

        public void CheckWinners()
        {
            string resetKey;
            int count = 0;

            //Clears console to display who won/lost and their winnings/losings
            Console.Clear();
            foreach (Player p in _playerList)
            {
                count++;

                //Runs through each player and checks multiple parameters to see if they won/lost/or pushed
                Thread.Sleep(1500);
                
                //Checks if the Player Blackjack is false before checking win conditions
                //This is because if the player hit blackjack you only have to check if their hand is equal to the dealers
                if (p.IsBlackjack == false)
                {

                    if (p.IsBust == true)
                    {
                        //Checks if the player has bust, displays a message saying they have, and then calls that players lose method
                        Console.WriteLine(p.GetName() + " Lost From Bust");
                        p.Lose();
                    }

                    else if (_dealer.IsBust == true)
                    {
                        //Checks if the dealer has bust, displays a message saying they have, and then calls that players win method
                        Console.WriteLine(p.GetName() + " Won From Dealer Bust");
                        p.Win();
                    }
                    else if (p.GetCardTotal() < _dealer.GetCardTotal())
                    {
                        //Checks if player stayed with a total less than the dealers, displays a message that they have
                        //and calls that players lose method
                        Console.WriteLine(p.GetName() + " Lost");
                        p.Lose();
                    }
                    else if (p.GetCardTotal() > _dealer.GetCardTotal())
                    {
                        //Checks if player stayed with a total less than the dealers, displays a message that they have
                        //and calls that players lose method
                        Console.WriteLine(p.GetName() + " Won");
                        p.Win();
                    }
                    else if (p.GetCardTotal() == _dealer.GetCardTotal())
                    {
                        //Checks if player stayed with a total equal to the dealers, displays a message that they have
                        //and calls that players lose method
                        p.Push();
                    }
                }

                //If player got blackjack check if the total is equal to the dealer which will result in a push
                //or if the player got uncontested balckjack which will result in a win of 1.5x the bet
                else
                {
                    if (p.GetCardTotal() == _dealer.GetCardTotal())
                    {
                        p.Push();
                    }
                    else
                    {
                        p.BlackJack();
                    }
                }
                
                
                //Checks if their is more than one player so that it can ask the user to press enter to continue 
                //to the next players results
                if (_playerList.Count > 1 && count < _playerList.Count)
                {
                    Console.WriteLine("\nPress Enter to see next players Result");
                    Console.ReadKey();
                    Console.Clear();
                }
                
            }

            foreach (Player p in _playerList)
            {
                //Checks if the player has run out of money
                //if they have it calls thagt players cash out method
                //and removes them from the list of players
                if (p.GetMoney() <= 0)
                {
                    Console.WriteLine($"{p.GetName()} Is out of Money");
                    p.CashOut();
                    _playerList.Remove(p);
                }

                //If they haven't run out of money the program asks if they want to play again
                //If they say no they are cashed out and removed from the player list
                //If they say yes they stay in the list
                else
                {
                    do
                    {
                        Console.Write($"{p.GetName()} Would You Like to Play Another Hand? [y,n]: ");
                        resetKey = Console.ReadLine();


                    } while (resetKey != "y" && resetKey != "n");

                    if (resetKey == "n")
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

            //Calls the reset function after checking winnings and who wants 
            Reset();
        }

        public void Reset()
        {
            //When all players have decided if they want to continue or not
            //or if they ran out of money
            //the game checks if their are any players left in the list
            //If their are The game resets all aprties and restarts the game
            //by calling dealer actions again

            if (_playerList.Count > 0)
            {
                foreach (Player p in _playerList)
                {
                    p.Reset();
                }
                _dealer.Reset();

                Console.Clear();

                foreach(Player p in _playerList)
                {
                    p.MakeBet();
                }

                DealerActions();
            }  
        }
    }
}
