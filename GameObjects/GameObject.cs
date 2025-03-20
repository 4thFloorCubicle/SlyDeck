using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlyDeck.Managers;

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
        private Dictionary<string, GameObject> childObjects;
        private GameObject? parent;

        /// <summary>
        /// A list of all the children of this GameObject
        /// </summary>
        public List<GameObject> Children
        {
            get { return childObjects.Values.ToList(); }
        }

        /// <summary>
        /// Position of this GameObject
        /// </summary>
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        /// <summary>
        /// True if this GameObject is enabled, false otherwise
        /// </summary>
        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        /// <summary>
        /// The name of this GameObject
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Creates a new GameObject
        /// </summary>
        /// <param name="position">The position of the game object</param>
        public GameObject(Vector2 position, string name)
        {
            this.position = position;
            this.name = name;

            enabled = true;
            childObjects = new Dictionary<string, GameObject>();

            GameObjectManager.Instance.TryAddGameObject(this);
        }

        public abstract void Draw(SpriteBatch spriteBatch);

        /// <summary>
        /// Toggles if the object if enabled or not, and sets all child objects equal to its toggled state
        /// </summary>
        public virtual void Toggle()
        {
            // Check if parent exists
            if (parent != null)
            {
                enabled = parent.enabled;
            }
            else
            {
                enabled = !enabled;
            }

            foreach (GameObject child in childObjects.Values)
            {
                child.Toggle();
            }
        }

        /// <summary>
        /// Adds a child object to this parent
        /// </summary>
        /// <param name="child">The child GameObject to add</param>
        public void AddChildObject(GameObject child)
        {
            childObjects.Add(child.name, child);
            child.parent = this;
        }

        /// <summary>
        /// Gets a child object by name
        /// </summary>
        /// <param name="name">The name of the child object</param>
        /// <returns>The retrieved GameObject</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the name is not a valid name of a child object</exception>
        public GameObject GetChildObject(string name)
        {
            if (childObjects.TryGetValue(name, out GameObject child))
            {
                return child;
            }

            throw new ArgumentOutOfRangeException(
                "name",
                $"{name} is not a name for a child object (is it being added to the children upon construction?)"
            );
        }
    }
}
