using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlyDeck.GameObjects.Card.CardEffects;
using SlyDeck.GameObjects.UI;
using SlyDeck.Managers;

// Authors: Cooper Fleishman
namespace SlyDeck.GameObjects.Card
{
    internal enum CardType
    {
        Title,
        List,
        Picture,
        Graph,
        Transition,
    }

    /// <summary>
    /// Class that represents cards to be played within the game
    /// </summary>
    internal class Card : GameObject, IClickable
    {
        private string description;
        private int power;
        private Texture2D cardTexture;
        private CardType type;
        private Dictionary<string, ICardEffect> effects; // different effect the card has

        private Button playButton; // button used to play the card
        private Label lbName; // label to display name of card
        private Label lbPower; // label to display power of card
        private Label lbType; // label to display type of card

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    cardTexture.Width,
                    cardTexture.Height
                );
            }
        }

        public event ClickedDelegate LeftClick;
        public event ClickedDelegate MiddleClick;
        public event ClickedDelegate RightClick;

        public Card(
            Vector2 position,
            string name,
            Texture2D cardTexture,
            string description,
            int power,
            CardType type
        )
            : base(position, name)
        {
            this.cardTexture = cardTexture;
            this.description = description;
            this.power = power;
            this.type = type;

            // create the labels
            // TODO, figure out where they need to be placed later (along with font size)
            lbName = new Label(
                Position,
                $"Card Name Label-{name}",
                name,
                AssetManager.Instance.GetAsset<SpriteFont>("Arial24")
            );
            AddChildObject(lbName);

            lbType = new Label(
                Position,
                $"Card Type Label-{name}",
                $"{type}",
                AssetManager.Instance.GetAsset<SpriteFont>("Arial24")
            );
            AddChildObject(lbType);

            lbPower = new Label(
                new Vector2(Position.X + 327, Position.Y + 515), // NOTE: Position will not work
                // once power goes beyond a single digit, itll leave the little circle on the card
                $"Card Power Label-{name}",
                $"{power}",
                AssetManager.Instance.GetAsset<SpriteFont>("Arial24")
            );
            AddChildObject(lbPower);

            playButton = new Button(
                new Vector2(position.X, position.Y - 50),
                $"Card Play Button-{name}",
                $"Play card",
                AssetManager.Instance.GetAsset<Texture2D>("testButton"),
                AssetManager.Instance.GetAsset<SpriteFont>("Arial24")
            );
            playButton.Position = new Vector2(
                playButton.Position.X + playButton.BackTexture.Width / 2,
                playButton.Position.Y
            );
            playButton.LeftClick += Play;
            LeftClick += playButton.Toggle; // toggle play button whenever the card is clicked
            AddChildObject(playButton);

            effects = new Dictionary<string, ICardEffect>();
        }

        /// <summary>
        /// Draws the card to a spritebatch
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw to</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                cardTexture,
                Position,
                cardTexture.Bounds,
                Color.White,
                0,
                Vector2.Zero,
                1,
                SpriteEffects.None,
                .1f
            );

            lbName.Draw(spriteBatch);
            lbPower.Draw(spriteBatch);
            lbType.Draw(spriteBatch);
        }

        /// <summary>
        /// Adds an effect to this card
        ///
        /// NOTE: we may remove effect name if its not necessary,
        ///     i just have it in here for the time being so refactoring doesnt become a nightmare
        /// </summary>
        /// <param name="effectName">The name of the effect</param>
        /// <param name="effect">The effect itself</param>
        public void AddEffect(string effectName, ICardEffect effect)
        {
            effects.Add(effectName, effect);
        }

        /// <summary>
        /// Plays this card, activating any effects attached to it
        /// </summary>
        public void Play()
        {
            foreach (ICardEffect effect in effects.Values)
            {
                effect.Perform();
            }
        }

        public void OnLeftClick()
        {
            LeftClick?.Invoke();
        }

        public void OnRightClick()
        {
            RightClick?.Invoke();
        }

        public void OnMiddleClick()
        {
            MiddleClick?.Invoke();
        }
    }
}
