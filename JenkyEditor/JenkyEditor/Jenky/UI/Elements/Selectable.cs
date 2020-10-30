using System;
using Jenky.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jenky.UI
{
    public class Selectable : UIElement
    {
        #region vars

        public int itemID;
        public Rectangle destinationRectangle;
        private Texture2D iconTexture;

        #endregion 

        #region init

        public Selectable(int positionX, int positionY, int _width, int _height, int _scale, int _itemID, Texture2D _iconTexture, StillFrame icon) : base(positionX, positionY, _width, _height, _scale)
        {
            itemID = _itemID;

            iconTexture = _iconTexture;

            icon.Resize(width, height);
            animations.AddSetAnimation(icon, Vector2.Zero);
        }

        #endregion 

        #region methods

        public void RePosition(Vector2 _position)
        {
            position = _position;
            animations.position = position;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animations.Draw(spriteBatch, iconTexture);
        }

        #endregion
    }
}
