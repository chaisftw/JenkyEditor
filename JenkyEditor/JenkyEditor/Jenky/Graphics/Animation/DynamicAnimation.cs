using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Jenky.Content;

namespace Jenky.Graphics
{
    public class DynamicAnimation
    {
        #region vars

        public Vector2 position;

        private float rotation;
        private Vector2 rotationOrigin;

        private float scale;

        private SpriteEffects spriteEffect;

        private Color color;

        private Rectangle sourceRect = new Rectangle(); // Area of the sprite sheet
        private Rectangle destinationRect = new Rectangle(); // Rectangle representing the displayed image (Where are we placing the image?)

        public AnimationQueue animationQueue;

        #endregion

        #region init

        public DynamicAnimation(float _scale)
        {
            // Insert new values

            color = Color.White;

            animationQueue = new AnimationQueue();
            scale = _scale;
            position = Vector2.Zero;
            rotationOrigin = Vector2.Zero;
            rotation = 0;
        }

        #endregion

        #region methods

        public void FlipHorizontal(bool flipped)
        {
            if (flipped)
            {
                spriteEffect = SpriteEffects.FlipHorizontally;
            }
            else
            {
                spriteEffect = SpriteEffects.None;
            }
        }

        public void FlipVertical(bool flipped)
        {
            if (flipped)
            {
                spriteEffect = SpriteEffects.FlipVertically;
            }
            else
            {
                spriteEffect = SpriteEffects.None;
            }
        }

        //Set rotation
        public void SetRotation(float _rotation)
        {
            rotation = _rotation;
        }
        
        //Set rotation origin
        public void SetRotationOrigin(Vector2 _rotationOrigin)
        {
            rotationOrigin = _rotationOrigin;
        }

        //Set draw color
        public void SetColor(Color _color)
        {
            color = _color;
        }

        //Run animation immediately
        public void RunAnimation(Animation animation)
        {
            animationQueue.Run(animation);
        }

        //Add animation to play after current animations
        public void AddAnimation(Animation animation)
        {
            animationQueue.Add(animation);
        }

        //Check if animation is idle
        public bool IsIdle()
        {
            return animationQueue.IsIdle();
        }

        public void Update(GameTime gameTime)
        {
            // Animation rectangles
            animationQueue.Update(gameTime);

            if (animationQueue.NotEmpty())
            {
                Rectangle animationRectangle = animationQueue.GetDestination();

                // Get the correct frame in the image strip by multiplying the currentFrame index by the frame width
                sourceRect = animationQueue.GetSource();

                // Get the our new destination frame
                destinationRect = new Rectangle((int)position.X + animationRectangle.Left, (int)position.Y + animationRectangle.Top, (int)(animationRectangle.Width * scale), (int)(animationRectangle.Height * scale));
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D spriteTexture)
        {
            // Only draw the animation when we are active
            if (animationQueue.NotEmpty())
            {
                spriteBatch.Draw(spriteTexture, destinationRect, sourceRect, color, rotation, rotationOrigin, spriteEffect, 0);
            }
        }

        #endregion
    }

    //Animation queue to play animations one after the other
    public class AnimationQueue
    {
        #region vars

        private readonly List<Animation> animationQueue = new List<Animation>();

        #endregion

        #region methods

        //Add an animation to the queue
        public void Add(Animation _animation)
        {
            animationQueue.Add(_animation);
        }

        //Interrupt the animations to run a new animation
        public void Run(Animation _animation)
        {
            
            animationQueue.Clear();
            animationQueue.Add(_animation);
        }

        //Get current animation's source rectangle
        public Rectangle GetSource()
        {
            return animationQueue[0].SourceRectangle;
        }

        //Get current animation's destination rectangle
        public Rectangle GetDestination()
        {
            return animationQueue[0].DestinationRectangle;
        }

        //Determine if the animation is currently idling
        public bool IsIdle()
        {
            return animationQueue.Count == 0 || animationQueue[0] is StillFrame;
        }

        //Return true if animation queue is empty
        public bool NotEmpty()
        {
            return animationQueue.Count > 0;
        }

        public void Update(GameTime gameTime)
        {
            //Update if not empty
            if (NotEmpty())
            {
                animationQueue[0].Update(gameTime);

                //Move to next animation if current animation is finished
                if (animationQueue[0].Finished)
                {
                    animationQueue.RemoveAt(0);

                    //Update animation if an animation exists
                    if (animationQueue.Count > 0)
                    {
                        animationQueue[0].Update(gameTime);
                    }
                }
            }
        }

        #endregion
    }
}