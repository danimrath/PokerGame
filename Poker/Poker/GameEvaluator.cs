using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace GameEvaluator
{
    static class Constants
    {
        public const string AlphanumericRegex = "^[a-zA-Z0-9]*$"; 
    }
    // Implements an object that can evaluate poker hands.
    // Constructor can take a series of strings representing players and their hands.
    // Provides methods to Add, Evaluate and clear the players.
    // ToString will print the list of players and their best hands.
    // Player objects can be created externally and compared to each other if desired.
    public class PokerHandEvaluator
    {
        public PokerHandEvaluator()
        {
            this.players = new ArrayList();
        }
        //passes player strings on to AddPlayer which creates new Player objects
        //validation is handled in the Player constructor
        public PokerHandEvaluator(params string[] playerStrings)
        {
            this.players = new ArrayList();
            foreach (string playerHand in playerStrings)
            {
                AddPlayer(playerHand);
            }
        }
        public void AddPlayer(Player newPlayer)
        {
            this.players.Add(newPlayer);
        }
        public void AddPlayer(string playerHand)
        {
            this.players.Add(new Player(playerHand));
        }
        public void AddPlayer(string playerName,string hand)
        {
            this.players.Add(new Player(playerName, hand));
        }
        //Override ToString to print all players and their best hands.
        public override string ToString()
        {
            string toString = "";
            if (players.Count > 0)
            {
                foreach (Player player in players)
                {
                    toString += player + "\n";
                }
                return toString;
            }
            else
            {
                return "No players found.";
            }
        }
        //EvaluateHands calls for a sort of the players
        //The winners will be all matching players at the front of the ArrayList
        public void EvaluateHands()
        {
            players.Sort();
            if (players.Count > 0)
            {
                Player topPlayer = (Player)players[0];
                foreach (Player player in players)
                {
                    if (player.CompareTo(topPlayer) == 0)
                    {
                        Console.WriteLine(player);
                        Debug.WriteLine(player);
                    }
                }
            }
            else
            {
                Console.WriteLine("No players found.");
                Debug.WriteLine("No players found.");
            }
        }
        //ClearPlayers removes the current players
        public void ClearPlayers()
        {
            this.players = new ArrayList();
        }
        //we only store Player type objects in this ArrayList
        private ArrayList players;
    }
    //Player stores the players name and their Hand
    public class Player : IComparable
    {
        //if there is only a single string passed in attempt to split it
        //into a name and hand of cards. Otherwise just use it as the name.
        public Player(string playerHand)
        {
            this.playerHand = null;
            if (playerHand.IndexOf(',') >= 0)
            {
                this.name = playerHand.Substring(0, playerHand.IndexOf(','));
                deal(playerHand.Substring(playerHand.IndexOf(',') + 1));
            }
            else
                this.name = playerHand;
        }
        //Set name and attempt to deal cards into playerHand
        public Player(string name,string cards)
        {
            this.playerHand = null;
            this.name = name;
            deal(cards);
        }
        //CompareTo for players handles the case of no Hand.
        //If both players have a hand then pass compare to the Hand.CompareTo
        public int CompareTo(Object rhs)
        {
            Player r = (Player)rhs;
            if (r.playerHand == null)
            {
                if (this.playerHand == null)
                {
                    return 0;
                }
                return -1;
            }
            else if (this.playerHand == null)
            {
                return 1;
            }
            return this.playerHand.CompareTo(r.playerHand);
        }
        //create a new Hand with the string of cards
        public void deal(string cards)
        {
            this.playerHand = new Hand(cards);
        }
        public override string ToString()
        {
            if (playerHand != null)
            {
                if (playerHand.bestHand != PokerHand.NoHand)
                    return name + ":" + playerHand.bestHand + " " + playerHand.highCard + " High";
                else
                    return name + ":" + playerHand.bestHand;
            }
            else
                return name + ":No Hand";

        }
        private Hand playerHand;
        private string name;
    }
    //Hand stores the ArrayList of cards as well as the best hand and high card
    public class Hand : IComparable
    {
        public Hand(string cards)
        {
            this.cards = new ArrayList();
            string[] newCards = cards.Split(',');
            
            try
            {
                //if there are not exactly five cards the hand is invalid.
                if (newCards.Count() != 5)
                    throw new System.ArgumentException("Invalid number of cards:" + cards, "cards");
                foreach (string card in newCards)
                {
                    //attempt to create and add the cards to the Hand.
                    this.cards.Add(new Card(card));
                }
                //Sort orders the cards from highest to lowest value
                this.cards.Sort();
                //determine the best possible hand and save it
                bestHand = BestHand();
            } catch {
                this.cards = null;
                bestHand = PokerHand.NoHand;
            }

        }
        public override string ToString()
        {
            string output = "";
            if (cards != null)
            {
                foreach (Card card in cards)
                {
                    output += card + " ";
                }
            }
            else
                output = "No Hand";
            return output;
        }
        public int CompareTo(Object rhs)
        {
            Hand r = (Hand)rhs;
            if (r.bestHand < this.bestHand)
            {
                return -1;
            }
            else if (r.bestHand == this.bestHand)
            {
                if (r.highCard < this.highCard)
                {
                    return -1;
                }
                else if (r.highCard == this.highCard)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                return 1;
            }
        }
        private PokerHand BestHand()
        {
            if (hasFlush(this.cards))
            {
                return PokerHand.Flush;
            }
            else if (findMatches(this.cards)==3)
            {
                return PokerHand.ThreeMatch;
            }
            else if (findMatches(this.cards)==2)
            {
                return PokerHand.OnePair;
            }
            else 
            {
                Card firstCard = (Card)cards[0];
                highCard = firstCard.getValue();
                return PokerHand.HighCard;
            }
        }
        private CardValue HighCard()
        {
            return highCard;
        }
        private bool hasFlush(ArrayList cards)
        {
            Card compareCard = (Card)cards[0];
            foreach(Card currentCard in cards)
            {
                if (!compareCard.CompareSuite(currentCard) || currentCard == null)
                {
                    return false;
                }
            }
            highCard = (CardValue)compareCard.getValue();
            return true;
        }
        private int findMatches(ArrayList cards)
        {
            int count = 0;
            int highCount = 1;
            foreach (Card currentCard in cards)
            {
                count = 0;
                foreach (Card compareCard in cards)
                {
                    if (currentCard.CompareTo(compareCard) == 0) count++;
                    if (count > highCount)
                    {
                        highCard = currentCard.getValue();
                        highCount = count;
                    }
                }
            }
            return highCount;
        }
        public PokerHand bestHand;
        public CardValue highCard;
        private ArrayList cards;
    }
    //Card stores a string, Suite and a CardValue
    public class Card : IComparable
    {
        public Card(string card)
        {
            //Remove whitespace, check for non-alphanumerical characters, nulls and number of chars
            card = card.Trim();
            Regex regex = new Regex(Constants.AlphanumericRegex);
            if (!string.IsNullOrEmpty(card) && card.Length <= 3 && regex.IsMatch(card) )
            {
                //pull the suite and value apart
                string suite = card.Substring(card.Length - 1);
                string value = card.Substring(0, card.Length - 1);
                this.card = card;
                //assign the Suite and CardValue enums or failing that throw an error
                if (!assignValue(value) || !assignSuite(suite))
                {
                    Debug.WriteLine("Invalid Card:" + card);
                    this.card = null;
                    throw new System.ArgumentException("Card is not valid:" + card, "card");
                }
            }
            else
            {
                Debug.WriteLine("Invalid Card:" + card);
                this.card = null;
                throw new System.ArgumentException("Card is not valid:" + card, "card");
            }
        }
        //assign Suite enum or return false if not a valid Suite
        private bool assignSuite(string suite)
        {
            switch (suite.ToUpper())
            {
                case "H":
                    this.suite = Suite.Hearts;
                    break;
                case "C":
                    this.suite = Suite.Clubs;
                    break;
                case "S":
                    this.suite = Suite.Spades;
                    break;
                case "D":
                    this.suite = Suite.Diamonds;
                    break;
                default:
                    return false;
            }
            return true;
        }
        //assign CardValue enum or return false if not a valid value
        private bool assignValue(string value)
        {
            switch (value.ToUpper())
            {
                case "A":
                    this.value = CardValue.Ace;
                    break;
                case "2":
                    this.value = CardValue.Two;
                    break;
                case "3":
                    this.value = CardValue.Three;
                    break;
                case "4":
                    this.value = CardValue.Four;
                    break;
                case "5":
                    this.value = CardValue.Five;
                    break;
                case "6":
                    this.value = CardValue.Six;
                    break;
                case "7":
                    this.value = CardValue.Seven;
                    break;
                case "8":
                    this.value = CardValue.Eight;
                    break;
                case "9":
                    this.value = CardValue.Nine;
                    break;
                case "10":
                    this.value = CardValue.Ten;
                    break;
                case "J":
                    this.value = CardValue.Jack;
                    break;
                case "Q":
                    this.value = CardValue.Queen;
                    break;
                case "K":
                    this.value = CardValue.King;
                    break;
                default:
                    return false;
            }
            return true;
        }
        //Default CompareTo for a card is determined by the CardValue
        public int CompareTo(Object rhs)
        {
            Card r = (Card)rhs;
            if (r.value < this.value)
            {
                return -1;
            }
            else if (r.value == this.value)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        //CompareSuite allows comparison of two cards Suites
        public bool CompareSuite(Card rhs)
        {
            if (rhs.suite == this.suite)
            {
                return true;
            }
            return false;
        }
        //Card ToString returns the string representation of the card
        public override string ToString()
        {
            return card;
        }
        //Accessors for CardValue and Suite.
        public CardValue getValue() { return value; }
        public Suite getSuite() { return suite; }
        private Suite suite;
        private CardValue value;
        private string card;
    }
    public enum Suite
    {
        Clubs,
        Hearts,
        Spades,
        Diamonds
    }
    public enum PokerHand
    {
        NoHand,
        HighCard,
        OnePair,
        TwoPair,
        ThreeMatch,
        Straight,
        Flush,
        FullHouse,
        FourMatch,
        StraightFlush
    }
    public enum CardValue
    {
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace
    }
}