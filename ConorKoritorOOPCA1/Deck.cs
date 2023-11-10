using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Players;
using BlackJackProgram;

namespace Decks
{
    internal class Deck
    {
        protected readonly List<Card> _cards = new List<Card>();
        private string FilePath = @"Cards.txt";

        public Deck()
        {
            /*The deck takes in no values in the constructor.
            When an object of deck is created it opens a file that lists every  card in a deck
            It loops through that entire list
            splits each line at commas into an array called split
            and then creates a card with the data retrieved
            split[0] is the symbol of the card
            split[1] is the value
            split[2] is the suit
            */
            string[] lines = File.ReadAllLines(FilePath);
            foreach(string line in lines)
            {
                string[] split = line.Split(',');

                Card c = new Card(split[0], int.Parse(split[1]), split[2]);
                _cards.Add(c);
            }
        }

        public List<Card> GetCards()
        {
            return _cards;
        }
    }
}
