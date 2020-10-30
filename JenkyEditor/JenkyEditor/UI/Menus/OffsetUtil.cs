using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Jenky.UI;
using Jenky.Graphics;
using Jenky.IO;
using System.Collections.Generic;



namespace JenkyEditor
{
    public class OffsetUtil : Menu
    {
        #region vars

        private int buttonWidth;
        private int buttonHeight;

        private int iconWidth;
        private int iconHeight;

        private int tileWidth;
        private int tileHeight;

        private int physicalButtonWidth;
        private int physicalButtonHeight;

        private bool hideSelection;
        private bool Scrolling { get; set; }

        private FrameScroller frameScroller;
        private Texture2D imageTexture;

        private Vector2 oldPixelOffset;

        private LeftWindowHeader header;
        private WindowStretched window;

        private ImageButton resetButton;
        
        private Label offsetXInputLabel;
        private Label offsetYInputLabel;
        
        private IntInput offsetXInput;
        private IntInput offsetYInput;

        private Rectangle picker;
        private Rectangle selection;

        private Vector2 selectionSource;

        private int padding;
        private int spacing;
        private int propScale;

        #endregion 

        #region init

        public OffsetUtil(int positionX, int positionY, int _width, int _height, int _scale, int _tileWidth, int _tileHeight, Texture2D _imageTexture, Texture2D uiTexture, Texture2D lineTexture,  SpriteFont font, InputHandler _input) : base(positionX, positionY, _width, _height, _scale, uiTexture, lineTexture, font, _input, false)
        {
            Scrolling = false;

            imageTexture = _imageTexture;

            tileWidth = _tileWidth;
            tileHeight = _tileHeight;
            propScale = 6;

            buttonWidth = 22;
            buttonHeight = 16;

            iconWidth = 12;
            iconHeight = 10;

            physicalButtonWidth = buttonWidth * scale;
            physicalButtonHeight = buttonHeight * scale;

            spacing = 3 * scale;
            padding = 4 * scale;

            header = new LeftWindowHeader(positionX, positionY - (headerSlices.SliceHeight * scale), width - 65, headerSlices.SliceHeight, scale, "Offset", uiTexture, font, headingColor, headerFront, headerSlices.Middle, headerSlices.End);

            windowSlices.TopLeft = connectedSlices.TopLeft;
            window = new WindowStretched(positionX, positionY, width, height, scale, uiTexture, windowSlices);
            SetupBar(font);

            int offsetY = (padding * 2) + physicalButtonHeight;

            frameScroller = new FrameScroller((int)position.X + padding, (int)position.Y + offsetY, physicalWidth - (padding * 2), physicalHeight - offsetY - padding, propScale, imageTexture, lineTexture, bodyColor, input);
            picker = new Rectangle((int)frameScroller.GetPosition().X, (int)frameScroller.GetPosition().Y, tileWidth * propScale, tileHeight * propScale);
            selectionSource = Vector2.Zero;
            selection = new Rectangle((int)frameScroller.GetPosition().X, (int)frameScroller.GetPosition().Y, tileWidth * propScale, tileHeight * propScale);

            oldPixelOffset = frameScroller.GetPixelOffset();
        }

        private void SetupBar(SpriteFont font)
        {
            int offsetX = (int)position.X + padding;
            int offsetY = (int)position.Y + padding;
            int inputSpacing = 6 * scale;

            resetButton = new ImageButton(offsetX, offsetY, buttonWidth, buttonHeight, scale, ResetPosition, uiTexture, lineTexture, font, headingColor, bodyColor, new StillFrame(72, 50, iconWidth, iconHeight), buttonSlices, inactiveSlices, hoverSlices, "Reset Position", input);

            offsetX = (int)resetButton.position.X + resetButton.GetPhysicalWidth() + spacing;
            offsetXInputLabel = new Label(offsetX, offsetY, 46, buttonHeight, scale, "OffsetX:", lineTexture, font, bodyColor);

            offsetX += offsetXInputLabel.GetPhysicalWidth() + 1;
            offsetXInput = new IntInput(offsetX, offsetY, 34, buttonHeight, scale, 5, lineTexture, font, bodyColor, input, true);

            offsetX += offsetXInput.GetPhysicalWidth() + inputSpacing;
            offsetYInputLabel = new Label(offsetX, offsetY, 46, buttonHeight, scale, "OffsetY:", lineTexture, font, bodyColor);

            offsetX += offsetYInputLabel.GetPhysicalWidth() + 1;
            offsetYInput = new IntInput(offsetX, offsetY, 34, buttonHeight, scale, 5, lineTexture, font, bodyColor, input, true);
        }

        #endregion

        #region methods

        public void SetSelectionSize(int _tileWidth, int _tileHeight)
        {
            tileWidth = _tileWidth;
            tileHeight = _tileHeight;

            picker.Width = tileWidth * propScale;
            picker.Height = tileHeight * propScale;

            selection.Width = tileWidth * propScale;
            selection.Height = tileHeight * propScale;
        }

        public void FrameImage(Texture2D _imageTexture)
        {
            imageTexture = _imageTexture;
            frameScroller.FrameImage(imageTexture);

            picker.X = (int)frameScroller.GetPosition().X;
            picker.Y = (int)frameScroller.GetPosition().Y;
        }

        public void SetOffsetFrame(StillFrame frame)
        {
            frameScroller.SetFrame(frame);
            oldPixelOffset = Vector2.Zero; 
            selectionSource = Vector2.Zero;
            offsetXInput.Reset();
            offsetYInput.Reset();
            AlignSelection();
        }

        private Vector2 GetMousePickerPosition()
        {
            int pixelPositionX = (int)input.MousePosition().X - (int)frameScroller.position.X;
            int pixelPositionY = (int)input.MousePosition().Y - (int)frameScroller.position.Y;

            int maxX = frameScroller.GetPhysicalWidth() - picker.Width;
            int maxY = frameScroller.GetPhysicalHeight() - picker.Height;

            if (pixelPositionX > maxX)
            {
                pixelPositionX = maxX;
            }

            if (pixelPositionY > maxY)
            {
                pixelPositionY = maxY;
            }

            pixelPositionX = (int)Math.Floor((decimal)pixelPositionX / propScale);
            pixelPositionY = (int)Math.Floor((decimal)pixelPositionY / propScale);

            pixelPositionX = pixelPositionX * propScale;
            pixelPositionY = pixelPositionY * propScale;

            return new Vector2(frameScroller.GetPosition().X + pixelPositionX, frameScroller.GetPosition().Y + pixelPositionY);
        }

        private Vector2 SelectSourcePosition()
        {
            int pixelPositionX = (int)input.MousePosition().X - (int)frameScroller.position.X;
            int pixelPositionY = (int)input.MousePosition().Y - (int)frameScroller.position.Y;
            
            pixelPositionX = (int)Math.Floor((decimal)pixelPositionX / propScale);
            pixelPositionY = (int)Math.Floor((decimal)pixelPositionY / propScale);
            
            return new Vector2(pixelPositionX - frameScroller.GetPixelOffset().X, pixelPositionY - frameScroller.GetPixelOffset().Y);
        }

        public Vector2 GetOffset()
        {
            return selectionSource;
        }

        public void TrimSelection()
        {
            hideSelection = false;
            selection.Width = tileWidth * propScale;
            selection.Height = tileHeight * propScale;

            if (selection.X < frameScroller.GetPosition().X)
            {
                int overlapX = (int)frameScroller.GetPosition().X - selection.X;

                if (overlapX >= tileWidth * propScale)
                {
                    hideSelection = true;
                }
                else
                {
                    selection.X += overlapX;
                    selection.Width -= overlapX;
                }
            }
            else if (selection.X > frameScroller.GetPosition().X + (frameScroller.GetPhysicalWidth() - selection.Width))
            {
                int overlapX = selection.X - ((int)frameScroller.GetPosition().X + (frameScroller.GetPhysicalWidth() - selection.Width));

                if (overlapX >= tileWidth * propScale)
                {
                    hideSelection = true;
                }
                else
                {
                    selection.Width -= overlapX;
                }
            }


            if (selection.Y < frameScroller.GetPosition().Y)
            {
                int overlapY = (int)frameScroller.GetPosition().Y - selection.Y;

                if (overlapY >= tileHeight * propScale)
                {
                    hideSelection = true;
                }
                else
                {
                    selection.Y += overlapY;
                    selection.Height -= overlapY;
                }
            }
            else if (selection.Y > frameScroller.GetPosition().Y + (frameScroller.GetPhysicalHeight() - selection.Height))
            {
                int overlapY = selection.Y - ((int)frameScroller.GetPosition().Y + (frameScroller.GetPhysicalHeight() - selection.Height));

                if (overlapY >= tileHeight * propScale)
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
            selection.X = (int)frameScroller.GetPosition().X + (((int)selectionSource.X + (int)frameScroller.GetPixelOffset().X) * propScale);
            selection.Y = (int)frameScroller.GetPosition().Y + (((int)selectionSource.Y + (int)frameScroller.GetPixelOffset().Y) * propScale);
            TrimSelection();
        }

        public void ResetData()
        {
            offsetXInput.Reset();
            offsetYInput.Reset();
            selectionSource = Vector2.Zero;
            ResetPosition();
            SetOffsetFrame(new StillFrame(0, 0, 0, 0));
        }

        public override void Update(GameTime gameTime)
        {
            resetButton.Update();
            frameScroller.Update(gameTime);

            int oldXInput = offsetXInput.Value;
            int oldYInput = offsetYInput.Value;

            offsetXInput.Update(gameTime);
            offsetYInput.Update(gameTime);

            if (oldXInput != offsetXInput.Value || oldYInput != offsetYInput.Value)
            {
                selectionSource = new Vector2(offsetXInput.Value, offsetYInput.Value);
                AlignSelection();
            }

            if (Scrolling)
            {
                AlignSelection();

                if (input.MouseThreeReleased())
                {
                    Scrolling = false;
                    frameScroller.Dettach();
                }
            }
            else
            {
                if (input.MouseThreePress() && frameScroller.InBounds(input.MousePosition()))
                {
                    Scrolling = true;
                    frameScroller.Attach();
                }
            }

            if (frameScroller.InBounds(input.MousePosition()))
            {
                var rectanglePosition = GetMousePickerPosition();
                picker.X = (int)rectanglePosition.X;
                picker.Y = (int)rectanglePosition.Y;

                if (input.MouseOnePress())
                {
                    selectionSource = SelectSourcePosition();
                    AlignSelection();

                    offsetXInput.SetInt((int)selectionSource.X);
                    offsetYInput.SetInt((int)selectionSource.Y);
                }
            }

            oldPixelOffset = frameScroller.GetPixelOffset();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Draw menu
            window.Draw(spriteBatch);
            header.Draw(spriteBatch);

            //Draw buttons
            resetButton.Draw(spriteBatch);
            
            //Draw inputs
            offsetXInputLabel.Draw(spriteBatch);
            offsetXInput.Draw(spriteBatch);

            offsetYInputLabel.Draw(spriteBatch);
            offsetYInput.Draw(spriteBatch);

            //Draw Scroller
            frameScroller.Draw(spriteBatch);

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
            resetButton.DrawTooltip(spriteBatch);
        }

        #endregion

        #region events

        public void ResetPosition()
        {
            frameScroller.Reset();
            AlignSelection();
        }

        #endregion

    }
}