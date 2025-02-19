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
        private Texture2D texture;

        public Vector2 Position { get { return position; } }
        public Texture2D Texture { get { return texture; } }

        /// <summary>
        /// Creates a new GameObject
        /// </summary>
        /// <param name="position">The position of the game object</param>
        /// <param name="texture">The texture of the game object</param>
        public GameObject(Vector2 position, Texture2D texture)
        {
            this.position = position;
            this.texture = texture;
        }

        public abstract void Draw(SpriteBatch spriteBatch);
    }
}
