using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace SlyDeck.GameObjects.UI
{
    public delegate void ClickedDelegate();
    internal interface IClickable
    {
        event ClickedDelegate Clicked;
        Rectangle Bounds { get; }
        void OnClick();
    }
}
