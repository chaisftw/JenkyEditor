using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Jenky.Objects;
using Jenky.IO;

namespace Jenky.Graphics
{
    public class Camera
    {
        #region vars

        public bool Scrolling { get; private set; }

        public Vector2 position;
        public float zoom;
        public float rotation;
        private Rectangle bounds;

        public InputHandler input;
        private CameraState cameraState;

        public GameObject focusObject;

        #endregion

        #region init

        public Camera(Viewport view, InputHandler _input)
        {
            input = _input;
            bounds = view.Bounds;
            zoom = 1;

            cameraState = new DragState(this, bounds);
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

        //Update transform matrix if camera is focused
        public void Update(GameTime gameTime)
        {
            cameraState.Update(gameTime);
        }

        //Get the transform matrix of the camera
        public Matrix GetTransForm()
        {
            return
            Matrix.CreateTranslation(new Vector3(position.X, position.Y, 0)) * //Set translation to camera position
            Matrix.CreateRotationZ(rotation) * // Set rotation
            Matrix.CreateScale(zoom)// * //Set zoom
            //Matrix.CreateTranslation(new Vector3(bounds.Width * 0.5f, bounds.Height * 0.5f, 0))
            ; //Set translation to center bounds on position
        }

        //Set object for camera to follow
        public void ChangeFocus(GameObject _focusObject)
        {
            focusObject = _focusObject;
            ChangeState(new FocusState(this));
        }

        //Standard state changing
        public void ChangeState(CameraState _cameraState)
        {
            cameraState = _cameraState;
        }

        #endregion
    }
}
