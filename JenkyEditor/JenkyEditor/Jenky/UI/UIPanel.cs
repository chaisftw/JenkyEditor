using Jenky.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jenky.UI
{
    public abstract class UIPanel
    {
        #region vars

        public Vector2 position;

        protected int width;
        protected int height;
        protected int physicalWidth;
        protected int physicalHeight;

        protected int scale;

        #endregion 

        #region abstract_methods

        public abstract void Draw(SpriteBatch spriteBatch);

        #endregion 

        #region init

        protected UIPanel(int positionX, int positionY, int _width, int _height, int _scale)
        {
            position = new Vector2(positionX, positionY);

            width = _width;
            height = _height;
            scale = _scale;

            physicalWidth = width * scale;
            physicalHeight = height * scale;
        }

        #endregion 

        #region methods

        public virtual bool InBounds(Vector2 point)
        {
            if (point.X >= position.X && point.X <= (position.X + physicalWidth)) // Within X
            {
                if (point.Y >= position.Y && point.Y <= (position.Y + physicalHeight)) // Within Y
                {
                    return true;
                }
            }
            return false;
        }

        public int GetWidth()
        {
            return width;
        }

        public int GetHeight()
        {
            return height;
        }

        public int GetPhysicalWidth()
        {
            return physicalWidth;
        }

        public int GetPhysicalHeight()
        {
            return physicalHeight;
        }

        #endregion
    }
}
