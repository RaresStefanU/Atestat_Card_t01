﻿using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace CardLib
{
    public class Card : ICloneable
    {
        //Flag pt carte mare (as din maneca)
        public static bool useTrumps = false;

        //folositor daca primul flag e adevarat
        public static Suit trump = Suit.Club;

        //flag care determina daca Asul este considerat cea mai mare carte sau mai mica decat 2
        public static bool isAceHigh = true;

        public object Clone() => MemberwiseClone();

        public readonly Rank rank;
        public readonly Suit suit;

        private Card() { }

        public Card(Suit newSuit, Rank newRank)
        {
            suit = newSuit;
            rank = newRank;
        }

        public override string ToString() => "The " + rank + " of " + suit + "s";

        //bunch of nonsense v1
        public static bool operator ==(Card card1, Card card2) => (card1?.suit == card2?.suit) && (card1?.rank == card2?.rank);
        public static bool operator !=(Card card1, Card card2) => !(card1 == card2);
        public override bool Equals(object card) => this == (Card)card;
        public override int GetHashCode() => 13 * (int)suit + (int)rank;
        public static bool operator >(Card card1, Card card2)
        { // why ,just why did I do this to myself? why whould you make this?!
            if (card1.suit == card2.suit) 
            {
                if (isAceHigh)
                {
                    if (card1.rank == Rank.Ace)
                    {
                        if (card2.rank == Rank.Ace)
                            return false;
                        else
                            return true;
                    }
                    else
                    {
                        if (card2.rank == Rank.Ace)
                            return false;
                        else
                            return (card1.rank > card2?.rank);
                    }
                }
                else
                {
                    return (card1.rank > card2.rank);
                }
            }
            else
            {
                if (useTrumps && (card2.suit == Card.trump))
                    return false;
                else
                    return true;
            }
        }
        public static bool operator <(Card card1, Card card2) => !(card1 >= card2);
        public static bool operator >=(Card card1, Card card2)
        {
            if (card1.suit == card2.suit)
            {
                if (isAceHigh)
                {
                    if (card1.rank == Rank.Ace)
                    {
                        return true;
                    }
                    else
                    {
                        if (card2.rank == Rank.Ace)
                            return false;
                        else
                            return (card1.rank >= card2.rank);
                    }
                }
                else
                {
                    return (card1.rank >= card2.rank);
                }
            }
            else
            {
                if (useTrumps &&(card2.suit == Card.trump))
                    return false;
                else
                    return true;
            }
        }
        public static bool operator <=(Card card1, Card card2) => !(card1 > card2);
    }
}