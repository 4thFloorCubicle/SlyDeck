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

// Authors: Cooper Fleishman
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
        private float basePower; // power from the card itself OR granted by permanant. permanant gains/losses
        private float effectPower; // power granted from temporary effects. temporary gains/losses.
        private Texture2D cardTexture;
        private CardType type;

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

            // create the labels
            // TODO, figure out where they need to be placed later (along with font size)
            lbName = new Label(
                new Vector2(position.X + cardTexture.Width / 4, position.Y + 30),
                $"Card Name Label-{name}",
                name,
                AssetManager.Instance.GetAsset<SpriteFont>("Arial24")
            );
            AddChildObject(lbName);

            lbType = new Label(
                new Vector2(Position.X + cardTexture.Width / 4, position.Y + 375),
                $"Card Type Label-{name}",
                $"{type} slide",
                AssetManager.Instance.GetAsset<SpriteFont>("Arial24"),
                Color.Gray
            );
            AddChildObject(lbType);

            lbPower = new Label(
                new Vector2(position.X + 327, position.Y + 515), // NOTE: Position will not work
                // once power goes beyond a single digit, itll leave the little circle on the card
                $"Card Power Label-{name}",
                $"{basePower}",
                AssetManager.Instance.GetAsset<SpriteFont>("Arial24")
            );
            AddChildObject(lbPower);

            lbDescription = new Label(
                new Vector2(position.X + cardTexture.Width / 4, position.Y + 425),
                $"Card Description Label ${name}",
                $"{description}",
                AssetManager.Instance.GetAsset<SpriteFont>("Arial12"),
                Color.Gray
            );
            AddChildObject(lbDescription);

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

            effects = new Dictionary<string, List<ICardEffect>>();
            attachers = new Dictionary<string, List<AttacherEffect>>();
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

            spriteBatch.Draw(
                cardArt,
                new Vector2(Position.X + 40, Position.Y + 80),
                cardArt.Bounds,
                Color.Wheat,
                0,
                Vector2.Zero,
                .23f,
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
        }

        /// <summary>
        /// Adds an effect to this card
        /// </summary>
        /// <param name="effect">The effect itself</param>
        public void AddEffect(ICardEffect effect)
        {
            if (effect is AttacherEffect)
            {
                if (effects.ContainsKey(effect.Name))
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
