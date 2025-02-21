using Microsoft.Xna.Framework.Graphics;
using SlyDeck.GameObjects;
using SlyDeck.GameObjects.UI;
using System;
using System.Collections.Generic;
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

        // acts like a constructor and get at the same time
        private static GameObjectManager GetInstance()
        {
            if (instance == null)
            {
                instance = new GameObjectManager();
            }

            return instance;
        }

        public void DrawAll(SpriteBatch spriteBatch)
        {
            foreach (GameObject gameObject in gameObjects.Values)
            {
                gameObject.Draw(spriteBatch);
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

        public bool TryGetGUIElement(string name, out IUserInterface value)
        {
            return gui.TryGetValue(name, out value);
        }

        public void AddGameObject(GameObject gameObject)
        {
            if (!gameObjects.TryAdd(gameObject.Name, gameObject))
            {
                throw new ArgumentException($"Element \"{gameObject.Name}\" was not added successfuly to GUI dictionary. (Is this element already present in the dictionary?)");
            }
        }

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
    }
}
