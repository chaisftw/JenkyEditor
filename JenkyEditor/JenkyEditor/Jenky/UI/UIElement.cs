using Jenky.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Jenky.UI
{
    public abstract class UIElement
    {
        #region vars

        public Vector2 position;

        protected int width;
        protected int height;
        protected int physicalWidth;
        protected int physicalHeight;

        protected int scale;

        protected AnimationSet animations;

        #endregion

        #region abstract_methods

        public abstract void Draw(SpriteBatch spriteBatch);

        #endregion

        #region methods

        //NOTE on optional isPreScaled arguement
        /*
        When instantiating an element the dimensions should be given in pixel size, however, for some elements (for example Grid) 
        we aren't working with pixel art and enter a desired resolution without needing any scaling at all. This is when you want 
        to set isPreScale to true to keep the physical size of the elements correct.
        */
        protected UIElement(int positionX, int positionY, int _width, int _height, int _scale, bool isPreScaled = false)
        {
            position = new Vector2(positionX, positionY);

            width = _width;
            height = _height;
            scale = _scale;

            
            if (isPreScaled)
            {
                physicalWidth = width;
                physicalHeight = height;
            }
            else
            {
                physicalWidth = width * scale;
                physicalHeight = height * scale;
            }

            animations = new AnimationSet(positionX, positionY, _scale);
        }

        //Check if a point is within bounds of the Element
        public bool InBounds(Vector2 point)
        {
            if (point.X >= position.X && point.X <= (position.X + physicalWidth)) // Within X
            {
                if (point.Y >= position.Y && point.Y <= (position.Y + physicalHeight)) // Within Y
                {
                    return true;
                }
            }
            return false;
        }

        //Build the element from nineslice pieces
        protected void BuildNineSliceElement(int columns, int rows, NineSlice nineSlice)
        {
            for (int i = 1; i <= rows; i++)
            {
                for (int j = 1; j <= columns; j++)
                {
                    if (i == 1)
                    {
                        if (j == 1)
                        {
                            animations.AddSetAnimation(nineSlice.TopLeft, Vector2.Zero);
                        }
                        else if (j == columns)
                        {
                            animations.AddSetAnimation(nineSlice.TopRight, new Vector2(nineSlice.SliceWidth * (j - 1), 0));
                        }
                        else
                        {
                            animations.AddSetAnimation(nineSlice.TopMiddle, new Vector2(nineSlice.SliceWidth * (j - 1), 0));
                        }
                    }
                    else if (i == rows)
                    {
                        if (j == 1)
                        {
                            animations.AddSetAnimation(nineSlice.BottomLeft, new Vector2(0, nineSlice.SliceHeight * (i - 1)));
                        }
                        else if (j == columns)
                        {
                            animations.AddSetAnimation(nineSlice.BottomRight, new Vector2(nineSlice.SliceWidth * (j - 1), nineSlice.SliceHeight * (i - 1)));
                        }
                        else
                        {
                            animations.AddSetAnimation(nineSlice.BottomMiddle, new Vector2(nineSlice.SliceWidth * (j - 1), nineSlice.SliceHeight * (i - 1)));
                        }
                    }
                    else
                    {
                        if (j == 1)
                        {
                            animations.AddSetAnimation(nineSlice.MiddleLeft, new Vector2(0, nineSlice.SliceHeight * (i - 1)));
                        }
                        else if (j == columns)
                        {
                            animations.AddSetAnimation(nineSlice.MiddleRight, new Vector2(nineSlice.SliceWidth * (j - 1), nineSlice.SliceHeight * (i - 1)));
                        }
                        else
                        {
                            animations.AddSetAnimation(nineSlice.Middle, new Vector2(nineSlice.SliceWidth * (j - 1), nineSlice.SliceHeight * (i - 1)));
                        }
                    }
                }
            }
        }

        protected void StretchNineSlice(NineSlice nineSlice)
        {
            int physicalSliceWidth = nineSlice.SliceWidth * scale;
            int physicalSliceHeight = nineSlice.SliceHeight * scale;

            //Create a nineslice and stretch for a window of a specified resolution
            nineSlice.Stretch(width, height);

            //Piece together the stillframes in an animation set
            animations.AddSetAnimation(nineSlice.TopLeft, Vector2.Zero);
            animations.AddSetAnimation(nineSlice.TopMiddle, new Vector2(physicalSliceWidth, 0));
            animations.AddSetAnimation(nineSlice.TopRight, new Vector2(physicalWidth - physicalSliceWidth, 0));

            animations.AddSetAnimation(nineSlice.MiddleLeft, new Vector2(0, physicalSliceHeight));
            animations.AddSetAnimation(nineSlice.Middle, new Vector2(physicalSliceWidth, physicalSliceHeight));
            animations.AddSetAnimation(nineSlice.MiddleRight, new Vector2(physicalWidth - physicalSliceWidth, physicalSliceHeight));

            animations.AddSetAnimation(nineSlice.BottomLeft, new Vector2(0, physicalHeight - physicalSliceHeight));
            animations.AddSetAnimation(nineSlice.BottomMiddle, new Vector2(physicalSliceWidth, physicalHeight - physicalSliceHeight));
            animations.AddSetAnimation(nineSlice.BottomRight, new Vector2(physicalWidth - physicalSliceWidth, physicalHeight - physicalSliceHeight));
        }

        protected void StretchThreeSlice(ThreeSlice threeSlice)
        {
            int physicalSliceWidth = threeSlice.SliceWidth * scale;
            int physicalSliceHeight = threeSlice.SliceHeight * scale;

            //Create a threeslice and stretch for an element of a specified resolution
            threeSlice.Stretch(width, height);

            if (threeSlice.isHorizontal)
            {
                //Piece together the stillframes horizontally in an animation set
                animations.AddSetAnimation(threeSlice.Start, Vector2.Zero);
                animations.AddSetAnimation(threeSlice.Middle, new Vector2(physicalSliceWidth, 0));
                animations.AddSetAnimation(threeSlice.End, new Vector2(physicalWidth - physicalSliceWidth, 0));
            }
            else
            {
                //Piece together the stillframes vertically in an animation set
                animations.AddSetAnimation(threeSlice.Start, Vector2.Zero);
                animations.AddSetAnimation(threeSlice.Middle, new Vector2(0, physicalSliceHeight));
                animations.AddSetAnimation(threeSlice.End, new Vector2(0, physicalHeight - physicalSliceHeight));
            }

        }

        protected void ClearAnimation()
        {
            animations.ClearAnimationSet();
        }

        public int GetWidth()
        {
            return width;
        }

        public int GetHeight()
        {
            return height;
        }

        public int GetPhysicalWidth()
        {
            return physicalWidth;
        }

        public int GetPhysicalHeight()
        {
            return physicalHeight;
        }

        #endregion
    }
}
