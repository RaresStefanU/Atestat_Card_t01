using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

#region comentarii vechi
// test 01 pt schimbari git --Functioneaza din Git Gui atunci cand inchid VS
// test 02 incercare din VisualStudio
// pas final in GitBash din folder-ul in care se afla .git-ul : git push -u origin main
// !!ATENTIE!! : NU DA COMMIT LA TOT. ADAUGA MAI INTAI DOAR FISIERELE SCHIMBATE DE TINE FOLOSIND SEMNUL DE "+" DIN DREAPTA (STAGE)
//              DOAR APOI APASA PE COMMIT & PUSH. (nu da stage la ceea ce se afla in \.vs\CardLib\...)
// test 03 push din Visual Studio de pe laptop
#endregion
/*
Problemele legate de git au fost rezolvate prin adaugarea unui ".gitignore".
ATENTIE: Inainte de a rula porgramul asigura-te ca ai indeplinit urmatorii pasi:
        1. Deschide "Solution Explorer"
        2. Da click-dreapta pe "CardClient"
        3. Da click-stanga pe "Set as Startup Project"
 */

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

        //public object Clone() => MemberwiseClone();//chestie care ori am ratat-o ori nu era || revizuieste?
        public override string ToString() => "The " + rank + " of " + suit + "s";

        //bunch of nonsense v1
        public static bool operator ==(Card card1, Card card2) => (card1?.suit == card2?.suit) && (card1?.rank == card2?.rank); //varianta care merge?
        //public static bool operator ==(Card card1, Card card2) => (card1.suit == card2.suit) && (card1.rank == card2.drank); //varianta lor
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
                        // return (card1.rank > card2.rank); //varianta lor
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