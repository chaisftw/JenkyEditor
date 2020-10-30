using Jenky.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenkyEditor
{
    public class PropSource
    {
        private StillFrame frame;
        public string Name { get; private set; }
        public int OffsetX { get; private set; }
        public int OffsetY { get; private set; }

        public PropSource(string _Name, int sourceX, int sourceY, int objectWidth, int objectHeight, int _OffsetX, int _OffsetY)
        {
            frame = new StillFrame(sourceX, sourceY, objectWidth, objectHeight);
            Name = _Name;
            OffsetY = _OffsetY;
            OffsetX = _OffsetX;
        }

        public StillFrame GetFrame()
        {
            return frame;
        }
    }
}
