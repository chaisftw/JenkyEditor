using Jenky.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using System.Collections.Generic;
using System;


namespace JenkyEditor
{
    public class Background
    {
        private int sizeX;
        private int sizeY;

        private int rows;
        private int columns;

        private int screenWidth;
        private int screenHeight;

        private List<Vector2> positions;
        private StillFrame tileFrame;

        public Background(int tileWidth, int tileHeight, int _screenWidth, int _screenHeight, int scale)
        {
            sizeX = tileWidth * scale;
            sizeY = tileHeight * scale;

            screenWidth = _screenWidth;
            screenHeight = _screenHeight;

            rows = (int)(screenHeight / sizeY) + 2;
            columns = (int)(screenWidth / sizeX) + 2;

            tileFrame = new StillFrame(0, 0, tileWidth, tileHeight);

            positions = new List<Vector2>();

            for(int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    positions.Add(new Vector2((j * sizeX),(i * sizeY)));
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                float x = positions[i].X - 2;
                float y = positions[i].Y + 1;

                positions[i] = new Vector2(x, y)
;
                if(x < -sizeX)
                {
                    positions[i] = new Vector2(x + (sizeX * columns), y);
                }

                if (y > screenHeight)
                {
                    positions[i] = new Vector2(x, y - (sizeY * rows));
                }

            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D backgroundTexture)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                Rectangle destination = new Rectangle((int)positions[i].X, (int)positions[i].Y, sizeX, sizeY);
                spriteBatch.Draw(backgroundTexture, destination, tileFrame.SourceRectangle, Color.White);
            }
        }
    }
}
