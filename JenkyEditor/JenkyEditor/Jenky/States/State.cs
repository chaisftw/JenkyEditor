using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jenky.States
{
    public abstract class State
    {
        #region abstract_methods

        public abstract void Update(GameTime gameTime);

        #endregion
    }
}
