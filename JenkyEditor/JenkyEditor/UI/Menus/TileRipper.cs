using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Jenky.UI;
using Jenky.Graphics;
using Jenky.IO;
using System.Collections.Generic;



namespace JenkyEditor
{
    public class TileRipper : Menu
    {
        #region vars

        private int buttonWidth;
        private int buttonHeight;

        private int iconWidth;
        private int iconHeight;

        private int tileWidth;
        private int tileHeight;

        private int spacing;

        private int physicalButtonWidth;
        private int physicalButtonHeight;

        private bool hideSelection;
        private bool Scrolling { get; set; }

        private ImageScroller imageScroller;
        private Texture2D imageTexture;

        private Vector2 oldPixelOffset;

        private LeftWindowHeader header;
        private WindowStretched window;

        private ImageButton addButton;
        private ImageButton closeButton;

        private Rectangle picker;
        private Rectangle selection;

        private Label nameInputLabel;
        private Label idInputLabel;
        private Label sourceXInputLabel;
        private Label sourceYInputLabel;

        private TextInput nameInput;
        private IntInput idInput;
        private IntInput sourceXInput;
        private IntInput sourceYInput;

        private Vector2 selectionSource;

        private int padding;
        private int tileScale;

        #endregion 

        #region init

        public TileRipper(int positionX, int positionY, int _width, int _height, int _scale, int _tileWidth, int _tileHeight, Action[] buttonEvents, Texture2D _imageTexture, Texture2D uiTexture, Texture2D lineTexture,  SpriteFont font, InputHandler _input) : base(positionX, positionY, _width, _height, _scale, uiTexture, lineTexture, font, _input, false)
        {
            Scrolling = false;

            imageTexture = _imageTexture;

            tileWidth = _tileWidth;
            tileHeight = _tileHeight;
            tileScale = 6;

            buttonWidth = 22;
            buttonHeight = 16;

            iconWidth = 12;
            iconHeight = 10;

            spacing = 3 * scale;

            physicalButtonWidth = buttonWidth * scale;
            physicalButtonHeight = buttonHeight * scale;

            padding = 4 * scale;

            header = new LeftWindowHeader(positionX, positionY - (headerSlices.SliceHeight * scale), width - 65, headerSlices.SliceHeight, scale, "Tile Ripper", uiTexture, font, headingColor, headerFront, headerSlices.Middle, headerSlices.End);

            windowSlices.TopLeft = connectedSlices.TopLeft;
            window = new WindowStretched(positionX, positionY, width, height, scale, uiTexture, windowSlices);
            

            int offsetY = SetupBar(buttonEvents, font);

            imageScroller = new ImageScroller((int)position.X + padding, (int)position.Y + offsetY, physicalWidth - (padding * 2), physicalHeight - offsetY - padding, tileScale, imageTexture, lineTexture, bodyColor, input);
            picker = new Rectangle((int)imageScroller.GetPosition().X, (int)imageScroller.GetPosition().Y, tileWidth * tileScale, tileHeight * tileScale);
            selectionSource = Vector2.Zero;
            selection = new Rectangle((int)imageScroller.GetPosition().X, (int)imageScroller.GetPosition().Y, tileWidth * tileScale, tileHeight * tileScale);

            oldPixelOffset = imageScroller.GetPixelOffset();
        }

        private int SetupBar(Action[] buttonEvents, SpriteFont font)
        {
            closeButton = new ImageButton((int)position.X + physicalWidth - physicalButtonWidth - padding, (int)position.Y + padding, buttonWidth, buttonHeight, scale, buttonEvents[23], uiTexture, lineTexture, font, headingColor, bodyColor, new StillFrame(0, 50, iconWidth, iconHeight), buttonSlices, inactiveSlices, hoverSlices, "Close", input);

            int offsetX = (int)position.X + padding;
            int offsetY = (int)position.Y + padding;
            int inputSpacing = 5 * scale;

            addButton = new ImageButton(offsetX, offsetY, buttonWidth, buttonHeight, scale, buttonEvents[10], uiTexture, lineTexture, font, headingColor, bodyColor, new StillFrame(12, 50, iconWidth, iconHeight), buttonSlices, inactiveSlices, hoverSlices, "Add Tile", input);

            offsetX = (int)addButton.position.X + addButton.GetPhysicalWidth() + spacing;
            
            idInputLabel = new Label(offsetX, (int)position.Y + padding, 14, buttonHeight, scale, "ID:", lineTexture, font, bodyColor);

            offsetX += idInputLabel.GetPhysicalWidth() + 1;
            idInput = new IntInput(offsetX, (int)position.Y + padding, 32, buttonHeight, scale, 5, lineTexture, font, bodyColor, input);

            offsetX += idInput.GetPhysicalWidth() + (5 * scale);
            nameInputLabel = new Label(offsetX, (int)position.Y + padding, 32, buttonHeight, scale, "Name:", lineTexture, font, bodyColor);

            offsetX += nameInputLabel.GetPhysicalWidth() + 1;
            nameInput = new TextInput(offsetX, (int)position.Y + padding, 245, buttonHeight, scale, 20, lineTexture, font, bodyColor, input);

            offsetY += physicalButtonHeight + spacing;

            offsetX = (int)addButton.position.X + addButton.GetPhysicalWidth() + spacing;
            sourceXInputLabel = new Label(offsetX, offsetY, 47, buttonHeight, scale, "SourceX:", lineTexture, font, bodyColor);

            offsetX += sourceXInputLabel.GetPhysicalWidth() + 1;
            sourceXInput = new IntInput(offsetX, offsetY, 31, buttonHeight, scale, 5, lineTexture, font, bodyColor, input);

            offsetX += sourceXInput.GetPhysicalWidth() + inputSpacing;
            sourceYInputLabel = new Label(offsetX, offsetY, 47, buttonHeight, scale, "SourceY:", lineTexture, font, bodyColor);

            offsetX += sourceYInputLabel.GetPhysicalWidth() + 1;
            sourceYInput = new IntInput(offsetX, offsetY, 31, buttonHeight, scale, 5, lineTexture, font, bodyColor, input);
            
            offsetY += physicalButtonHeight + spacing;

            return offsetY - (int)position.Y;
        }

            #endregion

            #region methods

            public void SetSelectionSize(int _tileWidth, int _tileHeight)
        {
            tileWidth = _tileWidth;
            tileHeight = _tileHeight;

            picker.Width = tileWidth * tileScale;
            picker.Height = tileHeight * tileScale;

            selection.Width = tileWidth * tileScale;
            selection.Height = tileHeight * tileScale;
        }

        public void FrameImage(Texture2D _imageTexture)
        {
            imageTexture = _imageTexture;
            imageScroller.FrameImage(imageTexture);

            picker.X = (int)imageScroller.GetPosition().X;
            picker.Y = (int)imageScroller.GetPosition().Y;

            selectionSource = Vector2.Zero;
            AlignSelection();
        }

        private Vector2 GetMousePickerPosition()
        {
            int pixelPositionX = (int)input.MousePosition().X - (int)imageScroller.position.X;
            int pixelPositionY = (int)input.MousePosition().Y - (int)imageScroller.position.Y;

            int maxX = imageScroller.GetPhysicalWidth() - picker.Width;
            int maxY = imageScroller.GetPhysicalHeight() - picker.Height;

            if (pixelPositionX > maxX)
            {
                pixelPositionX = maxX;
            }

            if (pixelPositionY > maxY)
            {
                pixelPositionY = maxY;
            }

            pixelPositionX = (int)Math.Floor((decimal)pixelPositionX / tileScale);
            pixelPositionY = (int)Math.Floor((decimal)pixelPositionY / tileScale);

            pixelPositionX = pixelPositionX * tileScale;
            pixelPositionY = pixelPositionY * tileScale;

            return new Vector2(imageScroller.GetPosition().X + pixelPositionX, imageScroller.GetPosition().Y + pixelPositionY);
        }

        private Vector2 SelectSourcePosition()
        {
            
            int pixelPositionX = (int)input.MousePosition().X - (int)imageScroller.position.X;
            int pixelPositionY = (int)input.MousePosition().Y - (int)imageScroller.position.Y;
            
            pixelPositionX = (int)Math.Floor((decimal)pixelPositionX / tileScale);
            pixelPositionY = (int)Math.Floor((decimal)pixelPositionY / tileScale);

            int maxX = imageScroller.DrawWidth - tileWidth;
            int maxY = imageScroller.DrawHeight - tileHeight;

            if (pixelPositionX > maxX)
            {
                pixelPositionX = maxX;
            }

            if (pixelPositionY > maxY)
            {
                pixelPositionY = maxY;
            }
            
            return new Vector2(pixelPositionX - imageScroller.GetPixelOffset().X, pixelPositionY - imageScroller.GetPixelOffset().Y);
        }

        public TileSource GetTileSource()
        {
            return new TileSource(nameInput.Text, sourceXInput.Value, sourceYInput.Value, tileWidth, tileHeight);
        }

        public int GetID()
        {
            return idInput.Value;
        }

        public void TrimSelection()
        {
            hideSelection = false;
            selection.Width = tileWidth * tileScale;
            selection.Height = tileHeight * tileScale;

            if (selection.X < imageScroller.GetPosition().X)
            {
                int overlapX = (int)imageScroller.GetPosition().X - selection.X;

                if (overlapX >= tileWidth * tileScale)
                {
                    hideSelection = true;
                }
                else
                {
                    selection.X += overlapX;
                    selection.Width -= overlapX;
                }
            }
            else if (selection.X > imageScroller.GetPosition().X + (imageScroller.GetPhysicalWidth() - selection.Width))
            {
                int overlapX = selection.X - ((int)imageScroller.GetPosition().X + (imageScroller.GetPhysicalWidth() - selection.Width));

                if (overlapX >= tileWidth * tileScale)
                {
                    hideSelection = true;
                }
                else
                {
                    selection.Width -= overlapX;
                }
            }


            if (selection.Y < imageScroller.GetPosition().Y)
            {
                int overlapY = (int)imageScroller.GetPosition().Y - selection.Y;

                if (overlapY >= tileHeight * tileScale)
                {
                    hideSelection = true;
                }
                else
                {
                    selection.Y += overlapY;
                    selection.Height -= overlapY;
                }
            }
            else if (selection.Y > imageScroller.GetPosition().Y + (imageScroller.GetPhysicalHeight() - selection.Height))
            {
                int overlapY = selection.Y - ((int)imageScroller.GetPosition().Y + (imageScroller.GetPhysicalHeight() - selection.Height));

                if (overlapY >= tileHeight * tileScale)
                {
                    hideSelection = true;
                }
                else
                {
                    selection.Height -= overlapY;
                }
            }
        }

        public void AlignSelection()
        {
            selection.X = (int)imageScroller.GetPosition().X + (((int)selectionSource.X + (int)imageScroller.GetPixelOffset().X) * tileScale);
            selection.Y = (int)imageScroller.GetPosition().Y + (((int)selectionSource.Y + (int)imageScroller.GetPixelOffset().Y) * tileScale);
            TrimSelection();
        }

        public void ResetData()
        {
            idInput.Reset();
            nameInput.Reset();

            sourceXInput.Reset();
            sourceYInput.Reset();

            selectionSource = Vector2.Zero;
            AlignSelection();
        }

        public void SetID(int value)
        {
            idInput.SetInt(value);
        }

        public override void Update(GameTime gameTime)
        {
            addButton.Update();
            closeButton.Update();
            imageScroller.Update(gameTime);
            idInput.Update(gameTime);
            nameInput.Update(gameTime);

            int oldXInput = sourceXInput.Value;
            int oldYInput = sourceYInput.Value;

            sourceXInput.Update(gameTime);
            sourceYInput.Update(gameTime);

            if (oldXInput != sourceXInput.Value || oldYInput != sourceYInput.Value)
            {
                selectionSource = new Vector2(sourceXInput.Value, sourceYInput.Value);
                AlignSelection();
            }

            if (Scrolling)
            {
                AlignSelection();

                if (input.MouseThreeReleased())
                {
                    Scrolling = false;
                    imageScroller.Dettach();
                }
            }
            else
            {
                if (input.MouseThreePress() && imageScroller.InBounds(input.MousePosition()))
                {
                    Scrolling = true;
                    imageScroller.Attach();
                }
            }

            if (imageScroller.InBounds(input.MousePosition()))
            {
                var rectanglePosition = GetMousePickerPosition();
                picker.X = (int)rectanglePosition.X;
                picker.Y = (int)rectanglePosition.Y;

                if (input.MouseOnePress())
                {
                    selectionSource = SelectSourcePosition();
                    AlignSelection();

                    sourceXInput.SetInt((int)selectionSource.X);
                    sourceYInput.SetInt((int)selectionSource.Y);
                }
            }

            oldPixelOffset = imageScroller.GetPixelOffset();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Draw menu
            window.Draw(spriteBatch);
            header.Draw(spriteBatch);

            //Draw buttons
            addButton.Draw(spriteBatch);
            closeButton.Draw(spriteBatch);

            //Draw Scroller
            imageScroller.Draw(spriteBatch);

            //Drawing text input
            idInputLabel.Draw(spriteBatch);
            idInput.Draw(spriteBatch);

            nameInputLabel.Draw(spriteBatch);
            nameInput.Draw(spriteBatch);

            sourceXInputLabel.Draw(spriteBatch);
            sourceXInput.Draw(spriteBatch);

            sourceYInputLabel.Draw(spriteBatch);
            sourceYInput.Draw(spriteBatch);

            //Drawing picker
            spriteBatch.Draw(lineTexture, new Rectangle(picker.X, picker.Y, picker.Width, 2), bodyColor);
            spriteBatch.Draw(lineTexture, new Rectangle(picker.X, picker.Y + picker.Height - 2, picker.Width, 2), bodyColor);
            spriteBatch.Draw(lineTexture, new Rectangle(picker.X + picker.Width - 2, picker.Y, 2, picker.Height), bodyColor);
            spriteBatch.Draw(lineTexture, new Rectangle(picker.X, picker.Y, 2, picker.Height), bodyColor);

            if (!hideSelection)
            {
                //Drawing selection
                spriteBatch.Draw(lineTexture, new Rectangle(selection.X, selection.Y, selection.Width, 2), Color.White);
                spriteBatch.Draw(lineTexture, new Rectangle(selection.X, selection.Y + selection.Height - 2, selection.Width, 2), Color.White);
                spriteBatch.Draw(lineTexture, new Rectangle(selection.X + selection.Width - 2, selection.Y, 2, selection.Height), Color.White);
                spriteBatch.Draw(lineTexture, new Rectangle(selection.X, selection.Y, 2, selection.Height), Color.White);
            }
        }

        public override void DrawTooltip(SpriteBatch spriteBatch)
        {
            addButton.DrawTooltip(spriteBatch);

            closeButton.DrawTooltip(spriteBatch);
        }

        #endregion 
    }
}