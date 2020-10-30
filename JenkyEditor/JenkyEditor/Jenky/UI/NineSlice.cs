using Jenky.Graphics;

namespace Jenky.Graphics
{
    public class NineSlice
    {
        #region vars

        //Defining a 3x3 set of stillframes
        public StillFrame TopLeft { get; set; }
        public StillFrame TopMiddle { get; set; }
        public StillFrame TopRight { get; set; }
        public StillFrame MiddleLeft { get; set; }
        public StillFrame Middle { get; set; }
        public StillFrame MiddleRight { get; set; }
        public StillFrame BottomLeft { get; set; }
        public StillFrame BottomMiddle { get; set; }
        public StillFrame BottomRight { get; set; }

        public int SliceWidth { get; private set; }
        public int SliceHeight { get; private set; }

        #endregion

        #region init

        public NineSlice(int originX, int originY, int _sliceWidth, int _sliceHeight)
        {
            SliceWidth = _sliceWidth;
            SliceHeight = _sliceHeight;

            // Put nineslice parts into frames
            TopLeft = new StillFrame(originX, originY, SliceWidth, SliceHeight);
            TopMiddle = new StillFrame(originX + SliceWidth, originY, SliceWidth, SliceHeight);
            TopRight = new StillFrame(originX + (SliceWidth * 2), originY, SliceWidth, SliceHeight);
            MiddleLeft = new StillFrame(originX, originY + SliceHeight, SliceWidth, SliceHeight);
            Middle = new StillFrame(originX + SliceWidth, originY + SliceHeight, SliceWidth, SliceHeight);
            MiddleRight = new StillFrame(originX + (SliceWidth * 2), originY + SliceHeight, SliceWidth, SliceHeight);
            BottomLeft = new StillFrame(originX, originY + (SliceHeight * 2), SliceWidth, SliceHeight);
            BottomMiddle = new StillFrame(originX + SliceWidth, originY + (SliceHeight * 2), SliceWidth, SliceHeight);
            BottomRight = new StillFrame(originX + (SliceWidth * 2), originY + (SliceHeight * 2), SliceWidth, SliceHeight);
        }

        #endregion

        #region methods

        //Stretch the nineslice to specified lengths
        public void Stretch(int width, int height)
        {
            TopMiddle.Resize((width - (SliceWidth * 2)), SliceHeight);
            BottomMiddle.Resize(width - (SliceWidth * 2), SliceHeight);
            MiddleLeft.Resize(SliceWidth, height - (SliceHeight * 2));
            MiddleRight.Resize(SliceWidth, height - (SliceHeight * 2));
            Middle.Resize(width - (SliceWidth * 2), height - (SliceHeight * 2));
        }

        #endregion
    }
}
