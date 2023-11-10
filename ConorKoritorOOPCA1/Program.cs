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

            amountOfPlayers = AmountOfPlayersEntry();
            amountOfDecks = AmountOfDecksEntry();

            Console.Clear();

            BlackjackGame blackjackGame = new BlackjackGame(amountOfPlayers, amountOfDecks);
        }

        static int AmountOfPlayersEntry()
        {
            int amountOfPlayers = 0;
            bool incorrectEntry;

            do
            {
                incorrectEntry = false;
                Console.Write($"Enter Amount of Players (1-4): ");

                try
                {
                    amountOfPlayers = Convert.ToInt32(Console.ReadLine());
                }

                catch (FormatException)
                {
                    // Tells the user what they did wrong
                    Console.WriteLine("You Entered the Amount of Players Incorrectly. Don't use Letters or Special Characters.\n");
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

            do
            {
                incorrectEntry = false;
                Console.Write($"\nEnter Amount Of Decks in the Shoe (1-10): ");

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
                    Console.WriteLine("You Entered the Amount of Decks Incorrectly. Use numbers between 1 and 10.");
                    incorrectEntry = true;
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("You Entered the Amount of Decks Incorrectly. Use numbers between 1 and 10.");
                    incorrectEntry = true;
                }

                if (amountOfDecks < 1 || amountOfDecks > 10)
                {
                    Console.WriteLine("You Entered the Amount of Decks Incorrectly. Use numbers between 1 and 10.");
                    incorrectEntry = true;
                }

            } while (incorrectEntry == true);

            return amountOfDecks;
        }
    }
}