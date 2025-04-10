using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlyDeck.Decks;
using SlyDeck.Enemies;
using SlyDeck.GameObjects.Card;
using SlyDeck.Managers;
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

        // The back of a card
        private Texture2D cardBack;

        // Singleton
        public static Board Instance { get; private set; }

        // Player
        private DiscardPile playerDiscardPile;
        private Deck playerDeck;
        private Stack<Card.Card> lastPlayedPlayer;
        private int playerPersuasion;
        private List<Card.Card> cardOptions = new List<Card.Card>(3);

        // Enemy
        private Enemy currentEnemy;
        private DiscardPile enemyDiscardPile;
        private int enemyPersuasion;

        // Graphics Device for Drawing
        private GraphicsDevice GD;

        // -- Properties -- \\
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
            Deck enemyDeck,
            GraphicsDevice GD,
            Card.Card testCard,
            Card.Card testCard2
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
                throw new Exception("Cannot initialize a second instance of the board class.");
            }

            this.playerDeck = playerDeck;
            lastPlayedPlayer = new Stack<Card.Card>();
            lastPlayedPlayer.Push(testCard);
            playerDiscardPile = new();
            playerPersuasion = 100;

            enemyDiscardPile = new();            
            currentEnemy = new(enemyName, enemyDeck);
            currentEnemy.LastPlayed = testCard2;
            enemyPersuasion = 20000;

            cardBack = AssetManager.Instance.GetAsset<Texture2D>("TempCardBack");

            this.GD = GD;
        }

        // -- Methods -- \\

        /// <summary>
        /// Present the user with the choice of three cards, choosing one and re-shuffles the other two back into the players deck.
        /// </summary>
        /// <returns></returns>
       /* public Card.Card CardChoice()
        {
            Card.Card finalCard;

            // Draw the top three cards from the deck
            cardOptions.Add(playerDeck.DrawCard());
            cardOptions.Add(playerDeck.DrawCard());
            cardOptions.Add(playerDeck.DrawCard());

            // TEMPORARY SUBSTITUTE FOR USER INPUT, THIS NEEDS TO CHANGE.
            int pretendInput = 1;
            finalCard = cardOptions[pretendInput];

            // Add the two unchosen cards back into the deck and shuffle it.
            cardOptions.RemoveAt(pretendInput);
            playerDeck.AddCardBottom(cardOptions[0]);
            playerDeck.AddCardBottom(cardOptions[1]);
            cardOptions.Clear();
            playerDeck.Shuffle();

            return finalCard;
        }
        */

        /// Temporary form of cardChoice so the drawing works.
        public void CardChoice()
        {
            //            Card.Card finalCard;

            // Draw the top three cards from the deck
            cardOptions.Add(lastPlayedPlayer.Peek());
            cardOptions.Add(lastPlayedPlayer.Peek());
            cardOptions.Add(lastPlayedPlayer.Peek());

            // TEMPORARY SUBSTITUTE FOR USER INPUT, THIS NEEDS TO CHANGE.
            //            int pretendInput = 1;
            //            finalCard = cardOptions[pretendInput];

            // Add the two unchosen cards back into the deck and shuffle it.
            //           cardOptions.RemoveAt(pretendInput);
            //           playerDeck.AddCardBottom(cardOptions[0]);
            //           playerDeck.AddCardBottom(cardOptions[1]);
            //           cardOptions.Clear();
            //           playerDeck.Shuffle();

            //           return finalCard;
        }

        /// <summary>
        /// Draw everything in the board onto the screen.
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw to.</param>
        /// <exception cref="NotImplementedException">Not implimented yet.</exception>
        public override void Draw(SpriteBatch spriteBatch)
        {

            // Draw the current player and enemy's persuasion values to the screen
            SpriteFont numberFont = AssetManager.Instance.GetAsset<SpriteFont>("Arial24");

            Vector2 playerNumberPosition = new(80 - numberFont.MeasureString(playerPersuasion.ToString()).X / 2, GD.Viewport.Height / 1.8f);
            Vector2 enemyNumberPosition = new(GD.Viewport.Width - 80 - numberFont.MeasureString(enemyPersuasion.ToString()).X / 2, GD.Viewport.Height / 2.2f);

            spriteBatch.DrawString(numberFont, playerPersuasion.ToString(), playerNumberPosition, Color.Red);
            spriteBatch.DrawString(numberFont, enemyPersuasion.ToString(), enemyNumberPosition, Color.Red);

            // Last played player and enemy card
            lastPlayedPlayer.Peek().Scale = .5f;
            currentEnemy.LastPlayed.Scale = .5f;

            lastPlayedPlayer.Peek().Position = new(GD.Viewport.Width / 2 - lastPlayedPlayer.Peek().Bounds.Width / 2, GD.Viewport.Height / 2 - lastPlayedPlayer.Peek().Bounds.Height - 50);
            lastPlayedPlayer.Peek().Draw(spriteBatch);

            currentEnemy.LastPlayed.Position = new(GD.Viewport.Width / 2 - lastPlayedPlayer.Peek().Bounds.Width / 2, GD.Viewport.Height / 2 + 50);
            currentEnemy.LastPlayed.Draw(spriteBatch);

            // Player hand
            if (cardOptions != null)
                for (int cur = 0; cur < 3; cur ++)
                {
                    cardOptions[cur].Scale = .6f;
                    cardOptions[cur].Position = new(GD.Viewport.Width * 6/7 - (cardOptions[cur].Bounds.Width * 1.1f) * cur, GD.Viewport.Height - cardOptions[cur].Bounds.Height * 1.05f);
                    cardOptions[cur].Draw(spriteBatch);
                }

           // Enemy hand
           for (int cur = 0; cur < 3; cur++)
            {
                spriteBatch.Draw(cardBack, new Vector2(GD.Viewport.Width * 1/7 - (cardBack.Width * 66/100) * (cur - 1), 10), null, Color.White, 0, Vector2.Zero, .6f, SpriteEffects.None, 0);
            }
        }
    }
}
