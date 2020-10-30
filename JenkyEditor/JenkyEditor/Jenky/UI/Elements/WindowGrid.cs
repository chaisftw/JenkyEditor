using Jenky.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jenky.UI
{
    public class WindowGrid : UIElement
    {
        #region vars

        private Texture2D uiTexture;

        #endregion

        #region init

        public WindowGrid(int positionX, int positionY, int columns, int rows, int _scale, Texture2D _uiTexture, NineSlice nineSlice) : base(positionX, positionY, nineSlice.SliceWidth * columns, nineSlice.SliceHeight * rows, _scale)
        {
            uiTexture = _uiTexture;
            BuildNineSliceElement(columns, rows, nineSlice);
        }

        #endregion

        #region methods

        public void Update(GameTime gameTime)
        {
            animations.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animations.Draw(spriteBatch, uiTexture);
        }

        #endregion
    }
}
