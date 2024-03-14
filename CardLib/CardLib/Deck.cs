using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CardLib
{
    public class Deck : ICloneable
    {
        public event EventHandler LastCardDrawn;

        public object Clone()
        {
            Deck newDeck = new Deck(cards.Clone() as CardCollection);
            return newDeck;
        }
        private Deck(CardCollection newCards) => cards = newCards;

        //private Card[] cards;
        private CardCollection cards = new CardCollection();

        public Deck()
        {
            //cards = new Card[52];
            for (int suitVal = 0; suitVal < 4; suitVal++)
            {
                for (int rankVal = 1; rankVal < 14; rankVal++)
                {
                    //cards[suitVal * 13 + rankVal - 1] = new Card((Suit)suitVal, (Rank)rankVal);
                    cards.Add(new Card((Suit)suitVal, (Rank)rankVal));
                }
            }
        }

        //da voie la optarea pt as ca si carte mare
        public Deck(bool isAceHigh) : this()
        {
            Card.isAceHigh = isAceHigh;
        }

        //da voie sa folosesti un atu
        public Deck(bool useTrumps, Suit trump) : this()
        {
            Card.useTrumps = useTrumps;
            Card.trump = trump;
        }

        //Da voie asilor si atuurilor
        public Deck(bool isAceHigh, bool useTrumps, Suit trump) : this()
        {
            Card.isAceHigh = isAceHigh;
            Card.useTrumps = useTrumps;
            Card.trump = trump;
        }

        public Card GetCard(int cardNum)
        {
            if (cardNum >= 0 && cardNum <= 51)
            {
                if ((cardNum == 51) && (LastCardDrawn != null))
                    LastCardDrawn(this, EventArgs.Empty);
                return cards[cardNum];
            }
            else
                throw new CardOutOfRangeException(cards.Clone() as Cards);
        }

        public void Shuffle()
        {
            //Card[] newDeck = new Card[52];
            CardCollection newDeck = new CardCollection();
            bool[] assigned = new bool[52];
            Random sourceGen = new Random();
            for (int i = 0; i < 52; i++)
            {
                //int destCard = 0;
                int sourceCard = 0;
                bool foundCard = false;
                while (foundCard == false)
                {
                    //destCard = sourceGen.Next(52);
                    sourceCard = sourceGen.Next(52);
                    //if (assigned[destCard] == false)
                    if (assigned[sourceCard] == false)
                        foundCard = true;
                }
                //assigned[destCard] = true;
                //newDeck[destCard] = cards[i];
                assigned[sourceCard] = true;
                newDeck.Add(cards[sourceCard]);
            }
            //newDeck.CopyTo(cards, 0);
            newDeck.CopyTo(cards);
        }
    }
}
