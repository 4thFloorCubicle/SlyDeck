using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlyDeck.GameObjects.Card.CardEffects;
using SlyDeck.GameObjects.UI;
using SlyDeck.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Authors: Cooper Fleishman
namespace SlyDeck.GameObjects.Card
{
    internal enum CardType
    {
        Title,
        List,
        Picture,
        Graph,
        Transition
    }

    /// <summary>
    /// Class that represents cards to be played within the game
    /// </summary>
    internal class Card : GameObject
    {
        private string description;
        private int power;
        private Texture2D cardTexture;
        private CardType type;
        private Dictionary<string, ICardEffect> effects;
        private Dictionary<string, Label> labels; // labels for displaying text
        private Button playButton; // button used to play the card

        public Card(Vector2 position, string name, Texture2D cardTexture, string description, int power, CardType type) : base(position, name)
        {
            this.cardTexture = cardTexture;
            this.description = description;
            this.power = power;
            this.type = type;

            labels = new Dictionary<string, Label>();

            //Label lbName = new Label(Position, "Card Name", name);

            //labels.Add();

            effects = new Dictionary<string, ICardEffect>();
        }

        /// <summary>
        /// Draws the card to a spritebatch
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw to</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(cardTexture, Position, cardTexture.Bounds, Color.White, 0, Position, 1, SpriteEffects.None, .1f);
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
    }
}
