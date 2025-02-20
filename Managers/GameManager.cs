using SlyDeck.GameObject.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlyDeck.Managers
{
    /// <summary>
    /// Manager for all GameObjects
    /// </summary>
    internal class GameManager
    {
        // Fields
        private Dictionary<string, IUserInterface> gui;
        // dictionary for non-ui objects

        // properties
        public Dictionary<string, IUserInterface> GUI { get { return gui; } }

        // Singleton
        private static GameManager instance;
        public static GameManager Instance { get { return GetInstance(); } }

        private GameManager()
        {
            gui = new Dictionary<string, IUserInterface>();
        } 

        // acts like a constructor and get at the same time
        private static GameManager GetInstance()
        {
            if (instance == null)
            {
                instance = new GameManager();
            }

            return instance;
        }

        /// <summary>
        /// Adds a GUI element to the game manager
        /// </summary>
        /// <param name="element">Element to add</param>
        public void AddGUIElement(IUserInterface element)
        {
            gui.Add(element.Name, element);
        }
    }
}
