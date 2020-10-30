using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Jenky.UI;
using Jenky.Graphics;
using Jenky.IO;
using System.Collections.Generic;



namespace JenkyEditor
{
    public class LayerDialog : Menu
    {
        #region vars

        private int iconWidth;
        private int iconHeight;

        private int buttonWidth;
        private int buttonHeight;

        private int physicalButtonWidth;
        private int physicalButtonHeight;
        
        private LeftWindowHeader header;
        private WindowStretched window;

        private ImageButton addButton;
        private ImageButton closeButton;

        private Label nameInputLabel;

        private TextInput nameInput;

        private int padding;
        private int spacing;

        #endregion 

        #region init

        public LayerDialog(int positionX, int positionY, int _width, int _height, int scale, Action[] buttonEvents,Texture2D uiTexture, Texture2D lineTexture,  SpriteFont font, InputHandler _input) : base(positionX, positionY, _width, _height, scale, uiTexture, lineTexture, font, _input, false)
        {
            buttonWidth = 22;
            buttonHeight = 16;

            iconWidth = 12;
            iconHeight = 10;

            physicalButtonWidth = buttonWidth * scale;
            physicalButtonHeight = buttonHeight * scale;

            spacing = 3 * scale;
            padding = 4 * scale;

            header = new LeftWindowHeader(positionX, positionY - (headerSlices.SliceHeight * scale), width - 65, headerSlices.SliceHeight, scale, "New Layer", uiTexture, font, headingColor, headerFront, headerSlices.Middle, headerSlices.End);

            windowSlices.TopLeft = connectedSlices.TopLeft;
            window = new WindowStretched(positionX, positionY, width, height, scale, uiTexture, windowSlices);
            SetupBar(buttonEvents, font);

            int offsetY = (padding * 2) + physicalButtonHeight;
        }

        private void SetupBar(Action[] buttonEvents, SpriteFont font)
        {

            int offsetX = (int)position.X + padding;
            int offsetY = (int)position.Y + padding;
            int inputSpacing = 6 * scale;

            closeButton = new ImageButton((int)position.X + physicalWidth - physicalButtonWidth - padding, (int)position.Y + padding, buttonWidth, buttonHeight, scale, buttonEvents[23], uiTexture, lineTexture, font, headingColor, bodyColor, new StillFrame(0, 50, iconWidth, iconHeight), buttonSlices, inactiveSlices, hoverSlices, "Close", input);

            addButton = new ImageButton(offsetX, offsetY, buttonWidth, buttonHeight, scale, buttonEvents[22], uiTexture, lineTexture, font, headingColor, bodyColor, new StillFrame(12, 50, iconWidth, iconHeight), buttonSlices, inactiveSlices, hoverSlices, "Add Layer", input);

            offsetX = (int)addButton.position.X + addButton.GetPhysicalWidth() + spacing;
            nameInputLabel = new Label(offsetX, (int)position.Y + padding, 32, buttonHeight, scale, "Name:", lineTexture, font, bodyColor);

            offsetX += nameInputLabel.GetPhysicalWidth() + 1;
            nameInput = new TextInput(offsetX, (int)position.Y + padding, 245, buttonHeight, scale, 20, lineTexture, font, bodyColor, input);
        }

        #endregion

        #region methods

        public void ResetData()
        {
            nameInput.Reset();
        }

        public string GetName()
        {
            return nameInput.Text;
        }

        public override void Update(GameTime gameTime)
        {
            addButton.Update();
            closeButton.Update();

            nameInput.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Draw menu
            window.Draw(spriteBatch);
            header.Draw(spriteBatch);

            //Draw buttons
            addButton.Draw(spriteBatch);
            closeButton.Draw(spriteBatch);
            
            //Draw inputs
            nameInputLabel.Draw(spriteBatch);
            nameInput.Draw(spriteBatch);
        }

        public override void DrawTooltip(SpriteBatch spriteBatch)
        {
            addButton.DrawTooltip(spriteBatch);
            closeButton.DrawTooltip(spriteBatch);
        }

        #endregion
    }
}