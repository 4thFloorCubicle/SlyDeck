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

// Authors: Vinny Keeler, Cooper Fleishman
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
        None,
    }

    internal class InputManager
    {
        //Singleton Params
        private static InputManager instance;
        public static InputManager Instance
        {
            get { return GetInstance(); }
        }

        //Button States
        private MouseState prevMouseState;
        private KeyboardState prevKeyState;

        private MouseState currentMouseState;
        private KeyboardState currentKeyboardState;

        private InputManager()
        {
            currentMouseState = Mouse.GetState();
            currentKeyboardState = Keyboard.GetState();
        }

        /// <summary>
        /// Gets the instance of the InputManager, creates one if an InputManager doesn't exist
        /// </summary>
        /// <returns>The instance of the InputManager</returns>
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
        /// <param name="mouseButton">Which button (Left, Middle, or Right) to check</param>
        /// <exception cref="ArgumentException">If the MouseButton is one that cannot be checked with this method</exception>
        /// <returns>True if the button was pressed this frame and not last frame, false otherwise</returns>
        public bool SingleMousePress(MouseButton mouseButton)
        {
            switch (mouseButton)
            {
                case MouseButton.Left:
                    return (prevMouseState.LeftButton == ButtonState.Released)
                        && (currentMouseState.LeftButton == ButtonState.Pressed);
                case MouseButton.Middle:
                    return (prevMouseState.MiddleButton == ButtonState.Released)
                        && (currentMouseState.MiddleButton == ButtonState.Pressed);
                case MouseButton.Right:
                    return (prevMouseState.RightButton == ButtonState.Released)
                        && (currentMouseState.RightButton == ButtonState.Pressed);
                default:
                    throw new ArgumentException(
                        $"{nameof(mouseButton)} is not an acceptable input"
                    ); // not totally sure what a good message would be here
            }
        }

        /// <summary>
        /// Checks if a key was pressed this frame and NOT last frame
        /// </summary>
        /// <param name="key">Which key to check</param>
        /// <returns>True if the key was pressed this frame and not last frame, false otherwise</returns>
        public bool SingleKeyPress(Keys key)
        {
            return (prevKeyState.IsKeyUp(key) && currentKeyboardState.IsKeyDown(key));
        }

        /// <summary>
        /// Checks if a mouse button was pressed LAST FRAME and released THIS frame
        /// </summary>
        /// <param name="mouseButton">The mouse button to be checked</param>
        /// <returns>True if the button is released this frame, false otherwise</returns>
        /// <exception cref="ArgumentException">Thrown when an invalid MouseButton is supplied</exception>
        public bool ReleaseMouse(MouseButton mouseButton)
        {
            switch (mouseButton)
            {
                case MouseButton.Left:
                    return (prevMouseState.LeftButton == ButtonState.Pressed)
                        && (currentMouseState.LeftButton == ButtonState.Released);
                case MouseButton.Middle:
                    return (prevMouseState.MiddleButton == ButtonState.Pressed)
                        && (currentMouseState.MiddleButton == ButtonState.Released);
                case MouseButton.Right:
                    return (prevMouseState.RightButton == ButtonState.Pressed)
                        && (currentMouseState.RightButton == ButtonState.Released);
                default:
                    throw new ArgumentException(
                        $"{nameof(mouseButton)} is not an acceptable input"
                    );
            }
        }

        /// <summary>
        /// Checks if a was key pressed LAST FRAME and released THIS frame
        /// </summary>
        /// <param name="key">The key to be checked</param>
        /// <returns>True if the key is released this frame, false otherwise</returns>
        public bool ReleaseKey(Keys key)
        {
            return (prevKeyState.IsKeyDown(key)) && (currentKeyboardState.IsKeyUp(key));
        }

        /// <summary>
        /// Checks for if a specific mouse button is pressed THIS frame
        /// </summary>
        /// <returns>Whether the button supplied was pressed or not</returns>
        public bool CheckMousePress(MouseButton btnPressed)
        {
            switch (btnPressed)
            {
                case MouseButton.Left:
                    prevMouseState = currentMouseState;
                    return currentMouseState.LeftButton == ButtonState.Pressed;
                case MouseButton.Middle:
                    prevMouseState = currentMouseState;
                    return currentMouseState.MiddleButton == ButtonState.Pressed;
                default:
                    prevMouseState = currentMouseState;
                    return currentMouseState.RightButton == ButtonState.Pressed;
            }
        }

        /// <summary>
        /// Checks if a specific key was pressed this frame
        /// </summary>
        /// <param name="key">The key to check</param>
        /// <returns>Whether the key supplied was pressed or not</returns>
        public bool CheckKeyDown(Keys key)
        {
            return currentKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// 'Refreshes' the keyboard/mouse states
        /// </summary>
        public void RefreshInput()
        {
            // refresh keyboard
            prevKeyState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            // refresh mouse
            prevMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
        }
    }
}
