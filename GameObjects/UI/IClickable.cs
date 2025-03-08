using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SlyDeck.GameObjects.UI
{
    public enum MouseButton
    {
        Left,
        Middle,
        Right
    }

    public delegate void ClickedDelegate();
    
    /// <summary>
    /// Interface defining clicking fucntionality via mouse
    /// </summary>
    internal interface IClickable
    {
        event ClickedDelegate LeftClick;
        event ClickedDelegate MiddleClick;
        event ClickedDelegate RightClick;
        Rectangle Bounds { get; }
        void OnLeftClick();
        void OnRightClick();
        void OnMiddleClick();
    }
}
