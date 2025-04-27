using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
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

            Deck deck;

            if (!decks.ContainsKey(deckName))
            {
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
                            ICardEffect effect = null; // passed in effect value
                            ICardEffect baseEffect = null; // effect @ AP 1
                            switch (keyword)
                            {
                                case "Confirm":
                                    {
                                        float baseVal = 2;
                                        PowerType type = PowerType.TempPersuasion;
                                        TargetMode target = TargetMode.OwnerNextCardPlayed;

                                        ICardEffect[] effects = CreateAdditiveEffect(baseVal, abilityPower, type, target);

                                        baseEffect = new AttacherEffect(effects[0], target);
                                        effect = new AttacherEffect(effects[1], target);
                                        break;
                                    }
                                case "Rebute":
                                    {
                                        float baseVal = 1;
                                        PowerType type = PowerType.TempPersuasion;
                                        TargetMode target = TargetMode.EnemyNextCardPlayed;

                                        ICardEffect[] effects = CreateAdditiveEffect(baseVal, abilityPower, type, target);

                                        baseEffect = new AttacherEffect(effects[0], target);
                                        effect = new AttacherEffect(effects[1], target);
                                        break;
                                    }
                                case "No_Effect":
                                    //add an effect that converts abilityPower to persuasion (stretch goal)
                                    effect = null;
                                    break;
                                case "Pizza_Party":
                                    {
                                        float baseVal = 1.5f;
                                        PowerType type = PowerType.TempPersuasion;
                                        TargetMode target = TargetMode.OwnerDeck;

                                        ICardEffect[] effects = CreateAdditiveEffect(baseVal, abilityPower, type, target);

                                        baseEffect = new AttacherEffect(effects[0], target);
                                        effect = new AttacherEffect(effects[1], target);
                                        break;
                                    }
                                case "Overtime":
                                    {
                                        float baseVal = .5f;
                                        PowerType type = PowerType.TempPersuasion;
                                        TargetMode target = TargetMode.EnemyDeck;

                                        ICardEffect[] effects = CreateAdditiveEffect(baseVal, abilityPower, type, target);

                                        baseEffect = new AttacherEffect(effects[0], target);
                                        effect = new AttacherEffect(effects[1], target);
                                        break;
                                    }
                                case "Favoritism":
                                    {
                                        //I don't know if I can make this work since base effect requires me to remove ability power
                                        //and this equation requres a variable
                                        ICardEffect attachment = new MultiplierPowerEffect(
                                            (float)((.8 * (6 / abilityPower + 6)) + .2),
                                            PowerType.TempPersuasion
                                        );
                                        effect = new AttacherEffect(
                                            attachment,
                                            TargetMode.EnemyNextCardPlayed
                                        );
                                        baseEffect = effect;
                                        break;
                                    }
                                case "Promotion": //Doesn't Work Yet
                                    {
                                        float[] baseVals = {1, .5f};
                                        float[] dynamicVals = { abilityPower, abilityPower};
                                        PowerType[] types = { PowerType.Persuasion, PowerType.AbilityEffect };
                                        TargetMode target = TargetMode.OwnerNextCardPlayed;

                                        List<ICardEffect>[] effects = CreateAdditiveEffectPair(baseVals, dynamicVals, types, target);

                                        baseEffect = new AttacherEffect(effects[0], target);
                                        effect = new AttacherEffect(effects[1], target);
                                        break;
                                    }
                                case "Undermine": //Doesn't Work Yet
                                    {
                                        ICardEffect attachment = new MultiplierPowerEffect(
                                            (float)((.8 * (6 / abilityPower + 6)) + .2),
                                            PowerType.TempAbilityEffect
                                        );
                                        effect = new AttacherEffect(
                                            attachment,
                                            TargetMode.EnemyNextCardPlayed
                                        );
                                        baseEffect = effect;
                                        break;
                                    }
                                case "Support": //Doesn't Work Yet
                                    {
                                        float[] baseVals = { 1.5f, 0 };
                                        float[] dynamicVals = { abilityPower * abilityPower, 0 };
                                        PowerType[] types = { PowerType.TempPersuasion, PowerType.TempAbilityEffect };
                                        TargetMode target = TargetMode.OwnerNextCardPlayed;

                                        List<ICardEffect>[] effects = CreateAdditiveEffectPair(baseVals, dynamicVals, types, target);

                                        baseEffect = new AttacherEffect(effects[0], target);
                                        effect = new AttacherEffect(effects[1], target);
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
                                effect,
                                abilityPower,
                                baseEffect
                            );
                            cardData.Add(data.Name, data);
                        }
                        Card card = new Card(new Vector2(100, 100), data);
                        card.Toggle();
                        cards.Add(card);
                    }
                }

                deck = new Deck(deckName, cards);
                decks.Add(deck.Name, deck);
            }
            else
            {
                deck = new Deck($"{deckName} (Copy)", decks[deckName].Cards);
            }
            
            return deck;
        }

        /// <summary>
        /// Creates an additive card effect and its base effect
        /// </summary>
        /// <param name="staticValue">The value that never changes in the effect</param>
        /// <param name="dynamicValue">The value that changes, multiplied with "staticValue"</param>
        /// <param name="type">The type of power affected</param>
        /// <param name="target">The target of the effect</param>
        /// <returns>Two card effects, the first being the base effect, the second being the current effect that will change based on the current affectValue</returns>
        public ICardEffect[] CreateAdditiveEffect(float staticValue, float dynamicValue, PowerType type, TargetMode target)
        {
            if (target == TargetMode.EnemyDeck || target == TargetMode.EnemyNextCardPlayed)
            {
                staticValue *= -1;
            }

            ICardEffect[] attachments = new ICardEffect[2];
            ICardEffect baseAttachment = new AdditivePowerEffect(
                staticValue,
                type
            );

            ICardEffect attachment = new AdditivePowerEffect(
                staticValue * dynamicValue,
                type
            );

            attachments[0] = baseAttachment;
            attachments[1] = attachment;

            return attachments;
        }

        /// <summary>
        /// Creates a multiplier card effect and its base effect
        /// </summary>
        /// <param name="staticValue">The value that never changes in the effect</param>
        /// <param name="dynamicValue">The value that changes, multiplied with "staticValue"</param>
        /// <param name="type">The type of power affected</param>
        /// <param name="target">The target of the effect</param>
        /// <returns>Two card effects, the first being the base effect, the second being the current effect that will change based on the current affectValue</returns>
        public ICardEffect[] CreateMultiplierEffect(float staticValue, float dynamicValue, PowerType type, TargetMode target)
        {
            if (target == TargetMode.EnemyDeck || target == TargetMode.EnemyNextCardPlayed)
            {
                staticValue *= -1;
            }

            ICardEffect[] attachments = new ICardEffect[2];
            ICardEffect baseAttachment = new MultiplierPowerEffect(
                staticValue,
                type
            );

            ICardEffect attachment = new MultiplierPowerEffect(
                staticValue * dynamicValue,
                type
            );

            attachments[0] = baseAttachment;
            attachments[1] = attachment;

            return attachments;
        }

        /// <summary>
        /// Creates a pair of additive card effects and their base effects in a list
        /// </summary>
        /// <param name="staticValues">The values that never change in the effects</param>
        /// <param name="dynamicValues">The values that change, multiplied with respective "staticValue"</param>
        /// <param name="types">The types of power affected</param>
        /// <param name="target">The target of the effect</param>
        /// <returns>A list of card effects, the first list being the base effect, the second being the current effect that will change based on the current affectValue</returns>
        public List<ICardEffect>[] CreateAdditiveEffectPair(float[] staticValues, float[] dynamicValues, PowerType[] types, TargetMode target)
        {
            if (target == TargetMode.EnemyDeck || target == TargetMode.EnemyNextCardPlayed)
            {
                for (int i = 0; i < staticValues.Length; i++)
                {
                    staticValues[i] *= -1; 
                }
                
            }

            List<ICardEffect>[] effects = new List<ICardEffect>[2];
            ICardEffect[] firstEffect = CreateAdditiveEffect(staticValues[0], dynamicValues[0], types[0], target);
            ICardEffect[] secondEffect = CreateAdditiveEffect(staticValues[1], dynamicValues[1], types[1], target);

            effects[0].Add(firstEffect[0]);
            effects[0].Add(secondEffect[0]);

            effects[1].Add(firstEffect[1]);
            effects[1].Add(secondEffect[1]);

            return effects;
        }

        /// <summary>
        /// Creates a pair of multiplier card effects and their base effects in a list
        /// </summary>
        /// <param name="staticValues">The values that never change in the effects</param>
        /// <param name="dynamicValues">The values that change, multiplied with respective "staticValue"</param>
        /// <param name="types">The types of power affected</param>
        /// <param name="target">The target of the effect</param>
        /// <returns>A list of card effects, the first list being the base effect, the second being the current effect that will change based on the current affectValue</returns>
        public List<ICardEffect>[] CreateMultiplierEffectPair(float[] staticValues, float[] dynamicValues, PowerType[] types, TargetMode target)
        {
            if (target == TargetMode.EnemyDeck || target == TargetMode.EnemyNextCardPlayed)
            {
                for (int i = 0; i < staticValues.Length; i++)
                {
                    staticValues[i] *= -1;
                }

            }

            List<ICardEffect>[] effects = new List<ICardEffect>[2];
            ICardEffect[] firstEffect = CreateMultiplierEffect(staticValues[0], dynamicValues[0], types[0], target);
            ICardEffect[] secondEffect = CreateMultiplierEffect(staticValues[1], dynamicValues[1], types[1], target);

            effects[0].Add(firstEffect[0]);
            effects[0].Add(secondEffect[0]);

            effects[1].Add(firstEffect[1]);
            effects[1].Add(secondEffect[1]);

            return effects;
        }
    }
}
