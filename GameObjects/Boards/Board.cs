using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using SlyDeck.Decks;
using SlyDeck.DiscardPiles;
using SlyDeck.Enemies;
// Author: Ben Haines
namespace SlyDeck.GameObjects.Boards
{
    internal class Board : GameObject
    {
        // -- Fields -- \\

        DiscardPile playerDiscardPile;
        Deck playerDeck;
        Card.Card lastPlayedPlayer;

        Enemy currentEnemy;
        DiscardPile enemyDiscardPile;

        // -- Constructor -- \\

        public Board(Vector2 position, string name, Deck playerDeck, string enemyName, Deck enemyDeck) : base(position, name)
        {
            this.playerDeck = playerDeck;
            lastPlayedPlayer = null;
            playerDiscardPile = new();
            
            enemyDiscardPile = new();
            currentEnemy = new(enemyName, enemyDeck);

        }

        // -- Methods -- \\

        /// <summary>
        /// Present the user with the choice of three cards, chossing one re-shuffles the other two back into the players deck.
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
;       }

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
