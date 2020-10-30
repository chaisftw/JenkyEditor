using System;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

using Jenky.IO;

namespace Jenky.UI
{
    public abstract class UIScrollableElement : UIElement
    {
        #region vars

        //Input
        protected InputHandler input;
        protected bool Scrolling;
        protected Vector2 oldMousePosition;

        #endregion

        #region init

        protected UIScrollableElement(int positionX, int positionY, int _width, int _height, int _scale, InputHandler _input) : base(positionX, positionY, _width, _height, _scale, true)
        {
            input = _input;
            oldMousePosition = input.MousePosition();
            Scrolling = false;
        }

        #endregion

        #region methods

        public void Attach()
        {
            Scrolling = true;
        }

        public void Dettach()
        {
            Scrolling = false;
        }

        protected Vector2 GetMoveDistance()
        {
            var mousePosition = input.MousePosition();
            return new Vector2(mousePosition.X - oldMousePosition.X, mousePosition.Y - oldMousePosition.Y);
        }

        #endregion
    }
}
