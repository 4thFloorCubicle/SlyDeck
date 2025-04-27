using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SlyDeck.Decks;
using SlyDeck.Enemies;
using SlyDeck.GameObjects.Card;
using SlyDeck.GameObjects.Card.CardEffects;
using SlyDeck.GameObjects.UI;
using SlyDeck.Managers;

// Authors: Ben Haines, Cooper Fleishman, Shane Packard
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
        private Deck playerDeck;
        private List<Card.Card> lastPlayedPlayer;
        private float playerPersuasion;
        private List<Card.Card> cardOptions = new List<Card.Card>(3);
        private Keys playerInput;
        private ICardEffect playerEffectOnPlay;

        // Enemy
        private Enemy currentEnemy;
        private float enemyPersuasion;
        private ICardEffect enemyEffectOnPlay;

        private int totalPlays;

        // UI elements
        private Label victoryLabel; // represents if a player wins/loses

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

        public ICardEffect PlayerEffectOnPlay
        {
            get { return playerEffectOnPlay; }
            set { playerEffectOnPlay = value; }
        }

        public ICardEffect EnemyEffectOnPlay
        {
            get { return enemyEffectOnPlay; }
            set { enemyEffectOnPlay = value; }
        }

        public List<Card.Card> PlayerHand
        {
            get { return cardOptions; }
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
            this.GD = GD;

            this.playerDeck = playerDeck;
            lastPlayedPlayer = new List<Card.Card>();
            playerPersuasion = 0;
            cardOptions = new List<Card.Card>(3);
            DrawCards();
            playerInput = default;

            currentEnemy = new(enemyName, enemyDeck);
            enemyPersuasion = 0;

            cardBack = AssetManager.Instance.GetAsset<Texture2D>("TempCardBack");

            victoryLabel = new Label(
                new Vector2(GD.Viewport.Width / 2, GD.Viewport.Height / 2),
                "Victory Label",
                "You win (default)",
                AssetManager.Instance.GetAsset<SpriteFont>("Arial24"),
                Color.Green
            );
            victoryLabel.Toggle();
        }

        // -- Methods -- \\

        public override void Update(GameTime gameTime)
        {
            Card.Card playedCard = null;
            foreach (Card.Card card in playerDeck.Cards)
            {
                card.TempPersuasion = 0;
                card.TempAbilityPower = 0;
            }
            foreach (Card.Card card in currentEnemy.Deck.Cards)
            {
                card.TempPersuasion = 0;
                card.TempAbilityPower = 0;
            }

            // Don't allow more than five cards played on the screen at once
            if (lastPlayedPlayer.Count > 4)
                return;
            // Note: cardOptions[x].HoverScale being greater than 0 means that the mouse is currently over the card.
            if (
                (
                    InputManager.Instance.SingleMousePress(MouseButton.Left)
                    && cardOptions[2].HoverScale != 0
                ) || InputManager.Instance.SingleKeyPress(Keys.Z)
            )
            {
                playedCard = cardOptions[2];

                cardOptions.RemoveAt(2);
            }
            else if (
                (
                    InputManager.Instance.SingleMousePress(MouseButton.Left)
                    && cardOptions[1].HoverScale != 0
                ) || InputManager.Instance.SingleKeyPress(Keys.X)
            )
            {
                playedCard = cardOptions[1];

                cardOptions.RemoveAt(1);
            }
            else if (
                (
                    InputManager.Instance.SingleMousePress(MouseButton.Left)
                    && cardOptions[0].HoverScale != 0
                ) || InputManager.Instance.SingleKeyPress(Keys.C)
            )
            {
                playedCard = cardOptions[0];

                cardOptions.RemoveAt(0);
            }
            // If no applicable button was pressed, don't continue
            else
                return;

            // apply effect to played card if one is queued;
            if (Instance.playerEffectOnPlay != null)
            {
                playedCard.AddEffect(playerEffectOnPlay);
                Instance.playerEffectOnPlay = null;
            }

            playedCard.Play(true);
            playerPersuasion += playedCard.TotalPower;
            lastPlayedPlayer.Insert(0, playedCard);

            // Position the new most recently played card
            playedCard.BaseScale = .5f;
            playedCard.BasePos = new(
                GD.Viewport.Width / 2 - playedCard.Bounds.Width / 2,
                GD.Viewport.Height / 2 + 50
            );

            // Loop through and position all of the rest of the lastPlayedPlayer cards.
            for (int cur = 1; cur < lastPlayedPlayer.Count; cur++)
            {
                lastPlayedPlayer[cur].BaseScale = .4f;
                lastPlayedPlayer[cur].BasePos = new(
                    GD.Viewport.Width / 3 - (lastPlayedPlayer[cur].Bounds.Width * 1.1f) * (cur - 1),
                    GD.Viewport.Height / 2 + 50
                );
            }

            // Add the two unchosen cards back into the deck and then draw new cards.
            playerDeck.AddCardBottom(cardOptions[0]);
            playerDeck.AddCardBottom(cardOptions[1]);
            DrawCards();

            // The enemy moves next.

            Card.Card enemyCard = currentEnemy.Deck.DrawCard();
            enemyCard.Toggle();

            if (Instance.enemyEffectOnPlay != null)
            {
                enemyCard.AddEffect(enemyEffectOnPlay);
                Instance.enemyEffectOnPlay = null;
            }

            enemyCard.Play(false);
            enemyPersuasion += enemyCard.TotalPower;
            currentEnemy.PlayCard(enemyCard);

            currentEnemy.LastPlayed[0].BaseScale = .5f;
            currentEnemy.LastPlayed[0].BasePos = new(
                GD.Viewport.Width / 2 - lastPlayedPlayer[0].Bounds.Width / 2,
                GD.Viewport.Height / 2 - lastPlayedPlayer[0].Bounds.Height - 50
            );

            for (int cur = 1; cur < currentEnemy.LastPlayed.Count; cur++)
            {
                currentEnemy.LastPlayed[cur].BaseScale = .4f;
                currentEnemy.LastPlayed[cur].BasePos = new(
                    GD.Viewport.Width * 2 / 3
                        + (currentEnemy.LastPlayed[cur].Bounds.Width * 1.1f) * (cur - 2),
                    GD.Viewport.Height / 2 - lastPlayedPlayer[0].Bounds.Height - 50
                );
            }

            if (totalPlays == 4)
            {
                if (CheckVictory())
                {
                    victoryLabel.Text = "You win";
                    victoryLabel.TextColor = Color.Green;
                }
                else
                {
                    victoryLabel.Text = "You lose";
                    victoryLabel.TextColor = Color.Red;
                }
                victoryLabel.Toggle();
            }
            else
            {
                totalPlays++;
            }
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

            // Line
            spriteBatch.Draw(
                cardBack,
                new Rectangle(0, GD.Viewport.Height / 2, GD.Viewport.Width, 1),
                null,
                Color.Black,
                0,
                Vector2.Zero,
                SpriteEffects.None,
                0
            );

            if (victoryLabel.Enabled)
            {
                victoryLabel.Draw(spriteBatch);
            }
        }

        /// <summary>
        /// Draw cards to fill the player's card options and then shuffle their deck.
        /// </summary>
        private void DrawCards()
        {
            // Turn off cards to dispose.
            foreach (Card.Card card in cardOptions)
                card.Toggle();
            cardOptions.Clear();
            cardOptions.Add(playerDeck.DrawCard());
            cardOptions.Add(playerDeck.DrawCard());
            cardOptions.Add(playerDeck.DrawCard());

            // Run through the cards and position/scale them.
            for (int cur = 0; cur < 3; cur++)
            {
                cardOptions[cur].BaseScale = .6f;
                cardOptions[cur].BasePos = new(
                    GD.Viewport.Width * 6 / 7 - (cardOptions[cur].Bounds.Width * 1.1f) * cur,
                    GD.Viewport.Height - cardOptions[cur].Bounds.Height * 1.05f
                );
            }

            // Turn on cards to draw.
            foreach (Card.Card card in cardOptions)
                card.Toggle();
            playerDeck.Shuffle();
        }

        private bool CheckVictory()
        {
            return playerPersuasion > enemyPersuasion;
        }
    }
}
