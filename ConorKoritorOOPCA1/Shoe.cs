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


        public Shoe(int amountOfDecks)
        {
            AmountOfDecks = amountOfDecks;

            DecksInShoe = new Deck[AmountOfDecks];

            /*Shoe takes in the amount of decks from the user
             * and then loops that many times
             * Each time it loops it crates a new deck. 
             * It then gets every card in that new deck
             * and adds it to the shoe one by one
             */
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
            /*Shuffle sets the list shuffled card in shoe
             * equal to the list of cards in shoe
             * It then uses the extension shuffle on Shuffled cards in shoe.
             * This is so the cards in shoe doesn't get shuffled so if you reshuffle
             * it will be with all of the cards in that shoe
             */
            _shuffledCardsInShoe.Clear();
            _shuffledCardsInShoe = _cardsInShoe;
            _shuffledCardsInShoe.Shuffle();
            return _shuffledCardsInShoe;
        }

       /* public void DisplayCards()
        {
            foreach(Card card in _cardsInShoe)
            {
                Console.WriteLine(card);
            }

        }
       */
    }

    
    public static class ThreadSafeRandom
    {
        /*Creates a new random object so that shuffle can randomize the order of the cards
        * This new random is created with a seed equal to the amount of ticks
        * the program has run for times 31 plus the ID of the current thread
        * This increases the randomness compared to a new random object with no seed
        */
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
            /* This extension can be used for Lists
             * It takes in the list and randomises the order of that list
             * It first gets the count of how many items are in the list
             * which is labelled by n
             */
            int n = list.Count;

            /*With n being the length of the list
             * this loop iterates through each item
             * in the list starting from the index List[List Length]
             * and ending at List[0]
             */
            while (n > 1)
            {
                n--;
                
                //k is set to a random index from 1-List.Count
                int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);

                //A obj of T(the type of values in the list) is created that is equal to the item in the list 
                //at index k
                T value = list[k];

                //the item at the index k in the list is then set to the item at index n of the list
                list[k] = list[n];

                //the item at the index n of the list is now set to the value that was originally at index k
                list[n] = value;
            }
        }
    }
}
