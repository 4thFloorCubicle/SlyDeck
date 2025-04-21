using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlyDeck.GameObjects;
using SlyDeck.GameObjects.Card.CardEffects;
using SlyDeck.GameObjects.UI;
using SlyDeck.Managers;

// Authors: Cooper Fleishman, Ben Haines, Shane Packard
namespace SlyDeck.GameObjects.Card
{
    /// <summary>
    /// Enum representing the 2 different types of power in the game
    /// </summary>
    public enum PowerType
    {
        Persuasion,
        TempPersuasion,
        AbilityEffect,
        TempAbilityEffect
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
        private float persuasion; // power from the card itself OR granted by permanant. permanant gains/losses
        private float tempPersuasion; // power granted from temporary effects. temporary gains/losses.
        private Texture2D cardTexture;
        private CardType type;
        private SpriteFont Arial24;
        private ICardEffect baseEffect;

        // 2 dictionaries required for effects. one for any attachers this card has, the other for card effects.
        private Dictionary<string, List<ICardEffect>> effects; // different effects the card has
        private Dictionary<string, List<AttacherEffect>> attachers; // different attachments this card has

        private Button playButton; // button used to play the card
        private Label lbName; // label to display name of card
        private Label lbPower; // label to display power of card
        private Label lbType; // label to display type of card
        private Label lbDescription; // label to display description of card
        private Texture2D cardArt; // art associated with the card

        public float TotalPower
        {
            get { return persuasion + tempPersuasion; }
        }

        public float Persuasion
        {
            get { return persuasion; }
            set { persuasion = value; }
        }

        public float TempPersuasion
        {
            get { return tempPersuasion; }
            set { tempPersuasion = value; }
        }

        public float AbilityPower { get; set => baseEffect.AbilityPower = value; }
        public float TempAbilityPower { get; set; }

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

                // Figure out the offset for the font given the text
                float nameOffset = Arial24.MeasureString(lbName.Text).X;
                float powerOffset = Arial24.MeasureString(lbPower.Text).X;
                float typeOffset = Arial24.MeasureString(lbType.Text).X;
                float descOffset = Arial24.MeasureString(lbDescription.Text).X;

                // Adjust the position for all of the labels
                lbName.Position = new(
                    Position.X + (cardTexture.Width - nameOffset) * value / 2,
                    Position.Y + 30 * value
                );
                lbPower.Position = new(
                    Position.X + (cardTexture.Width - 65 - powerOffset / 2) * value,
                    Position.Y + (cardTexture.Height - 83) * value
                );
                lbType.Position = new(
                    Position.X + (cardTexture.Width - typeOffset) * value / 2,
                    Position.Y + 375 * value
                );
                lbDescription.Position = new(
                    Position.X + (cardTexture.Width - descOffset) * value / 2,
                    Position.Y + 515 * value
                );
            }
        }

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    (int)(cardTexture.Width * Scale),
                    (int)(cardTexture.Height * Scale)
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
            float persuasion,
            CardType type,
            Texture2D cardArt
        )
            : base(position, name)
        {
            this.cardTexture = cardTexture;
            this.description = description;
            this.persuasion = persuasion;
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

            lbPower = new Label(Vector2.Zero, $"Card Power Label-{name}", $"{persuasion}", Arial24);
            AddChildObject(lbPower);

            lbDescription = new Label(
                Vector2.Zero,
                $"Card Description Label ${name}",
                $"{description}",
                AssetManager.Instance.GetAsset<SpriteFont>("Arial12"),
                Color.Gray
            );
            AddChildObject(lbDescription);

            Scale = 1;

            playButton = new Button(
                new Vector2(position.X, position.Y - 50),
                $"Card Play Button-{name}",
                $"Play card",
                AssetManager.Instance.GetAsset<Texture2D>("testButton"),
                Arial24
            );
            playButton.Position = new Vector2(
                playButton.Position.X + playButton.BackTexture.Width / 2,
                playButton.Position.Y
            );
            playButton.LeftClick += Play;
            LeftClick += playButton.Toggle; // toggle play button whenever the card is clicked
            AddChildObject(playButton);

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
            baseEffect = cardData.BaseEffect;

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
                .1f
            );

            spriteBatch.Draw(
                cardArt,
                new Vector2(Position.X + 40 * Scale, Position.Y + 80 * Scale),
                cardArt.Bounds,
                Color.Wheat,
                0,
                Vector2.Zero,
                .23f * Scale,
                SpriteEffects.None,
                .15f
            );

            lbName.Draw(spriteBatch);
            lbPower.Draw(spriteBatch);
            lbType.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            lbPower.Text = $"{TotalPower}";

            if (tempPersuasion > 0)
            {
                lbPower.TextColor = Color.Green;
            }
            else if (tempPersuasion < 0)
            {
                lbPower.TextColor = Color.Red;
            }
            else
            {
                lbPower.TextColor = Color.White;
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
