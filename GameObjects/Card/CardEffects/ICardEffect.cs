﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Author: Cooper Fleishman
namespace SlyDeck.GameObjects.Card.CardEffects
{
    /// <summary>
    /// An interface for all different card effects
    /// </summary>
    internal interface ICardEffect
    {
        /// <summary>
        /// 'Owner' of the effect (the card the effect is attached to)
        /// </summary>
        Card Owner { get; set; }

        /// <summary>
        /// Name of the effect
        /// </summary>
        string Name { get; }

        public float AbilityPower { get; set; }

        public float TempAbilityPower { get; set; }

        /// <summary>
        /// Performs the logic this effect contains
        /// </summary>
        public void Perform();
    }
}
