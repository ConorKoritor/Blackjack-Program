using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Decks;
using BlackJackProgram;
using System.Data.SqlTypes;
using System.Reflection.PortableExecutable;
using System.Runtime.CompilerServices;

namespace Players
{
    internal class Dealer : Player
    {
        private Shoe Shoe { get; set; }
        private List<Player> _playersInGame {get; set;}

        public Dealer(Shoe shoe, List<Player> players) : base(0, "Dealer")
        {
            Shoe = shoe;
            _playersInGame = players;
        }

        //Calls the shuffle method in shoe so the shoe is shuffled before dealing
        public void ShuffleShoe()
        {
            Shoe.Shuffle();
        }

        /*Initial deal is called at the beggining of the Blackjack Game
         * The for loop loops twice as each player and the dealer are dealt 2 cards
         * It then loops through the list of all players in the game and deals a card
         * per loop in the for loop
         * It then deals a card to the dealer for each loop in the for loop
         */
        public void InitialDeal()
        {
            for(int i = 0; i<2; i++)
            {
                for(int j = 0; j < _playersInGame.Count; j++)
                {
                    //To deal it takes the top card of the show, adds it to the players hand and
                    //then removes the card from the shoe
                    _playersInGame[j].Hand.Add(Shoe._shuffledCardsInShoe[0]);
                    Shoe._shuffledCardsInShoe.RemoveAt(0);
                }

                Hand.Add(Shoe._shuffledCardsInShoe[0]);
                Shoe._shuffledCardsInShoe.RemoveAt(0);
            }

            //This checks if the Dealer has hit a blackjack off the initial deal
            CheckBlackjack();
            if (IsBlackjack == false)
            {
                /*If the dealer has not hit a black jack
                it makes the first card in the hand not visible 
                As the dealer deals 1 card face down
                and one card face up in blackjack*/

                Hand[0].MakeNotVisible();
                DisplayHand();
            }
            else
            {
                /*If the dealer has hit blackjack off the initial deal
                 * It displays both cards as visible
                 * because the dealer immediately flips the 
                 * face down card when there is a blackjack
                 */
                DisplayHand();
            }
        }

        //Hit player is called whenever the player asks for a hit
        public void HitPlayer(Player player)
        {
            /*When the player hits it adds the top card
             * in the shoe to the players hand
             * and then removes that card from the shoe
             * This method takes in the player that hit
             * so it deals to the right player
             */
            player.Hand.Add(Shoe._shuffledCardsInShoe[0]);
            Shoe._shuffledCardsInShoe.RemoveAt(0);

            //Program displays the card and bet after the hit
            player.DisplayHand();
            player.DisplayBet();
        }

        //This runs the dealers actions
        //It is called after the players have finished their actions
        //The dealer hits until their Card total is over 17
        //It is a hard 17 so the dealer can't stay when the total is equal to 17
        public void DealerAI()
        {
            //Dealer flips their first card before taking actions and the
            //program displays the hand with the first card visible
            FlipCard();
            DisplayHand();

            //Hits while Card total is less than or equal to 17
            //All the thread sleeps are so the player has time to take in what the dealer is doing
            while (CardTotal <= 17)
            {
                //Same as any other deal the top card of the shoe is added to
                //the dealers hand and that card is removed from the shoe
                Hand.Add(Shoe._shuffledCardsInShoe[0]);
                Shoe._shuffledCardsInShoe.RemoveAt(0);
                Thread.Sleep(2500);
                Console.WriteLine("\n\nHit \n");
                DisplayHand(); 
            }
            Console.WriteLine();
            /*Checks if the card total is less than or equal to
             * 21 after the dealer has taken their actions
             * if it is the Console displays the message stay
             */
            if (CardTotal <= 21)
            {
                Thread.Sleep(2500);
                Console.Write("\nStay");
            }
            /*If card total is not <= 21 after the dealers game actions
             * it means they bust.
             * this sets is bust = true and then displays 
             * that the dealer has bust
             */
            else
            {
                Thread.Sleep(2500);
                Console.Write("\nBust");
                IsBust = true;
            }

        }

        //Called when the dealer begins their actions. It calls the Make visible method in card which changes
        //its visual to show the face of the card
        public void FlipCard()
        {
            Hand[0].MakeVisible();
        }

    }

   
}
