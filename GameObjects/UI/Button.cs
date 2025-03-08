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

        public event ClickedDelegate LeftClick;
        public event ClickedDelegate MiddleClick;
        public event ClickedDelegate RightClick;

        public Rectangle Bounds { get; }

        public Button(Vector2 position, string name, string displayText, Texture2D backTexture) : base(position, name)
        {
            this.backTexture = backTexture;
            this.displayText = displayText;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Calls all subscribers to the LeftClicked event.
        /// </summary>
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
