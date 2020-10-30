using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Jenky.UI;
using Jenky.Graphics;
using Jenky.IO;
using System.Collections.Generic;



namespace JenkyEditor
{
    public class PropRipper : Menu
    {
        #region vars

        private int buttonWidth;
        private int buttonHeight;

        private int iconWidth;
        private int iconHeight;

        private int propWidth;
        private int propHeight;

        private int spacing;

        private int physicalButtonWidth;
        private int physicalButtonHeight;

      
        private bool hideSelection;
        private bool Scrolling { get; set; }
        private bool Selecting { get; set; }

        private ImageScroller imageScroller;
        private Texture2D imageTexture;

        private OffsetUtil offsetUtil;

        private Vector2 oldPixelOffset;

        private LeftWindowHeader selectHeader;
        private WindowStretched selectWindow;

        private ImageButton addButton;
        private ImageButton closeButton;

        private Rectangle picker;
        private Rectangle selection;

        private Label nameInputLabel;
        private Label idInputLabel;
        private Label sourceXInputLabel;
        private Label sourceYInputLabel;
        private Label widthInputLabel;
        private Label heightInputLabel;

        private TextInput nameInput;
        private IntInput idInput;
        private IntInput sourceXInput;
        private IntInput sourceYInput;
        private IntInput widthInput;
        private IntInput heightInput;

        private Vector2 selectionStart;
        private Vector2 selectionEnd;
        private Vector2 selectionSource;

        private int padding;
        private int propScale;

        #endregion 

        #region init

        public PropRipper(int positionX, int positionY, int _width, int _height, int _scale, int tileWidth, int tileHeight, Action[] buttonEvents, Texture2D _imageTexture, Texture2D uiTexture, Texture2D lineTexture,  SpriteFont font, InputHandler _input) : base(positionX, positionY, _width, _height, _scale, uiTexture, lineTexture, font, _input, false)
        {
            Scrolling = false;
            Selecting = false;

            imageTexture = _imageTexture;

            propWidth = 0;
            propHeight = 0;
            propScale = 6;

            buttonWidth = 22;
            buttonHeight = 16;

            iconWidth = 12;
            iconHeight = 10;

            spacing = 3 * scale;

            physicalButtonWidth = buttonWidth * scale;
            physicalButtonHeight = buttonHeight * scale;

            padding = 4 * scale;

            selectHeader = new LeftWindowHeader(positionX, positionY - (headerSlices.SliceHeight * scale), width - 65, headerSlices.SliceHeight, scale, "Prop Ripper", uiTexture, font, headingColor, headerFront, headerSlices.Middle, headerSlices.End);

            windowSlices.TopLeft = connectedSlices.TopLeft;
            selectWindow = new WindowStretched(positionX, positionY, width, height, scale, uiTexture, windowSlices);

            int offsetY = SetupBar(buttonEvents, font);
            imageScroller = new ImageScroller((int)position.X + padding, (int)position.Y + offsetY, physicalWidth - (padding * 2), physicalHeight - offsetY - padding, propScale, imageTexture, lineTexture, bodyColor, input);
            picker = new Rectangle((int)imageScroller.GetPosition().X, (int)imageScroller.GetPosition().Y, 1 * propScale, 1 * propScale);
            selectionStart = Vector2.Zero;
            selectionSource = Vector2.Zero;
            selection = new Rectangle((int)imageScroller.GetPosition().X, (int)imageScroller.GetPosition().Y, 1 * propScale, 1 * propScale);

            hideSelection = true;
            oldPixelOffset = imageScroller.GetPixelOffset();

            int offsetX = physicalWidth + (2 * scale);
            offsetUtil = new OffsetUtil((int)position.X + offsetX, (int)position.Y + (physicalHeight / 2), width / 2, height / 2, scale, tileWidth, tileHeight, imageTexture, uiTexture, lineTexture, font, input);
        }

        private int SetupBar(Action[] buttonEvents, SpriteFont font)
        {
            closeButton = new ImageButton((int)position.X + physicalWidth - physicalButtonWidth - padding, (int)position.Y + padding, buttonWidth, buttonHeight, scale, buttonEvents[23], uiTexture, lineTexture, font, headingColor, bodyColor, new StillFrame(0, 50, iconWidth, iconHeight), buttonSlices, inactiveSlices, hoverSlices, "Close", input);

            int inputSpacing = 5 * scale;
            int offsetX = (int)position.X + padding;
            int offsetY = (int)position.Y + padding;

            addButton = new ImageButton(offsetX, offsetY, buttonWidth, buttonHeight, scale, buttonEvents[14], uiTexture, lineTexture, font, headingColor, bodyColor, new StillFrame(12, 50, iconWidth, iconHeight), buttonSlices, inactiveSlices, hoverSlices, "Add Prop", input);

            offsetX = (int)addButton.position.X + addButton.GetPhysicalWidth() + spacing;
            idInputLabel = new Label(offsetX, offsetY, 14, buttonHeight, scale, "ID:", lineTexture, font, bodyColor);

            offsetX += idInputLabel.GetPhysicalWidth() + 1;
            idInput = new IntInput(offsetX, offsetY, 32, buttonHeight, scale, 5, lineTexture, font, bodyColor, input);

            offsetX += idInput.GetPhysicalWidth() + inputSpacing;
            nameInputLabel = new Label(offsetX, offsetY, 32, buttonHeight, scale, "Name:", lineTexture, font, bodyColor);

            offsetX += nameInputLabel.GetPhysicalWidth() + 1;
            nameInput = new TextInput(offsetX, offsetY, 245, buttonHeight, scale, 20, lineTexture, font, bodyColor, input);
            
            offsetY += physicalButtonHeight + spacing;

            offsetX = (int)addButton.position.X + addButton.GetPhysicalWidth() + spacing;
            sourceXInputLabel = new Label(offsetX, offsetY, 47, buttonHeight, scale, "SourceX:", lineTexture, font, bodyColor);

            offsetX += sourceXInputLabel.GetPhysicalWidth() + 1;
            sourceXInput = new IntInput(offsetX, offsetY, 31, buttonHeight, scale, 5, lineTexture, font, bodyColor, input);

            offsetX += sourceXInput.GetPhysicalWidth() + inputSpacing;
            sourceYInputLabel = new Label(offsetX, offsetY, 47, buttonHeight, scale, "SourceY:", lineTexture, font, bodyColor);

            offsetX += sourceYInputLabel.GetPhysicalWidth() + 1;
            sourceYInput = new IntInput(offsetX, offsetY, 31, buttonHeight, scale, 5, lineTexture, font, bodyColor, input);

            offsetX += sourceYInput.GetPhysicalWidth() + inputSpacing;
            widthInputLabel = new Label(offsetX, offsetY, 47, buttonHeight, scale, "Width:", lineTexture, font, bodyColor);

            offsetX += widthInputLabel.GetPhysicalWidth() + 1;
            widthInput = new IntInput(offsetX, offsetY, 31, buttonHeight, scale, 5, lineTexture, font, bodyColor, input);

            offsetX += widthInput.GetPhysicalWidth() + inputSpacing;
            heightInputLabel = new Label(offsetX, offsetY, 47, buttonHeight, scale, "Height:", lineTexture, font, bodyColor);

            offsetX += heightInputLabel.GetPhysicalWidth() + 1;
            heightInput = new IntInput(offsetX, offsetY, 31, buttonHeight, scale, 5, lineTexture, font, bodyColor, input);

            offsetY += physicalButtonHeight + spacing;

            return offsetY - (int)position.Y;
        }

            #endregion

        #region methods

        public void SetSelectionSize(int tileWidth, int tileHeight)
        {
            
        }

        public void FrameImage(Texture2D _imageTexture)
        {
            imageTexture = _imageTexture;
            imageScroller.FrameImage(imageTexture);

            picker.X = (int)imageScroller.GetPosition().X;
            picker.Y = (int)imageScroller.GetPosition().Y;

            selectionStart = Vector2.Zero;
            AlignSelection();

            offsetUtil.FrameImage(_imageTexture);
        }

        private Vector2 GetMousePickerPosition()
        {
            int pixelPositionX = (int)input.MousePosition().X - (int)imageScroller.position.X;
            int pixelPositionY = (int)input.MousePosition().Y - (int)imageScroller.position.Y;

            if(pixelPositionX < 0)
            {
                pixelPositionX = 0;
            }
            else if (pixelPositionX > imageScroller.GetPhysicalWidth())
            {
                pixelPositionX = imageScroller.GetPhysicalWidth();
            }

            if (pixelPositionY < 0)
            {
                pixelPositionY = 0;
            }
            else if (pixelPositionY > imageScroller.GetPhysicalHeight())
            {
                pixelPositionY = imageScroller.GetPhysicalHeight();
            }

            pixelPositionX = (int)Math.Floor((decimal)pixelPositionX / propScale);
            pixelPositionY = (int)Math.Floor((decimal)pixelPositionY / propScale);

            pixelPositionX = pixelPositionX * propScale;
            pixelPositionY = pixelPositionY * propScale;

            return new Vector2(imageScroller.GetPosition().X + pixelPositionX, imageScroller.GetPosition().Y + pixelPositionY);
        }

        private Vector2 SelectSourcePosition()
        {
            int pixelPositionX;
            int pixelPositionY;

            if (selectionStart.X > selectionEnd.X)
            {
                pixelPositionX = (int)selectionEnd.X - (int)imageScroller.position.X;
            }
            else
            {
                pixelPositionX = (int)selectionStart.X - (int)imageScroller.position.X;
            }

            if (selectionStart.Y > selectionEnd.Y)
            {
                pixelPositionY = (int)selectionEnd.Y - (int)imageScroller.position.Y;
            }
            else
            {
                pixelPositionY = (int)selectionStart.Y - (int)imageScroller.position.Y;
            }
            
            pixelPositionX = (int)Math.Floor((decimal)pixelPositionX / propScale);
            pixelPositionY = (int)Math.Floor((decimal)pixelPositionY / propScale);

            int maxX = imageScroller.DrawWidth - propWidth;
            int maxY = imageScroller.DrawHeight - propHeight;

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

        public PropSource GetPropSource()
        {
            Vector2 offset = offsetUtil.GetOffset();
            return new PropSource(nameInput.Text, sourceXInput.Value, sourceYInput.Value, widthInput.Value, heightInput.Value, (int)offset.X, (int)offset.Y);
        }

        public int GetID()
        {
            return idInput.Value;
        }

        public void TrimSelection()
        {
            hideSelection = false;
            selection.Width = propWidth * propScale;
            selection.Height = propHeight * propScale;

            if (selection.X < imageScroller.GetPosition().X)
            {
                int overlapX = (int)imageScroller.GetPosition().X - selection.X;

                if (overlapX >= propWidth * propScale)
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

                if (overlapX >= propWidth * propScale)
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

                if (overlapY >= propHeight * propScale)
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

                if (overlapY >= propHeight * propScale)
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
            selection.X = (int)imageScroller.GetPosition().X + (((int)selectionSource.X + (int)imageScroller.GetPixelOffset().X) * propScale);
            selection.Y = (int)imageScroller.GetPosition().Y + (((int)selectionSource.Y + (int)imageScroller.GetPixelOffset().Y) * propScale);
            TrimSelection();
        }

        public void ResetData()
        {
            idInput.Reset();
            nameInput.Reset();
            sourceXInput.Reset();
            sourceYInput.Reset();
            widthInput.Reset();
            heightInput.Reset();

            selectionSource = Vector2.Zero;
            propWidth = 0;
            propHeight = 0;

            AlignSelection();

            offsetUtil.ResetData();
        }

        public void SetID(int value)
        {
            idInput.SetInt(value);
        }

        public override bool InBounds(Vector2 point)
        {
            if (base.InBounds(point))
            {
                return true;
            }

            if (offsetUtil.InBounds(point))
            {
                return true;
            }

            return false;
        }

        public override void Update(GameTime gameTime)
        {
            addButton.Update();
            closeButton.Update();
            imageScroller.Update(gameTime);
            offsetUtil.Update(gameTime);
            idInput.Update(gameTime);
            nameInput.Update(gameTime);

            int oldXInput = sourceXInput.Value;
            int oldYInput = sourceYInput.Value;
            int oldWidthInput = widthInput.Value;
            int oldHeightInput = heightInput.Value;

            sourceXInput.Update(gameTime);
            sourceYInput.Update(gameTime);
            widthInput.Update(gameTime);
            heightInput.Update(gameTime);

            if (oldXInput != sourceXInput.Value || oldYInput != sourceYInput.Value)
            {
                selectionSource = new Vector2(sourceXInput.Value, sourceYInput.Value);
                AlignSelection();
                offsetUtil.SetOffsetFrame(new StillFrame((int)selectionSource.X, (int)selectionSource.Y, propWidth, propHeight));
            }

            if (oldWidthInput != widthInput.Value || oldHeightInput != heightInput.Value)
            {
                propWidth = widthInput.Value;
                propHeight = heightInput.Value;
                AlignSelection();
                offsetUtil.SetOffsetFrame(new StillFrame((int)selectionSource.X, (int)selectionSource.Y, propWidth, propHeight));
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

            if (Selecting)
            {
                if (input.MouseOneReleased())
                {
                    Selecting = false;

                    picker.Width = propScale;
                    picker.Height = propScale;

                    propWidth = (int)Math.Abs((decimal)selectionEnd.X - (decimal)selectionStart.X) / propScale;
                    propHeight = (int)Math.Abs((decimal)selectionEnd.Y - (decimal)selectionStart.Y) / propScale;
                    
                    selectionSource = SelectSourcePosition();
                    AlignSelection();

                    sourceXInput.SetInt((int)selectionSource.X);
                    sourceYInput.SetInt((int)selectionSource.Y);

                    widthInput.SetInt(propWidth);
                    heightInput.SetInt(propHeight);

                    offsetUtil.SetOffsetFrame(new StillFrame((int)selectionSource.X, (int)selectionSource.Y, propWidth, propHeight));
                }
                else
                {
                    selectionEnd = GetMousePickerPosition();
                    picker.Width = (int)Math.Abs((decimal)selectionEnd.X - (decimal)selectionStart.X);
                    picker.Height = (int)Math.Abs((decimal)selectionEnd.Y - (decimal)selectionStart.Y);

                    if (selectionStart.X >= selectionEnd.X)
                    {
                        picker.X = (int)selectionEnd.X;
                    }
                    else
                    {
                        picker.X = (int)selectionStart.X;
                    }

                    if (selectionStart.Y >= selectionEnd.Y)
                    {
                        
                        picker.Y = (int)selectionEnd.Y;
                    }
                    else
                    {
                        picker.Y = (int)selectionStart.Y;
                    }
                }
            }
            else
            {
                if (imageScroller.InBounds(input.MousePosition()))
                {
                    selectionStart = GetMousePickerPosition();
                    picker.X = (int)selectionStart.X;
                    picker.Y = (int)selectionStart.Y;

                    if (input.MouseOnePress())
                    {
                        Selecting = true;
                    }
                }
            }
            
            oldPixelOffset = imageScroller.GetPixelOffset();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Draw menu
            selectWindow.Draw(spriteBatch);
            selectHeader.Draw(spriteBatch);

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

            widthInputLabel.Draw(spriteBatch);
            widthInput.Draw(spriteBatch);

            heightInputLabel.Draw(spriteBatch);
            heightInput.Draw(spriteBatch);

            offsetUtil.Draw(spriteBatch);

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

            offsetUtil.DrawTooltip(spriteBatch);
        }

        #endregion 
    }
}