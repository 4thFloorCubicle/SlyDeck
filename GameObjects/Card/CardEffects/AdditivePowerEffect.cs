using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Authors: Cooper Fleishman, Shane Packard
namespace SlyDeck.GameObjects.Card.CardEffects
{
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

        public string Name { get; }

        public AdditivePowerEffect(float powerIncrease, PowerType powerType)
        {
            Name = $"Add {powerIncrease} ({powerType})";

            this.powerIncrease = powerIncrease;
            this.powerType = powerType;
        }

        public void Perform()
        {
            switch (powerType)
            {
                case PowerType.Persuasion:
                    Owner.Persuasion += powerIncrease;
                    break;
                case PowerType.TempPersuasion:
                    Owner.TempPersuasion += powerIncrease;
                    break;
                case PowerType.AbilityEffect:
                    break;
                case PowerType.TempAbilityEffect:
                    break;
            }
        }
    }
}
