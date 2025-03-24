using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SlyDeck.GameObjects.UI;
using SlyDeck.Managers;

// Authors: Cooper Fleishman
namespace SlyDeck.GameObjects.Card.CardEffects
{
    /// <summary>
    /// For testing decorator functionality only
    /// </summary>
    internal class TestEffect : ICardEffect
    {
        private int procCount;

        public void Perform()
        {
            procCount++;
            Label label = (Label)GameObjectManager.Instance.GetGameObject("Test Label");

            label.Text = $"Effect use count: {procCount}";
            label.TextColor = Color.Red;
        }
    }
}
