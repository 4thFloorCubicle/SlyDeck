using Microsoft.Xna.Framework.Graphics;
using SlyDeck.GameObjects;
using SlyDeck.GameObjects.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SlyDeck.Managers
{
    /// <summary>
    /// Manager for all GameObjects
    /// </summary>
    internal class GameObjectManager
    {
        // Fields
        private Dictionary<string, IUserInterface> gui;
        // dictionary for non-ui objects
        private Dictionary<string, GameObject> gameObjects;

        // Singleton
        private static GameObjectManager instance;
        public static GameObjectManager Instance { get { return GetInstance(); } }

        private GameObjectManager()
        {
            gui = new Dictionary<string, IUserInterface>();
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
        /// Adds an element to the GUI
        /// </summary>
        /// <param name="element">Element to add to GUI</param>
        /// <exception cref="ArgumentException"></exception>
        public void AddGUIElement(IUserInterface element)
        {
            if (!gui.TryAdd(element.Name, element))
            {
                throw new ArgumentException($"Element \"{element.Name}\" was not added successfuly to GUI dictionary. (Is this element already present in the dictionary?)");
            }

            gameObjects.Add(element.Name, (GameObject)element);
        }

        /// <summary>
        /// Attempts to add an element to the GUI
        /// </summary>
        /// <param name="element">Element to add to the GUI</param>
        /// <returns>True if it was successfully added, false otherwise</returns>
        public bool TryAddGUIElement(IUserInterface element)
        {
            bool success = gui.TryAdd(element.Name, element);
            
            if (success)
            {
                gameObjects.Add(element.Name, (GameObject)element);
            }

            return success;
        }

        /// <summary>
        /// Gets a GUI element by a specified name
        /// </summary>
        /// <param name="name">The name of the GUI element</param>
        /// <returns>The GUI element found</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public IUserInterface GetGUIElement(string name)
        {
            if (gui.TryGetValue(name, out IUserInterface value))
            {
                return value;
            }
            else
            {
                throw new ArgumentOutOfRangeException("name", $"{name} is not a valid key in GUI dictionary");
            }
        }

        /// <summary>
        /// Attempts to get a GUI element by name
        /// </summary>
        /// <param name="name">The name of the GUI element</param>
        /// <param name="element">The value of the GUI element, if found</param>
        /// <returns>True if found, False otherwise</returns>
        public bool TryGetGUIElement(string name, out IUserInterface element)
        {
            return gui.TryGetValue(name, out element);
        }

        /// <summary>
        /// Adds a gameobject to a lookup table
        /// </summary>
        /// <param name="gameObject">The gameobject to add</param>
        /// <exception cref="ArgumentException"></exception>
        public void AddGameObject(GameObject gameObject)
        {
            if (!gameObjects.TryAdd(gameObject.Name, gameObject))
            {
                throw new ArgumentException($"Element \"{gameObject.Name}\" was not added successfuly to GUI dictionary. (Is this element already present in the dictionary?)");
            }
        }

        /// <summary>
        /// Gets a gameobject by a specified name
        /// </summary>
        /// <param name="name">The name of the gameobject</param>
        /// <returns>The gameobject found</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public GameObject GetGameObject(string name)
        {
            if (gameObjects.TryGetValue(name, out GameObject value))
            {
                return value;
            }
            else
            {
                throw new ArgumentOutOfRangeException("name", $"{name} is not a valid key in the dictionary");
            }
        }
        
        /// <summary>
        /// Gets all game objects added to the lookup table
        /// </summary>
        /// <returns>A list containing all game objects</returns>
        public List<GameObject> GetAllGameObjects()
        {
            return gameObjects.Values.ToList<GameObject>();
        }
    }
}
