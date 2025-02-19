using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlyDeck.GameObject.Card
{
    internal enum CardType
    {
        Header,
        Footer,
        Picture,
        Graph,
        Transition
    }

    internal class Card : GameObject
    {
        private string name;
        private string description;
        private int stat1;
        private int stat2;
        private int cost;

        public Card(Vector2 position, Texture2D texture) : base(position, texture)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            throw new NotImplementedException();
        }
    }
}
