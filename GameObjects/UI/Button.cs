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
        private SpriteFont font; // font for the buttons text

        public event ClickedDelegate Clicked;
        public Rectangle Bounds { get { return new Rectangle((int)Position.X, (int)Position.Y, backTexture.Width, backTexture.Height); } }

        public Button(Vector2 position, string name, string displayText, Texture2D backTexture, SpriteFont font) : base(position, name)
        {
            this.backTexture = backTexture;
            this.displayText = displayText;
            this.font = font;
        }

        /// <summary>
        /// Draws the button back texture, THEN the text ontop
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backTexture, Position, backTexture.Bounds, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, .5f); // temporary depth vals
            spriteBatch.DrawString(font, displayText, Position, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, .51f);
        }

        /// <summary>
        /// Invoked when the button is clicked. Calls all subscribers to the clicked event.
        /// </summary>
        public void OnClick()
        {
            Clicked?.Invoke(); // optional for null safety, could be clicked w/o any subscribers
        }
    }
}
