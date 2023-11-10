using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Decks;
using BlackJackProgram;
using System.Data.SqlTypes;

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

        public void ShuffleShoe()
        {
            Shoe.Shuffle();
        }

        public void InitialDeal()
        {
            for(int i = 0; i<2; i++)
            {
                for(int j = 0; j < _playersInGame.Count; j++)
                {
                    _playersInGame[j].Hand.Add(Shoe._shuffledCardsInShoe[0]);
                    Shoe._shuffledCardsInShoe.RemoveAt(0);
                }

                Hand.Add(Shoe._shuffledCardsInShoe[0]);
                Shoe._shuffledCardsInShoe.RemoveAt(0);
            }

            CheckBlackjack();
            if (IsBlackjack == false)
            {
                Hand[0].MakeNotVisible();
                DisplayHand();
            }
            else
            {
                DisplayHand();
            }
        }

        public void HitPlayer(Player player)
        {
            player.Hand.Add(Shoe._shuffledCardsInShoe[0]);
            Shoe._shuffledCardsInShoe.RemoveAt(0);
            player.DisplayHand();
            player.DisplayBet();
        }

        public void DealerAI()
        {
            FlipCard();
            DisplayHand();

            while (CardTotal < 17)
            {
                Hand.Add(Shoe._shuffledCardsInShoe[0]);
                Shoe._shuffledCardsInShoe.RemoveAt(0);
                Thread.Sleep(2000);
                Console.WriteLine("\n\nHit \n");
                DisplayHand(); 
            }
            Console.WriteLine();
            if (CardTotal <= 21)
            {
                Thread.Sleep(2000);
                Console.Write("\nStay");
            }
            else
            {
                Thread.Sleep(2000);
                Console.Write("\nBust");
                IsBust = true;
            }

        }

        public void FlipCard()
        {
            Hand[0].MakeVisible();
        }

    }

   
}
