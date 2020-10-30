using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Jenky.Graphics;
using Jenky.IO;
using Jenky.UI;

namespace JenkyEditor
{
    public class LayerPanel : Menu
    {
        #region vars

        private Vector2 labelPosition;
        private SpriteFont font;

        private Rectangle underlineRectangle;
        private Rectangle borderTopRectangle;

        private ImageButton layerButton;
        private ImageButton deleteButton;
        private ImageButton moveUpButton;
        private ImageButton moveDownButton;

        private ImageButton scrollUpButton;
        private ImageButton scrollDownButton;

        private LayerSelectionTable selector;

        private string label;

        private int padding;

        private int buttonWidth;
        private int buttonHeight;

        private int physicalButtonWidth;
        private int physicalButtonHeight;

        private int spacing;

        #endregion

        #region init

        public LayerPanel(int positionX, int positionY, int _width, int _height, int _scale, Action[] buttonEvents, Action<int> HideLayer, Action<int> ShowLayer, Texture2D uiTexture, Texture2D lineTexture, SpriteFont _font, InputHandler _input) : base(positionX, positionY, _width, _height, _scale, uiTexture, lineTexture, _font, _input)
        {
            font = _font;

            buttonWidth = 22;
            buttonHeight = 16;

            physicalButtonWidth = buttonWidth * scale;
            physicalButtonHeight = buttonHeight * scale;

            padding = 4 * scale;
            spacing = 3 * scale;

            label = "Layers";

            borderTopRectangle = new Rectangle((int)position.X, (int)position.Y, GetPhysicalWidth(), 2);

            int offsetY = padding;

            labelPosition = position + new Vector2(padding, padding);

            Vector2 textDimensions = font.MeasureString(label);

            textDimensions = textDimensions * scale;

            offsetY += (int)textDimensions.Y + scale;
            underlineRectangle = new Rectangle((int)position.X, (int)position.Y + offsetY, GetPhysicalWidth(), 2);

            offsetY += underlineRectangle.Height + padding;

            int buttonOffset = offsetY;

            offsetY +=  physicalButtonHeight + padding;
            
            selector = new LayerSelectionTable(positionX + padding, positionY + offsetY, width - (2 * (padding / scale)), height - ((offsetY + buttonOffset + padding) / scale), scale, 4, HideLayer, ShowLayer, uiTexture, lineTexture, font ,headingColor, bodyColor, input);

            SetupButtons(buttonEvents, font, buttonOffset);
        }

        private void SetupButtons(Action[] buttonEvents, SpriteFont font, int offsetY)
        {
            int offsetX = padding;

            int iconWidth = 12;
            int iconHeight = 10;
            
            layerButton = new ImageButton((int)position.X + offsetX, (int)position.Y + offsetY, buttonWidth, buttonHeight, scale, buttonEvents[18], uiTexture, lineTexture, font, headingColor, bodyColor, new StillFrame(24, 40, iconWidth, iconHeight), buttonSlices, inactiveSlices, hoverSlices, "Add Layers", input);

            offsetX += physicalButtonWidth + spacing;
            deleteButton = new ImageButton((int)position.X + offsetX, (int)position.Y + offsetY, buttonWidth, buttonHeight, scale, buttonEvents[19], uiTexture, lineTexture, font, headingColor, bodyColor, new StillFrame(24, 50, iconWidth, iconHeight), buttonSlices, inactiveSlices, hoverSlices, "Delete Layer", input);

            moveUpButton = new ImageButton((int)position.X + physicalWidth - padding - physicalButtonWidth, (int)position.Y + offsetY, buttonWidth, buttonHeight, scale, buttonEvents[20], uiTexture, lineTexture, font, headingColor, bodyColor, new StillFrame(36, 40, iconWidth, iconHeight), buttonSlices, inactiveSlices, hoverSlices, "Move Layer Up", input);
            moveDownButton = new ImageButton((int)position.X + physicalWidth - padding - physicalButtonWidth - spacing - physicalButtonWidth, (int)position.Y + offsetY, buttonWidth, buttonHeight, scale, buttonEvents[21], uiTexture, lineTexture, font, headingColor, bodyColor, new StillFrame(48, 40, iconWidth, iconHeight), buttonSlices, inactiveSlices, hoverSlices, "Move Layer Down", input);

            offsetX = physicalWidth - padding - physicalButtonWidth;
            offsetY = (int)selector.position.Y + selector.GetPhysicalHeight() + padding;

            scrollUpButton = new ImageButton((int)position.X + offsetX, offsetY, buttonWidth, buttonHeight, scale, ScrollUp, uiTexture, lineTexture, font, headingColor, bodyColor, new StillFrame(48, 50, iconWidth, iconHeight), buttonSlices, inactiveSlices, hoverSlices, "Scroll Up", input);

            offsetX -= physicalButtonWidth + spacing;

            scrollDownButton = new ImageButton((int)position.X + offsetX, offsetY, buttonWidth, buttonHeight, scale, ScrollDown, uiTexture, lineTexture, font, headingColor, bodyColor, new StillFrame(60, 50, iconWidth, iconHeight), buttonSlices, inactiveSlices, hoverSlices, "Scroll Down", input);
        }

        #endregion

        #region methods


        public int GetSelection()
        {
            return selector.SelectedID;
        }

        public void ClearSelector()
        {
            selector.Clear();
        }

        public void LoadLayer(string name)
        {
            selector.InsertLayer(name);
        }

        public void RemoveLayer(int depth)
        {
            selector.RemoveLayer(depth);
        }

        public void RaiseLayer(int depth)
        {
            selector.RaiseLayer(depth);
        }

        public void LowerLayer(int depth)
        {
            selector.LowerLayer(depth);
        }

        public void SelectorPressed()
        {
            if (selector.InBounds(input.MousePosition()))
            {
                selector.CheckSelection();
            }
        }

        private void ScrollUp()
        {
            selector.ScrollUp();
        }

        private void ScrollDown()
        {
            selector.ScrollDown();
        }

        public override void Update(GameTime gameTime)
        {
            layerButton.Update();
            deleteButton.Update();
            moveUpButton.Update();
            moveDownButton.Update();

            scrollUpButton.Update();
            scrollDownButton.Update();

            if (selector.InBounds(input.MousePosition()) && input.MouseOnePress())
            {
                SelectorPressed();
            }

            selector.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            layerButton.Draw(spriteBatch);
            deleteButton.Draw(spriteBatch);
            moveUpButton.Draw(spriteBatch);
            moveDownButton.Draw(spriteBatch);

            scrollUpButton.Draw(spriteBatch);
            scrollDownButton.Draw(spriteBatch);

            spriteBatch.DrawString(font, label, labelPosition, bodyColor, 0, Vector2.Zero, scale, SpriteEffects.None, 0);

            selector.Draw(spriteBatch);

            //Border lines
            spriteBatch.Draw(lineTexture, underlineRectangle, bodyColor);
            spriteBatch.Draw(lineTexture, borderTopRectangle, bodyColor);
        }

        public override void DrawTooltip(SpriteBatch spriteBatch)
        {
            layerButton.DrawTooltip(spriteBatch);
            deleteButton.DrawTooltip(spriteBatch);
            moveUpButton.DrawTooltip(spriteBatch);
            moveDownButton.DrawTooltip(spriteBatch);

            scrollUpButton.DrawTooltip(spriteBatch);
            scrollDownButton.DrawTooltip(spriteBatch);
        }

        #endregion
    }
}
