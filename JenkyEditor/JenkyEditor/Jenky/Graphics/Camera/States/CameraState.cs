using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Jenky.States;

namespace Jenky.Graphics
{
    public abstract class CameraState : State
    {
        #region vars

        protected Camera camera;

        #endregion

        #region init

        public CameraState(Camera _camera)
        {
            camera = _camera;
        }

        #endregion
    }
}

