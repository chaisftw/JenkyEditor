using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jenky.Graphics;
using Jenky.IO;
using Jenky.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JenkyEditor
{
    public class MarkerPanel : ColorSelectionPanel
    {
        #region vars

        private Vector2 labelPosition;

        private Rectangle underlineRectangle;
        private Rectangle borderTopRectangle;
        
        private string label;

        #endregion

        #region init

        public MarkerPanel(int positionX, int positionY, int _width, int _height, int itemWidth, int itemHeight, int _scale, Action[] buttonEvents, Texture2D uiTexture, Texture2D lineTexture, SpriteFont _font, InputHandler _input) : base(positionX, positionY, _width, _height, itemWidth, itemHeight, _scale, buttonEvents, uiTexture, lineTexture, _font, _input)
        {
            label = "Markers";

            borderTopRectangle = new Rectangle((int)position.X, (int)position.Y, GetPhysicalWidth(), 2);

            labelPosition = position + new Vector2(padding, padding);

            Vector2 textDimensions = font.MeasureString(label);

            textDimensions = textDimensions * scale;

            int offsetY = padding;

            offsetY += (int)textDimensions.Y + scale;

            underlineRectangle = new Rectangle((int)position.X, (int)position.Y + offsetY, GetPhysicalWidth(), 2);

            offsetY += underlineRectangle.Height + padding;

            int buttonOffset = offsetY;

            offsetY += physicalButtonHeight + padding;
            int selectorHeight = height - ((((padding + physicalButtonHeight) * 2) / scale) + (2 * 2) + ((int)textDimensions.Y));

            selector = new ColorSelectionGrid(positionX + padding, positionY + offsetY, width - (2 * (padding / scale)), selectorHeight, scale, 16, 16, 7, 4, lineTexture, font, headingColor, bodyColor, input);

            SetupButtons(buttonEvents, font, buttonOffset);
        }

        protected override void SetupButtons(Action[] buttonEvents, SpriteFont font, int offsetY)
        {
            int offsetX = padding;
            int iconWidth = 12;
            int iconHeight = 10;

            itemButton = new ImageButton((int)position.X + offsetX, (int)position.Y + offsetY, buttonWidth, buttonHeight, scale, buttonEvents[15], uiTexture, lineTexture, font, headingColor, bodyColor, new StillFrame(84, 30, iconWidth, iconHeight), buttonSlices, inactiveSlices, hoverSlices, "Add Markers", input);

            offsetX += physicalButtonWidth + spacing;
            deleteButton = new ImageButton((int)position.X + offsetX, (int)position.Y + offsetY, buttonWidth, buttonHeight, scale, buttonEvents[16], uiTexture, lineTexture, font, headingColor, bodyColor, new StillFrame(24, 50, iconWidth, iconHeight), buttonSlices, inactiveSlices, hoverSlices, "Delete Prop", input);

            offsetX = physicalWidth - padding - physicalButtonWidth;

            offsetY = (int)selector.position.Y + selector.GetPhysicalHeight() + padding;

            scrollUpButton = new ImageButton((int)position.X + offsetX, offsetY, buttonWidth, buttonHeight, scale, ScrollUp, uiTexture, lineTexture, font, headingColor, bodyColor, new StillFrame(48, 50, iconWidth, iconHeight), buttonSlices, inactiveSlices, hoverSlices, "Scroll Up", input);

            offsetX -= physicalButtonWidth + spacing;

            scrollDownButton = new ImageButton((int)position.X + offsetX, offsetY, buttonWidth, buttonHeight, scale, ScrollDown, uiTexture, lineTexture, font, headingColor, bodyColor, new StillFrame(60, 50, iconWidth, iconHeight), buttonSlices, inactiveSlices, hoverSlices, "Scroll Down", input);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.DrawString(font, label, labelPosition, bodyColor, 0, Vector2.Zero, scale, SpriteEffects.None, 0);

            //Border lines
            spriteBatch.Draw(lineTexture, underlineRectangle, bodyColor);
            spriteBatch.Draw(lineTexture, borderTopRectangle, bodyColor);
        }

        #endregion
    }
}
