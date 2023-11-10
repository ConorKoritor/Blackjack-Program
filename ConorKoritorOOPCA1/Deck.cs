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
