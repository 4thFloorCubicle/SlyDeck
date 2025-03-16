using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

// Authors: Cooper Fleishman
namespace SlyDeck.GameObjects.UI
{
    /// <summary>
    /// Represents the different mouse buttons to be used as controls within the game
    /// </summary>
    /// Move to input manager in the future
    public enum MouseButton
    {
        Left,
        Middle,
        Right
    }

    public delegate void ClickedDelegate();
    
    /// <summary>
    /// Interface defining clicking functionality via mouse (a 'clickable')
    /// </summary>
    internal interface IClickable
    {
        event ClickedDelegate LeftClick;
        event ClickedDelegate MiddleClick;
        event ClickedDelegate RightClick;
        Rectangle Bounds { get; } // bounds for which this clickable can be clicked
        void OnLeftClick();
        void OnRightClick();
        void OnMiddleClick();
    }
}
