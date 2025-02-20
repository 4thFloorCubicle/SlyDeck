using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlyDeck.GameObject
{
    /// <summary>
    /// Base class for any player-facing objects to inherit from (i.e visible to the player)
    /// </summary>
    internal abstract class GameObject
    {
        private Vector2 position;
        public Vector2 Position { get { return position; } }

        /// <summary>
        /// Creates a new GameObject
        /// </summary>
        /// <param name="position">The position of the game object</param>
        public GameObject(Vector2 position)
        {
            this.position = position;
        }

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
