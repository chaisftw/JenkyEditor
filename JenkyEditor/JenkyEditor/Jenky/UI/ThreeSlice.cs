using Jenky.Graphics;

namespace Jenky.Graphics
{
    public class ThreeSlice
    {
        #region vars

        //Defining a 3x3 set of stillframes
        public StillFrame Start { get; private set; }
        public StillFrame Middle { get; private set; }
        public StillFrame End { get; private set; }

        public int SliceWidth { get; private set; }
        public int SliceHeight { get; private set; }

        public bool isHorizontal;

        #endregion 

        #region init

        public ThreeSlice(int originX, int originY, int _sliceWidth, int _sliceHeight, bool _isHorizontal = true)
        {
            SliceWidth = _sliceWidth;
            SliceHeight = _sliceHeight;
            isHorizontal = _isHorizontal;

            // Put threeslice parts into frames
            if (isHorizontal)
            {
                Start = new StillFrame(originX, originY, SliceWidth, SliceHeight);
                Middle = new StillFrame(originX + SliceWidth, originY, SliceWidth, SliceHeight);
                End = new StillFrame(originX + (SliceWidth * 2), originY, SliceWidth, SliceHeight);
            }
            else
            {
                Start = new StillFrame(originX, originY, SliceWidth, SliceHeight);
                Middle = new StillFrame(originX, originY + SliceHeight, SliceWidth, SliceHeight);
                End = new StillFrame(originX, originY + (SliceHeight * 2), SliceWidth, SliceHeight);
            }

        }

        #endregion 

        #region methods

        //Stretch the threeslice to specified lengths
        public void Stretch(int width, int height)
        {
            if (isHorizontal)
            {
                Middle.Resize(width - (SliceWidth * 2), SliceHeight);
            }
            else
            {
                Middle.Resize(SliceWidth, height - (SliceHeight * 2));
            }
        }

        #endregion 
    }
}
