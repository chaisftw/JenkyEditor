using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Jenky.IO;

namespace Jenky.UI
{
    public class Grid : UIScrollableElement
    {
        #region vars

        private int wrapWidth;//Maximum x position for cell before wrapping
        private int wrapHeight;//Maximum y position for cell before wrapping

        private int cellWidth;
        private int cellHeight;

        //Line positions
        private List<int> horizontals;
        private List<int> verticals;

        private Texture2D lineTexture;
        private Color lineColor;

        #endregion 

        #region init

        public Grid(int positionX, int positionY, int _width, int _height, int _scale, int _cellWidth, int _cellHeight, Texture2D _lineTexture, Color _lineColor, InputHandler _input) : base(positionX, positionY, _width, _height, _scale, _input)
        {
            lineColor = _lineColor;
            lineTexture = _lineTexture;

            SetLines(_cellWidth, _cellHeight);
        }

        #endregion 

        #region methods

        public void SetLines(int _cellWidth, int _cellheight)
        {
            cellWidth = _cellWidth * scale;
            cellHeight = _cellheight * scale;

            horizontals = new List<int>();
            verticals = new List<int>();

            decimal result = width / cellWidth;
            int columns = (int)Math.Ceiling(result);
            result = height / cellHeight;
            int rows = (int)Math.Ceiling(result);
            wrapWidth = cellWidth * (columns + 1);
            wrapHeight = cellHeight * (rows + 1);

            for (int i = 0; i <= rows; i++)
            {
                horizontals.Add((int)position.Y + (i * cellHeight));
            }

            for (int i = 0; i <= columns; i++)
            {
                verticals.Add((int)position.X + (i * cellWidth));
            }
        }

        public void Update(GameTime gameTime)
        {
            if (Scrolling)
            {
                Vector2 moveDistance = GetMoveDistance();

                for (int i = 0; i < horizontals.Count; i++)
                {
                    int newY = horizontals[i] + (int)moveDistance.Y;
                    horizontals[i] = newY;

                    //Wrapping vertically
                    if (horizontals[i] > position.Y + wrapHeight)//If too far down, append line to top of grid
                    {
                        int offset = horizontals[i] - (int)position.Y - wrapHeight;
                        horizontals[i] = (int)position.Y + offset;
                    }
                    else if (horizontals[i] < position.Y)//If too far above, append line to bottom of grid
                    {
                        int offset = horizontals[i] - (int)position.Y;
                        horizontals[i] = (int)position.Y + wrapHeight + offset;
                    }
                }


                for (int i = 0; i < verticals.Count; i++)
                {
                    int newX = verticals[i] + (int)moveDistance.X;
                    verticals[i] = newX;

                    //Wrapping horizontally
                    if (verticals[i] > position.X + wrapWidth)//If too far right, append line to left of grid
                    {
                        int offset = verticals[i] - (int)position.X - wrapWidth;
                        verticals[i] = (int)position.X + offset;
                    }
                    else if (verticals[i] < position.X)//If too far left, append line to right of grid
                    {
                        int offset = verticals[i] - (int)position.X;
                        verticals[i] = (int)position.X + wrapWidth + offset;
                    }
                }
            }

            oldMousePosition = input.MousePosition();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < horizontals.Count; i++)
            {
                if (horizontals[i] <= position.Y + height - 1)
                {
                    spriteBatch.Draw(lineTexture, new Rectangle((int)position.X, horizontals[i], width, 1), lineColor);
                }
            }

            for (int i = 0; i < verticals.Count; i++)
            {
                if (verticals[i] <= position.X + width - 1)
                {
                    spriteBatch.Draw(lineTexture, new Rectangle(verticals[i], (int)position.Y, 1, height), lineColor);
                }
            }

            //drawing bounds
            spriteBatch.Draw(lineTexture, new Rectangle((int)position.X, (int)position.Y, width, 1), lineColor);
            spriteBatch.Draw(lineTexture, new Rectangle((int)position.X, (int)position.Y + height - 1, width, 1), lineColor);
            spriteBatch.Draw(lineTexture, new Rectangle((int)position.X + width - 1, (int)position.Y, 1, height), lineColor);
            spriteBatch.Draw(lineTexture, new Rectangle((int)position.X, (int)position.Y, 1, height), lineColor);
        }

        #endregion
    }
}
