using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Players;
using BlackJackProgram;

namespace Decks
{
    internal class Shoe
    {
        private int AmountOfDecks { get; set; }
        private Deck[] DecksInShoe;
        private List<Card> _cardsInShoe = new List<Card>();
        public List<Card> _shuffledCardsInShoe = new List<Card>();

        public Shoe()
        {
            AmountOfDecks = 0;
        }

        public Shoe(int amountOfDecks)
        {
            AmountOfDecks = amountOfDecks;

            DecksInShoe = new Deck[AmountOfDecks];

            for(int i = 0; i < AmountOfDecks; i++)
            {
                Deck d = new Deck();

                DecksInShoe[i] = d;

                List<Card> _cardsInDeck = DecksInShoe[i].GetCards();

                foreach (Card c in _cardsInDeck)
                {
                    _cardsInShoe.Add(c);
                }
            }
        }

        public List<Card> Shuffle()
        {
            _shuffledCardsInShoe.Clear();
            foreach(Card c in _cardsInShoe)
            {
                _shuffledCardsInShoe.Add(c);
            }
            _shuffledCardsInShoe.Shuffle();
            return _shuffledCardsInShoe;
        }

        public void DisplayCards()
        {
            foreach(Card card in _cardsInShoe)
            {
                Console.WriteLine(card);
            }

        }
    }

    public static class ThreadSafeRandom
    {
        [ThreadStatic] private static Random Local;

        public static Random ThisThreadsRandom
        {
            get { return Local ?? (Local = new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId))); }
        }
    }

    static class MyExtensions
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;

            while (n > 1)
            {
                n--;
                int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
