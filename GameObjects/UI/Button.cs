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
    /// Class for Button UI objects.
    /// </summary>
    internal class Button : GameObject, IClickable
    {
        private string displayText; // text contained within the UI element shown to the user
        private Texture2D backTexture; // texture for the button
        private SpriteFont font; // font for rendering displaytext

        public Texture2D BackTexture
        {
            get { return backTexture; }
        }

        public event ClickedDelegate LeftClick;
        public event ClickedDelegate MiddleClick;
        public event ClickedDelegate RightClick;

        public Rectangle Bounds
        {
            get
            {
                return new Rectangle(
                    (int)Position.X,
                    (int)Position.Y,
                    backTexture.Width,
                    backTexture.Height
                );
            }
        }

        /// <summary>
        /// Creates a new button with text
        /// </summary>
        /// <param name="position">Position of the button</param>
        /// <param name="name">The name of the button</param>
        /// <param name="displayText">The text the user sees ontop of the button</param>
        /// <param name="backTexture">The texture of the button</param>
        /// <param name="font">The font of the display text</param>
        public Button(
            Vector2 position,
            string name,
            string displayText,
            Texture2D backTexture,
            SpriteFont font
        )
            : base(position, name)
        {
            this.backTexture = backTexture;
            this.displayText = displayText;
            this.font = font;
        }

        public Button(Vector2 position, string name, Texture2D backTexture)
            : this(
                position,
                name,
                "",
                backTexture,
                AssetManager.Instance.GetAsset<SpriteFont>("Arial24")
            ) { }

        /// <summary>
        /// Draws the button to the provided spritebatch
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw to</param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                backTexture,
                Bounds,
                backTexture.Bounds,
                Color.White,
                0f,
                Vector2.Zero,
                SpriteEffects.None,
                .5f
            );

            // no reason to draw text when there is none to be drawn
            if (displayText.Length > 0)
            {
                spriteBatch.DrawString(
                    font,
                    displayText,
                    Position,
                    Color.White,
                    0f,
                    Vector2.Zero,
                    1,
                    SpriteEffects.None,
                    .501f
                );
            }
        }

        public void OnLeftClick()
        {
            LeftClick?.Invoke(); // optional for null safety, could be clicked w/o any subscribers
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
