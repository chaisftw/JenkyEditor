using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Jenky.Graphics;
using Jenky.IO;
using Jenky.UI;

namespace JenkyEditor
{
    public class PropPanel : ImageSelectionPanel
    {
        #region init
        
        public PropPanel(int positionX, int positionY, int _width, int _height, int itemWidth, int itemHeight, int _scale, Action[] buttonEvents, Texture2D _itemTexture, Texture2D uiTexture, Texture2D lineTexture, SpriteFont _font, InputHandler _input) : base(positionX, positionY, _width, _height, itemWidth, itemHeight, _scale, buttonEvents, _itemTexture, uiTexture, lineTexture, _font, _input)
        {
            int offsetY = padding + physicalButtonHeight + padding;

            selector = new ItemSelectionGrid(positionX + padding, positionY + offsetY, width - (2 * (padding / scale)), height - ((offsetY * 2) / scale), scale, 16, 16, 7, 4, itemTexture, lineTexture, font, headingColor, bodyColor, input);

            SetupButtons(buttonEvents, font, padding);
        }

        protected override void SetupButtons(Action[] buttonEvents, SpriteFont font, int offsetY)
        {
            int offsetX = padding;
            int iconWidth = 12;
            int iconHeight = 10;
            
            itemButton = new ImageButton((int)position.X + offsetX, (int)position.Y + offsetY, buttonWidth, buttonHeight, scale, buttonEvents[11], uiTexture, lineTexture, font, headingColor, bodyColor, new StillFrame(12, 40, iconWidth, iconHeight), buttonSlices, inactiveSlices, hoverSlices, "Add Props", input);

            offsetX += physicalButtonWidth + spacing;
            deleteButton = new ImageButton((int)position.X + offsetX, (int)position.Y + offsetY, buttonWidth, buttonHeight, scale, buttonEvents[12], uiTexture, lineTexture, font, headingColor, bodyColor, new StillFrame(24, 50, iconWidth, iconHeight), buttonSlices, inactiveSlices, hoverSlices, "Delete Prop", input);

            offsetX = physicalWidth - padding - physicalButtonWidth;

            pngButton = new ImageButton((int)position.X + offsetX, (int)position.Y + offsetY, buttonWidth, buttonHeight, scale, buttonEvents[13], uiTexture, lineTexture, font, headingColor, bodyColor, new StillFrame(36, 50, iconWidth, iconHeight), buttonSlices, inactiveSlices, hoverSlices, "Load Prop Source", input);

            offsetY = (int)selector.position.Y + selector.GetPhysicalHeight() + padding;

            scrollUpButton = new ImageButton((int)position.X + offsetX, offsetY, buttonWidth, buttonHeight, scale, ScrollUp, uiTexture, lineTexture, font, headingColor, bodyColor, new StillFrame(48, 50, iconWidth, iconHeight), buttonSlices, inactiveSlices, hoverSlices, "Scroll Up", input);

            offsetX -= physicalButtonWidth + spacing;

            scrollDownButton = new ImageButton((int)position.X + offsetX, offsetY, buttonWidth, buttonHeight, scale, ScrollDown, uiTexture, lineTexture, font, headingColor, bodyColor, new StillFrame(60, 50, iconWidth, iconHeight), buttonSlices, inactiveSlices, hoverSlices, "Scroll Down", input);
        }

        #endregion 
    }
}
