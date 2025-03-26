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
    enum Turn
    {
        Player,
        Enemy,
        None
    }
    
    //Author: Vinny Keeler
    internal class RoundManager
    {
        //Singleton
        private static RoundManager instance;
        public static RoundManager Instance
        {
            get { return GetInstance(); }
        }

        private static RoundManager GetInstance()
        {
            if (instance == null)
            {
                instance = new RoundManager();
            }

            return instance;
        }
    }
}
