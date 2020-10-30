using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Jenky.UI;
using Jenky.Graphics;
using Jenky.IO;
using System.Collections.Generic;

namespace JenkyEditor
{
    public class EditorWindow : Menu
    {
        #region vars

        private LeftWindowHeader header;
        private WindowStretched sideBar;

        private SelectorTabs tabs;
        private TilePanel tilePanel;
        private PropPanel propPanel;
        private MarkerPanel markerPanel;
        private LayerPanel layerPanel;

        private ImageButton saveButton;
        private ImageButton saveAsButton;
        private ImageButton loadButton;
        private ImageButton newButton;
        private ImageButton exitButton;
        private ImageButton gridButton;
        private ImageButton markerButton;

        private Texture2D tileTexture;
        private Texture2D propTexture;

        private int padding;

        #endregion

        #region init

        public EditorWindow(int positionX, int positionY, int _width, int _height, int _scale, int tileWidth, int tileHeight, Action[] buttonEvents, Action<int> HideLayer, Action<int> ShowLayer, Texture2D _tileTexture, Texture2D _propTexture, Texture2D uiTexture, Texture2D lineTexture, SpriteFont font, InputHandler _input) : base(positionX, positionY, _width, _height, _scale, uiTexture, lineTexture, font, _input)
        {
            if (buttonEvents.Length != 24)
            {
                throw new ArgumentException("Number of button events must equal 20 for the Editor buttons");
            }

            tileTexture = _tileTexture;
            propTexture = _propTexture;

            padding = 4 * scale;

            header = new LeftWindowHeader(positionX, positionY - (headerSlices.SliceHeight * scale), width - 65, headerSlices.SliceHeight, scale, "Editor", uiTexture , font, headingColor, headerFront, headerSlices.Middle, headerSlices.End);
            windowSlices.TopLeft = connectedSlices.TopLeft;
            sideBar = new WindowStretched(positionX, positionY, width, height, scale, uiTexture, windowSlices);

            int buttonWidth = 22;
            int buttonHeight = 16;
            
            int physicalButtonWidth = buttonWidth * scale;
            int physicalButtonHeight = buttonHeight * scale;

            int offsetY = ((padding * 3) + (physicalButtonHeight * 2));

            tabs = new SelectorTabs(positionX, positionY + offsetY, width, buttonHeight, scale, lineTexture, bodyColor, font);

            offsetY += tabs.GetPhysicalHeight();

            tilePanel = new TilePanel(positionX, positionY + offsetY, width, 123, tileWidth, tileHeight, scale, buttonEvents, tileTexture, uiTexture, lineTexture, font, input);
            
            propPanel = new PropPanel(positionX, positionY + offsetY, width, 123, tileWidth, tileHeight, scale, buttonEvents, propTexture, uiTexture, lineTexture, font, input);
            propPanel.DeActivate();

            offsetY += propPanel.GetPhysicalHeight();

            markerPanel = new MarkerPanel(positionX, positionY + offsetY, width, 141, tileWidth, tileHeight, scale, buttonEvents, uiTexture, lineTexture, font, input);

            offsetY += markerPanel.GetPhysicalHeight();

            layerPanel = new LayerPanel(positionX, positionY + offsetY, width, 141, scale, buttonEvents, HideLayer, ShowLayer, uiTexture, lineTexture, font, input);
            SetupButtons(buttonEvents, font, buttonWidth, buttonHeight, physicalButtonWidth, physicalButtonHeight);
        }

        private void SetupButtons(Action[] buttonEvents, SpriteFont font, int buttonWidth, int buttonHeight, int physicalButtonWidth, int physicalButtonHeight)
        {
            int iconWidth = 12;
            int iconHeight = 10;

            int spacing = 3 * scale;

            int offsetX = padding; 

            exitButton = new ImageButton((int)position.X + physicalWidth - padding - physicalButtonWidth, (int)position.Y + padding, buttonWidth, buttonHeight, scale, buttonEvents[0], uiTexture, lineTexture, font, headingColor, bodyColor, new StillFrame(48, 30, iconWidth, iconHeight), buttonSlices, inactiveSlices, hoverSlices, "Exit", input);

            saveButton = new ImageButton((int)position.X + offsetX, (int)position.Y + padding, buttonWidth, buttonHeight, scale, buttonEvents[1], uiTexture, lineTexture, font, headingColor, bodyColor, new StillFrame(0, 30, iconWidth, iconHeight), buttonSlices, inactiveSlices, hoverSlices, "Save", input);

            offsetX += physicalButtonWidth + spacing;
            saveAsButton = new ImageButton((int)position.X + offsetX, (int)position.Y + padding, buttonWidth, buttonHeight, scale, buttonEvents[2], uiTexture, lineTexture, font, headingColor, bodyColor, new StillFrame(12, 30, iconWidth, iconHeight), buttonSlices, inactiveSlices, hoverSlices, "Save As", input);

            offsetX += physicalButtonWidth + spacing;
            loadButton = new ImageButton((int)position.X + offsetX, (int)position.Y + padding, buttonWidth, buttonHeight, scale, buttonEvents[3], uiTexture, lineTexture, font, headingColor, bodyColor, new StillFrame(24, 30, iconWidth, iconHeight), buttonSlices, inactiveSlices, hoverSlices, "Load Project", input);

            offsetX += physicalButtonWidth + spacing;
            newButton = new ImageButton((int)position.X + offsetX, (int)position.Y + padding, buttonWidth, buttonHeight, scale, buttonEvents[4], uiTexture, lineTexture, font, headingColor, bodyColor, new StillFrame(36, 30, iconWidth, iconHeight), buttonSlices, inactiveSlices, hoverSlices, "New Project", input);

            int offsetY = (padding * 2) + physicalButtonHeight;
            offsetX = padding;
            gridButton = new ImageButton((int)position.X + offsetX, (int)position.Y + offsetY, buttonWidth, buttonHeight, scale, buttonEvents[5], uiTexture, lineTexture, font, headingColor, bodyColor, new StillFrame(60, 30, iconWidth, iconHeight), buttonSlices, inactiveSlices, hoverSlices, "Toggle Grid", input);

            offsetX += physicalButtonWidth + spacing;
            markerButton = new ImageButton((int)position.X + offsetX, (int)position.Y + offsetY, buttonWidth, buttonHeight, scale, buttonEvents[6], uiTexture, lineTexture, font, headingColor, bodyColor, new StillFrame(84, 30, iconWidth, iconHeight), buttonSlices, inactiveSlices, hoverSlices, "Toggle Markers", input);

        }

        #endregion 

        #region methods

        public void ResetData()
        {
            tilePanel.ClearSelector();
            propPanel.ClearSelector();
            markerPanel.ClearSelector();
            layerPanel.ClearSelector();
        }

        public int GetTileSelection()
        {
            return tilePanel.GetSelection();
        }

        public void LoadTile(int itemID, string itemName, StillFrame icon)
        {
            tilePanel.LoadItem(itemID, itemName, icon);
        }

        public void RemoveTile(int tileID)
        {
            tilePanel.RemoveItem(tileID);
        }

        public void SetTileImage(Texture2D newTexture)
        {
            tilePanel.SetSourceTexture(newTexture);
        }

        public int GetPropSelection()
        {
            return propPanel.GetSelection();
        }

        public void LoadProp(int propID, string propName, StillFrame icon)
        {
            propPanel.LoadItem(propID, propName, icon);
        }

        public void RemoveProp(int propID)
        {
            propPanel.RemoveItem(propID);
        }

        public void SetPropImage(Texture2D newTexture)
        {
            propPanel.SetSourceTexture(newTexture);
        }

        public int GetMarkerSelection()
        {
            return markerPanel.GetSelection();
        }

        public void LoadMarker(int markerID, string markerName, Color rgb)
        {
            markerPanel.LoadItem(markerID, markerName, rgb);
        }

        public void RemoveMarker(int markerID)
        {
            markerPanel.RemoveItem(markerID);
        }

        public int GetLayerSelection()
        {
            return layerPanel.GetSelection();
        }

        public void LoadLayer(string name)
        {
            layerPanel.LoadLayer(name);
        }

        public void RemoveLayer(int depth)
        {
            layerPanel.RemoveLayer(depth);
        }

        public void RaiseLayer(int depth)
        {
            layerPanel.RaiseLayer(depth);
        }

        public void LowerLayer(int depth)
        {
            layerPanel.LowerLayer(depth);
        }

        public override void Update(GameTime gameTime)
        {
            exitButton.Update();
            saveButton.Update();
            saveAsButton.Update();
            loadButton.Update();
            newButton.Update();
            markerButton.Update();
            gridButton.Update();

            if (tilePanel.Active)
            {
                tilePanel.Update(gameTime);
            }

            if (propPanel.Active)
            {
                propPanel.Update(gameTime);
            }

            markerPanel.Update(gameTime);
            layerPanel.Update(gameTime);

            if (input.MouseOnePress())
            {
                if (tabs.InBounds(input.MousePosition()))
                {
                    tabs.ChangeTab(input.MousePosition());

                    if (tabs.tileMode)
                    {
                        tilePanel.Activate();
                        propPanel.Deselect();
                        propPanel.DeActivate();
                    }
                    else 
                    {
                        propPanel.Activate();
                        tilePanel.Deselect();
                        tilePanel.DeActivate();
                    }
                }

                if (markerPanel.InBounds(input.MousePosition()))
                {
                    propPanel.Deselect();
                    tilePanel.Deselect();
                }
                else if(tilePanel.InBounds(input.MousePosition()) || propPanel.InBounds(input.MousePosition()))
                {
                    markerPanel.Deselect();
                }

            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Draw menu
            header.Draw(spriteBatch);
            sideBar.Draw(spriteBatch);

            //Draw buttons
            exitButton.Draw(spriteBatch);
            saveButton.Draw(spriteBatch);
            saveAsButton.Draw(spriteBatch);
            loadButton.Draw(spriteBatch);
            newButton.Draw(spriteBatch);
            markerButton.Draw(spriteBatch);
            gridButton.Draw(spriteBatch);

            tabs.Draw(spriteBatch);
            markerPanel.Draw(spriteBatch);
            layerPanel.Draw(spriteBatch);

            if (tilePanel.Active)
            {
                tilePanel.Draw(spriteBatch);
            }

            if (propPanel.Active)
            {
                propPanel.Draw(spriteBatch);
            }
        }

        public override void DrawTooltip(SpriteBatch spriteBatch)
        {
            //Draw button Tooltips
            exitButton.DrawTooltip(spriteBatch);
            saveButton.DrawTooltip(spriteBatch);
            saveAsButton.DrawTooltip(spriteBatch);
            loadButton.DrawTooltip(spriteBatch);
            newButton.DrawTooltip(spriteBatch);
            gridButton.DrawTooltip(spriteBatch);
            markerButton.DrawTooltip(spriteBatch);

            tilePanel.DrawTooltip(spriteBatch);
            propPanel.DrawTooltip(spriteBatch);
            markerPanel.DrawTooltip(spriteBatch);
            layerPanel.DrawTooltip(spriteBatch);
        }
            #endregion 
    }
}
