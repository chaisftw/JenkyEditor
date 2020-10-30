using Microsoft.Xna.Framework;
using Jenky.Graphics;
using System.Collections.Generic;

namespace Jenky.Graphics
{
    public class FrameSeries : Animation
    {
        #region vars

        public bool Looping { get; set; } //Determines if the animation will cycle
        
        private int currentFrame; //Current column for the frame
        private int elapsedTime; //time elapsed for current frame

        private List<AnimationFrame> frames;
        #endregion

        #region init

        public FrameSeries(bool _looping)
        {
            elapsedTime = 0;
            currentFrame = 0;

            Looping = _looping;
            Finished = false;

            frames = new List<AnimationFrame>();
        }

        #endregion

        #region methods

        public void AddFrame(StillFrame frame, int frameTime)
        {
            frames.Add(new AnimationFrame(frame, frameTime));

            if(frames.Count == 1)
            {
                SetFrame();
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (frames.Count == 0)
            {
                return;
            }

            elapsedTime += (int)gameTime.ElapsedGameTime.TotalMilliseconds; // << Looks at gameTime to determine amount of milliseconds passed since the last update

            //Check if current frame's time is up
            if (elapsedTime > frames[currentFrame].FrameTime)
            {
                currentFrame++;

                //If final frame
                if (currentFrame > frames.Count - 1)
                {
                    //If not looping end animation
                    if (Looping == false)
                    {
                        Finished = true;
                    }
                    else //Switch back to first frame
                    {
                        currentFrame = 0;
                    }
                }

                SetFrame();
            }
        }

        private void SetFrame()
        {
            // Reset the elapsed time to 0
            elapsedTime = 0;

            // Get the correct frame in the frame set
            SourceRectangle = frames[currentFrame].Frame.SourceRectangle;

            // Get the our new destination frame
            DestinationRectangle = frames[currentFrame].Frame.DestinationRectangle;
        }

        #endregion
    }



    public class AnimationFrame
    {
        #region vars

        public StillFrame Frame { get; private set; }
        public int FrameTime { get; private set; }

        #endregion

        #region init

        public AnimationFrame(StillFrame _Frame, int _FrameTime)
        {
            Frame = _Frame;
            FrameTime = _FrameTime;
        }

        #endregion
    }
}


