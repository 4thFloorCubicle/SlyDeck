//system imports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//monogame imports
using SlyDeck.GameObjects;
using SlyDeck.GameObjects.Card;
using SlyDeck.GameObjects.Card.CardEffects;
using SlyDeck.GameObjects.UI;

namespace SlyDeck.Managers
{
    /// <summary>
    /// Which part of the turn is happening currently (Player or Enemy)
    /// </summary>
    public enum TurnState
    {
        Player,
        Enemy,
    }

    //Author: Vinny Keeler
    internal class RoundManager
    {
        private TurnState ts;

        public TurnState TS
        {
            get { return ts; }
            private set { ts = value; }
        }

        //Singleton
        private static RoundManager instance;
        public static RoundManager Instance
        {
            get { return GetInstance(); }
        }

        /// <summary>
        /// Gets an instance of the Round Manager, if one doesn't exist, create it.
        /// </summary>
        /// <returns>The RoundManager instance</returns>
        private static RoundManager GetInstance()
        {
            if (instance == null)
            {
                instance = new RoundManager();
            }

            return instance;
        }

        /// <summary>
        /// RoundManager constructor
        /// </summary>
        private RoundManager()
        {
            ts = TurnState.Player;
        }

        /// <summary>
        /// Advances to the next round
        /// </summary>
        public void NextRound()
        {
            switch (ts)
            {
                case TurnState.Player:
                    ts = TurnState.Enemy;
                    break;
                case TurnState.Enemy:
                    ts = TurnState.Player;
                    break;
            }
        }
    }
}
