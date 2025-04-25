using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlyDeck.Decks;
using SlyDeck.GameObjects.Card;

// Author: Ben Haines
namespace SlyDeck.Enemies
{
    internal class Enemy
    {
        // -- Fields -- \\
        private string name;
        private Deck deck;
        private List<Card> lastPlayed;

        // -- Properties -- \\
        /// <summary>
        /// Get returns the last played card by the enemy, set sets the last played card.
        /// </summary>
        public List<Card> LastPlayed
        {
            get { return lastPlayed; }
        }

        /// <summary>
        /// Get returns the enemy's current deck, set allows for the deck to be changed.
        /// </summary>
        public Deck Deck
        {
            get { return deck; }
            set { deck = value; }
        }

        /// <summary>
        /// Returns the name of the enemy.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        // -- Constructor -- \\
        /// <summary>
        /// Build a new enemy of a specified name and with a starting deck.
        /// </summary>
        /// <param name="name">The name of the enemy.</param>
        /// <param name="deck">The starting deck for the enemy.</param>
        public Enemy(string name, Deck deck)
        {
            this.name = name;
            this.deck = deck;
            lastPlayed = new List<Card>();
        }

        // -- Method -- \\
        /// <summary>
        /// Add a card into the list of played cards.
        /// </summary>
        /// <param name="card">The card to add.</param>
        public void PlayCard(Card card)
        {
            lastPlayed.Insert(0, card);
        }
    }
}
