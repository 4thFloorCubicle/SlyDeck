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

        /// <summary>
        /// Draws this object to a spritebatch with a modifier to it's depth.
        /// </summary>
        /// <param name="spriteBatch">The spriteback to draw to</param>
        /// <param name="depthMod">The depth to draw at.</param>
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

        /// <summary>
        /// Draw specifically the description of the card, removing braces if they're present.
        /// </summary>
        /// <param name="spriteBatch">The spritebatch</param>
        /// <param name="depthMod">The depth to draw at.</param>
        /// <param name="power">The power of the card to mention in the description.</param>
        public void DrawDesc(
            SpriteBatch spriteBatch,
            float depthMod,
            float power,
            float cardWidth,
            SpriteFont font
        )
        {
            spriteBatch.DrawString(
                spriteFont,
                Debrace(power, cardWidth, font),
                Position,
                color,
                0,
                Vector2.Zero,
                Scale,
                SpriteEffects.None,
                .2f + depthMod
            );
        }

        /// <summary>
        /// Debrace the text, substituting it in with numbers. Also make sure it fits with new lines.
        /// </summary>
        /// <param name="power">The power to print.</param>
        /// <returns>The string that's debraced.</returns>
        public string Debrace(float power, float cardWidth, SpriteFont font)
        {
            // If there is no brace, just return.
            if (!text.Contains('{'))
                return AddSpace(text, cardWidth, font);

            // Make a new string that contains everything inside of the brackets.
            string equation = text.Substring(
                text.IndexOf('{') + 1,
                text.IndexOf('}') - 1 - text.IndexOf('{')
            );
            string newString = text.Substring(0, text.IndexOf('{'));

            // If the brackets just reads "ability value", then just replace it with power.
            if (equation == "ability value")
            {
                newString += $"{power}";
            }
            // Check and perform any operations.
            else
            {
                string[] brokenDown = equation.Split(" ");
                if (brokenDown[1] == "+")
                    newString += $"{double.Parse(brokenDown[0]) + power}";
                else if (brokenDown[1] == "*")
                    newString += $"{double.Parse(brokenDown[0]) * power}";
            }

            newString += text.Substring(text.IndexOf('}') + 1);

            return AddSpace(newString, cardWidth, font);
        }

        /// <summary>
        /// Add white space so none of the text goes off the side of the card.
        /// </summary>
        /// <param name="toAdd">Text to add space to.</param>
        /// <param name="cardWidth">The width of the parent card.</param>
        /// <param name="font">The font.</param>
        /// <returns>The text with white space added.</returns>
        public string AddSpace(string toAdd, float cardWidth, SpriteFont font)
        {
            cardWidth *= 1.8f;
            // Break up the text by word.
            string[] brokenUp = toAdd.Split(" ");

            string finalNewString = "";
            string tempString = "";

            for (int currentWord = 0; currentWord < brokenUp.Length; currentWord++)
            {
                // If the text in tempString (representing one line) goes off the side, add a space.
                if (font.MeasureString(tempString).X > cardWidth)
                {
                    finalNewString += tempString + "\n" + brokenUp[currentWord] + " ";
                    tempString = "";
                }
                else
                {
                    tempString += brokenUp[currentWord] + " ";
                }
            }
            finalNewString += tempString;

            return finalNewString;
        }
    }
}
