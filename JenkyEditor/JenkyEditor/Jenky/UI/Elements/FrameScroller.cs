using System;

using Jenky.Graphics;
using Jenky.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jenky.UI
{
    public class FrameScroller : UIScrollableElement
    {
        #region vars

        private Vector2 offset;
        private Vector2 imagePosition;

        public int DrawWidth { get; private set; }
        public int DrawHeight { get; private set; }

        private Texture2D imageTexture;
        private Texture2D lineTexture;

        private StillFrame frame;

        private Color lineColor;

        private int physicalImageWidth;
        private int physicalImageHeight;

        private int pixelWidth;
        private int pixelHeight;

        #endregion

        #region init

        public FrameScroller(int positionX, int positionY, int _width, int _height, int _scale, Texture2D _imageTexture, Texture2D _lineTexture, Color _lineColor, InputHandler _input) : base(positionX, positionY, _width, _height, _scale, _input)
        {
            offset = Vector2.Zero;

            pixelWidth = ((int)Math.Floor((decimal)(width / scale)));
            pixelHeight = ((int)Math.Floor((decimal)(height / scale)));

            imageTexture = _imageTexture;
            lineTexture = _lineTexture;

            lineColor = _lineColor;

            physicalWidth = pixelWidth * scale;
            physicalHeight = pixelHeight * scale;

            frame = new StillFrame(0, 0, 0, 0);

            imagePosition = Vector2.Zero;

            FrameImage(_imageTexture);
        }

        #endregion

        #region methods

        //Resets image dimensions and orientation
        public void FrameImage(Texture2D newImage)
        {
            imageTexture = newImage;

            physicalImageWidth = frame.SourceRectangle.Width * scale;
            physicalImageHeight = frame.SourceRectangle.Height * scale;

            offset = Vector2.Zero;

            //Check if image overflows bounds
            if (physicalImageWidth > physicalWidth)
            {
                DrawWidth = pixelWidth;
                
                if (width > physicalWidth)
                {
                   offset.X += (width - physicalWidth) / 2;
                }
            }
            else
            {
                DrawWidth = imageTexture.Width;
                offset.X = 0;
            }

            if (physicalImageHeight > physicalHeight)
            {
                DrawHeight = pixelHeight;
                
                if (height > physicalHeight)
                {
                    offset.Y += (height - physicalHeight) / 2;
                }
                
            }
            else
            {
                DrawHeight = imageTexture.Height;
                offset.Y = 0;
            }

            TrimFrame();
        }

        public void SetFrame(StillFrame _frame)
        {
            imagePosition = Vector2.Zero;

            frame = _frame;

            TrimFrame();
        }

        public Vector2 GetPosition()
        {
            return position + offset;
        }

        public Vector2 GetPixelOffset()
        {
            int offsetX = (int)Math.Floor((decimal)(imagePosition.X / scale));
            int offsetY = (int)Math.Floor((decimal)(imagePosition.Y / scale));
            
            return new Vector2(offsetX, offsetY);
        }

        private void TrimFrame()
        {
            int animationX = (int)GetPosition().X;
            int animationY = (int)GetPosition().Y;

            animations.ClearAnimationSet();

            var pixelOffset = GetPixelOffset();
            Console.WriteLine("-----");
            Console.WriteLine(GetPixelOffset());

            Console.WriteLine(imagePosition);
            int sliceX = frame.SourceRectangle.X;
            int sliceY = frame.SourceRectangle.Y;

            int sliceWidth = frame.SourceRectangle.Width;
            int sliceHeight = frame.SourceRectangle.Height;

            int overlapX = sliceWidth + (int)pixelOffset.X;
            int overlapY = sliceHeight + (int)pixelOffset.Y;

            if (pixelOffset.X < 0)
            {
                if (overlapX >= pixelWidth)
                {
                    sliceX += ((int)pixelOffset.X * -1);
                    sliceWidth = pixelWidth;
                }
                else if (overlapX > 0 && overlapX < pixelWidth)
                {
                    sliceX += ((int)pixelOffset.X * -1);
                    sliceWidth = overlapX;
                }
                else
                {
                    sliceWidth = 0;
                }
            }
            else
            {
                if(overlapX >= pixelWidth)
                {
                    sliceWidth = pixelWidth - (int)pixelOffset.X;
                }

                animationX += (int)pixelOffset.X * scale;
            }

            if (pixelOffset.Y < 0)
            {
                if (overlapY >= pixelHeight)
                {
                    sliceY += ((int)pixelOffset.Y * -1);
                    sliceHeight = pixelHeight;
                }
                else if (overlapY > 0 && overlapY < pixelHeight)
                {
                    sliceY += ((int)pixelOffset.Y * -1);
                    sliceHeight = overlapY;
                }
                else
                {
                    sliceHeight = 0;
                }
            }
            else
            {
                if (overlapY >= pixelHeight)
                {
                    sliceHeight = pixelHeight - (int)pixelOffset.Y;
                }

                animationY += (int)pixelOffset.Y * scale;
            }

            if (sliceWidth < 0)
            {
                sliceWidth = 0;
            }

            if (sliceHeight < 0)
            {
                sliceHeight = 0;
            }

            

            StillFrame visibleFrame = new StillFrame(sliceX, sliceY, sliceWidth, sliceHeight);

            animations.AddSetAnimation(visibleFrame, offset);
            animations.position = new Vector2(animationX, animationY);
        }

        public void Reset()
        {
            SetFrame(frame);
        }

        public void Update(GameTime gameTime)
        {
            if (Scrolling)
            {
                //Calculate mouse drag distance
                Vector2 moveDistance = GetMoveDistance();
                
                imagePosition += moveDistance;

                TrimFrame();
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
