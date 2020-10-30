using Jenky.Graphics;
using Jenky.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Jenky.UI
{
    public class TextButton : Button
    {
        #region vars

        private Vector2 labelPosition;

        private SpriteFont font;

        private Color lineColor;
        private string label;

        #endregion

        #region init

        public TextButton( int positionX, int positionY, int _width, int _height, int _scale, Action _pressEvent, string _label, Texture2D _uiTexture, SpriteFont _font, Color _lineColor, NineSlice _buttonSlice, NineSlice _inactiveSlice, NineSlice _hoverSlice, InputHandler _input) : base(positionX, positionY, _width, _height, _scale, _pressEvent, _uiTexture, _buttonSlice, _inactiveSlice, _hoverSlice, _input)
        {
            label = _label;
            font = _font;

            Vector2 textDimensions = font.MeasureString(label);
            
            uiTexture = _uiTexture;

            lineColor = _lineColor;
            labelPosition = position + ((new Vector2(physicalWidth, physicalHeight) - (textDimensions * scale)) / 2);
            StretchNineSlice(buttonSlice);
        }

        #endregion

        #region methods

        public override void Draw(SpriteBatch spriteBatch)
        {
            animations.Draw(spriteBatch, uiTexture);
            spriteBatch.DrawString(font, label, labelPosition, lineColor, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        public void OffsetLabel(int offsetX, int offsetY)
        {
            labelPosition = labelPosition + new Vector2(offsetX * scale, offsetY * scale);
        }

        #endregion
    }
}
