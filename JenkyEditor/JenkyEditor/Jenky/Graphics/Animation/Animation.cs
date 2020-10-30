using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jenky.Graphics
{
    public abstract class Animation
    {
        #region vars

        public bool Finished { get; protected set; }
        public Rectangle SourceRectangle { get; protected set; }
        public Rectangle DestinationRectangle { get; protected set; }

        #endregion

        #region abstract_methods

        public abstract void Update(GameTime gameTime);

        #endregion
    }
}
