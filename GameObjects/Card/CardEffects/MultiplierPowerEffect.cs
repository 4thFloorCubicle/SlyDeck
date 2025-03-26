using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Authors: Cooper Fleishman
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
                case PowerType.BasePower:
                    Owner.BasePower *= multiplier;
                    break;
                case PowerType.TotalPower:
                    Owner.EffectPower *= multiplier;
                    break;
            }
        }
    }
}
