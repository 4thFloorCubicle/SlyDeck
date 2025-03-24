using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Authors: Cooper Fleishman
namespace SlyDeck.GameObjects.Card.CardEffects
{
    /// <summary>
    /// Enum representing the 2 different types of power in the game
    /// </summary>
    public enum PowerType
    {
        BasePower,
        TotalPower,
    }

    internal class AdditivePowerEffect : ICardEffect
    {
        private float powerIncrease; // value to boost by
        private PowerType powerType;

        /// <summary>
        /// The value to boost the owner by
        /// </summary>
        private float PowerIncrease
        {
            get { return powerIncrease; }
            set { powerIncrease = value; }
        }
        public Card Owner { get; set; }

        public AdditivePowerEffect(float powerIncrease, PowerType powerType)
        {
            this.powerIncrease = powerIncrease;
            this.powerType = powerType;
        }

        public void Perform()
        {
            switch (powerType)
            {
                case PowerType.BasePower:
                    Owner.BasePower += powerIncrease;
                    break;
                case PowerType.TotalPower:
                    Owner.EffectPower += powerIncrease;
                    break;
            }
        }
    }
}
