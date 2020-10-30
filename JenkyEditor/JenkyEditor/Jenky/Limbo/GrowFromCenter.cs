using Microsoft.Xna.Framework;

namespace Jenky.Graphics
{
    public class GrowCard : Animation
    {
        private int width;
        private int frameHeight;
        private int frameWidth;
        private int speed;

        public GrowCard(int row, int column, int _frameWidth, int _frameHeight, int _speed)
        {
            frameHeight = _frameHeight;

            speed = _speed;
            frameWidth = _frameWidth;

            width = 0;

            SourceRectangle = new Rectangle(column * _frameWidth, row * _frameHeight, _frameWidth, _frameHeight);

            Finished = false;
        }

        public override void Update(GameTime gameTime)
        {
            width += speed;
            if (width < frameWidth)
            {
                DestinationRectangle = new Rectangle((frameWidth - width) / 2, 0, width, frameHeight);
            }
            else
            {
                Finished = true;
            }

        }
        
    }
}