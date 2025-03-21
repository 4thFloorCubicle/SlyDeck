using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlyDeck.GameObjects.Card;

// Author: Ben Haines
namespace SlyDeck.DiscardPiles
{
    internal class DiscardPile
    {
        // -- Fields -- \\
        private Stack<Card> pile;

        // -- Properties -- \\
        /// <summary>
        /// Return the number of cards currently in the discard pile.
        /// </summary>
        public int Count
        {
            get { return pile.Count; }
        }

        /// <summary>
        /// Return the current top card on the discard pile, do not remove it.
        /// </summary>
        public Card TopCard
        {
            get
            {
                if (Count != 0)
                    return pile.Peek();
                throw new Exception("Cannot view the top card as the discard pile is empty.");
            }
        }

        // -- Constructor -- \\
        public DiscardPile()
        {
            pile = new Stack<Card>();
        }

        // -- Methods -- \\
        /// <summary>
        /// Add a new card to the top of the discard pile.
        /// </summary>
        /// <param name="toDiscard">The card to add to the discard pile.</param>
        public void DiscardCard(Card toDiscard)
        {
            pile.Push(toDiscard);
        }

        /// <summary>
        /// Remove the top card from the discard pile and return it.
        /// </summary>
        /// <returns>The card that was on top of the discard pile.</returns>
        /// <exception cref="Exception">If the discard pile is empty, then there is no card on top.</exception>
        public Card RecardCard()
        {
            if (Count != 0)
                return pile.Pop();
            throw new Exception("Cannot recard the top card as the discard pile is empty.");
        }
    }
}
