using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenkyEditor
{
    public class JLayer
    {
        public string Name;
        public int?[,] Tiles { get; set; }
        public int?[,] Props { get; set; }
        public int?[,] Markers { get; set; }

        public JLayer()
        {

        }

        public JLayer(string _Name, MapLayer layer, Rectangle mapBounds)
        {
            Name = _Name;
            Tiles = GetMapArray(layer.GetTiles(), mapBounds);
            Props = GetMapArray(layer.GetProps(), mapBounds);
            Markers = GetMapArray(layer.GetMarkers(), mapBounds);
        }

        public int?[,] GetMapArray(List<MapObject> mapObjects, Rectangle mapBounds)
        {
            if (mapObjects.Count > 0)
            {
                int offsetX = mapBounds.X * -1;
                int offsetY = mapBounds.Y * -1;

                //Nullable 2D array
                int?[,] map = new int?[mapBounds.Width, mapBounds.Height];

                //Append existing tiles
                for (int i = 0; i < mapObjects.Count; i++)
                {
                    if (mapObjects[i].ID != -1)
                    {
                        int indexX = (int)mapObjects[i].mapPosition.X + offsetX;
                        int indexY = (int)mapObjects[i].mapPosition.Y + offsetY;

                        map[indexX, indexY] = mapObjects[i].ID;
                    }
                }

                //Nested loop to iterate through the 2D int array and fill empty tiles with index -1
                for (int x = 0; x < map.GetLength(0); x++)
                {
                    for (int y = 0; y < map.GetLength(1); y++)
                    {
                        if (!map[x, y].HasValue)
                        {
                            map[x, y] = -1;
                        }
                    }
                }

                return map;
            }
            else
            {
                return null;
            }
        }
    }
}
