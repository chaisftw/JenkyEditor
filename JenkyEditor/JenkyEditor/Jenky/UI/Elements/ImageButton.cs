using System;
using System.Collections.Generic;

using Jenky.Graphics;
using Jenky.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace Jenky.UI
{
    public class ImageButton : Button
    {
        #region vars

        private Rectangle iconBounds;

        private StillFrame icon;

        private Tooltip tooltip;

        private SpriteFont font;
        private Texture2D lineTexture;

        #endregion

        #region init

        public ImageButton( int positionX, int positionY, int _width, int _height, int _scale, Action _pressEvent, Texture2D uiTexture, Texture2D _lineTexture, SpriteFont _font, Color backgroundColor, Color lineColor, StillFrame _icon, NineSlice _buttonSlice, NineSlice _inactiveSlice, NineSlice _hoverSlice, string tooltipText, InputHandler _input) : base(positionX, positionY, _width, _height, _scale, _pressEvent, uiTexture, _buttonSlice, _inactiveSlice, _hoverSlice, _input)
        {
            icon = _icon;
            lineTexture = _lineTexture;
            font = _font;

            int iconWidth = icon.SourceRectangle.Width * scale;
            int iconHeight = icon.SourceRectangle.Height * scale;
            int iconX = (int)position.X + ((physicalWidth - iconWidth) / 2);
            int iconY = (int)position.Y + ((physicalHeight - iconHeight) / 2);
            
            iconBounds = new Rectangle(iconX, iconY, iconWidth, iconHeight);

            tooltip = new Tooltip(positionX, positionY, physicalWidth, physicalHeight, tooltipText, scale, lineTexture, font, backgroundColor, lineColor);

            StretchNineSlice(buttonSlice);
        }

        #endregion

        #region methods

        public override void Draw(SpriteBatch spriteBatch)
        {
            animations.Draw(spriteBatch, uiTexture);
            spriteBatch.Draw(uiTexture, iconBounds, icon.SourceRectangle, Color.White);
        }
        
        public void DrawTooltip(SpriteBatch spriteBatch)
        {
            if (hovering)
            {
                tooltip.Draw(spriteBatch);
            }
        }

        #endregion
    }
}
