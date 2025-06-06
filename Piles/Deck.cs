﻿using System;
using System.Collections.Generic;
using SlyDeck.GameObjects.Card;
using SlyDeck.GameObjects.Card.CardEffects;

// Authors: Ben Haines, Cooper Fleishman
namespace SlyDeck.Decks
{
    internal class Deck
    {
        // -- Fields -- \\

        private List<Card> cards;
        private Random shuffle;
        private string name;

        // -- Properties -- \\

        /// <summary>
        /// Gets the cards this deck contains
        /// </summary>
        public List<Card> Cards
        {
            get { return cards; }
        }

        /// <summary>
        /// Gets the size of the deck.
        /// </summary>
        public int Size
        {
            get { return cards.Count; }
        }

        /// <summary>
        /// Gets the top card of the deck
        /// </summary>
        public Card TopCard
        {
            get { return cards[0]; }
        }

        public string Name
        {
            get { return name; }
        }

        // -- Constructor -- \\

        /// <summary>
        /// Create a new deck of cards.
        /// </summary>
        /// <param name="cards">The cards that the deck will use.</param>
        public Deck(string name, List<Card> cards)
        {
            this.name = name;
            this.cards = cards;
            shuffle = new Random();
            Shuffle();
        }

        // -- Methods -- \\

        /// <summary>
        /// Draw a card from the deck of a specified cardNum, 0 being the top of the deck.
        /// </summary>
        /// <param name="cardNum">The number of the card to draw.</param>
        /// <returns>The Card object drawn.</returns>
        public Card DrawCard(int cardNum)
        {
            Card cardDrawn = cards[cardNum];
            cards.RemoveAt(cardNum);
            return cardDrawn;
        }

        /// <summary>
        /// Draw the card on the top of the deck, index 0.
        /// </summary>
        /// <returns>The Card object drawn.</returns>
        public Card DrawCard()
        {
            return DrawCard(0);
        }

        /// <summary>
        /// Adds a specified card to the bottom of the deck.
        /// </summary>
        /// <param name="toAdd">The Card object to add.</param>
        public void AddCardBottom(Card toAdd)
        {
            cards.Add(toAdd);
        }

        /// <summary>
        /// Adds a specified card to the top of the deck.
        /// </summary>
        /// <param name="toAdd">The Card object to add.</param>
        public void AddCardTop(Card toAdd)
        {
            cards.Insert(0, toAdd);
        }

        /// <summary>
        /// Shuffle the deck of cards by randomly ordering them in a new deck.
        /// </summary>
        public void Shuffle()
        {
            List<Card> newCards = new();
            while (cards.Count != 0)
            {
                int randomIndex = shuffle.Next(0, cards.Count);
                newCards.Add(cards[randomIndex]);
                cards.RemoveAt(randomIndex);
            }
            cards.AddRange(newCards);
        }

        /// <summary>
        /// Adds a supplied card effect to the entire deck
        /// </summary>
        /// <param name="effect"></param>
        public void ApplyDeckwideEffect(ICardEffect effect)
        {
            foreach (Card card in cards)
            {
                card.AddEffect(effect);
            }
        }
    }
}
