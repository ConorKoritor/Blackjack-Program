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

            //Code initally had unicode symbols for each suit
            //Switchs through the suits and sets the pip to match the suit
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

            //Creates the visual of the card that will be dsiplayed to the screen
            Visual = CreateCardVisual();

        }

        public string[] CreateCardVisual()
        {
            //the card visual is constructed of strings in an array that represent each line that must be drawn
            //Program checks if the card is supposed to be visible
            if(IsVisible)
            {
                //If it is visible it shows the face of the card which has the pip and the symbol
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
                //If it isn't visible it shows the back of the card
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
            //When make visible is run it recreates the card visual with the Is visible bool set to true
            IsVisible = true;
            Visual = CreateCardVisual();
        }

        public void MakeNotVisible()
        {
            //When make not visible is run it recreates the card visual with the Is visible bool set to false
            IsVisible = false;
            Visual = CreateCardVisual();
        }

        public string GetSymbol()
        {
            return Symbol;
        }

        public override string ToString()
        {
            string card = $"{Symbol} of {Suit}";
            return card;
        }
    }
}
