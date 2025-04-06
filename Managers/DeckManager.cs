using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlyDeck.Decks;
using SlyDeck.GameObjects.Card;

// Authors: Cooper Fleishman
namespace SlyDeck.Managers
{
    /// <summary>
    /// Manager for decks. Manages deck creation and other card storage needs
    /// </summary>
    internal class DeckManager
    {
        private static DeckManager instance;
        private Dictionary<string, Deck> decks; // all the diff decks in the game
        private Dictionary<string, CardData> cardData; // all the diff cards in the game

        public static DeckManager Instance
        {
            get { return GetInstance(); }
        }

        public DeckManager()
        {
            decks = new Dictionary<string, Deck>();
            cardData = new Dictionary<string, CardData>();
        }

        private static DeckManager GetInstance()
        {
            if (instance == null)
            {
                instance = new DeckManager();
            }

            return instance;
        }

        /// <summary>
        /// Creates a deck from a file
        /// </summary>
        /// <param name="filepath">The filepath to the deck file</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Deck DeckFromFile(string filepath)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Loads all cards from a folder
        /// </summary>
        /// <param name="cardDirectory">The folder to all cards useable in the game</param>
        public void LoadAllCards(string cardDirectory)
        {
            throw new NotImplementedException();
        }
    }
}
