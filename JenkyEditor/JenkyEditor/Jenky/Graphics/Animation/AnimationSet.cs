using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jenky.Graphics
{
    public class AnimationSet
    {
        #region vars

        public Vector2 position;
        private int scale;
        private Color color; 
       
        public List<SingleSetAnimation> animations;

        #endregion

        #region init

        public AnimationSet(int positionX, int positionY, int _scale)
        {
            // Insert new values
            
            scale = _scale;
            position = new Vector2(positionX, positionY);
            color = Color.White;

            animations = new List<SingleSetAnimation>();
        }

        #endregion

        #region methods

        // Set draw color
        public void SetColor(Color _color)
        {
            color = _color;
        }

        //Reposition the animation set
        public void RePosition(Vector2 _position)
        {
            position = _position;
        }

        //Appends a SingleSetAnimation to the animation set
        public void AddSetAnimation(Animation animation, Vector2 position)
        {
            SingleSetAnimation setAnimation = new SingleSetAnimation(animation, position);
            animations.Add(setAnimation);
        }

        //Appends a SingleSetAnimation to the animation set
        public void ClearAnimationSet()
        {
            animations.Clear();
        }

        //Updates the list of animations
        public void Update(GameTime gameTime)
        {
            for(int i = 0; i < animations.Count; i++)
            {
                animations[i].Update(gameTime);
            }
        }

        //Draw the set
        public void Draw(SpriteBatch spriteBatch, Texture2D spriteTexture)
        {
            // For each animation, draw in its position relative to the whole set's position
            for (int i = 0; i < animations.Count; i++)
            {
                Rectangle destination = new Rectangle((int)position.X + (int)animations[i].setPosition.X + animations[i].setAnimation.DestinationRectangle.Left, (int)position.Y + (int)animations[i].setPosition.Y + animations[i].setAnimation.DestinationRectangle.Top, (int)(animations[i].setAnimation.DestinationRectangle.Width * scale), (int)(animations[i].setAnimation.DestinationRectangle.Height * scale));
                spriteBatch.Draw(spriteTexture, destination, animations[i].setAnimation.SourceRectangle, color);
            }
        }

        #endregion
    }

    //Class to hold an animation and position variable
    public class SingleSetAnimation
    {
        #region vars

        public Vector2 setPosition; //The animations position within the set
        public Animation setAnimation;

        #endregion

        #region init

        public SingleSetAnimation(Animation _setAnimation, Vector2 _setPosition)
        {
            setPosition = _setPosition;
            setAnimation = _setAnimation;
        }

        #endregion

        #region methods

        public void Update(GameTime gameTime)
        {
            setAnimation.Update(gameTime);
        }

        #endregion
    }
}
