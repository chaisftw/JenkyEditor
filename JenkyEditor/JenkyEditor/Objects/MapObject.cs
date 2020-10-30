using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Jenky.Graphics;

namespace JenkyEditor
{
    public class MapObject
    {
        #region vars

        public int ID { get; set; }

        public Rectangle destinationRectangle;
        public Vector2 mapPosition;

        #endregion

        #region init

        public MapObject(Vector2 _mapPosition, Rectangle _destinationRectangle, int _ID)
        {
            ID = _ID;
            mapPosition = _mapPosition;
            destinationRectangle = _destinationRectangle;
        }

        #endregion

        #region methods

        public void Draw(SpriteBatch spriteBatch, Texture2D spriteTexture, Rectangle sourceRectangle)
        {
            spriteBatch.Draw(spriteTexture, destinationRectangle, sourceRectangle, Color.White);
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D spriteTexture, Color rgb)
        {
            Color spriteColor = new Color((int)rgb.R, (int)rgb.G, (int)rgb.B, 10);
            spriteBatch.Draw(spriteTexture, destinationRectangle, new Rectangle(0, 0, 1, 1), spriteColor);
        }

        #endregion
    }
}