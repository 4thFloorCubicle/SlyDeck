using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Authors: Cooper Fleishman, Shane Packard
namespace SlyDeck.GameObjects.Card.CardEffects
{
    internal class MultiplierPowerEffect : ICardEffect
    {
        private float multiplier; // amount to multiply power by
        private PowerType powerType;

        public Card Owner { get; set; }
        public string Name { get; private set; }

        public float Multiplier
        {
            get { return multiplier; }
            set { multiplier = value; }
        }

        /// <summary>
        /// Creates a new multiplier effect
        /// </summary>
        /// <param name="multiplier">The amount to multiply power by</param>
        /// <param name="powerType">The type of power to multiply</param>
        public MultiplierPowerEffect(float multiplier, PowerType powerType)
        {
            Name = $"Multiply {multiplier} ({powerType})";

            this.multiplier = multiplier;
            this.powerType = powerType;
        }

        public void Perform()
        {
            switch (powerType)
            {
                case PowerType.Persuasion:
                    Owner.Persuasion *= multiplier;
                    break;
                case PowerType.TempPersuasion:
                    Owner.TempPersuasion *= multiplier;
                    break;
                case PowerType.AbilityEffect:
                    break;
                case PowerType.TempAbilityEffect:
                    break;
            }
        }
    }
}
