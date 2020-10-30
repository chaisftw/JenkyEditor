using Jenky.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenkyEditor
{
    public class TileSource
    {
        private StillFrame frame;
        public string Name { get; set; }

        public TileSource(string _Name, int sourceX, int sourceY, int tileWidth, int tileHeight)
        {
            frame = new StillFrame(sourceX, sourceY, tileWidth, tileHeight);
            Name = _Name;
        }

        public StillFrame GetFrame()
        {
            return frame;
        }
    }
}
