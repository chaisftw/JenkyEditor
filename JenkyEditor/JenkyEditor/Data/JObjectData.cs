using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace JenkyEditor
{
    public class JObjectData
    {
        public int TileWidth { get; set; }
        public int TileHeight { get; set; }

        public List<JTile> Tiles { get; set; }
        public List<JProp> Props { get; set; }
        public List<JMarker> Markers { get; set; }

        public JObjectData(int _TileWidth, int _TileHeight, List<JTile> _Tiles, List<JProp> _Props, List<JMarker> _Colliders)
        {
            TileWidth = _TileWidth;
            TileHeight = _TileHeight;
            Tiles = _Tiles;
            Props = _Props;
            Markers = _Colliders;
        }
    }

    public class JTile
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public int SourceX { get; set; }
        public int SourceY { get; set; }

        public JTile(string _Name, int _SourceX, int _SourceY, int _ID)
        {
            Name = _Name;
            SourceX = _SourceX;
            SourceY = _SourceY;
            ID = _ID;
        }
    }

    public class JProp
    {
        public string Name { get; set; }
        public int ID { get; set; }
        public int SourceX { get; set; }
        public int SourceY { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int OffsetX { get; set; }
        public int OffsetY { get; set; }
        
        public JProp(string _Name, int _SourceX, int _SourceY, int _Width, int _Height, int _OffsetX, int _OffsetY, int _ID)
        {
            Name = _Name;
            SourceX = _SourceX;
            SourceY = _SourceY;
            Width = _Width;
            Height = _Height;
            OffsetX = _OffsetX;
            OffsetY = _OffsetY;
            ID = _ID;
        }
    }

    public class JMarker
    {
        public string Name { get; set; }
        public int ID;
        public int[] RGB;

        public JMarker(string _Name, Color markerColor, int _ID)
        {
            Name = _Name;
            RGB = new int[3]{ markerColor.R, markerColor.G, markerColor.B };
            ID = _ID;
        }
    }
}
