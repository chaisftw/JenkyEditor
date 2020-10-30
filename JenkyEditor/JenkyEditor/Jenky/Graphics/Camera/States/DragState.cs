using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Jenky.IO;
using System;

namespace Jenky.Graphics
{
    public class DragState : CameraState
    {
        #region vars

        private Vector2 oldMousePosition;

        private Rectangle dragBounds; //Bounds which allow camera interaction

        #endregion

        #region init

        public DragState(Camera _camera, Rectangle _dragBounds) : base(_camera)
        {
            dragBounds = _dragBounds;
            oldMousePosition = camera.input.MousePosition();
        }

        #endregion

        #region methods

        public override void Update(GameTime gameTime)
        {
            InputHandler currentInput = camera.input;

            if (camera.Scrolling)
            {
                //Calculate mouse drag distance
                var mousePosition = currentInput.MousePosition();
                Vector2 moveDistance = new Vector2(mousePosition.X - oldMousePosition.X, mousePosition.Y - oldMousePosition.Y);

                //Move accordingly
                camera.position += moveDistance;
            }

            oldMousePosition = currentInput.MousePosition();
        }

        #endregion
    }
}

