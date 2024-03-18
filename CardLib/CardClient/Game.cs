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
            // Jocul se deruleaza doar atunci cand exista jucatori
            if (players == null)
                return -1;
            // imparte mana initiala de carti
            DealHands();
            // initializeaza variabilele, inclusiv un card initial plasat pe masa de joc: playCard
            bool GameWon = false;
            int currentPlayer;
            Card playCard = playDeck.GetCard(currentCard++);
            discardedCards.Add(playCard);
            // Secventa repetitiva principala, continua pana cand GameWon == true
            do
            {
                // Trece pe la fiecare jucator in fiecare tura
                for (currentPlayer = 0; currentPlayer < players.Length; currentPlayer++) 
                {
                    // Scrie jucatorul curent, mana jucatorului si cardul de pe masa
                    Console.WriteLine($"{players[currentPlayer].Name}, este randul tau.");
                    Console.WriteLine("Mana curenta:");
                    foreach (Card card in players[currentPlayer].PlayHand)
                    {
                        Console.WriteLine(card);
                    }
                    Console.WriteLine($"Cardul in joc: {playCard}");
                    //Spune jucatoruluui sa ia cardul de pe masa sau sa traga unul nou.
                    bool inputOK = false;
                    do
                    {
                        Console.WriteLine("Apasa tasta T (Take) pentru a lua cardul de pe masa sau D (Draw) pentru a extrage unul nou:");
                        string input = Console.ReadLine();
                        if (input.ToLower() == "t")
                        {
                            // Adauga cartea/cardul la mana jucatorului.
                            Console.WriteLine($"Ai extras: {playCard}");
                            // Inlatura cartile "uzate" (decartate?) daca este posibil (cand teancul este reamestecat nu va mai fi acolo)
                            if (discardedCards.Contains(playCard))
                            {
                                discardedCards.Remove(playCard);
                            }
                            players[currentPlayer].PlayHand.Add(playCard);
                            inputOK = true;
                        }
                        if (input.ToLower() == "d")
                        {
                            // Adauga cartea din teanc la mana jucatorului.
                            Card newCard;
                            // Adauga cartea numai daca nuu se afla in mana altui jucator sau a fost decartata anterior.
                            bool cardIsAvailable;
                            do
                            {
                                newCard = playDeck.GetCard(currentCard++);
                                // Verifica daca carea a fost decartata anterior.
                                cardIsAvailable = !discardedCards.Contains(newCard);
                                if (cardIsAvailable)
                                {
                                    // Verifica mainile fiecarui jucator pentru a vedea daca newCard se afla deja acolo.
                                    foreach (Player testPlayer in players)
                                    {
                                        if (testPlayer.PlayHand.Contains(newCard))
                                        {
                                            cardIsAvailable = false;
                                            break;
                                        }
                                    }
                                }
                            } while (!cardIsAvailable);
                            // Adauga cardul gasit la mana jucatorului.
                            Console.WriteLine($"Ai extras: {newCard}");
                            players[currentPlayer].PlayHand.Add(newCard);
                            inputOK = true;
                        }
                    } while (inputOK == false);
                    // Afiseaza noua mana de carti numerotata.
                    Console.WriteLine("Noua mana de carti:");
                    for (int i = 0; i < players[currentPlayer].PlayHand.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}: {players[currentPlayer].PlayHand[i]}");
                    }
                    // Spune jucatorului sa decarteze un card.
                    inputOK = false;
                    int choice = -1;
                    do
                    {
                        Console.WriteLine("Alege un card pe care sa il decartezi:");
                        string input = Console.ReadLine();
                        try
                        {
                            // Incearca sa convertesti input-ul intr-un numar valid de card.
                            choice = Convert.ToInt32(input);
                            if ((choice > 0) && (choice <= 8))
                                inputOK = true;
                        }
                        catch
                        {
                            // Ignora conversiile esuate, continua prompt-ul
                        }
                    } while (inputOK == false);
                    // Adauga referinta la cartile inlaturate in playCard (adauga cardul pe masa),
                    // apoi inlatura cardul din mana jucatorului si adauga la teancul cartilor decartate.
                    playCard = players[currentPlayer].PlayHand[choice - 1];
                    players[currentPlayer].PlayHand.RemoveAt(choice - 1);
                    discardedCards.Add(playCard);
                    Console.WriteLine($"Se decarteaza: {playCard}");
                    // spatiaza textul; incearca mai tarziu cu "\n"
                    Console.WriteLine();
                    // Vezi daca jucatorul a castigat, si iesi din structura repetitiva daca este cazul.
                    GameWon = players[currentPlayer].HasWon();
                    if (GameWon == true)
                        break;
                }
            } while (GameWon == false);
            // Incheie jocul, anuntand castigatorul.
            return currentPlayer;
        }
    }
}
