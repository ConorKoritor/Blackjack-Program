using Decks;
using Players;
using System.Xml.Linq;

namespace BlackJackProgram
{
    internal class Program
    {
        static void Main(string[] args)
        {
            int amountOfPlayers;
            int amountOfDecks;

            //Displays a splash screen with the rules
            BlackjackIntroScreen();
            Console.Clear();

            //Asks players for entry of the number of players and the number of decks in the shoe
            amountOfPlayers = AmountOfPlayersEntry();
            amountOfDecks = AmountOfDecksEntry();

            //clears the console for the blackjack game
            Console.Clear();

            //creates the blackjack game passing in the values for number of players and number of decks
            BlackjackGame blackjackGame = new BlackjackGame(amountOfPlayers, amountOfDecks);
        }

        static int AmountOfPlayersEntry()
        {
            int amountOfPlayers = 0;
            bool incorrectEntry;
            
            do
            {
                /*asks the player to enter a number betwewen 1 and 4 for the amount of players
                It is within a do/while loop so that if the player enters the number wrong it will
                loop to the top and ask the player to enter again*/
                incorrectEntry = false;
                Console.Write($"Enter Amount of Players (1-4): ");

                try
                {
                    //Tries to convert the entry string to Int32.
                    amountOfPlayers = Convert.ToInt32(Console.ReadLine());
                }

                catch (FormatException)
                {
                    // Tells the user what they did wrong
                    Console.WriteLine("You Entered the Amount of Players Incorrectly. Don't use Letters or Special Characters.\n");
                    
                    /*Sets incorrevct entry to true so the Do/While Loop will run again.
                    The Do/While Loop only exits if this value is false*/
                    incorrectEntry = true;

                }
                catch (OverflowException)
                {
                    Console.WriteLine("You Entered the Amount of Players Incorrectly. Use numbers between 1 and 4.\n");
                    incorrectEntry = true;
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("You Entered the Amount of Players Incorrectly. Use numbers between 1 and 4.\n");
                    incorrectEntry = true;
                }

                /*This is another catch after the try. Because entering a number outside of the bounds 
                 * of 1-4 wouldn't create an error I had to make it as an if statement*/
                if (amountOfPlayers < 1 || amountOfPlayers > 4)
                {
                    Console.WriteLine("You Entered the Amount of Players Incorrectly. Use numbers between 1 and 4.\n");
                    incorrectEntry = true;
                }

            } while (incorrectEntry == true);

            return amountOfPlayers;
        }

        static int AmountOfDecksEntry()
        {
            int amountOfDecks = 0;
            bool incorrectEntry;

            /* This works exactly the same as the Number of Players Entry. The only difference is that the amount of decks
             * in the shoe is limited to 2-8 instead of 1-4*/
            do
            {
                incorrectEntry = false;
                Console.Write($"\nEnter Amount Of Decks in the Shoe (2-8): ");

                try
                {
                    amountOfDecks = Convert.ToInt32(Console.ReadLine());
                }

                catch (FormatException)
                {
                    // Tells the user what they did wrong
                    Console.WriteLine("You Entered the Amount of Decks Incorrectly. Don't use Letters or Special Characters.");
                    incorrectEntry = true;

                }
                catch (OverflowException)
                {
                    Console.WriteLine("You Entered the Amount of Decks Incorrectly. Use numbers between 2 and 8.");
                    incorrectEntry = true;
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("You Entered the Amount of Decks Incorrectly. Use numbers between 2 and 8.");
                    incorrectEntry = true;
                }

                if (amountOfDecks < 2 || amountOfDecks > 8)
                {
                    Console.WriteLine("You Entered the Amount of Decks Incorrectly. Use numbers between 2 and 8.");
                    incorrectEntry = true;
                }

            } while (incorrectEntry == true);

            return amountOfDecks;
        }

        static void BlackjackIntroScreen()
        {
            //Creating a splash screen before the game to explain the rules and how the program works
            Console.WriteLine
                (
                "------------------------------------Welcome to Blackjack!------------------------------------" +
                "\nFirst You will be asked to enter the amount of players playing." +
                "\nThen you wil be asked to enter the amount of decks in the shoe." +
                "\nThe shoe is multiple decks shuffled together to try and stop card counting." +
                "\nThe program will now ask you to enter the Name of each player and their starting capital." +
                "\nThe dealer will deal out everyones cards." +
                "\nThe display will only show the Dealers and the First Players cards." +
                "\nIt will then ask the First Player if they want to Hit, Stay, or Double Down" +
                "\nWhen the first player Stays or Busts it will then show the next players cards"+
                "\n-------------------------------------------Rules-------------------------------------------" +
                "\nThe aim of the game is to get a card total as near to 21 as possible" +
                "\nTo win you want the total of your hand to be higher than the Dealers" +
                "\nIf your Hand Total goes over 21, you Bust and automatically lose the hand" +               
                "\nBlackjack" +
                "\n-When the dealer deals the cards everyone, including the Dealer, checks for Blackjack" +
                "\n-If the Dealer gets Blackjack, Play ends immediately" +
                "\n-Every player loses their bet unless they also got a Blackjack. See Push Rules if that is the case" +
                "\n-If the Dealer does not get a Blackjack but a player does, that player instantly wins 1.5x their bet"+
                "\nActions Player can take" +
                "\n-Hit: If a player asks for a Hit Another card is dealt to them and the value of that card is added to their total." +
                "\n-Double Down: You cannot Double Down after hitting. Doubling Down means that you must add up to" +
                "\n              100% of your original bet. Your are then dealt 1 more card and forced to stay." +
                "\n-Stay: Stay means that you are not dealt any more cards. You have locked in your hand for that round" +
                "\n-----------------------------------------Winnings------------------------------------------" +
                "\nIf you beat the Dealers total hand you get back your initial bet and win money equal to that bet" +
                "\nIf you tie with the Dealer this is known as a Push. You will be given your bet back but win no additional money" +
                "\nIf you win by Blackjack you will be given your bet back and win money equal to your bet times 1.5" +
                "\nGood Luck!!" +
                "\nPress Enter to Continue" 
                );

           
            Console.ReadKey();
        }
    }
}