using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlyDeck.Managers;
using SlyDeck.GameObjects.UI;
using SlyDeck.GameObjects.Card.CardEffects;


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
        private Dictionary<string, ICardEffect> effects;
        private Button playButton; // button used to play the card

        private Label lbName;
        private Label lbPower;
        private Label lbType;

        public Rectangle Bounds { get { return new Rectangle((int)Position.X, (int)Position.Y, cardTexture.Width, cardTexture.Height); } }

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
            //lbName = new Label(Position, $"Card Name Label-{name}", name, AssetManager.Instance.GetAsset<SpriteFont>("Arial24"));
            //lbType = new Label(Position, $"Card Type Label-{name}", $"{type}", AssetManager.Instance.GetAsset<SpriteFont>("Arial24"));
            //lbPower = new Label(Position, $"Card Power Label-{name}", $"{type}", AssetManager.Instance.GetAsset<SpriteFont>("Arial24"));

            

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
                Position,
                1,
                SpriteEffects.None,
                .9f
            );
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
            throw new NotImplementedException();
        }

        public void OnRightClick()
        {
            throw new NotImplementedException();
        }

        public void OnMiddleClick()
        {
            throw new NotImplementedException();
        }
    }
}
