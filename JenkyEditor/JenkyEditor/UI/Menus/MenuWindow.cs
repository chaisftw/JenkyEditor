using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Jenky.UI;
using Jenky.Graphics;
using Jenky.IO;

namespace JenkyEditor
{
    public class MenuWindow : Menu
    {
        #region vars

        private WindowHeader header;
        private WindowStretched menuWindow;
        private TextButton newButton;
        private TextButton loadButton;
        private TextButton exitButton;

        #endregion

        #region init

        public MenuWindow(int positionX, int positionY, int _width, int _height, int scale, Action[] buttonEvents, Texture2D uiTexture, Texture2D lineTexture, SpriteFont font, InputHandler _input) : base(positionX, positionY, _width, _height, scale, uiTexture, lineTexture, font, _input)
        {

            if (buttonEvents.Length != 5)
            {
                throw new ArgumentException("Number of button events must equal 5 for the Menu Window");
            }

            input = _input;

            header = new WindowHeader(positionX + (5 * scale), positionY - (headerSlices.SliceHeight * scale), width - 10, headerSlices.SliceHeight, scale, "Jenky Editor", uiTexture, font, headingColor, headerSlices);

            menuWindow = new WindowStretched(positionX, positionY, width, height, scale, uiTexture, windowSlices);

            SetupButtons(buttonEvents, font);
        }

        private void SetupButtons(Action[] buttonEvents, SpriteFont font)
        {
            int alignmentX = (int)position.X + (3 * scale);
            int buttonWidth = width - 6;
            int buttonHeight = 20;
            int offsetY = (int)position.Y + (5 * scale);
            int buttonSpacing = 2;

            newButton = new TextButton(alignmentX, offsetY, buttonWidth, buttonHeight, scale, buttonEvents[0], "New Project", uiTexture, font, headingColor, buttonSlices, inactiveSlices, hoverSlices, input);
            offsetY = offsetY + ((buttonHeight + buttonSpacing) * scale);

            loadButton = new TextButton(alignmentX, offsetY, buttonWidth, buttonHeight, scale, buttonEvents[1], "Load Project", uiTexture, font, headingColor, buttonSlices, inactiveSlices, hoverSlices, input); 
            offsetY = offsetY + ((buttonHeight + buttonSpacing) * scale);

            exitButton = new TextButton(alignmentX, offsetY, buttonWidth, buttonHeight, scale, buttonEvents[2], "Exit", uiTexture, font, headingColor, buttonSlices, inactiveSlices, hoverSlices, input); 
        }

        #endregion 

        #region methods

        public override void Update(GameTime gameTime)
        {
            if (InBounds(input.MousePosition()))
            {
                newButton.Update();
                loadButton.Update();
                exitButton.Update();
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            menuWindow.Draw(spriteBatch);
            newButton.Draw(spriteBatch);
            loadButton.Draw(spriteBatch);
            exitButton.Draw(spriteBatch);
            header.Draw(spriteBatch);
        }

        public override void DrawTooltip(SpriteBatch spriteBatch)
        {

        }

        #endregion 
    }
}
