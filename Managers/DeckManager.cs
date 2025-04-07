using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlyDeck.Decks;
using SlyDeck.GameObjects.Card;
using SlyDeck.GameObjects.Card.CardEffects;

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
        /// <returns>A deck constructed from the deck file</returns>
        /// <exception cref="FormatException">Thrown when part of the file is not in format</exception>
        public Deck DeckFromFile(string filepath)
        {
            List<Card> cards = new List<Card>();

            using (StreamReader reader = new StreamReader(filepath))
            {
                while (!reader.EndOfStream)
                {
                    //Header1|Header|Keyword2|2|First Header Ability|1|\Images\Header.png
                    string[] cardValues = reader.ReadLine().Split('|');

                    string cardName = cardValues[0];

                    CardType cardType;
                    switch (cardValues[1])
                    {
                        case "Title":
                            cardType = CardType.Title;
                            break;
                        case "List":
                            cardType = CardType.List;
                            break;
                        case "Picture":
                            cardType = CardType.Picture;
                            break;
                        case "Graph":
                            cardType = CardType.Graph;
                            break;
                        case "Transition":
                            cardType = CardType.Transition;
                            break;
                        default:
                            throw new FormatException(
                                $"Invalid card type found in file {filepath}"
                            );
                    }

                    string keyword = cardValues[2];
                    int effectValue = int.Parse(cardValues[3]);
                    ICardEffect effect;
                    switch (keyword)
                    {
                        case "Boost":
                            effect = new AdditivePowerEffect(effectValue, PowerType.EffectPower);
                            break;
                        default:
                            throw new NotImplementedException(
                                "Effect names have not been declared"
                            );
                    }

                    string effectDescription = cardValues[4];
                    int basePower = int.Parse(cardValues[5]);
                    string imageDirectory = cardValues[6];
                    string imageName = imageDirectory.Split('\\')[1].Split('.')[0];
                    Texture2D cardArt = AssetManager.Instance.GetAsset<Texture2D>(imageName);

                    CardData data = new CardData(
                        cardName,
                        AssetManager.Instance.GetAsset<Texture2D>("CardDraft"),
                        effectDescription,
                        basePower,
                        cardType,
                        cardArt,
                        new List<ICardEffect> { effect }
                    );

                    Card card = new Card(new Vector2(100, 100), data);
                    card.Toggle();
                    cards.Add(card);
                }
            }

            return new Deck(cards);
        }
    }
}
