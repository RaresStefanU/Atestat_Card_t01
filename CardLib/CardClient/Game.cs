using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CardLib;

namespace CardClient
{
    public class Game
    {
        private int currentCard;
        private Deck playDeck;
        private Player[] players;
        private Cards discardedCards;
        public Game() 
        {
            currentCard = 0;
            playDeck = new Deck(true);
            playDeck.LastCardDrawn += LastCardDrawnEventHandler;
            playDeck.Shuffle();
            discardedCards = new Cards();
        }
        private void LastCardDrawnEventHandler(object source, EventArgs args)
        {
            Console.WriteLine("Discarded cards reshuffled into deck.");
            ((Deck)source).Shuffle();
            discardedCards.Clear();
            currentCard = 0;
        }
        public void SetPlayers(Player[] newPlayers)
        {
            if (newPlayers.Length > 7)
                throw new ArgumentException("A maximum of 7 players may play this game.");
            if (newPlayers.Length < 2)
                throw new ArgumentException("A minimum of 2 players may play this game.");
            players = newPlayers;
        }
        private void DealHands()
        {
            for (int p = 0; p <= players.Length; p++) 
            {
                for (int c = 0; c < 7; c++) 
                {
                    players[p].PlayHand.Add(playDeck.GetCard(currentCard));
                }
            }
        }
        public int PlayGame()
        {
            // "Code to follow" they said
        }
    }
}
