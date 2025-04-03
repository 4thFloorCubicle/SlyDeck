using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using SlyDeck.Decks;
using SlyDeck.Enemies;
using SlyDeck.GameObjects.Card;
using SlyDeck.Piles;

// Authors: Ben Haines, Cooper Fleishman
namespace SlyDeck.GameObjects.Boards
{
    /// <summary>
    /// Represents state of the game
    /// </summary>
    internal class Board : GameObject
    {
        // -- Fields -- \\

        // Singleton
        public static Board Instance { get; private set; }

        // Player
        private DiscardPile playerDiscardPile;
        private Deck playerDeck;
        private Card.Card lastPlayedPlayer;

        private Enemy currentEnemy;
        private DiscardPile enemyDiscardPile;

        public Deck PlayerDeck
        {
            get { return playerDeck; }
        }

        public Enemy CurrentEnemy
        {
            get { return currentEnemy; }
        }

        // -- Constructor -- \\
        public Board(
            Vector2 position,
            string name,
            Deck playerDeck,
            string enemyName,
            Deck enemyDeck
        )
            : base(position, name)
        {
            // Because board is a gameobject, implementation of a singleton must be done slightly differently
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                throw new Exception("Cannot initiallize a second instance of the board class.");
            }

            this.playerDeck = playerDeck;
            lastPlayedPlayer = null;
            playerDiscardPile = new();

            enemyDiscardPile = new();
            currentEnemy = new(enemyName, enemyDeck);
        }

        // -- Methods -- \\

        /// <summary>
        /// Present the user with the choice of three cards, choosing one re-shuffles the other two back into the players deck.
        /// </summary>
        /// <returns></returns>
        public Card.Card CardChoice()
        {
            Card.Card finalCard;

            // Draw the top three cards from the deck
            List<Card.Card> cardOptions = new List<Card.Card>(3);
            cardOptions.Add(playerDeck.DrawCard());
            cardOptions.Add(playerDeck.DrawCard());
            cardOptions.Add(playerDeck.DrawCard());

            // TEMPORARY SUBSTITUTE FOR USER INPUT, THIS NEEDS TO CHANGE.
            int pretendInput = 1;
            finalCard = cardOptions[pretendInput];

            // Add the two unchoosen cards back into the deck and shuffle it.
            cardOptions.RemoveAt(pretendInput);
            playerDeck.AddCardBottom(cardOptions[0]);
            playerDeck.AddCardBottom(cardOptions[1]);
            playerDeck.Shuffle();

            return finalCard;
        }

        /// <summary>
        /// Draw everything in the board onto the screen.
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw to.</param>
        /// <exception cref="NotImplementedException">Not implimented yet.</exception>
        public override void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}
