using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Jenky.IO
{
    public class InputHandler
    {
        #region vars

        private KeyboardState newKeyboardState;
        private KeyboardState oldKeyboardState;

        private MouseState newMouseState;
        private MouseState oldMouseState;

        #endregion

        #region init

        public InputHandler()
        {
            newKeyboardState = Keyboard.GetState();
            newMouseState = Mouse.GetState();
        }

        #endregion

        #region methods

        public void Update()
        {
            oldKeyboardState = newKeyboardState;
            newKeyboardState = Keyboard.GetState();

            oldMouseState = newMouseState;
            newMouseState = Mouse.GetState();
        }

        #endregion

        #region keyboard

        public bool OnPress(Keys key)
        {
            if (newKeyboardState.IsKeyDown(key) && oldKeyboardState.IsKeyUp(key))
            {
                return true;
            }
            else return false;
        }

        public bool OnRelease(Keys key)
        {
            if (newKeyboardState.IsKeyUp(key) && oldKeyboardState.IsKeyDown(key))
            {
                return true;
            }
            else return false;
        }

        // These functions may be redundant, maybe use inheritance instead?
        public bool IsKeyDown(Keys key)
        {
            return newKeyboardState.IsKeyDown(key);
        }

        public bool IsKeyUp(Keys key)
        {
            return newKeyboardState.IsKeyUp(key);
        }

        #endregion keyboard

        #region mouse

        public bool MouseOnePress()
        {
            if (newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
            {
                return true;
            }
            else return false;
        }

        public bool MouseOneDown()
        {
            if (newMouseState.LeftButton == ButtonState.Pressed)
            {
                return true;
            }
            else return false;
        }

        public bool MouseOneReleased()
        {
            if (newMouseState.LeftButton == ButtonState.Released && oldMouseState.LeftButton == ButtonState.Pressed)
            {
                return true;
            }
            else return false;
        }

        public bool MouseTwoPress()
        {
            if (newMouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released)
            {
                return true;
            }
            else return false;
        }

        public bool MouseTwoDown()
        {
            if (newMouseState.RightButton == ButtonState.Pressed)
            {
                return true;
            }
            else return false;
        }

        public bool MouseTwoReleased()
        {
            if (newMouseState.RightButton == ButtonState.Released && oldMouseState.RightButton == ButtonState.Pressed)
            {
                return true;
            }
            else return false;
        }

        public bool MouseThreePress()
        {
            if (newMouseState.MiddleButton == ButtonState.Pressed && oldMouseState.MiddleButton == ButtonState.Released)
            {
                return true;
            }
            else return false;
        }

        public bool MouseThreeDown()
        {
            if (newMouseState.MiddleButton == ButtonState.Pressed)
            {
                return true;
            }
            else return false;
        }

        public bool MouseThreeReleased()
        {
            if (newMouseState.MiddleButton == ButtonState.Released && oldMouseState.MiddleButton == ButtonState.Pressed)
            {
                return true;
            }
            else return false;
        }

        public Vector2 MousePosition()
        {
            var mousePosition = newMouseState.Position;
            return new Vector2(mousePosition.X, mousePosition.Y);
        }

        //Check if cursor position is within bounds of a rectangle
        public bool MouseInBounds(Rectangle bounds)
        {
            if (newMouseState.X >= bounds.Left && newMouseState.X <= bounds.Right) // Within X
            {
                if (newMouseState.Y >= bounds.Top && newMouseState.Y <= bounds.Bottom) // Within Y
                {
                    return true;
                }
            }
            return false;
        }

        #endregion mouse
    }
}
