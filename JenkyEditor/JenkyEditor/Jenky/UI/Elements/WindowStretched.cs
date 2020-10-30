using Jenky.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jenky.UI
{
    public class WindowStretched : UIElement
    {
        #region vars

        private Texture2D uiTexture;

        #endregion

        #region init

        public WindowStretched(int positionX, int positionY, int _width, int _height, int _scale, Texture2D _uiTexture, NineSlice nineSlice) : base(positionX, positionY, _width, _height, _scale)
        {
            uiTexture = _uiTexture;
            StretchNineSlice(nineSlice);
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

