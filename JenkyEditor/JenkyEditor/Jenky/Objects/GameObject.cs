using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Jenky.Objects
{
    public abstract class GameObject
    {
        #region vars

        public bool Active { get; protected set; } //Object still exists

        public int Width { get; protected set; } //Pixel dimensions of the object
        public int Height { get; protected set; }
        public int PhysicalWidth { get; protected set; } //Physical dimensions take scale into account
        public int PhysicalHeight { get; protected set; } 

        protected Vector2 position;

        #endregion

        #region init

        public GameObject(float positionX, float positionY, int _Width, int _Height, int scale)
        {
            position = new Vector2(positionX, positionY);
            SetDimensions(_Width, _Height, scale);
            Active = true;
        }

        #endregion

        #region abstract_methods

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch, Texture2D spriteTexture);

        #endregion

        #region methods

        public Vector2 GetPosition() { return position; }

        public Vector2 GetCenterPosition()
        {
            return new Vector2(position.X + (PhysicalWidth / 2), position.Y + (PhysicalHeight / 2));
        }
        
        protected void SetDimensions(int _Width, int _Height, int scale)
        {
            Width = _Width;
            Height = _Height;
            
            PhysicalWidth = _Width * scale;
            PhysicalHeight = _Height * scale;
        }

        #endregion
    }
}
