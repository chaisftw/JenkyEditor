using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Jenky.IO;
using Jenky.Graphics;
using Jenky.Content;

namespace Jenky.States
{
    public abstract class GameState : State
    {
        #region vars

        protected ContentHandler contentHandler;
        protected GraphicsDevice graphicsDevice;
        protected Camera camera;
        protected InputHandler input;
        #endregion

        #region init

        public GameState(GraphicsDevice _graphicsDevice, ContentHandler _contentHandler)
        {
            graphicsDevice = _graphicsDevice;
            contentHandler = _contentHandler;
            input = new InputHandler();
            camera = new Camera(graphicsDevice.Viewport, input);
        }

        #endregion

        #region abstract_methods

        public abstract void Draw(SpriteBatch spriteBatch);

        #endregion

        #region methods

        public Matrix GetCameraTransform()
        {
            return camera.GetTransForm();
        }

        #endregion
    }
}
