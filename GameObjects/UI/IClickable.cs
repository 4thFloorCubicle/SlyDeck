using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

// Authors: Cooper Fleishman
namespace SlyDeck.GameObjects.UI
{
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

        /// <summary>
        /// Calls all subscribers to the LeftClicked event.
        /// </summary>
        void OnLeftClick();

        /// <summary>
        /// Calls all subscribers to the RightClicked event;
        /// </summary>
        void OnRightClick();

        /// <summary>
        /// Calls all subscribers to the MiddleClicked event;
        /// </summary>
        void OnMiddleClick();
    }
}
