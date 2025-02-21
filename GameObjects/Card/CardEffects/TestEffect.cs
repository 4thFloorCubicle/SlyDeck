using Microsoft.Xna.Framework;
using SlyDeck.GameObjects.UI;
using SlyDeck.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlyDeck.GameObjects.Card.CardEffects
{
    internal class TestEffect : ICardEffect
    {
        private string text;
        
        public TestEffect(string text)
        {
            this.text = text;
        }
        
        public void Perform()
        {
            Label label = (Label)GameObjectManager.Instance.GetGUIElement("Test Label");

            label.Text = text; 
            label.TextColor = Color.LightBlue;
        }
    }
}
