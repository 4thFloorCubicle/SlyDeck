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
            string deckName = filepath.Split('\\')[1].Split('.')[0]; // need to change to not rely on magic numbers
            List<Card> cards = new List<Card>();

            using (StreamReader reader = new StreamReader(filepath))
            {
                while (!reader.EndOfStream)
                {
                    //Header1|Header|Keyword2|2|First Header Ability|1|\Images\Header.png
                    string[] cardValues = reader.ReadLine().Split('|');

                    string cardName = cardValues[0];

                    CardData data;

                    // if card has already been read why reread the entire file vs reusing the already created data
                    if (cardData.ContainsKey(cardName))
                    {
                        data = cardData[cardName];
                    }
                    else
                    {
                        CardType cardType = (CardType)Enum.Parse(typeof(CardType), cardValues[1]);

                        // construct card effect
                        string keyword = cardValues[2];
                        int effectValue = int.Parse(cardValues[3]);
                        ICardEffect effect;
                        switch (keyword)
                        {
                            case "Confirm": // confirm
                            {
                                ICardEffect attachment = new AdditivePowerEffect(
                                    effectValue,
                                    PowerType.EffectPower
                                );
                                effect = new AttacherEffect(attachment, TargetMode.PlayerDeck);
                                break;
                            }
                            case "Rebute": // rebute
                            {
                                ICardEffect attachment = new AdditivePowerEffect(
                                    effectValue,
                                    PowerType.EffectPower
                                );
                                effect = new AttacherEffect(attachment, TargetMode.EnemyDeck);
                                break;
                            }
                            case "No_Effect":
                                effect = null;
                                break;
                            default:
                                throw new NotImplementedException(
                                    $"Effect keyword {keyword} have not been declared"
                                );
                        }

                        string effectDescription = cardValues[4];
                        int basePower = int.Parse(cardValues[5]);
                        string imageDirectory = cardValues[6];
                        string imageName = imageDirectory.Split('\\')[2].Split('.')[0];
                        Texture2D cardArt = AssetManager.Instance.GetAsset<Texture2D>(imageName);

                        // construct card data from read values
                        data = new CardData(
                            cardName,
                            AssetManager.Instance.GetAsset<Texture2D>("CardDraft"),
                            effectDescription,
                            basePower,
                            cardType,
                            cardArt,
                            new List<ICardEffect> { effect }
                        );
                    }

                    cardData.Add(data.Name, data);
                    Card card = new Card(new Vector2(100, 100), data);
                    card.Toggle();
                    cards.Add(card);
                }
            }

            Deck deck;

            if (decks.ContainsKey(deckName))
            {
                deck = new Deck(deckName, cards);
                decks.Add(deck.Name, deck);
            }
            else
            {
                deck = new Deck($"{deckName} (Copy)", cards);
            }

            return deck;
        }
    }
}
