using System;

using Jenky.Graphics;
using Jenky.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jenky.UI
{
    public class ImageScroller : UIScrollableElement
    {
        #region vars

        private Vector2 offset;
        private Vector2 imagePosition;

        public int DrawWidth { get; private set; }
        public int DrawHeight { get; private set; }

        private Texture2D imageTexture;
        private Texture2D lineTexture;

        private Color lineColor;

        private int physicalImageWidth;
        private int physicalImageHeight;

        private int pixelWidth;
        private int pixelHeight;
        
        private bool overflowX;
        private bool overflowY;

        #endregion

        #region init

        public ImageScroller(int positionX, int positionY, int _width, int _height, int _scale, Texture2D _imageTexture, Texture2D _lineTexture, Color _lineColor, InputHandler _input) : base(positionX, positionY, _width, _height, _scale, _input)
        {
            offset = Vector2.Zero;

            pixelWidth = ((int)Math.Floor((decimal)(width / scale)));
            pixelHeight = ((int)Math.Floor((decimal)(height / scale)));

            imageTexture = _imageTexture;
            lineTexture = _lineTexture;

            lineColor = _lineColor;

            FrameImage(_imageTexture);
        }

        #endregion

        #region methods

        //Resets image dimensions and orientation
        public void FrameImage(Texture2D newImage)
        {
            imagePosition = Vector2.Zero;

            imageTexture = newImage;

            physicalWidth = pixelWidth * scale;
            physicalHeight = pixelHeight * scale;

            physicalImageWidth = imageTexture.Width * scale;
            physicalImageHeight = imageTexture.Height * scale;

            offset = Vector2.Zero;

            //Check if image overflows bounds
            if (physicalImageWidth > physicalWidth)
            {
                overflowX = true;
                DrawWidth = pixelWidth;
                
                if (width > physicalWidth)
                {
                   offset.X += (width - physicalWidth) / 2;
                }
            }
            else
            {
                physicalWidth = physicalImageWidth;
                overflowX = false;
                DrawWidth = imageTexture.Width;
                offset.X = 0;
            }

            if (physicalImageHeight > physicalHeight)
            {
                overflowY = true;
                DrawHeight = pixelHeight;
                
                if (height > physicalHeight)
                {
                    offset.Y += (height - physicalHeight) / 2;
                }
                
            }
            else
            {
                physicalHeight = physicalImageHeight;
                overflowY = false;
                DrawHeight = imageTexture.Height;
                offset.Y = 0;
            }

            animations.ClearAnimationSet();

            StillFrame visibleFrame = new StillFrame(0, 0, DrawWidth, DrawHeight);

            animations.AddSetAnimation(visibleFrame, offset);
        }

        public Vector2 GetPosition()
        {
            return position + offset;
        }

        public Vector2 GetPixelOffset()
        {
            int offsetX = 0;
            int offsetY = 0;
            if (overflowX)
            {
                offsetX = (int)Math.Floor((decimal)(imagePosition.X / scale));
            }

            if (overflowY)
            {
                offsetY = (int)Math.Floor((decimal)(imagePosition.Y / scale));
            }

            return new Vector2(offsetX, offsetY);
        }

        public void Update(GameTime gameTime)
        {
            if (Scrolling)
            {
                //Calculate mouse drag distance
                Vector2 moveDistance = GetMoveDistance();
                
                imagePosition += moveDistance;

                //Keep position in bounds
                imagePosition.X = Math.Min(imagePosition.X, 0);
                imagePosition.X = Math.Max(imagePosition.X, -(physicalImageWidth - physicalWidth));
                imagePosition.Y = Math.Min(imagePosition.Y, 0);
                imagePosition.Y = Math.Max(imagePosition.Y, -(physicalImageHeight - physicalHeight));

                animations.ClearAnimationSet();

                var pixelOffset = GetPixelOffset();
                StillFrame visibleFrame = new StillFrame(-(int)pixelOffset.X, -(int)pixelOffset.Y, DrawWidth, DrawHeight);

                animations.AddSetAnimation(visibleFrame, offset);
            }

            oldMousePosition = input.MousePosition();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animations.Draw(spriteBatch, imageTexture);

            //drawing bounds
            spriteBatch.Draw(lineTexture, new Rectangle((int)GetPosition().X - 1, (int)GetPosition().Y - 1, physicalWidth + 2, 1), lineColor);
            spriteBatch.Draw(lineTexture, new Rectangle((int)GetPosition().X - 1, (int)GetPosition().Y + physicalHeight, physicalWidth + 2, 1), lineColor);
            spriteBatch.Draw(lineTexture, new Rectangle((int)GetPosition().X + physicalWidth, (int)GetPosition().Y - 1, 1, physicalHeight + 2), lineColor);
            spriteBatch.Draw(lineTexture, new Rectangle((int)GetPosition().X - 1, (int)GetPosition().Y - 1, 1, physicalHeight + 2), lineColor);
        }

        #endregion
    }
}
