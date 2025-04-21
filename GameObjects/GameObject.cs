using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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
        private Vector2 position; // the position of this gameobject
        private bool globallyEnabled; // if the object is enable with respect to the parent
        private bool locallyEnabled; // if the object is enabled with respect to itself
        private string name; // the name of this gameobject
        private Dictionary<string, GameObject> childObjects; // the children of this gameobject
        private GameObject? parent; // the parent of this gameobject
        private float scale; // The scale of the gameobject when drawing to the screen

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
        public virtual Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public virtual float Scale
        {
            get { return scale; }
            set { scale = value; }
        }

        /// <summary>
        /// True if this GameObject is both locally and globally enabled, false otherwise
        /// </summary>
        public bool Enabled
        {
            get { return globallyEnabled && locallyEnabled; }
        }

        /// <summary>
        /// True if this GmaeObject is enabled locally (with respect to itself), false otherwise
        /// </summary>
        public bool LocallyEnabled
        {
            get { return locallyEnabled; }
        }

        /// <summary>
        /// True if this GameObject is enabled globally (with respect to its parent), false otherwise
        /// </summary>
        public bool GloballyEnabled
        {
            get { return globallyEnabled; }
        }

        /// <summary>
        /// The name of this GameObject
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Creates a new GameObject
        /// </summary>
        /// <param name="position">The position of the game object</param>
        public GameObject(Vector2 position, string name)
        {
            this.position = position;
            this.name = name;

            scale = 1;

            globallyEnabled = true;
            locallyEnabled = true;
            childObjects = new Dictionary<string, GameObject>();

            GameObjectManager.Instance.TryAddGameObject(this);
        }

        public abstract void Draw(SpriteBatch spriteBatch);

        /// <summary>
        /// Toggles if the object if enabled or not, and sets all child objects equal to its toggled state
        /// </summary>
        public virtual void Toggle()
        {
            locallyEnabled = !locallyEnabled;

            if (parent != null)
            {
                globallyEnabled = parent.Enabled;
            }
            else
            {
                globallyEnabled = !globallyEnabled;
            }

            foreach (GameObject child in childObjects.Values)
            {
                child.Toggle();
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            // do nothing, let classes implement own update as needed
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
