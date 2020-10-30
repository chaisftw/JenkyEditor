using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Jenky.UI;
using Jenky.Graphics;
using Jenky.IO;
using System.Collections.Generic;

namespace JenkyEditor
{
    public class MarkerDialog : Menu
    {
        #region vars

        private int buttonWidth;
        private int buttonHeight;

        private int iconWidth;
        private int iconHeight;

        private int spacing;

        private int physicalButtonWidth;
        private int physicalButtonHeight;

        private LeftWindowHeader header;
        private WindowStretched window;

        private ImageButton addButton;
        private ImageButton closeButton;

        private Label nameInputLabel;
        private Label idInputLabel;
        private Label redInputLabel;
        private Label greenInputLabel;
        private Label blueInputLabel;

        private TextInput nameInput;
        private IntInput idInput;
        private IntInput redInput;
        private IntInput greenInput;
        private IntInput blueInput;
        
        private int padding;

        #endregion 

        #region init

        public MarkerDialog(int positionX, int positionY, int _width, int _height, int _scale, Action[] buttonEvents, Texture2D uiTexture, Texture2D lineTexture, SpriteFont font, InputHandler _input) : base(positionX, positionY, _width, _height, _scale, uiTexture, lineTexture, font, _input, false)
        {

            buttonWidth = 22;
            buttonHeight = 16;

            iconWidth = 12;
            iconHeight = 10;

            spacing = 3 * scale;

            physicalButtonWidth = buttonWidth * scale;
            physicalButtonHeight = buttonHeight * scale;

            padding = 4 * scale;

            header = new LeftWindowHeader(positionX, positionY - (headerSlices.SliceHeight * scale), width - 65, headerSlices.SliceHeight, scale, "Add Marker", uiTexture, font, headingColor, headerFront, headerSlices.Middle, headerSlices.End);

            windowSlices.TopLeft = connectedSlices.TopLeft;
            window = new WindowStretched(positionX, positionY, width, height, scale, uiTexture, windowSlices);


            SetupBar(buttonEvents, font);
        }

        private void SetupBar(Action[] buttonEvents, SpriteFont font)
        {
            closeButton = new ImageButton((int)position.X + physicalWidth - physicalButtonWidth - padding, (int)position.Y + padding, buttonWidth, buttonHeight, scale, buttonEvents[23], uiTexture, lineTexture, font, headingColor, bodyColor, new StillFrame(0, 50, iconWidth, iconHeight), buttonSlices, inactiveSlices, hoverSlices, "Close", input);

            int offsetX = (int)position.X + padding;
            int offsetY = (int)position.Y + padding;
            int inputSpacing = 5 * scale;

            addButton = new ImageButton(offsetX, offsetY, buttonWidth, buttonHeight, scale, buttonEvents[17], uiTexture, lineTexture, font, headingColor, bodyColor, new StillFrame(12, 50, iconWidth, iconHeight), buttonSlices, inactiveSlices, hoverSlices, "Add Marker", input);

            offsetX = (int)addButton.position.X + addButton.GetPhysicalWidth() + spacing;

            idInputLabel = new Label(offsetX, offsetY, 14, buttonHeight, scale, "ID:", lineTexture, font, bodyColor);

            offsetX += idInputLabel.GetPhysicalWidth() + 1;
            idInput = new IntInput(offsetX, offsetY, 32, buttonHeight, scale, 5, lineTexture, font, bodyColor, input);

            offsetX += idInput.GetPhysicalWidth() + (5 * scale);
            nameInputLabel = new Label(offsetX, offsetY, 32, buttonHeight, scale, "Name:", lineTexture, font, bodyColor);

            offsetX += nameInputLabel.GetPhysicalWidth() + 1;
            nameInput = new TextInput(offsetX, offsetY, 245, buttonHeight, scale, 20, lineTexture, font, bodyColor, input);

            offsetY += physicalButtonHeight + spacing;
            offsetX = (int)addButton.position.X + addButton.GetPhysicalWidth() + spacing;
            redInputLabel = new Label(offsetX, offsetY, 34, buttonHeight, scale, "Red:", lineTexture, font, bodyColor);

            offsetX += redInputLabel.GetPhysicalWidth() + 1;
            redInput = new IntInput(offsetX, offsetY, 32, buttonHeight, scale, 5, lineTexture, font, bodyColor, input);
            redInput.SetRange(0, 255);

            offsetX += redInput.GetPhysicalWidth() + inputSpacing;
            greenInputLabel = new Label(offsetX, offsetY, 34, buttonHeight, scale, "Green:", lineTexture, font, bodyColor);

            offsetX += greenInputLabel.GetPhysicalWidth() + 1;
            greenInput = new IntInput(offsetX, offsetY, 32, buttonHeight, scale, 5, lineTexture, font, bodyColor, input);
            greenInput.SetRange(0, 255);

            offsetX += greenInput.GetPhysicalWidth() + inputSpacing;
            blueInputLabel = new Label(offsetX, offsetY, 34, buttonHeight, scale, "Blue:", lineTexture, font, bodyColor);

            offsetX += blueInputLabel.GetPhysicalWidth() + 1;
            blueInput = new IntInput(offsetX, offsetY, 32, buttonHeight, scale, 5, lineTexture, font, bodyColor, input);
            blueInput.SetRange(0, 255);
        }

        #endregion

        #region methods


        public MarkerSource GetMarkerSource()
        {
            return new MarkerSource(nameInput.Text, new Color(redInput.Value, greenInput.Value, blueInput.Value));
        }

        public int GetID()
        {
            return idInput.Value;
        }

        public void ResetData()
        {
            idInput.Reset();
            nameInput.Reset();

            redInput.Reset();
            greenInput.Reset();
            blueInput.Reset();
        }

        public void SetID(int value)
        {
            idInput.SetInt(value);
        }

        public override void Update(GameTime gameTime)
        {
            addButton.Update();
            closeButton.Update();

            idInput.Update(gameTime);
            nameInput.Update(gameTime);
            redInput.Update(gameTime);
            greenInput.Update(gameTime);
            blueInput.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //Draw menu
            window.Draw(spriteBatch);
            header.Draw(spriteBatch);

            //Draw buttons
            addButton.Draw(spriteBatch);
            closeButton.Draw(spriteBatch);

            //Drawing text input
            idInputLabel.Draw(spriteBatch);
            idInput.Draw(spriteBatch);

            nameInputLabel.Draw(spriteBatch);
            nameInput.Draw(spriteBatch);

            redInputLabel.Draw(spriteBatch);
            redInput.Draw(spriteBatch);

            greenInputLabel.Draw(spriteBatch);
            greenInput.Draw(spriteBatch);

            blueInputLabel.Draw(spriteBatch);
            blueInput.Draw(spriteBatch);
        }

        public override void DrawTooltip(SpriteBatch spriteBatch)
        {
            addButton.DrawTooltip(spriteBatch);
            closeButton.DrawTooltip(spriteBatch);
        }

        #endregion 
    }
}
