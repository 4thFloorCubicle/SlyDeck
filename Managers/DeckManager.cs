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

// Authors: Cooper Fleishman, Shane Packard
namespace SlyDeck.Managers
{
    /// <summary>
    /// Manager for decks. Manages deck creation and other card storage needs
    /// </summary>
    internal class DeckManager
    {
        private static DeckManager instance;
        private Dictionary<string, Deck> decks; // all the diff decks in the game
        public Dictionary<string, CardData> cardData; // all the diff cards in the game

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
                        int abilityPower = int.Parse(cardValues[3]);
                        ICardEffect effect; // base effect
                        switch (keyword)
                        {
                            case "Confirm": // confirm
                            {
                                ICardEffect attachment = new AdditivePowerEffect(
                                    2 * abilityPower,
                                    PowerType.TempPersuasion
                                );
                                effect = new AttacherEffect(attachment, TargetMode.PlayerHand);
                                break;
                            }
                            case "Rebute": // rebute
                            {
                                ICardEffect attachment = new AdditivePowerEffect(
                                    abilityPower,
                                    PowerType.TempPersuasion
                                );
                                effect = new AttacherEffect(
                                    attachment,
                                    TargetMode.EnemyNextCardPlayed
                                );
                                break;
                            }
                            case "No_Effect":
                                //add an effect that converts abilityPower to persuasion
                                effect = null;
                                break;
                            case "Pizza_Party":
                            {
                                ICardEffect attachment = new AdditivePowerEffect(
                                    (float)Math.Round(1.5 * abilityPower + 0.5),
                                    PowerType.TempPersuasion
                                );
                                effect = new AttacherEffect(attachment, TargetMode.PlayerDeck);
                                break;
                            }
                            case "Overtime":
                            {
                                ICardEffect attachment = new AdditivePowerEffect(
                                    (float)Math.Round(.5 * abilityPower + 0.5),
                                    PowerType.TempPersuasion
                                );
                                effect = new AttacherEffect(attachment, TargetMode.EnemyDeck);
                                break;
                            }
                            case "Favoritism":
                            {
                                //make sure this decrease by the right amount I think it does the opposite i.e. 80% instead of 20%
                                //doesnt work with support (20% degredation at 0 Power)
                                ICardEffect attachment = new MultiplierPowerEffect(
                                    (float)(.8 * (1 - (3 / Math.Round(abilityPower + 3.5)))),
                                    PowerType.TempPersuasion
                                );
                                effect = new AttacherEffect(
                                    attachment,
                                    TargetMode.EnemyNextCardPlayed
                                );
                                break;
                            }
                            case "Promotion": //Doesn't Work Yet
                            {
                                //Upgrades persuasion?
                                ICardEffect attachment = new AdditivePowerEffect(
                                    abilityPower,
                                    PowerType.Persuasion
                                );
                                effect = new AttacherEffect(
                                    attachment,
                                    TargetMode.PlayerNextCardPlayed
                                );

                                //Supposed to upgrade effect value
                                attachment = new AdditivePowerEffect(
                                    (float)Math.Floor(0.5 * abilityPower),
                                    PowerType.AbilityEffect
                                );
                                effect = new AttacherEffect(
                                    attachment,
                                    TargetMode.PlayerNextCardPlayed
                                );
                                break;
                            }
                            case "Undermine": //Doesn't Work Yet
                            {
                                //same problem as favoritism
                                ICardEffect attachment = new MultiplierPowerEffect(
                                    (float)(.8 * (1 - (3 / Math.Round(abilityPower + 3.5)))),
                                    PowerType.TempAbilityEffect
                                );
                                effect = new AttacherEffect(
                                    attachment,
                                    TargetMode.EnemyNextCardPlayed
                                );
                                break;
                            }
                            case "Support": //Doesn't Work Yet
                            {
                                ICardEffect attachment1 = new AdditivePowerEffect(
                                    1.5f * abilityPower * abilityPower,
                                    PowerType.TempPersuasion
                                );

                                ICardEffect attachment2 = new MultiplierPowerEffect(
                                    0,
                                    PowerType.TempAbilityEffect
                                );

                                List<ICardEffect> attachments = new List<ICardEffect>();
                                attachments.Add(attachment1);
                                attachments.Add(attachment2);

                                effect = new AttacherEffect(attachments, TargetMode.PlayerHand);
                                break;
                            }

                            default:
                                throw new NotImplementedException(
                                    $"Effect keyword {keyword} have not been declared"
                                );
                        }
                        string effectDescription = cardValues[4];
                        int persuasion = int.Parse(cardValues[5]);
                        string imageDirectory = cardValues[6];
                        string imageName = imageDirectory.Split('\\')[2].Split('.')[0];
                        Texture2D cardArt = AssetManager.Instance.GetAsset<Texture2D>(imageName);

                        // construct card data from read values
                        data = new CardData(
                            cardName,
                            AssetManager.Instance.GetAsset<Texture2D>("CardDraft"),
                            effectDescription,
                            persuasion,
                            cardType,
                            cardArt,
                            new List<ICardEffect> { effect },
                            abilityPower
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
