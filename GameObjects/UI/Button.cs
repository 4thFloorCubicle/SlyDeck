using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlyDeck.GameObjects.UI
{
    internal class Button : GameObject, IUserInterface, IClickable
    {
        private string displayText; // text contained within the UI element shown to the user
        private Texture2D backTexture; // texture for the button
        private SpriteFont font;

        public event ClickedDelegate Clicked;
        public Rectangle Bounds { get { return new Rectangle((int)Position.X, (int)Position.Y, backTexture.Width, backTexture.Height); } }

        public Button(Vector2 position, string name, string displayText, Texture2D backTexture, SpriteFont font) : base(position, name)
        {
            this.backTexture = backTexture;
            this.displayText = displayText;
        }

        public event ClickedDelegate clicked;

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backTexture, Position, backTexture.Bounds, Color.White, 0, backTexture.Bounds.Center.ToVector2(), 1, SpriteEffects.None, .5f);
            spriteBatch.DrawString(font, displayText, Position, Color.White, 0, backTexture.Bounds.Center.ToVector2(), 1, SpriteEffects.None, .51f);
        }

        /// <summary>
        /// Invoked when the button is clicked. Calls all subscribers to the clicked event.
        /// </summary>
        public void OnClick()
        {
            clicked?.Invoke(); // optional for null safety, could be clicked w/o any subscribers
        }
    }
}
