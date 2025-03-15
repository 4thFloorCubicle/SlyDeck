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
    /// <summary>
    /// For testing decorator functionality only
    /// </summary>
    internal class TestEffect : ICardEffect
    {
        private string text;
        
        public TestEffect(string text)
        {
            this.text = text;
        }
        
        public void Perform()
        {
            Label label = (Label)GameObjectManager.Instance.GetGameObject("Test Label");

            label.Text = text; 
            label.TextColor = Color.LightBlue;
        }
    }
}
