using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        private List<Card.Card> lastPlayedPlayer;
        private int playerPersuasion;
        private List<Card.Card> cardOptions = new List<Card.Card>(3);
        private Keys playerInput;

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
            GraphicsDevice GD
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
            lastPlayedPlayer = new List<Card.Card>();
            playerDiscardPile = new();
            playerPersuasion = 0;
            cardOptions = new List<Card.Card>(3);
            cardOptions.Add(playerDeck.DrawCard());
            cardOptions.Add(playerDeck.DrawCard());
            cardOptions.Add(playerDeck.DrawCard());
            playerInput = default;

            enemyDiscardPile = new();
            currentEnemy = new(enemyName, enemyDeck);
            enemyPersuasion = 0;

            cardBack = AssetManager.Instance.GetAsset<Texture2D>("TempCardBack");

            this.GD = GD;
        }

        // -- Methods -- \\

        public override void Update(GameTime gameTime)
        {
            Card.Card playedCard = null;

            // Don't allow more than five cards played on the screen at once
            if (lastPlayedPlayer.Count > 4)
                return;

            if (InputManager.Instance.SingleKeyPress(Keys.Z))
            {
                playedCard = cardOptions[0];

                cardOptions.RemoveAt(0);
            }
            else if (InputManager.Instance.SingleKeyPress(Keys.X))
            {
                playedCard = cardOptions[1];

                cardOptions.RemoveAt(1);
            }
            else if (InputManager.Instance.SingleKeyPress(Keys.C))
            {
                playedCard = cardOptions[2];

                cardOptions.RemoveAt(2);
            }
            // If no applicable button was pressed, don't continue
            else
                return;

            playedCard.Play();
            lastPlayedPlayer.Insert(0, playedCard);

            // Add the two unchosen cards back into the deck and shuffle it.
            playerDeck.AddCardBottom(cardOptions[0]);
            playerDeck.AddCardBottom(cardOptions[1]);
            cardOptions.Clear();
            cardOptions.Add(playerDeck.DrawCard());
            cardOptions.Add(playerDeck.DrawCard());
            cardOptions.Add(playerDeck.DrawCard());
            playerDeck.Shuffle();

            // The enemy moves next.
            Card.Card enemyPlayedCard = currentEnemy.Deck.DrawCard();
            currentEnemy.PlayCard(enemyPlayedCard);
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
            Vector2 playerNumberPosition = new(20, GD.Viewport.Height / 1.1f);
            Vector2 enemyNumberPosition = new(
                GD.Viewport.Width - 150 - numberFont.MeasureString(enemyPersuasion.ToString()).X,
                20
            );

            spriteBatch.DrawString(
                numberFont,
                playerPersuasion.ToString(),
                playerNumberPosition,
                Color.SaddleBrown,
                0,
                Vector2.Zero,
                2,
                SpriteEffects.None,
                .5f
            );
            spriteBatch.DrawString(
                numberFont,
                enemyPersuasion.ToString(),
                enemyNumberPosition,
                Color.SaddleBrown,
                0,
                Vector2.Zero,
                2,
                SpriteEffects.None,
                .5f
            );

            // Last played player and enemy card
            if (lastPlayedPlayer.Count != 0)
            {
                lastPlayedPlayer[0].Scale = .5f;
            lastPlayedPlayer[0].Position = new(
                GD.Viewport.Width / 2 - lastPlayedPlayer[0].Bounds.Width / 2,
                GD.Viewport.Height / 2 + 50
            );
            lastPlayedPlayer[0].Draw(spriteBatch);
            }
            if (currentEnemy.LastPlayed.Count != 0)
            {
                currentEnemy.LastPlayed[0].Scale = .5f;
                currentEnemy.LastPlayed[0].Position = new(
                    GD.Viewport.Width / 2 - lastPlayedPlayer[0].Bounds.Width / 2,
                    GD.Viewport.Height / 2 - lastPlayedPlayer[0].Bounds.Height - 50
                );
                currentEnemy.LastPlayed[0].Draw(spriteBatch);
            }

            // Player hand
            if (cardOptions != null)
                for (int cur = 0; cur < 3; cur++)
                {
                    cardOptions[cur].Scale = .6f;
                    cardOptions[cur].Position = new(
                        GD.Viewport.Width * 6 / 7 - (cardOptions[cur].Bounds.Width * 1.1f) * cur,
                        GD.Viewport.Height - cardOptions[cur].Bounds.Height * 1.05f
                    );
                    cardOptions[cur].Draw(spriteBatch);
                }

            // Enemy hand
            for (int cur = 0; cur < 3; cur++)
            {
                // Saved for when the cards need to be flipped
                //spriteBatch.Draw(cardBack, new Vector2(GD.Viewport.Width * 1/7 - (cardBack.Width * 66/100) * (cur - 1), 10), null, Color.White, (float)Math.PI, new(cardBack.Width, cardBack.Height), .6f, SpriteEffects.None, 0);
                spriteBatch.Draw(
                    cardBack,
                    new Vector2(
                        GD.Viewport.Width * 1 / 7 - (cardBack.Width * 66 / 100) * (cur - 1),
                        10
                    ),
                    null,
                    Color.White,
                    0,
                    Vector2.Zero,
                    .6f,
                    SpriteEffects.None,
                    0
                );
            }

            // Player Cards
            for (int cur = 1; cur < lastPlayedPlayer.Count; cur++)
            {
                lastPlayedPlayer[cur].Scale = .4f;
                lastPlayedPlayer[cur].Position = new(
                    GD.Viewport.Width / 3 - (lastPlayedPlayer[cur].Bounds.Width * 1.1f) * (cur - 1),
                    GD.Viewport.Height / 2 + 50
                );
                lastPlayedPlayer[cur].Draw(spriteBatch);
            }

            // Enemy Cards
            for (int cur = 1; cur < currentEnemy.LastPlayed.Count; cur++)
            {
                currentEnemy.LastPlayed[cur].Scale = .4f;
                currentEnemy.LastPlayed[cur].Position = new(
                    GD.Viewport.Width * 2 / 3
                        + (currentEnemy.LastPlayed[cur].Bounds.Width * 1.1f) * (cur - 2),
                    GD.Viewport.Height / 2 - lastPlayedPlayer[0].Bounds.Height - 50
                );
                currentEnemy.LastPlayed[cur].Draw(spriteBatch);
            }

            // Line
            spriteBatch.Draw(
                cardBack,
                new Rectangle(0, GD.Viewport.Height / 2, GD.Viewport.Width, 1),
                Color.Black
            );
        }
    }
}
