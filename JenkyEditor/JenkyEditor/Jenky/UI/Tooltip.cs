using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Jenky.IO;

namespace Jenky.UI
{
    public class Tooltip
    {
        private Vector2 labelPosition;

        private Texture2D lineTexture;
        private SpriteFont font;

        private Color backgroundColor;
        private Color lineColor;

        private string text;
        private int scale;

        public Rectangle Bounds { get; private set; }
        
        public Tooltip(int positionX, int positionY, int physicalWidth, int physicalHeight, string _text, int _scale, Texture2D _lineTexture, SpriteFont _font, Color _backgroundColor, Color _lineColor)
        {
            text = _text;

            lineTexture = _lineTexture;
            font = _font;

            scale = _scale;

            backgroundColor = _backgroundColor;
            lineColor = _lineColor;

            Vector2 labelBounds = font.MeasureString(text) * scale;
            labelPosition = new Vector2(positionX + (physicalWidth / 2) - ((int)labelBounds.X / 2), positionY - (int)labelBounds.Y - (scale * 3));
            
            int tooltipX = (int)labelPosition.X - (scale * 2);
            int tooltipY = (int)labelPosition.Y - scale;
            int tooltipWidth = (int)labelBounds.X + (scale * 3);
            int tooltipHeight = (int)labelBounds.Y + scale;

            Bounds = new Rectangle(tooltipX, tooltipY, tooltipWidth, tooltipHeight);
            ClampToScreen();
        }

        public void RePosition(int positionX, int positionY, int physicalWidth, int physicalHeight, SpriteFont font)
        {
            Vector2 labelBounds = font.MeasureString(text) * scale;
            labelPosition = new Vector2(positionX + (physicalWidth / 2) - ((int)labelBounds.X / 2), positionY - (int)labelBounds.Y - (scale * 3));

            int tooltipX = (int)labelPosition.X - (scale * 2);
            int tooltipY = (int)labelPosition.Y - scale;
            int tooltipWidth = (int)labelBounds.X + (scale * 3);
            int tooltipHeight = (int)labelBounds.Y + scale;

            Bounds = new Rectangle(tooltipX, tooltipY, tooltipWidth, tooltipHeight);
            ClampToScreen();
        }

        public void ClampToScreen()
        {
            if(Bounds.X < 0)
            {
                int moveDistance = Bounds.X * -1;
                Bounds = new Rectangle(0, Bounds.Y, Bounds.Width, Bounds.Height);
                labelPosition.X += moveDistance;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(lineTexture, Bounds, backgroundColor);

            spriteBatch.Draw(lineTexture, new Rectangle(Bounds.X, Bounds.Y, Bounds.Width, 1), lineColor);
            spriteBatch.Draw(lineTexture, new Rectangle(Bounds.X, Bounds.Y + Bounds.Height - 1, Bounds.Width, 1), lineColor);
            spriteBatch.Draw(lineTexture, new Rectangle(Bounds.X, Bounds.Y, 1, Bounds.Height), lineColor);
            spriteBatch.Draw(lineTexture, new Rectangle(Bounds.X + Bounds.Width - 1, Bounds.Y, 1, Bounds.Height), lineColor);

            spriteBatch.DrawString(font, text, labelPosition, lineColor, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }
    }
}
