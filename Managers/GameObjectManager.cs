using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Graphics;
using SlyDeck.GameObjects;
using SlyDeck.GameObjects.UI;

// Authors: Cooper Fleishman, Vinny Keeler
namespace SlyDeck.Managers
{
    /// <summary>
    /// Manager for all GameObjects.
    /// </summary>
    internal class GameObjectManager
    {
        private Dictionary<string, GameObject> gameObjects;

        // Singleton
        private static GameObjectManager instance;
        public static GameObjectManager Instance
        {
            get { return GetInstance(); }
        }

        private GameObjectManager()
        {
            gameObjects = new Dictionary<string, GameObject>();
        }

        /// <summary>
        /// Gets the singleton instance of this manager. If no instance exists, it creates a new one.
        /// </summary>
        /// <returns>The instance of this manager</returns>
        private static GameObjectManager GetInstance()
        {
            if (instance == null)
            {
                instance = new GameObjectManager();
            }

            return instance;
        }

        /// <summary>
        /// Draws all elements in the gameobjects lookup table
        /// </summary>
        /// <param name="spriteBatch">The spritebatch to draw to</param>
        public void DrawAll(SpriteBatch spriteBatch)
        {
            foreach (GameObject gameObject in gameObjects.Values)
            {
                if (gameObject.Enabled)
                {
                    gameObject.Draw(spriteBatch);
                }
            }
        }

        /// <summary>
        /// Adds a gameobject to a lookup table
        /// </summary>
        /// <param name="gameObject">The gameobject to add</param>
        /// <exception cref="ArgumentException">If the gameobject could not be added</exception>
        public void AddGameObject(GameObject gameObject)
        {
            if (!gameObjects.TryAdd(gameObject.Name, gameObject))
            {
                throw new ArgumentException(
                    $"Element \"{gameObject.Name}\" was not added successfuly to GUI dictionary. (Is this element already present in the dictionary?)"
                );
            }
        }

        /// <summary>
        /// Attempts to add a gameobject to the lookup table
        /// </summary>
        /// <param name="gameObject">The gameobject to try to add</param>
        /// <returns>True if the gameobject was added, false otherwise</returns>
        public bool TryAddGameObject(GameObject gameObject)
        {
            return gameObjects.TryAdd(gameObject.Name, gameObject);
        }

        /// <summary>
        /// Gets a gameobject by a specified name
        /// </summary>
        /// <param name="name">The name of the gameobject</param>
        /// <returns>The gameobject found</returns>
        /// <exception cref="ArgumentOutOfRangeException">If the name does not exist in the dictionary</exception>
        public GameObject GetGameObject(string name)
        {
            if (gameObjects.TryGetValue(name, out GameObject value))
            {
                return value;
            }
            else
            {
                throw new ArgumentOutOfRangeException(
                    "name",
                    $"{name} is not a valid key in the dictionary"
                );
            }
        }

        /// <summary>
        /// Attempts to get a gameobject by a specified name
        /// </summary>
        /// <param name="name">The name of the gameobject</param>
        /// <param name="value">When this method returns, contains the game object with the provided name, if found. Returns a default GameObject otherwise.</param>
        /// <returns>True if the gameobject was found, false otherwise</returns>
        public bool TryGetGameObject(string name, out GameObject value)
        {
            return gameObjects.TryGetValue(name, out value); // what does a default gameobject even look like??
        }

        /// <summary>
        /// Removes a gameobject by a specified name
        /// </summary>
        /// <param name="name">The name of the gamobject to remove</param>
        public void RemoveGameObject(string name)
        {
            gameObjects.Remove(name);
        }

        /// <summary>
        /// Gets all game objects added to the lookup table
        /// </summary>
        /// <returns>A list containing all game objects</returns>
        public List<GameObject> GetAllGameObjects()
        {
            return gameObjects.Values.ToList();
        }


        /// <summary>
        /// Clears all GameObjects in the GameObjects dictionary
        /// </summary>
        public void ClearAllGameObjects()
        {
            gameObjects.Clear();
        }
    }
}
