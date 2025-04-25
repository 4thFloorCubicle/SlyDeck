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
            this.Draw(spriteBatch, 0);
        }

        public void Draw(SpriteBatch spriteBatch, float depthMod)
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
                .2f + depthMod
            );
        }

        public void DrawDesc(SpriteBatch spriteBatch, float depthMod, float power)
        {
            string toPrint = "";
            // If a '{' is present, then a substitution needs to be made for the card's description.
            if (text.Contains('{'))
            {
                string equation = text.Substring(
                    text.IndexOf('{') + 1,
                    text.IndexOf('}') - 1 - text.IndexOf('{')
                );
                toPrint += text.Substring(0, text.IndexOf('{'));

                if (equation == "ability value")
                {
                    toPrint += $"{power}";
                }
                else
                {
                    string[] brokenDown = equation.Split(" ");
                    if (brokenDown[1] == "+")
                        toPrint += $"{double.Parse(brokenDown[0]) + power}";
                    else if (brokenDown[1] == "*")
                        toPrint += $"{double.Parse(brokenDown[0]) * power}";
                }

                toPrint += text.Substring(text.IndexOf('}') + 1);
            }
            else
            {
                toPrint = text;
            }

            spriteBatch.DrawString(
                spriteFont,
                toPrint,
                Position,
                color,
                0,
                Vector2.Zero,
                Scale,
                SpriteEffects.None,
                .2f + depthMod
            );
        }
    }
}
