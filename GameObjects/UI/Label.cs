using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlyDeck.Managers;

// Authors: Cooper Fleishman
namespace SlyDeck.GameObjects.UI
{
    /// <summary>
    /// Class for Label UI Objects
    /// </summary>
    internal class Label : GameObject
    {
        private string text;
        private SpriteFont spriteFont;
        private Color color;
        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        public Color TextColor
        {
            get { return color; }
            set { color = value; }
        }

        /// <summary>
        /// Creates a new label
        /// </summary>
        /// <param name="position">The position of the label</param>
        /// <param name="name">The name for this label</param>
        /// <param name="text">The text content this label contains</param>
        /// <param name="spriteFont">The spritefont to use for this label</param>
        public Label(Vector2 position, string name, string text, SpriteFont spriteFont)
            : base(position, name)
        {
            this.text = text;
            this.spriteFont = spriteFont;
            color = Color.White;
        }

        public Label(Vector2 position, string name, string text, SpriteFont font, Color color)
            : this(position, name, text, font)
        {
            this.color = color;
        }

        /// <summary>
        /// Draws this object to a spritebatch
        /// </summary>
        /// <param name="spriteBatch">The spriteback to draw to</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(
                spriteFont,
                text,
                Position,
                color,
                0,
                Vector2.Zero,
                Scale,
                SpriteEffects.None,
                1f
            );
        }
    }
}
