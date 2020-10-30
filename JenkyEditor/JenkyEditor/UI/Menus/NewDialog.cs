using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Jenky.UI;
using Jenky.IO;
using Jenky.Graphics;

namespace JenkyEditor
{
    public class NewDialog : Menu
    {
        #region vars

        private Label nameInputLabel;
        private Label xInputLabel;
        private Label yInputLabel;
        private Label tilePngLabel;
        private Label propPngLabel;

        private TextInput folderTextInput;
        private IntInput xTextInput;
        private IntInput yTextInput;

        private TextButton newButton;
        private TextButton cancelButton;
        private ImageButton tilePngButton;
        private ImageButton propPngButton;

        private DialogManager dialog;

        private Action TilePngPress;
        private Action PropPngPress;

        private WindowHeader header;
        private WindowStretched dialogWindow;

        private int buttonWidth;
        private int buttonHeight;

        private int spacing;
        private int padding;

        private int physicalButtonWidth;
        private int physicalButtonHeight;

        public string TilePng { get; set; }
        public string PropPng { get; set; }

        #endregion

        #region init

        public NewDialog(int positionX, int positionY, int _width, int _height, int scale, Action newPress, Action cancelPress, Texture2D uiTexture, Texture2D lineTexture, SpriteFont font, InputHandler _input, DialogManager _dialog) : base(positionX, positionY, _width, _height, scale, uiTexture, lineTexture, font, _input)
        {
            Active = false;

            input = _input;

            dialog = _dialog;

            TilePngPress = LoadTileTexture;
            PropPngPress = LoadPropTexture;

            spacing = 3 * scale;
            padding = 4 * scale;

            buttonWidth = 64;
            buttonHeight = 16;

            physicalButtonWidth = buttonWidth * scale;
            physicalButtonHeight = buttonHeight * scale;

            header = new WindowHeader(positionX + (5 * scale), positionY - (headerSlices.SliceHeight * scale), width - 10, headerSlices.SliceHeight, scale, "New Project", uiTexture, font, headingColor, headerSlices);

            dialogWindow = new WindowStretched(positionX, positionY, width, height, scale, uiTexture, windowSlices);

            SetupElements(newPress, cancelPress, font);
        }

        private void SetupElements(Action NewPress, Action CancelPress, SpriteFont font)
        {
            int inputWidth = 242;

            int offsetX = (int)position.X + padding;
            int offsetY = (int)position.Y + padding;

            nameInputLabel = new Label(offsetX, offsetY, buttonWidth, buttonHeight, scale, "Name:", lineTexture, font, bodyColor);

            offsetX += nameInputLabel.GetPhysicalWidth() + 1;
            folderTextInput = new TextInput(offsetX, offsetY, inputWidth, buttonHeight, scale, 20, lineTexture, font, bodyColor, input);

            offsetY += physicalButtonHeight + spacing;
            offsetX = (int)position.X + padding;
            xInputLabel = new Label(offsetX, offsetY, buttonWidth, buttonHeight, scale, "Tile Width:", lineTexture, font, bodyColor);

            offsetX += nameInputLabel.GetPhysicalWidth() + 1;
            xTextInput = new IntInput(offsetX, offsetY, inputWidth, buttonHeight, scale, 5, lineTexture, font, bodyColor, input);

            offsetY += physicalButtonHeight + spacing;
            offsetX = (int)position.X + padding;
            yInputLabel = new Label(offsetX, offsetY, buttonWidth, buttonHeight, scale, "Tile Height:", lineTexture, font, bodyColor);

            offsetX += nameInputLabel.GetPhysicalWidth() + 1;
            yTextInput = new IntInput(offsetX, offsetY, inputWidth, buttonHeight, scale, 5, lineTexture, font, bodyColor, input);


            offsetY += physicalButtonHeight + spacing;
            offsetX = (int)position.X + padding;
            tilePngButton = new ImageButton(offsetX, offsetY, buttonWidth, buttonHeight, scale, TilePngPress, uiTexture, lineTexture, font, headingColor, bodyColor, new StillFrame(36, 50, 12, 10), buttonSlices, inactiveSlices, hoverSlices, "Load Tile Source", input);

            offsetX += nameInputLabel.GetPhysicalWidth() + 1;
            tilePngLabel = new Label(offsetX, offsetY, inputWidth, buttonHeight, scale, "...", lineTexture, font, bodyColor);

            offsetY += physicalButtonHeight + spacing;
            offsetX = (int)position.X + padding;
            propPngButton = new ImageButton(offsetX, offsetY, buttonWidth, buttonHeight, scale, PropPngPress, uiTexture, lineTexture, font, headingColor, bodyColor, new StillFrame(36, 50, 12, 10), buttonSlices, inactiveSlices, hoverSlices, "Load Prop Source", input);

            offsetX += nameInputLabel.GetPhysicalWidth() + 1;
            propPngLabel = new Label(offsetX, offsetY, inputWidth, buttonHeight, scale, "...", lineTexture, font, bodyColor);

            offsetY += physicalButtonHeight + spacing;
            offsetX = (int)position.X + padding;
            newButton = new TextButton(offsetX, offsetY, buttonWidth, buttonHeight, scale, NewPress, "Create", uiTexture, font, headingColor, buttonSlices, inactiveSlices, hoverSlices, input);

            offsetX += physicalButtonWidth + spacing;

            cancelButton = new TextButton(offsetX, offsetY, buttonWidth, buttonHeight, scale, CancelPress, "Cancel", uiTexture, font, headingColor, buttonSlices, inactiveSlices, hoverSlices, input);
        }

            #endregion

        #region methods

        public void Reset()
        {
            folderTextInput.Reset();
            yTextInput.Reset();
            xTextInput.Reset();
            tilePngLabel.Text = "...";
            propPngLabel.Text = "...";
        }

        public Tuple<string, int, int> GetNewMap()
        {
            return new Tuple<string, int, int>(folderTextInput.Text, xTextInput.Value, yTextInput.Value);
        }

        public override void Update(GameTime gameTime)
        {
            newButton.Update();
            cancelButton.Update();
            tilePngButton.Update();
            propPngButton.Update();

            folderTextInput.Update(gameTime);
            xTextInput.Update(gameTime);
            yTextInput.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            header.Draw(spriteBatch);
            dialogWindow.Draw(spriteBatch);

            newButton.Draw(spriteBatch);
            cancelButton.Draw(spriteBatch);

            tilePngButton.Draw(spriteBatch);
            propPngButton.Draw(spriteBatch);

            tilePngLabel.Draw(spriteBatch);
            propPngLabel.Draw(spriteBatch);

            nameInputLabel.Draw(spriteBatch);
            folderTextInput.Draw(spriteBatch);

            xInputLabel.Draw(spriteBatch);
            xTextInput.Draw(spriteBatch);

            yInputLabel.Draw(spriteBatch);
            yTextInput.Draw(spriteBatch);
        }

        public override void DrawTooltip(SpriteBatch spriteBatch)
        {
            tilePngButton.DrawTooltip(spriteBatch);
            propPngButton.DrawTooltip(spriteBatch);
        }

        #endregion

        #region events

        private void LoadTileTexture()
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            TilePng = dialog.GetImagePath(documentsPath);
            tilePngLabel.Text = TilePng;
        }

        private void LoadPropTexture()
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            PropPng = dialog.GetImagePath(documentsPath);
            propPngLabel.Text = PropPng;
        }

        #endregion
    }
}