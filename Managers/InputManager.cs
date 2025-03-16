//System Imports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//MonoGame Imports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SlyDeck.Managers
{
    //enums
    /// <summary>
    /// Represents the different mouse buttons to be used as controls within the game
    /// </summary>
    public enum MouseButton
    {
        Left,
        Middle,
        Right,
        None
    }

    //author: Vinny Keeler
    internal class InputManager
    {
        //Singleton Params
        private static InputManager instance;
        public static InputManager Instance { get { return GetInstance() ; } }

        //Button States
        private MouseState prevMouseState;
        private KeyboardState prevKeyState;

        private MouseState currentMouseState;
        private KeyboardState currentKeyboardState;

        /// <summary>
        /// Gets the instance of the InputManager, creates one if an InputManager doesn't exist
        /// </summary>
        /// <returns>The InputManager</returns>
        private static InputManager GetInstance()
        {
            if (instance == null)
            {
                instance = new InputManager();
            }

            return instance;
        }
        
        /// <summary>
        /// Checks if a mouse button was pressed this frame and NOT last frame
        /// </summary>
        /// <param name="btnPressed">Which button (Left, Middle, or Right) was pressed</param>
        /// <returns>True if the button was pressed this frame and not last frame, false otherwise</returns>
        public bool SingleMousePress(MouseButton btnPressed)
        {
            if (btnPressed == MouseButton.Left)
            {
                return (prevMouseState.LeftButton == ButtonState.Released) && (currentMouseState.LeftButton == ButtonState.Pressed);
            }
            else if (btnPressed == MouseButton.Middle)
            {
                return (prevMouseState.MiddleButton == ButtonState.Released) && (currentMouseState.MiddleButton == ButtonState.Pressed);
            }
            else
            {
                return (prevMouseState.RightButton == ButtonState.Released) && (currentMouseState.RightButton == ButtonState.Pressed);
            }
        }

        /// <summary>
        /// Checks if a key was pressed this frame and NOT last frame
        /// </summary>
        /// <param name="key">Which key was pressed</param>
        /// <returns>True if the key was pressed this frame and not last frame, false otherwise</returns>
        public bool SingleKeyPress(Keys key)
        {
            return (prevKeyState.IsKeyUp(key) && currentKeyboardState.IsKeyDown(key));
        }

        /// <summary>
        /// Checks for Mouse Input
        /// </summary>
        /// <returns>Whether the button supplied was pressed or not</returns>
        public bool CheckMouseInput(MouseButton btnPressed)
        {
            currentMouseState = Mouse.GetState();
            if (btnPressed == MouseButton.Left)
            {
                prevMouseState = currentMouseState;
                return (currentMouseState.LeftButton == ButtonState.Pressed);
            }
            else if (btnPressed == MouseButton.Middle)
            {
                prevMouseState = currentMouseState;
                return (currentMouseState.MiddleButton == ButtonState.Pressed);
            }
            else
            {
                prevMouseState = currentMouseState;
                return (currentMouseState.RightButton == ButtonState.Pressed);
            }
        }

        /// <summary>
        /// Checks for Keyboard Input
        /// </summary>
        /// <param name="key">The key to be pressed</param>
        /// <returns>Whether the key supplied was pressed or not</returns>
        public bool CheckKeyboardInput(Keys key)
        {
            currentKeyboardState = Keyboard.GetState();
            prevKeyState = currentKeyboardState;

            return currentKeyboardState.IsKeyDown(key);
        }
    }
}
