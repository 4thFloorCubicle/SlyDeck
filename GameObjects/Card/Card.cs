using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        private List<ICardEffect> effects;

        public Card(Vector2 position, Texture2D texture, string name, string description, int stat1, int stat2, int cost) : base(position, texture)
        {
            this.name = name;
            this.description = description;
            this.stat1 = stat1;
            this.stat2 = stat2;
            this.cost = cost;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}
