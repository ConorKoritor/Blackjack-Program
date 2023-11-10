using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Players;
using BlackJackProgram;

namespace Decks
{
    internal class Card
    {
        public int Value {  get; set; }
        private string Symbol { get; set; }
        private string Suit {get; set; }
        private string Pip { get; set; }
        public bool IsVisible = true;
        public string[] Visual = new string[5];

        public Card(string symbol, int value, string suit) 
        {
            Value = value;
            Symbol = symbol;
            Suit = suit;

            switch (suit){
                case "Hearts":
                    Pip = "H";
                    break;
                case "Diamonds":
                    Pip = "D";
                    break;
                case "Spades":
                    Pip = "S";
                    break;
                case "Clubs":
                    Pip = "C";
                    break;
                default:
                    Pip = "";
                    break;
            }

            Visual = CreateCardVisual();

        }

        public string[] CreateCardVisual()
        {
            if(IsVisible)
            {
                string[] CardVisual =
                {
                    "╔══════╗",
                   $"║{Pip}     ║",
                    "║      ║",
                   String.Format($"║{Symbol.PadLeft(6)}║"),
                    "╚══════╝"
                };
                return CardVisual;
            }
            else
            {
                string[] CardVisual = 
                {
                    "╔══════╗",
                    "║░░░░░░║",
                    "║░░░░░░║",
                    "║░░░░░░║",
                    "╚══════╝"
                };
                return CardVisual;
            }


        }

        public void MakeVisible()
        {
            IsVisible = true;
            Visual = CreateCardVisual();
        }

        public void MakeNotVisible()
        {
            IsVisible = false;
            Visual = CreateCardVisual();
        }

        public override string ToString()
        {
            string card = $"{Symbol} of {Suit}";
            return card;
        }
    }
}
