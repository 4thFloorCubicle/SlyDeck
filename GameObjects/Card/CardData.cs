using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using SlyDeck.GameObjects.Card.CardEffects;

// Authors: Cooper Fleishman
namespace SlyDeck.GameObjects.Card
{
    /// <summary>
    /// Class that encapsulates card data
    /// </summary>
    internal class CardData
    {
        private string name;
        private Texture2D backTexture;
        private string description;
        private float basePower;
        private CardType type;
        private Texture2D cardArt;
        private List<ICardEffect> effects;
        private float abilityPower;

        /// <summary>
        /// Name of the card
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Back Texture of the card (the texture of the card itself)
        /// </summary>
        public Texture2D BackTexture
        {
            get { return backTexture; }
        }

        /// <summary>
        /// Description of the cards effects
        /// </summary>
        public string Description
        {
            get { return description; }
        }

        /// <summary>
        /// Base power of this card
        /// </summary>
        public float BasePower
        {
            get { return basePower; }
        }

        /// <summary>
        /// The CardType of this card
        /// </summary>
        public CardType Type
        {
            get { return type; }
        }

        /// <summary>
        /// The art of the card
        /// </summary>
        public Texture2D CardArt
        {
            get { return cardArt; }
        }

        public List<ICardEffect> Effects
        {
            get { return effects; }
        }

        public float AbilityPower
        {
            get { return abilityPower; }
        }

        public CardData(
            string name,
            Texture2D backTexture,
            string description,
            float basePower,
            CardType type,
            Texture2D cardArt,
            float abilityPower
        )
        {
            this.name = name;
            this.backTexture = backTexture;
            this.description = description;
            this.basePower = basePower;
            this.type = type;
            this.cardArt = cardArt;
            this.abilityPower = abilityPower;
        }

        public CardData(
            string name,
            Texture2D backTexture,
            string description,
            float basePower,
            CardType type,
            Texture2D cardArt,
            List<ICardEffect> effects,
            float abilityPower
        )
            : this(name, backTexture, description, basePower, type, cardArt, abilityPower)
        {
            this.effects = effects;
        }
    }
}
