using Microsoft.Xna.Framework;
using SlyDeck.GameObject.UI;
using SlyDeck.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlyDeck.GameObject.Card.CardEffects
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
            Label label = (Label) GameManager.Instance.GUI["Test Label"];

            label.Text = "Effect used!"; 
            label.TextColor = Color.LightBlue;
        }
    }
}
