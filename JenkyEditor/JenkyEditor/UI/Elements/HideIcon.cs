using System;
using System.Collections.Generic;

using Jenky.Graphics;
using Jenky.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;



namespace Jenky.UI
{
    public class HideIcon : UIElement
    {
        #region vars

        private int iconWidth;
        private int iconHeight;
        private int physicalIconWidth;
        private int physicalIconHeight;

        private StillFrame visible;
        private StillFrame invisible;
        
        private Texture2D uiTexture;

        #endregion

        #region init

        public HideIcon( int positionX, int positionY, int _width, int _height, int _scale, Texture2D _uiTexture) : base(positionX, positionY, _width, _height, _scale)
        {
            uiTexture = _uiTexture;

            iconWidth = 12;
            iconHeight = 10;

            physicalIconWidth = iconWidth * scale;
            physicalIconHeight = iconHeight * scale;

            visible = new StillFrame(60, 40, iconWidth, iconHeight);
            invisible = new StillFrame(72, 40, iconWidth, iconHeight);
            
            animations.AddSetAnimation(visible, GetIconPosition());
        }

        #endregion

        #region methods

        public void Reposition(Vector2 newPosition)
        {
            position = newPosition;
            animations.RePosition(newPosition);
        }

        private Vector2 GetIconPosition()
        {
            int iconX = (physicalWidth / 2) - (physicalIconWidth / 2);
            int iconY = (physicalHeight / 2) - (physicalIconHeight / 2);

            return new Vector2(iconX, iconY);
        }

        public void Hide(int depth)
        {
            ClearAnimation();
            animations.AddSetAnimation(invisible, GetIconPosition());
        }

        public void Show(int depth)
        {
            ClearAnimation();
            animations.AddSetAnimation(visible, GetIconPosition());
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animations.Draw(spriteBatch, uiTexture);
        }

        #endregion
    }
}
