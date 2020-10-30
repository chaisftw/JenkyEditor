using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace JenkyEditor
{
    public class Map
    {
        public List<MapLayer> layers;

        public Map()
        {
            layers = new List<MapLayer>();
        }

        public void Reset()
        {
            layers.Clear();
        }

        public void RemoveLayer(int depth)
        {
            layers.RemoveAt(depth);
        }

        public void AddLayer(string name)
        {
            layers.Add(new MapLayer(name));
        }

        public void RaiseLayer(int depth)
        {
            PrintLayers();
            if (depth < layers.Count - 1)
            {
                var temp = layers[depth + 1];
                layers[depth + 1] = layers[depth];
                layers[depth] = temp;
            }
            PrintLayers();
        }

        public void LowerLayer(int depth)
        {
            PrintLayers();

            if (depth > 0)
            {
                var temp = layers[depth - 1];
                layers[depth - 1] = layers[depth];
                layers[depth] = temp;
            }

            PrintLayers();
        }

        public void HideLayer(int depth)
        {
            layers[depth].Hide();
        }

        public void ShowLayer(int depth)
        {
            layers[depth].Show();
        }

        private void PrintLayers()
        {
            Console.WriteLine("Printing");
            for (int i = 0; i < layers.Count; i++)
            {
                Console.WriteLine("Depth: " + i + "- Name: " + layers[i].Name);
            }
        }

        public void AddTile(int depth, MapObject newTile)
        {
            layers[depth].AddTile(newTile);
        }

        public void AddProp(int depth, MapObject newProp)
        {
            layers[depth].AddProp(newProp);
        }

        public void AddMarker(int depth, MapObject newCollider)
        {
            layers[depth].AddCollider(newCollider);
        }

        public void RemoveTile(int depth, Vector2 mapPosition)
        {
            layers[depth].RemoveTile(mapPosition);
        }

        public void RemoveProp(int depth, Vector2 mapPosition)
        {
            layers[depth].RemoveProp(mapPosition);
        }

        public void RemoveMarker(int depth, Vector2 mapPosition)
        {
            layers[depth].RemoveCollider(mapPosition);
        }

        public void RemoveTilesByID(int id)
        {
            for (int i = 0; i < layers.Count; i++)
            {
                layers[i].RemoveTilesByID(id);
            }
        }

        public void RemovePropsByID(int id)
        {
            for (int i = 0; i < layers.Count; i++)
            {
                layers[i].RemovePropsByID(id);
            }
        }

        public void RemoveMarkersByID(int id)
        {
            for (int i = 0; i < layers.Count; i++)
            {
                layers[i].RemoveCollidersByID(id);
            }
        }

        public void CheckTileDiscard(int id)
        {
            for (int i = 0; i < layers.Count; i++)
            {
                layers[i].CheckTileDiscard(id);
            }
        }

        public void CheckPropDiscard(int id, int mapScale, PropSource newProp, Func<Vector2, Vector2> GetPosition)
        {
            for (int i = 0; i < layers.Count; i++)
            {
                layers[i].CheckPropDiscard(id, mapScale, newProp, GetPosition);
            }
        }

        public void CheckMarkerDiscard(int id)
        {
            for (int i = 0; i < layers.Count; i++)
            {
                layers[i].CheckColliderDiscard(id);
            }
        }

        public Rectangle GetMapBounds()
        {
            int minX = int.MaxValue;
            int minY = int.MaxValue;
            int maxX = int.MinValue;
            int maxY = int.MinValue;

            int mapWidth = 0;
            int mapHeight = 0;

            //Find dimensions of map from maptile list
            for (int i = 0; i < layers.Count; i++)
            {
                var layerBounds = layers[i].GetLayerBounds();

                if (layerBounds.X < minX)
                {
                    minX = layerBounds.X;
                }

                if (layerBounds.X + layerBounds.Width > maxX)
                {
                    maxX = layerBounds.X + layerBounds.Width;
                }

                if (layerBounds.Y < minY)
                {
                    minY = layerBounds.Y;
                }

                if (layerBounds.Y + layerBounds.Height > maxY)
                {
                    maxY = layerBounds.Y + layerBounds.Height;
                }
            }
            
            mapWidth = Math.Abs(maxX - minX);
            mapHeight = Math.Abs(maxY - minY);

            return new Rectangle(minX, minY, mapWidth, mapHeight);
        }
    }
}
