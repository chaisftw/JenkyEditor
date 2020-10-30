using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenkyEditor
{
    public class MapLayer
    {
        public string Name { get; private set; }

        public bool Hidden { get; private set; }

        private List<MapObject> mapTiles;
        private List<MapObject> mapProps;
        private List<MapObject> mapMarkers;

        private List<MapObject> discardedMapTiles;
        private List<MapObject> discardedMapProps;
        private List<MapObject> discardedMapMarkers;
        
        public MapLayer(string _name)
        {
            Name = _name;

            Hidden = false;

            mapTiles = new List<MapObject>();
            mapProps = new List<MapObject>();
            mapMarkers = new List<MapObject>();

            discardedMapTiles = new List<MapObject>();
            discardedMapProps = new List<MapObject>();
            discardedMapMarkers = new List<MapObject>();
        }

        public List<MapObject> GetTiles()
        {
            return mapTiles;
        }

        public List<MapObject> GetProps()
        {
            return mapProps;
        }

        public List<MapObject> GetMarkers()
        {
            return mapMarkers;
        }

        public void AddTile(MapObject newTile)
        {
            for (int i = 0; i < mapTiles.Count; i++)
            {
                if (mapTiles[i].mapPosition == newTile.mapPosition)
                {
                    mapTiles.RemoveAt(i);
                }
            }

            for (int i = 0; i < discardedMapTiles.Count; i++)
            {
                if (discardedMapTiles[i].mapPosition == newTile.mapPosition)
                {
                    discardedMapTiles.RemoveAt(i);
                }
            }

            mapTiles.Add(newTile);
        }

        public void AddProp(MapObject newObject)
        {
            for (int i = 0; i < mapProps.Count; i++)
            {
                if (mapProps[i].mapPosition == newObject.mapPosition)
                {
                    mapProps.RemoveAt(i);
                }
            }

            for (int i = 0; i < discardedMapProps.Count; i++)
            {
                if (discardedMapProps[i].mapPosition == newObject.mapPosition)
                {
                    discardedMapProps.RemoveAt(i);
                }
            }

            mapProps.Add(newObject);

            mapProps.Sort((x, y) => x.mapPosition.Y.CompareTo(y.mapPosition.Y));
        }

        public void AddCollider(MapObject newCollider)
        {
            for (int i = 0; i < mapMarkers.Count; i++)
            {
                if (mapMarkers[i].mapPosition == newCollider.mapPosition)
                {
                    mapMarkers.RemoveAt(i);
                }
            }

            for (int i = 0; i < discardedMapMarkers.Count; i++)
            {
                if (discardedMapMarkers[i].mapPosition == newCollider.mapPosition)
                {
                    discardedMapMarkers.RemoveAt(i);
                }
            }

            mapMarkers.Add(newCollider);
        }

        public void RemoveTile(Vector2 mapPosition)
        {
            for (int i = 0; i < mapTiles.Count; i++)
            {
                if (mapTiles[i].mapPosition == mapPosition)
                {
                    mapTiles.RemoveAt(i);
                }
            }
        }

        public void RemoveProp(Vector2 mapPosition)
        {
            for (int i = 0; i < mapProps.Count; i++)
            {
                if (mapProps[i].mapPosition == mapPosition)
                {
                    mapProps.RemoveAt(i);
                }
            }
        }

        public void RemoveCollider(Vector2 mapPosition)
        {
            for (int i = 0; i < mapMarkers.Count; i++)
            {
                if (mapMarkers[i].mapPosition == mapPosition)
                {
                    mapMarkers.RemoveAt(i);
                }
            }
        }

        public void RemoveTilesByID(int id)
        {
            for (int i = 0; i < mapTiles.Count; i++)
            {
                if (mapTiles[i].ID == id)
                {
                    var oldTile = new MapObject(mapTiles[i].mapPosition, mapTiles[i].destinationRectangle, mapTiles[i].ID);
                    discardedMapTiles.Add(oldTile);
                    mapTiles[i].ID = -1;
                }
            }
        }

        public void RemovePropsByID(int id)
        {
            for (int i = 0; i < mapProps.Count; i++)
            {
                if(mapProps[i].ID == id)
                {
                    var oldProp = new MapObject(mapProps[i].mapPosition, mapProps[i].destinationRectangle, mapProps[i].ID);
                    discardedMapProps.Add(oldProp);
                    mapProps[i].ID = -1;
                }
            }
        }

        public void RemoveCollidersByID(int id)
        {
            for (int i = 0; i < mapMarkers.Count; i++)
            {
                if (mapMarkers[i].ID == id)
                {
                    var oldCollider = new MapObject(mapMarkers[i].mapPosition, mapMarkers[i].destinationRectangle, mapMarkers[i].ID);
                    discardedMapMarkers.Add(oldCollider);
                    mapMarkers[i].ID = -1;
                }
            }
        }

        public void CheckTileDiscard(int ID)
        {
            for (int i = discardedMapTiles.Count - 1; i >= 0; i--)
            {
                if (discardedMapTiles[i].ID == ID)
                {
                    for (int j = 0; j < mapTiles.Count; j++)
                    {
                        if (mapTiles[j].mapPosition == discardedMapTiles[i].mapPosition)
                        {
                            mapTiles[j] = discardedMapTiles[i];
                            break;
                        }
                    }
                    discardedMapTiles.RemoveAt(i);
                }
            }
        }

        public void CheckPropDiscard(int id, int mapScale, PropSource newProp, Func<Vector2, Vector2> GetPosition)
        {
            for (int i = discardedMapProps.Count - 1; i >= 0; i--)
            {
                if (discardedMapProps[i].ID == id)
                {
                    for (int j = 0; j < mapProps.Count; j++)
                    {
                        if (mapProps[j].mapPosition == discardedMapProps[i].mapPosition)
                        {
                            Vector2 propPosition = GetPosition(discardedMapProps[i].mapPosition);
                            Rectangle propRectangle = new Rectangle((int)propPosition.X - (newProp.OffsetX * mapScale), (int)propPosition.Y - (newProp.OffsetY * mapScale), newProp.GetFrame().SourceRectangle.Width * mapScale, newProp.GetFrame().SourceRectangle.Height * mapScale);
                            
                            mapProps[j] = new MapObject(discardedMapProps[i].mapPosition, propRectangle, discardedMapProps[i].ID); 
                            break;
                        }
                    }
                    discardedMapProps.RemoveAt(i);
                }
            }
        }

        public void CheckColliderDiscard(int ID)
        {
            for (int i = discardedMapMarkers.Count - 1; i >= 0; i--)
            {
                if (discardedMapMarkers[i].ID == ID)
                {
                    for (int j = 0; j < mapMarkers.Count; j++)
                    {
                        if (mapMarkers[j].mapPosition == discardedMapMarkers[i].mapPosition)
                        {
                            mapMarkers[j] = discardedMapTiles[i];
                            break;
                        }
                    }
                    discardedMapMarkers.RemoveAt(i);
                }
            }
        }

        public void Hide()
        {
            Hidden = true;
        }

        public void Show()
        {
            Hidden = false;
        }

        public Rectangle GetLayerBounds()
        {
            int minX = int.MaxValue;
            int minY = int.MaxValue;
            int maxX = int.MinValue;
            int maxY = int.MinValue;

            int layerWidth = 0;
            int layerHeight = 0;

            //Find dimensions of map from object lists
            for (int i = 0; i < mapTiles.Count; i++)
            {
                if (mapTiles[i].ID != -1)
                {
                    if (mapTiles[i].mapPosition.X < minX)
                    {
                        minX = (int)mapTiles[i].mapPosition.X;
                    }

                    if (mapTiles[i].mapPosition.X > maxX)
                    {
                        maxX = (int)mapTiles[i].mapPosition.X;
                    }

                    if (mapTiles[i].mapPosition.Y < minY)
                    {
                        minY = (int)mapTiles[i].mapPosition.Y;
                    }

                    if (mapTiles[i].mapPosition.Y > maxY)
                    {
                        maxY = (int)mapTiles[i].mapPosition.Y;
                    }
                }
            }

            for (int i = 0; i < mapProps.Count; i++)
            {
                if (mapProps[i].ID != -1)
                {
                    if (mapProps[i].mapPosition.X < minX)
                    {
                        minX = (int)mapProps[i].mapPosition.X;
                    }

                    if (mapProps[i].mapPosition.X > maxX)
                    {
                        maxX = (int)mapProps[i].mapPosition.X;
                    }

                    if (mapProps[i].mapPosition.Y < minY)
                    {
                        minY = (int)mapProps[i].mapPosition.Y;
                    }

                    if (mapProps[i].mapPosition.Y > maxY)
                    {
                        maxY = (int)mapProps[i].mapPosition.Y;
                    }
                }
            }

            for (int i = 0; i < mapMarkers.Count; i++)
            {
                if (mapMarkers[i].ID != -1)
                {
                    if (mapMarkers[i].mapPosition.X < minX)
                    {
                        minX = (int)mapMarkers[i].mapPosition.X;
                    }

                    if (mapMarkers[i].mapPosition.X > maxX)
                    {
                        maxX = (int)mapMarkers[i].mapPosition.X;
                    }

                    if (mapMarkers[i].mapPosition.Y < minY)
                    {
                        minY = (int)mapMarkers[i].mapPosition.Y;
                    }

                    if (mapMarkers[i].mapPosition.Y > maxY)
                    {
                        maxY = (int)mapMarkers[i].mapPosition.Y;
                    }
                }
            }

            //plus 1 due to list indexing
            layerWidth = Math.Abs(maxX - minX) + 1;
            layerHeight = Math.Abs(maxY - minY) + 1;

            return new Rectangle(minX, minY, layerWidth, layerHeight);
        }
    }
}
