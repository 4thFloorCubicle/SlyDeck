using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Authors: Cooper Fleishman
namespace SlyDeck.GameObjects
{
    /// <summary>
    /// Base class for any player-facing objects to inherit from (i.e visible to the player)
    /// </summary>
    internal abstract class GameObject
    {
        private Vector2 position;
        private bool enabled;
        private string name;

        public Vector2 Position { get { return position; } }
        public bool Enabled { get { return enabled; } set { enabled = value; } }
        public string Name { get { return name; } set { name = value; } }


        /// <summary>
        /// Creates a new GameObject
        /// </summary>
        /// <param name="position">The position of the game object</param>
        public GameObject(Vector2 position, string name)
        {
            this.position = position;
            this.name = name;

            enabled = true;
        }

        public abstract void Draw(SpriteBatch spriteBatch);
        
        /// <summary>
        /// Toggles if the object if enabled or not
        /// </summary>
        public virtual void Toggle()
        {
            enabled = !enabled;
        }
    }
}
