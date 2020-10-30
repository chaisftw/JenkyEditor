using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Jenky.Graphics;

namespace Jenky.UI
{
    public class WindowHeader : UIElement
    {
        #region vars

        private string label;
        private Vector2 labelPosition;

        private Texture2D uiTexture;
        private SpriteFont font;

        private Color fontColor;
        private ThreeSlice threeSlice;

        #endregion

        #region init

        public WindowHeader(int positionX, int positionY, int _width, int _height, int _scale, string _label,  Texture2D _uiTexture, SpriteFont _font, Color _fontColor, ThreeSlice _threeSlice) : base(positionX, positionY, _width, _height, _scale)
        {
            threeSlice = _threeSlice;
            label = _label;

            uiTexture = _uiTexture;
            font = _font;

            fontColor = _fontColor;
            Vector2 textDimensions = font.MeasureString(label);

            textDimensions = textDimensions * scale;

            labelPosition = position + new Vector2((physicalWidth / 2) - (textDimensions.X / 2), 1 * scale);

            StretchThreeSlice(threeSlice);
        }

        #endregion

        #region methods

        public override void Draw(SpriteBatch spriteBatch)
        {
            animations.Draw(spriteBatch, uiTexture);
            spriteBatch.DrawString(font, label, labelPosition, fontColor, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        #endregion
    }
}
