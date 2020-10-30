using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Jenky.Graphics;

namespace Jenky.UI
{
    public class LeftWindowHeader : UIElement
    {
        #region vars

        private string label;
        private Vector2 labelPosition;

        private Texture2D uiTexture;

        private SpriteFont font;
        private Color fontColor;

        private StillFrame start;
        private StillFrame middle;
        private StillFrame end;

        #endregion

        #region init

        public LeftWindowHeader(int positionX, int positionY, int _width, int _height, int _scale, string _label, Texture2D _uiTexture, SpriteFont _font, Color _fontColor, StillFrame _start, StillFrame _middle, StillFrame _end) : base(positionX, positionY, _width, _height, _scale)
        {
            start = _start;
            middle = _middle;
            end = _end;

            uiTexture = _uiTexture;

            font = _font;
            fontColor = _fontColor;

            label = _label;

            //Resize the stillframes to fit described width
            int middleLength = width - (start.DestinationRectangle.Width + end.DestinationRectangle.Width);

            middle.Resize(middleLength, height);

            animations.AddSetAnimation(start, Vector2.Zero);
            animations.AddSetAnimation(middle, new Vector2((start.DestinationRectangle.Width * scale), 0));
            animations.AddSetAnimation(end, new Vector2(((start.DestinationRectangle.Width + middle.DestinationRectangle.Width) * scale), 0));
            
            labelPosition = position + new Vector2((_start.DestinationRectangle.Width + 1) * scale, 1 * scale);
            
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
