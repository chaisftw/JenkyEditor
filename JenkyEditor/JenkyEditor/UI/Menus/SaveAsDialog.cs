using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Jenky.UI;
using Jenky.IO;
using Jenky.Graphics;

namespace JenkyEditor
{
    public class SaveAsDialog : Menu
    {
        #region vars

        private Label nameInputLabel;

        private TextInput folderTextInput;

        private TextButton saveButton;
        private TextButton cancelButton;

        private WindowHeader header;
        private WindowStretched dialogWindow;

        private int buttonWidth;
        private int buttonHeight;

        private int spacing;
        private int padding;

        private int physicalButtonWidth;
        private int physicalButtonHeight;

        #endregion

        #region init

        public SaveAsDialog(int positionX, int positionY, int _width, int _height, int scale, Action SavePress, Action CancelPress, Texture2D uiTexture, Texture2D lineTexture, SpriteFont font, InputHandler _input, DialogManager _dialog) : base(positionX, positionY, _width, _height, scale, uiTexture, lineTexture, font, _input)
        {
            Active = false;

            input = _input;

            spacing = 3 * scale;
            padding = 4 * scale;

            buttonWidth = 64;
            buttonHeight = 16;

            physicalButtonWidth = buttonWidth * scale;
            physicalButtonHeight = buttonHeight * scale;

            header = new WindowHeader(positionX + (5 * scale), positionY - (headerSlices.SliceHeight * scale), width - 10, headerSlices.SliceHeight, scale, "Save Project As", uiTexture, font, headingColor, headerSlices);

            dialogWindow = new WindowStretched(positionX, positionY, width, height, scale, uiTexture, windowSlices);

            SetupElements(SavePress, CancelPress, font);
        }

        private void SetupElements(Action SavePress, Action CancelPress, SpriteFont font)
        {
            int inputWidth = 242;

            int offsetX = (int)position.X + padding;
            int offsetY = (int)position.Y + padding;

            nameInputLabel = new Label(offsetX, offsetY, buttonWidth, buttonHeight, scale, "Save As:", lineTexture, font, bodyColor);

            offsetX += nameInputLabel.GetPhysicalWidth() + 1;
            folderTextInput = new TextInput(offsetX, (int)position.Y + padding, inputWidth, buttonHeight, scale, 20, lineTexture, font, bodyColor, input);

            offsetY += physicalButtonHeight + spacing;
            offsetX = (int)position.X + padding;
            saveButton = new TextButton(offsetX, offsetY, buttonWidth, buttonHeight, scale, SavePress, "Save", uiTexture, font, headingColor, buttonSlices, inactiveSlices, hoverSlices, input);

            offsetX += physicalButtonWidth + spacing;

            cancelButton = new TextButton(offsetX, offsetY, buttonWidth, buttonHeight, scale, CancelPress, "Cancel", uiTexture, font, headingColor, buttonSlices, inactiveSlices, hoverSlices, input);
        }

            #endregion

            #region methods

        public void Reset()
        {
            folderTextInput.Reset();
        }

        public string GetNewFolder()
        {
            return folderTextInput.Text;
        }

        public override void Update(GameTime gameTime)
        {
            saveButton.Update();
            cancelButton.Update();

            folderTextInput.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            header.Draw(spriteBatch);
            dialogWindow.Draw(spriteBatch);

            saveButton.Draw(spriteBatch);
            cancelButton.Draw(spriteBatch);

            nameInputLabel.Draw(spriteBatch);
            folderTextInput.Draw(spriteBatch);
        }

        public override void DrawTooltip(SpriteBatch spriteBatch)
        {

        }

        #endregion
    }
}