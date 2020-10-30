using System;
using System.IO;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Jenky.Objects;
using Jenky.Graphics;
using Jenky.States;
using Jenky.UI;
using Jenky.IO;
using Jenky.Content;

namespace JenkyEditor
{
    public class EditState : GameState
    {
        #region vars
        public bool ValidFiles { get; private set; }
        private bool Scrolling { get; set; }
        private bool gridVisible;
        private bool markerVisible;
        private bool awaitingMouseRelease;
        private string projectPath;
        private string documentsPath;

        private Action LoadMenu;

        private TileRipper tileRipper;
        private PropRipper propRipper;
        private MarkerDialog markerDialog;
        private LayerDialog layerDialog;

        private EditorWindow editorWindow;
        private NewDialog newDialog;
        private SaveAsDialog saveAsDialog;

        DialogManager dialog;

        private IDictionary<int, TileSource> tileSprites;
        private IDictionary<int, PropSource> propSprites;
        private IDictionary<int, MarkerSource> markerSprites;

        private Action[] buttonEvents;

        private Grid grid;

        private Color gridColor;

        private Map map;

        private Texture2D lineTexture;
        private Texture2D tileTexture;
        private Texture2D propTexture;
        private Texture2D uiTexture;
        private StillFrame emptyTile;
        private SpriteFont font;

        private int tileWidth;
        private int tileHeight;

        private int physicalTileWidth;
        private int physicalTileHeight;

        private int windowMargin;
        private int headerHeight;
        private int windowHeight;

        private int mapScale;
        private int uiScale;

        #endregion 

        #region init

        public EditState(GraphicsDevice _graphicsDevice, ContentHandler _contentHandler, Action _LoadMenu, int _tileWidth, int _tileHeight, string _projectPath, string tilePath, string propPath) : base(_graphicsDevice, _contentHandler)
        {
            projectPath = _projectPath;

            tileWidth = _tileWidth;
            tileHeight = _tileHeight;

            File.Copy(tilePath, projectPath + @"\world_tiles.png", true);
            File.Copy(propPath, projectPath + @"\world_props.png", true);

            LoadMenu = _LoadMenu;

            InitiateVariables();

            SaveMap();
        }

        public EditState(GraphicsDevice _graphicsDevice, ContentHandler _contentHandler, Action _LoadMenu, string _projectPath) : base(_graphicsDevice, _contentHandler)
        {
            projectPath = _projectPath;

            var assetPath = projectPath + @"\asset_config.json";
            
            LoadMenu = _LoadMenu;

            if (File.Exists(assetPath))
            {
                GetTileSizeFromFile(assetPath);
            }
            else
            {
                ValidFiles = false;
                return;
            }
            
            InitiateVariables();
            
            LoadProject();
        }

        public void InitiateVariables()
        {
            ValidFiles = true;

            mapScale = 4;
            uiScale = 2;

            gridVisible = true;
            markerVisible = true;
            awaitingMouseRelease = false;

            documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            buttonEvents = new Action[24] { ExitPress, SavePress, SaveAsPress, LoadPress, NewPress, ToggleGrid, ToggleMarkers, NewTilePress, DeleteTilePress, TilePngPress, AddTilePress, NewPropPress, DeletePropPress, PropPngPress, AddPropPress, NewMarkerPress, DeleteMarkerPress, AddMarkerPress, NewLayerPress, DeleteLayerPress, RaiseLayerPress, LowerLayerPress, AddLayerPress, ClosePress};

            physicalTileWidth = tileWidth * mapScale;
            physicalTileHeight = tileHeight * mapScale;

            emptyTile = new StillFrame(27, 0, 16, 16);
            gridColor = new Color(62, 89, 92);

            input = new InputHandler();

            windowMargin = 6 * uiScale;
            headerHeight = 12 * uiScale;
            windowHeight = (graphicsDevice.Viewport.Height / uiScale) - (windowMargin * 2);

            lineTexture = new Texture2D(graphicsDevice, 1, 1);
            lineTexture.SetData(new[] { Color.White });

            uiTexture = contentHandler.GetSpriteSheet("ui_elements");
            font = contentHandler.GetSpriteFont("font");

            tileTexture = contentHandler.GetImageFile(graphicsDevice, projectPath + @"\world_tiles.png");
            propTexture = contentHandler.GetImageFile(graphicsDevice, projectPath + @"\world_props.png");

            dialog = new DialogManager();
            tileSprites = new Dictionary<int, TileSource>();
            propSprites = new Dictionary<int, PropSource>();
            markerSprites = new Dictionary<int, MarkerSource>();
            camera = new Camera(graphicsDevice.Viewport, input);

            map = new Map();

            editorWindow = new EditorWindow(windowMargin, windowMargin + headerHeight, 140, windowHeight, uiScale, tileWidth, tileHeight, buttonEvents, HideLayer, ShowLayer, tileTexture, propTexture, uiTexture, lineTexture, font, input);
            grid = new Grid(0, 0, graphicsDevice.Viewport.Width, graphicsDevice.Viewport.Height, mapScale, tileWidth, tileHeight, lineTexture, gridColor, input);

            int offsetX = windowMargin + editorWindow.GetPhysicalWidth() + (2 * uiScale);
            tileRipper = new TileRipper(offsetX, windowMargin + headerHeight, 400, windowHeight, uiScale, tileWidth, tileHeight, buttonEvents, tileTexture, uiTexture, lineTexture, font, input);
            propRipper = new PropRipper(offsetX, windowMargin + headerHeight, 400, windowHeight, uiScale, tileWidth, tileHeight, buttonEvents, propTexture, uiTexture, lineTexture, font, input);
            markerDialog = new MarkerDialog(offsetX, windowMargin + headerHeight, 400, 44, uiScale, buttonEvents, uiTexture, lineTexture, font, input);
            layerDialog = new LayerDialog(offsetX, windowMargin + headerHeight, 334, 24, uiScale, buttonEvents, uiTexture, lineTexture, font, input);

            newDialog = new NewDialog(offsetX, windowMargin + headerHeight, 314, 118, uiScale, NewConfirmPress, CancelPress, uiTexture, lineTexture, font, input, dialog);
            saveAsDialog = new SaveAsDialog(offsetX, windowMargin + headerHeight, 314, 42, uiScale, SaveAsConfirmPress, CancelPress, uiTexture, lineTexture, font, input, dialog);
        }

        #endregion

        #region methods

        //Clear assets before loading from another project
        private void ClearProjectData()
        {
            physicalTileWidth = tileWidth * mapScale;
            physicalTileHeight = tileHeight * mapScale;

            grid.SetLines(tileWidth, tileHeight);

            tileRipper.SetSelectionSize(tileWidth, tileHeight);
            tileTexture = contentHandler.GetImageFile(graphicsDevice, projectPath + @"\world_tiles.png");
            tileRipper.FrameImage(tileTexture);
            editorWindow.SetTileImage(tileTexture);

            propRipper.SetSelectionSize(tileWidth, tileHeight);
            propTexture = contentHandler.GetImageFile(graphicsDevice, projectPath + @"\world_props.png");
            propRipper.FrameImage(propTexture);
            editorWindow.SetPropImage(propTexture);

            markerDialog.ResetData();
            layerDialog.ResetData();

            camera = new Camera(graphicsDevice.Viewport, input);

            map.Reset();

            tileSprites.Clear();
            propSprites.Clear();
            markerSprites.Clear();

            editorWindow.ResetData();
        }

        private void RefreshProjectData()
        {
            physicalTileWidth = tileWidth * mapScale;
            physicalTileHeight = tileHeight * mapScale;

            grid.SetLines(tileWidth, tileHeight);

            tileRipper.SetSelectionSize(tileWidth, tileHeight);
            tileTexture = contentHandler.GetImageFile(graphicsDevice, projectPath + @"\world_tiles.png");
            tileRipper.FrameImage(tileTexture);
            editorWindow.SetTileImage(tileTexture);

            propRipper.SetSelectionSize(tileWidth, tileHeight);
            propTexture = contentHandler.GetImageFile(graphicsDevice, projectPath + @"\world_props.png");
            propRipper.FrameImage(propTexture);
            editorWindow.SetPropImage(propTexture);
        }

        //Check if no window is obstructing grid at mouse position
        private bool IsHoveringGrid()
        {
            bool hovering = true;

            if (editorWindow.InBounds(input.MousePosition()))
            {
                hovering = false;
            }

            if (newDialog.Active && newDialog.InBounds(input.MousePosition()))
            {
                hovering = false;
            }

            if (saveAsDialog.Active && saveAsDialog.InBounds(input.MousePosition()))
            {
                hovering = false;
            }

            if (tileRipper.Active && tileRipper.InBounds(input.MousePosition()))
            {
                hovering = false;
            }

            if (propRipper.Active && propRipper.InBounds(input.MousePosition()))
            {
                hovering = false;
            }

            if (layerDialog.Active && layerDialog.InBounds(input.MousePosition()))
            {
                hovering = false;
            }

            if (markerDialog.Active && markerDialog.InBounds(input.MousePosition()))
            {
                hovering = false;
            }

            return hovering;
        }

        private Vector2 GetPhysicalPosition(Vector2 mapPosition)
        {
            int positionX = (int)mapPosition.X * (tileWidth * mapScale);
            int positionY = (int)mapPosition.Y * (tileHeight * mapScale);

            return new Vector2(positionX, positionY);
        }

        private Vector2 GetMapPosition(Vector2 position)
        {
            int tileX = (int)Math.Floor((decimal)(position.X / (tileWidth * mapScale)));
            int tileY = (int)Math.Floor((decimal)(position.Y / (tileHeight * mapScale)));

            return new Vector2(tileX, tileY);
        }

        public void SaveMap()
        {
            JMapData jMap = new JMapData(map);
            JObjectData objectData = new JObjectData(tileWidth, tileHeight, GetJTiles(), GetJProps(), GetJMarkers());
            JReadWriter jrw = new JReadWriter();
            jrw.SaveJMap(projectPath + @"\map_config.json", jMap);
            jrw.SaveJObjectData(projectPath + @"\asset_config.json", objectData);
        }

        public void GetTileSizeFromFile(string filePath)
        {
            JReadWriter jrw = new JReadWriter();
            JObjectData jMapper = jrw.GetAssets(filePath);

            tileWidth = jMapper.TileWidth;
            tileHeight = jMapper.TileHeight;
        }

        public void LoadProject()
        {
            var assetPath = projectPath + @"\asset_config.json";
            var mapPath = projectPath + @"\map_config.json";

            if (File.Exists(assetPath) && File.Exists(mapPath))
            {
                LoadAssets(assetPath);
                RefreshProjectData();
                LoadMap(mapPath);
            }
            else
            {
                ValidFiles = false;
                LoadMenu();
            }
        }

        private void LoadAssets(string filePath)
        {
            JReadWriter jrw = new JReadWriter();
            JObjectData jMapper = jrw.GetAssets(filePath);
            
            tileWidth = jMapper.TileWidth;
            tileHeight = jMapper.TileHeight;

            List<JTile> jTiles = jMapper.Tiles;
            List<JProp> jProps = jMapper.Props;
            List<JMarker> jMarkers = jMapper.Markers;

            tileSprites.Clear();
            propSprites.Clear();
            markerSprites.Clear();

            for (int i = 0; i < jTiles.Count; i++)
            {
                TileSource tile = new TileSource(jTiles[i].Name,jTiles[i].SourceX, jTiles[i].SourceY, tileWidth, tileHeight);

                tileSprites.Add(jTiles[i].ID, tile);
                editorWindow.LoadTile(jTiles[i].ID, jTiles[i].Name, tile.GetFrame());
            }

            for (int i = 0; i < jProps.Count; i++)
            {
                PropSource prop = new PropSource(jProps[i].Name, jProps[i].SourceX, jProps[i].SourceY, jProps[i].Width, jProps[i].Height, jProps[i].OffsetX, jProps[i].OffsetY);

                propSprites.Add(jProps[i].ID, prop);
                editorWindow.LoadProp(jProps[i].ID, jProps[i].Name, prop.GetFrame());
            }

            for (int i = 0; i < jMarkers.Count; i++)
            {
                Color rgb = new Color(jMarkers[i].RGB[0], jMarkers[i].RGB[1], jMarkers[i].RGB[2]);
                MarkerSource marker = new MarkerSource(jMarkers[i].Name, rgb);

                markerSprites.Add(jMarkers[i].ID, marker);
                editorWindow.LoadMarker(jMarkers[i].ID, jMarkers[i].Name, rgb);
            }
        }

        public void LoadMap(string filePath)
        {
            JReadWriter jrw = new JReadWriter();
            JMapData jMap = jrw.GetJMap(filePath);

            map.Reset();

            //Nested loop to iterate through the 2D int arrays and add objects to the map

            for (int i = 0; i < jMap.Layers.Count; i++)
            {
                map.AddLayer(jMap.Layers[i].Name);
                editorWindow.LoadLayer(jMap.Layers[i].Name);

                if (jMap.Layers[i].Tiles != null)
                {
                    for (int x = 0; x < jMap.Layers[i].Tiles.GetLength(0); x++)
                    {
                        for (int y = 0; y < jMap.Layers[i].Tiles.GetLength(1); y++)
                        {
                            if (jMap.Layers[i].Tiles[x, y] != -1)
                            {
                                Vector2 mapPosition = new Vector2(x, y);
                                Vector2 tilePosition = GetPhysicalPosition(mapPosition);
                                MapObject tile = new MapObject(mapPosition, new Rectangle((int)tilePosition.X, (int)tilePosition.Y, physicalTileWidth, physicalTileHeight), (int)jMap.Layers[i].Tiles[x, y]);
                                map.AddTile(i, tile);
                            }
                        }
                    }
                }

                if (jMap.Layers[i].Props != null)
                {
                    for (int x = 0; x < jMap.Layers[i].Props.GetLength(0); x++)
                    {
                        for (int y = 0; y < jMap.Layers[i].Props.GetLength(1); y++)
                        {
                            if (jMap.Layers[i].Props[x, y] != -1)
                            {
                                Vector2 mapPosition = new Vector2(x, y);
                                Vector2 propPosition = GetPhysicalPosition(mapPosition);

                                var propSource = propSprites[(int)jMap.Layers[i].Props[x, y]];

                                Rectangle propRectangle = new Rectangle((int)propPosition.X - (propSource.OffsetX * mapScale), (int)propPosition.Y - (propSource.OffsetY * mapScale), propSource.GetFrame().SourceRectangle.Width * mapScale, propSource.GetFrame().SourceRectangle.Height * mapScale);

                                MapObject prop = new MapObject(mapPosition, propRectangle, (int)jMap.Layers[i].Props[x, y]);
                                map.AddProp(i, prop);
                            }
                        }
                    }
                }

                if(jMap.Layers[i].Markers != null)
                {
                    for (int x = 0; x < jMap.Layers[i].Markers.GetLength(0); x++)
                    {
                        for (int y = 0; y < jMap.Layers[i].Markers.GetLength(1); y++)
                        {
                            if (jMap.Layers[i].Markers[x, y] != -1)
                            {
                                Vector2 mapPosition = new Vector2(x, y);
                                Vector2 markerPosition = GetPhysicalPosition(mapPosition);
                                MapObject Marker = new MapObject(mapPosition, new Rectangle((int)markerPosition.X, (int)markerPosition.Y, physicalTileWidth, physicalTileHeight), (int)jMap.Layers[i].Markers[x, y]);
                                map.AddMarker(i, Marker);
                            }
                        }
                    }
                }
            }
        }

        public List<JTile> GetJTiles()
        {
            List<JTile> tiles = new List<JTile>();

            foreach (KeyValuePair<int, TileSource> tile in tileSprites)
            {
                var source = tile.Value.GetFrame().SourceRectangle;
                tiles.Add(new JTile(tile.Value.Name, source.X, source.Y, tile.Key));
            }

            return tiles;
        }

        public List<JProp> GetJProps()
        {
            List<JProp> props = new List<JProp>();

            foreach (KeyValuePair<int, PropSource> prop in propSprites)
            {
                var source = prop.Value.GetFrame().SourceRectangle;
                props.Add(new JProp(prop.Value.Name, source.X, source.Y, source.Width, source.Height, prop.Value.OffsetX, prop.Value.OffsetY, prop.Key));
            } 

            return props;
        }

        public List<JMarker> GetJMarkers()
        {
            List<JMarker> markers = new List<JMarker>();

            foreach (KeyValuePair<int, MarkerSource> marker in markerSprites)
            {
                markers.Add(new JMarker(marker.Value.Name, marker.Value.MarkerColor, marker.Key));
            }

            return markers;
        }

        public bool ValidateTile()
        {
            TileSource newTile = tileRipper.GetTileSource();
            if (newTile.Name == string.Empty || newTile.Name == "")
            {
                return false;
            }

            //Return true if tile does not exist
            return !TileExists(tileRipper.GetID());
        }

        private bool TileExists(int id)
        {
            foreach (KeyValuePair<int, TileSource> tile in tileSprites)
            {
                if (tile.Key == id)
                {
                    return true;
                }
            }

            return false;
        }

        public bool ValidateProp()
        {
            PropSource newProp = propRipper.GetPropSource();
            if (newProp.Name == string.Empty || newProp.Name == "")
            {
                return false;
            }
            
            return !PropExists(propRipper.GetID());
        }

        private bool PropExists(int id)
        {
            foreach (KeyValuePair<int, PropSource> prop in propSprites)
            {
                if (prop.Key == id)
                {
                    return true;
                }
            }

            return false;
        }

        public bool ValidateMarker()
        {
            MarkerSource newMarker = markerDialog.GetMarkerSource();
            if (newMarker.Name == string.Empty || newMarker.Name == "")
            {
                return false;
            }

            return !MarkerExists(markerDialog.GetID());
        }

        private bool MarkerExists(int id)
        {
            foreach (KeyValuePair<int, MarkerSource> marker in markerSprites)
            {
                if (marker.Key == id)
                {
                    return true;
                }
            }

            return false;
        }

        public override void Update(GameTime gameTime)
        {

            input.Update();
            editorWindow.Update(gameTime);

            if (awaitingMouseRelease)
            {
                if (input.MouseOneReleased())
                {
                    awaitingMouseRelease = false;
                }
            }

            if (newDialog.Active)
            {
                newDialog.Update(gameTime);
            }
            else if (saveAsDialog.Active)
            {
                saveAsDialog.Update(gameTime);
            }
            else if (tileRipper.Active)
            {
                tileRipper.Update(gameTime);
            }
            else if (propRipper.Active)
            {
                propRipper.Update(gameTime);
            }
            else if (markerDialog.Active)
            {
                markerDialog.Update(gameTime);
            }
            else if (layerDialog.Active)
            {
                layerDialog.Update(gameTime);
            }

            if (Scrolling)
            {
                if (input.MouseThreeReleased())
                {
                    Scrolling = false;
                    grid.Dettach();
                    camera.Dettach();
                }
            }
            else
            {
                if (input.MouseThreePress() && IsHoveringGrid())
                {
                    Scrolling = true;
                    grid.Attach();
                    camera.Attach();
                }
            }

            camera.Update(gameTime);
            grid.Update(gameTime);

            var mousePosition = input.MousePosition();

            int depth = editorWindow.GetLayerSelection();


            if (depth > -1 && !awaitingMouseRelease)
            {
                if (input.MouseOneDown() && IsHoveringGrid())
                {
                    /*
                    * This line gets the mouse position in respect to the game world by multiplying mousePosition Vector with the inverted camera matrix,
                    * for more info take a look at how to transorm vectors by matrices https://www.khanacademy.org/math/precalculus/x9e81a4f98389efdf:matrices/x9e81a4f98389efdf:matrices-as-transformations/v/transforming-position-vector
                    */
                    var clickPosition = Vector2.Transform(new Vector2(mousePosition.X, mousePosition.Y), Matrix.Invert(camera.GetTransForm()));

                    Vector2 mapPosition = GetMapPosition(clickPosition);
                    Vector2 tilePosition = GetPhysicalPosition(mapPosition);

                    if (editorWindow.GetTileSelection() > -1)
                    {
                        var newTile = new MapObject(mapPosition, new Rectangle((int)tilePosition.X, (int)tilePosition.Y, physicalTileWidth, physicalTileHeight), editorWindow.GetTileSelection());
                        map.AddTile(depth, newTile);
                    }

                    if (editorWindow.GetPropSelection() > -1)
                    {
                        var propSource = propSprites[editorWindow.GetPropSelection()];
                        Rectangle propRectangle = new Rectangle((int)tilePosition.X - (propSource.OffsetX * mapScale), (int)tilePosition.Y - (propSource.OffsetY * mapScale), propSource.GetFrame().SourceRectangle.Width * mapScale, propSource.GetFrame().SourceRectangle.Height * mapScale);
                        var newProp = new MapObject(mapPosition, propRectangle, editorWindow.GetPropSelection());
                        map.AddProp(depth, newProp);
                    }

                    if (editorWindow.GetMarkerSelection() > -1)
                    {
                        var newMarker = new MapObject(mapPosition, new Rectangle((int)tilePosition.X, (int)tilePosition.Y, physicalTileWidth, physicalTileHeight), editorWindow.GetMarkerSelection());
                        map.AddMarker(depth, newMarker);
                    }
                }

                if (input.MouseTwoDown() && IsHoveringGrid())
                {
                    var clickPosition = Vector2.Transform(new Vector2(mousePosition.X, mousePosition.Y), Matrix.Invert(camera.GetTransForm()));

                    Vector2 mapPosition = GetMapPosition(clickPosition);

                    if (editorWindow.GetTileSelection() > -1)
                    {
                        map.RemoveTile(depth, mapPosition);
                    }

                    if (editorWindow.GetPropSelection() > -1)
                    {
                        map.RemoveProp(depth, mapPosition);
                    }

                    if (editorWindow.GetMarkerSelection() > -1)
                    {
                        map.RemoveMarker(depth, mapPosition);
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            // Start drawing
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, transformMatrix: GetCameraTransform()); // ARGUEMENTS 1 AND 2 ARE DEFAULTS

            for (int i = 0; i < map.layers.Count; i++)
            {
                var tiles = map.layers[i].GetTiles();
                var props = map.layers[i].GetProps();
                var markers = map.layers[i].GetMarkers();

                if (!map.layers[i].Hidden)
                {
                    for (int j = 0; j < tiles.Count; j++)
                    {
                        if (tiles[j].ID > -1)
                        {
                            Vector2 tilePosition = GetPhysicalPosition(tiles[j].mapPosition);
                            tiles[j].Draw(spriteBatch, tileTexture, tileSprites[tiles[j].ID].GetFrame().SourceRectangle);
                        }
                        else
                        {
                            tiles[j].Draw(spriteBatch, uiTexture, emptyTile.SourceRectangle);
                        }
                    }

                    for (int j = 0; j < props.Count; j++)
                    {
                        if (props[j].ID > -1)
                        {
                            Vector2 tilePosition = GetPhysicalPosition(props[j].mapPosition);
                            props[j].Draw(spriteBatch, propTexture, propSprites[props[j].ID].GetFrame().SourceRectangle);
                        }
                        else
                        {
                            props[j].Draw(spriteBatch, uiTexture, emptyTile.SourceRectangle);
                        }
                    }

                    if (markerVisible && editorWindow.GetLayerSelection() == i)
                    {
                        for (int j = 0; j < markers.Count; j++)
                        {
                            if (markers[j].ID > -1)
                            {
                                Vector2 tilePosition = GetPhysicalPosition(markers[j].mapPosition);
                                markers[j].Draw(spriteBatch, lineTexture, markerSprites[markers[j].ID].MarkerColor);
                            }
                            else
                            {
                                markers[j].Draw(spriteBatch, uiTexture, emptyTile.SourceRectangle);
                            }
                        }
                    }
                }
            }



            // Stop drawing
            spriteBatch.End();

            // Start drawing
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

            if (gridVisible)
            {
                grid.Draw(spriteBatch);
            }

            //Draw menus
            editorWindow.Draw(spriteBatch);
            
            if (newDialog.Active)
            {
                newDialog.Draw(spriteBatch);
                newDialog.DrawTooltip(spriteBatch);
            }
            else if (saveAsDialog.Active)
            {
                saveAsDialog.Draw(spriteBatch);
                saveAsDialog.DrawTooltip(spriteBatch);
            }
            else if (tileRipper.Active)
            {
                tileRipper.Draw(spriteBatch);
                tileRipper.DrawTooltip(spriteBatch);
            }
            else if (propRipper.Active)
            {
                propRipper.Draw(spriteBatch);
                propRipper.DrawTooltip(spriteBatch);
            }
            else if (markerDialog.Active)
            {
                markerDialog.Draw(spriteBatch);
                markerDialog.DrawTooltip(spriteBatch);
            }
            else if (layerDialog.Active)
            {
                layerDialog.Draw(spriteBatch);
                layerDialog.DrawTooltip(spriteBatch);
            }

            editorWindow.DrawTooltip(spriteBatch);
            // Stop drawing
            spriteBatch.End();
        }

        #endregion

        #region events
        
        public void ExitPress()
        {
            LoadMenu();
        }

        public void SavePress()
        {
            SaveMap();
        }

        public void SaveAsPress()
        {
            saveAsDialog.Activate();
            newDialog.DeActivate();
            tileRipper.DeActivate();
            propRipper.DeActivate();
            markerDialog.DeActivate();
            layerDialog.DeActivate();
        }

        public void LoadPress()
        {
            projectPath = Path.Combine(documentsPath, @"JenkyEditor\");
            projectPath = dialog.GetProjectPath(projectPath);

            if (projectPath != string.Empty)
            {

                ClearProjectData();
                LoadProject();

                if (!ValidFiles) return;
            }
        }

        public void NewPress()
        {
            newDialog.Activate();
            saveAsDialog.DeActivate();
            tileRipper.DeActivate();
            propRipper.DeActivate();
            markerDialog.DeActivate();
            layerDialog.DeActivate();
        }

        //Tile Panel
        public void NewTilePress()
        {
            tileRipper.Activate();
            propRipper.DeActivate();
            newDialog.DeActivate();
            saveAsDialog.DeActivate();
            markerDialog.DeActivate();
            layerDialog.DeActivate();
        }

        public void DeleteTilePress()
        {
            map.RemoveTilesByID(editorWindow.GetTileSelection());
            tileSprites.Remove(editorWindow.GetTileSelection());
            editorWindow.RemoveTile(editorWindow.GetTileSelection());
        }

        public void TilePngPress()
        {
            dialog.SelectPng(documentsPath, projectPath + @"\world_tiles.png");
            tileTexture = contentHandler.GetImageFile(graphicsDevice, projectPath + @"\world_tiles.png");
            tileRipper.FrameImage(tileTexture);
            editorWindow.SetTileImage(tileTexture);
        }

        //Tile Ripper
        public void AddTilePress()
        {
            if (ValidateTile())
            {
                TileSource newTile = tileRipper.GetTileSource();
                int tileID = tileRipper.GetID();
                tileSprites.Add(tileID, newTile);
                map.CheckTileDiscard(tileID);

                //set tileRipper inputs
                int idInput = tileRipper.GetID();

                while (TileExists(idInput))
                {
                    idInput++;
                }

                tileRipper.ResetData();
                tileRipper.SetID(idInput);

                editorWindow.LoadTile(tileID, newTile.Name, newTile.GetFrame());
            }
        }

        //Prop Panel
        public void NewPropPress()
        {
            propRipper.Activate();
            tileRipper.DeActivate();
            newDialog.DeActivate();
            saveAsDialog.DeActivate();
            markerDialog.DeActivate();
            layerDialog.DeActivate();
        }

        public void DeletePropPress()
        {
            map.RemovePropsByID(editorWindow.GetPropSelection());
            propSprites.Remove(editorWindow.GetPropSelection());
            editorWindow.RemoveProp(editorWindow.GetPropSelection());
        }

        public void PropPngPress()
        {
            dialog.SelectPng(documentsPath, projectPath + @"\world_props.png");
            propTexture = contentHandler.GetImageFile(graphicsDevice, projectPath + @"\world_props.png");
            propRipper.FrameImage(propTexture);
            editorWindow.SetPropImage(propTexture);
        }

        //Prop Ripper
        public void AddPropPress()
        {
            if (ValidateProp())
            {
                PropSource newProp = propRipper.GetPropSource();

                int propID = propRipper.GetID();
                propSprites.Add(propID, newProp);
                map.CheckPropDiscard(propID, mapScale, newProp, GetPhysicalPosition);

                //set propRipper inputs
                int idInput = propRipper.GetID();

                while (PropExists(idInput))
                {
                    idInput++;
                }

                propRipper.ResetData();
                propRipper.SetID(idInput);

                editorWindow.LoadProp(propID, newProp.Name, newProp.GetFrame());
            }

        }

        //Marker Panel
        public void NewMarkerPress()
        {
            propRipper.DeActivate();
            tileRipper.DeActivate();
            newDialog.DeActivate();
            saveAsDialog.DeActivate();
            markerDialog.Activate();
            layerDialog.DeActivate();
        }
        
        public void DeleteMarkerPress()
        {
            map.RemoveMarkersByID(editorWindow.GetMarkerSelection());
            markerSprites.Remove(editorWindow.GetMarkerSelection());
            editorWindow.RemoveMarker(editorWindow.GetMarkerSelection());
        }
        
        //Add Marker Window 
        public void AddMarkerPress()
        {
            if (ValidateMarker())
            {
                MarkerSource newMarker = markerDialog.GetMarkerSource();
                int markerID = markerDialog.GetID();
                markerSprites.Add(markerID, newMarker);
                map.CheckMarkerDiscard(markerID);

                //set tileRipper inputs
                int idInput = markerDialog.GetID();

                while (MarkerExists(idInput))
                {
                    idInput++;
                }

                markerDialog.ResetData();
                markerDialog.SetID(idInput);

                editorWindow.LoadMarker(markerID, newMarker.Name, newMarker.MarkerColor);
            }
        }

        //Layer Panel
        public void NewLayerPress()
        {
            propRipper.DeActivate();
            tileRipper.DeActivate();
            newDialog.DeActivate();
            saveAsDialog.DeActivate();
            markerDialog.DeActivate();
            layerDialog.Activate();
        }

        public void DeleteLayerPress()
        {
            int depth = editorWindow.GetLayerSelection();
            if(depth > -1)
            {
                map.RemoveLayer(depth);
                editorWindow.RemoveLayer(depth);
            }
        }

        //Layer Dialog
        public void AddLayerPress()
        {
            var layerName = layerDialog.GetName();

            if (layerName != string.Empty || layerName != "")
            {
                map.AddLayer(layerName);
                layerDialog.ResetData();
                editorWindow.LoadLayer(layerName);
            }
        }

        public void ClosePress()
        {
            if (tileRipper.Active)
            {
                tileRipper.DeActivate();
            }
            else if (propRipper.Active)
            {
                propRipper.DeActivate();
            }
            else if (markerDialog.Active)
            {
                markerDialog.DeActivate();
            }
            else if (layerDialog.Active)
            {
                layerDialog.DeActivate();
            }

            awaitingMouseRelease = true;
        }

        public void RaiseLayerPress()
        {
            if(editorWindow.GetLayerSelection() != -1)
            {
                map.RaiseLayer(editorWindow.GetLayerSelection());
                editorWindow.RaiseLayer(editorWindow.GetLayerSelection());
            }
        }

        public void LowerLayerPress()
        {
            if (editorWindow.GetLayerSelection() != -1)
            {
                map.LowerLayer(editorWindow.GetLayerSelection());
                editorWindow.LowerLayer(editorWindow.GetLayerSelection());
            }
        }

        public void ToggleGrid()
        {
            if (gridVisible)
            {
                gridVisible = false;
            }
            else
            {
                gridVisible = true;
            }

        }

        public void ToggleMarkers()
        {
            if (markerVisible)
            {
                markerVisible = false;
            }
            else
            {
                markerVisible = true;
            }
        }

        public void NewConfirmPress()
        {
            Tuple<string, int, int> inputText = newDialog.GetNewMap();
            string oldPath = projectPath;
            projectPath = Path.Combine(documentsPath, @"JenkyEditor\" + inputText.Item1);

            if (!Directory.Exists(projectPath) && newDialog.TilePng != string.Empty)
            {
                newDialog.DeActivate();
                newDialog.Reset();

                awaitingMouseRelease = true;

                Directory.CreateDirectory(projectPath);

                File.Copy(newDialog.TilePng, projectPath + @"\world_tiles.png", true);
                File.Copy(newDialog.TilePng, projectPath + @"\world_props.png", true);

                tileWidth = inputText.Item2;
                tileHeight = inputText.Item3;

                ClearProjectData();

                SaveMap();
            }
        }

        public void SaveAsConfirmPress()
        {
            string newPath = saveAsDialog.GetNewFolder();
            string oldPath = projectPath;
            
            if (newPath != string.Empty)
            {
                projectPath = Path.Combine(documentsPath, @"JenkyEditor\" + newPath);
                if (!Directory.Exists(projectPath))
                {
                    saveAsDialog.DeActivate();
                    saveAsDialog.Reset();

                    awaitingMouseRelease = true;

                    Directory.CreateDirectory(projectPath);

                    File.Copy(oldPath + @"\world_tiles.png", projectPath + @"\world_tiles.png", true);
                    File.Copy(oldPath + @"\world_props.png", projectPath + @"\world_props.png", true);

                    SaveMap();
                }
            }
        }

        private void HideLayer(int depth)
        {
            map.HideLayer(depth);
        }

        private void ShowLayer(int depth)
        {
            map.ShowLayer(depth);
        }

        public void CancelPress()
        {
            newDialog.DeActivate();
            saveAsDialog.DeActivate();
            awaitingMouseRelease = true;
        }

        #endregion 
    }
}
