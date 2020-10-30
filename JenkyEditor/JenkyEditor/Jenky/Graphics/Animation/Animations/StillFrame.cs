using Microsoft.Xna.Framework;

namespace Jenky.Graphics
{
    //Static frame animation
    public class StillFrame : Animation
    {
        #region init

        public StillFrame(int originX, int originY, int frameWidth, int frameHeight)
        {
            // Get the correct frame in the image strip by multiplying the currentFrame index by the frame width
            SourceRectangle = new Rectangle(originX, originY, frameWidth, frameHeight);
            DestinationRectangle = new Rectangle(0, 0, frameWidth, frameHeight);

            Finished = false;
        }

        #endregion

        #region methods

        public void Resize(int width, int height)
        {
            DestinationRectangle = new Rectangle(0, 0, width, height);
        }

        public void SetOffset(int offsetX, int offsetY)
        {
            DestinationRectangle = new Rectangle(offsetX, offsetY, DestinationRectangle.Width, DestinationRectangle.Height);
        }
        
        public override void Update(GameTime gameTime)
        {

        }

        #endregion
    }
}
