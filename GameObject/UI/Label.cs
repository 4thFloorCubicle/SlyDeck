using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlyDeck.GameObject.UI
{
    /// <summary>
    /// Class for Label UI Objects
    /// </summary>
    internal class Label : GameObject, IUserInterface
    {
        private string name;
        private string text;
        private SpriteFont spriteFont;
        private Color color;
        public string Name { get { return name; } }
        public string Text { get { return text; } set { text = value; } }
        public Color TextColor { get { return color; } set { color = value; } }

        public Label(Vector2 position, string name, SpriteFont spriteFont) : base(position)
        {
            this.name = name;
            text = "";
            this.spriteFont = spriteFont;
            color = Color.White;
        }

        public Label(Vector2 position, string name, string text, SpriteFont spriteFont) : base(position)
        {
            this.name = name;
            this.text = text;
            this.spriteFont = spriteFont;
            color = Color.White;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(spriteFont, text, Position, color);
        }
    }
}
