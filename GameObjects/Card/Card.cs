using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SlyDeck.GameObjects;
using SlyDeck.GameObjects.Card.CardEffects;
using SlyDeck.GameObjects.UI;
using SlyDeck.Managers;

// Authors: Cooper Fleishman, Ben Haines
namespace SlyDeck.GameObjects.Card
{
    /// <summary>
    /// Enum representing the 2 different types of power in the game
    /// </summary>
    public enum PowerType
    {
        BasePower,
        EffectPower,
    }

    internal enum CardType
    {
        Header,
        Footer,
        Quote,
        Graph,
        Closer,
    }

    /// <summary>
    /// Class that represents cards to be played within the game
    /// </summary>
    internal class Card : GameObject, IClickable
    {
        private string description;
        private float basePower; // power from the card itself OR granted by permanant. permanant gains/losses
        private float effectPower; // power granted from temporary effects. temporary gains/losses.
        private Texture2D cardTexture;
        private CardType type;
        private SpriteFont Arial24;

        // 2 dictionaries required for effects. one for any attachers this card has, the other for card effects.
        private Dictionary<string, List<ICardEffect>> effects; // different effects the card has
        private Dictionary<string, List<AttacherEffect>> attachers; // different attachments this card has

        private Label lbName; // label to display name of card
        private Label lbPower; // label to display power of card
        private Label lbType; // label to display type of card
        private Label lbDescription; // label to display description of card
        private Texture2D cardArt; // art associated with the card

        private float hoverScale; // A temporary scalar for the card when hovered over
        private float baseScale; // The base value of the scale
        private Vector2 basePos; // The base position of the card

        public float TotalPower
        {
            get { return basePower + effectPower; }
        }

        public float BasePower
        {
            get { return basePower; }
            set { basePower = value; }
        }

        public float EffectPower
        {
            get { return effectPower; }
            set { effectPower = value; }
        }

        public Vector2 BasePos { get { return basePos; } set { basePos = value; Position = value; } }

        /// <summary>
        /// Property to handle the temporary scaling when the card is hovered over, also adjusting labels.
        /// </summary>
        public float HoverScale
        {
            get { return hoverScale; }
            set
            {
                if (value + baseScale > 1 || value < 0)
                    return;
                if (value < .05)
                    value = 0;
                hoverScale = value;
                Scale = hoverScale + baseScale;      
                // Checks if the card is below a certain point and, if it is, enlarges the card away from the bottom of the screen
                // This is highly scuffed and should change.
                if(basePos.Y > 700)
                    Position = new(basePos.X - Bounds.Width * hoverScale, basePos.Y - Bounds.Height * hoverScale * 2);
                else
                    Position = new(basePos.X - Bounds.Width * hoverScale, basePos.Y - Bounds.Height * hoverScale);
                AdjustLabels();
            }
        }

        public float BaseScale
        {
            get { return baseScale; }
            set 
            { 
                baseScale = value; 
                Scale = baseScale; 
                hoverScale = 0; }
        }

        /// <summary>
        /// Property to handle scaling the card at smaller sizes, adjusting the text
        /// </summary>
        public override float Scale
        {
            get { return base.Scale; }
            set
            {
                base.Scale = value;

                // Set the scales for all of the text so they change in size
                lbName.Scale = value;
                lbPower.Scale = value;
                lbType.Scale = value;
                lbDescription.Scale = value;

            }
        }

        public override Vector2 Position
        {
            get { return base.Position; }
            set
            {
                base.Position = value;
                AdjustLabels();
            }
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    (int)(cardTexture.Width * baseScale),
                    (int)(cardTexture.Height * baseScale)
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
            float basePower,
            CardType type,
            Texture2D cardArt
        )
            : base(position, name)
        {
            this.cardTexture = cardTexture;
            this.description = description;
            this.basePower = basePower;
            this.type = type;
            this.cardArt = cardArt;

            Arial24 = AssetManager.Instance.GetAsset<SpriteFont>("Arial24");
            // create the labels
            // TODO, figure out where they need to be placed later (along with font size)
            lbName = new Label(Vector2.Zero, $"Card Name Label-{name}", name, Arial24);
            AddChildObject(lbName);

            lbType = new Label(
                Vector2.Zero,
                $"Card Type Label-{name}",
                $"{type} slide",
                Arial24,
                Color.Gray
            );
            AddChildObject(lbType);

            lbPower = new Label(Vector2.Zero, $"Card Power Label-{name}", $"{basePower}", Arial24);
            AddChildObject(lbPower);

            lbDescription = new Label(
                Vector2.Zero,
                $"Card Description Label ${name}",
                $"{description}",
                AssetManager.Instance.GetAsset<SpriteFont>("Arial12"),
                Color.Gray
            );
            AddChildObject(lbDescription);

            baseScale = 1;

            effects = new Dictionary<string, List<ICardEffect>>();
            attachers = new Dictionary<string, List<AttacherEffect>>();
        }

        public Card(Vector2 position, CardData cardData)
            : this(
                position,
                cardData.Name,
                cardData.BackTexture,
                cardData.Description,
                cardData.BasePower,
                cardData.Type,
                cardData.CardArt
            )
        {
            foreach (ICardEffect effect in cardData.Effects)
            {
                AddEffect(effect);
            }
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
                Scale,
                SpriteEffects.None,
                .05f + this.hoverScale
            );
            spriteBatch.Draw(
                cardArt,
                new Vector2(Position.X + cardTexture.Width * Scale / 2, Position.Y + cardTexture.Height * Scale / 3 + 10),
                cardArt.Bounds,
                Color.Wheat,
                0,
                new(cardArt.Width / 2, cardArt.Height / 2),
                .8f * Scale,
                SpriteEffects.None,
                .15f + this.hoverScale
            );
            lbDescription.Draw(spriteBatch, hoverScale);
            lbName.Draw(spriteBatch, hoverScale);
            lbPower.Draw(spriteBatch, hoverScale);
            lbType.Draw(spriteBatch, hoverScale);
        }

        public override void Update(GameTime gameTime)
        {
            lbPower.Text = $"{TotalPower}";

            if (effectPower > 0)
            {
                lbPower.TextColor = Color.Green;
            }
            else if (effectPower < 0)
            {
                lbPower.TextColor = Color.Red;
            }
            else
            {
                lbPower.TextColor = Color.White;
            }

            Rectangle tempBounds = new((int)basePos.X, (int)basePos.Y, Bounds.Width, Bounds.Height);
            if (tempBounds.Contains(Mouse.GetState().Position))
            {               
               HoverScale += .05f;
            }
            else
            {
               HoverScale -= .05f;
            }
        }

        /// <summary>
        /// Adds an effect to this card
        /// </summary>
        /// <param name="effect">The effect itself</param>
        public void AddEffect(ICardEffect effect)
        {
            if (effect is AttacherEffect)
            {
                if (attachers.ContainsKey(effect.Name))
                {
                    attachers[effect.Name].Add((AttacherEffect)effect);
                }
                else
                {
                    attachers.Add(effect.Name, new List<AttacherEffect> { (AttacherEffect)effect });
                }
            }
            else
            {
                if (effects.ContainsKey(effect.Name))
                {
                    effects[effect.Name].Add(effect);
                }
                else
                {
                    effects.Add(effect.Name, new List<ICardEffect> { effect });
                }
            }

            effect.Owner = this;
        }

        /// <summary>
        /// Removes an effect from this card
        /// </summary>
        /// <param name="effectName">The name of effect to remove</param>
        public void RemoveEffect(string effectName)
        {
            effects.Remove(effectName);
            attachers.Remove(effectName);
        }

        /// <summary>
        /// Plays this card, activating any effects attached to it
        /// </summary>
        public void Play()
        {
            // attachment step
            if (attachers.Count > 0)
            {
                foreach (List<AttacherEffect> attacherSet in attachers.Values)
                {
                    foreach (AttacherEffect attacher in attacherSet)
                    {
                        attacher.Perform();
                    }
                }
            }

            // effect step
            foreach (List<ICardEffect> effectSet in effects.Values)
            {
                foreach (ICardEffect effect in effectSet)
                {
                    effect.Perform();
                }
            }
        }

        public void AdjustLabels()
        {
            float nameOffset = Arial24.MeasureString(lbName.Text).X;
            float powerOffset = Arial24.MeasureString(lbPower.Text).X;
            float typeOffset = Arial24.MeasureString(lbType.Text).X;
            float descOffset = Arial24.MeasureString(lbDescription.Text).X;

            // Adjust the position for all of the labels
            lbName.Position = new(
                Position.X + (cardTexture.Width - nameOffset) * Scale / 2,
                Position.Y + 30 * Scale
            );
            lbPower.Position = new(
                Position.X + (cardTexture.Width - 65 - powerOffset / 2) * Scale,
                Position.Y + (cardTexture.Height - 83) * Scale
            );
            lbType.Position = new(
                Position.X + (cardTexture.Width - typeOffset) * Scale / 2,
                Position.Y + 375 * Scale
            );
            lbDescription.Position = new(
                Position.X + (cardTexture.Width - descOffset) * Scale / 2,
                Position.Y + 515 * Scale
            );
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
