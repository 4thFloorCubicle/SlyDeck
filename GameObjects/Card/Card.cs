using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SlyDeck.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlyDeck.GameObjects.Card
{
    internal enum CardType
    {
        Title,
        List,
        Picture,
        Graph,
        Transition
    }

    internal class Card : GameObject
    {
        private string description;
        private int stat1;
        private int stat2;
        private int cost;
        private CardType type;
        private Dictionary<string, ICardEffect> effects;

        public Card(Vector2 position, string name, string description, int stat1, int stat2, int cost, CardType type) : base(position, name)
        {
            this.description = description;
            this.stat1 = stat1;
            this.stat2 = stat2;
            this.cost = cost;
            this.type = type;

            effects = new Dictionary<string, ICardEffect>();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }

        public void AddEffect(string effectName, ICardEffect effect)
        {
            effects.Add(effectName, effect);
        }

        public void Play()
        {
            foreach (ICardEffect effect in effects.Values)
            {
                effect.Perform();
            }
        }
    }
}
