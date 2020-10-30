using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Jenky.Graphics;
using Jenky.IO;
using Jenky.UI;

namespace JenkyEditor
{
    public abstract class ItemSelectionPanel : Menu
    {
        #region vars

        protected SpriteFont font;

        protected ImageButton deleteButton;
        protected ImageButton itemButton;

        protected ImageButton scrollUpButton;
        protected ImageButton scrollDownButton;

        protected ItemSelectionGrid selector;

        protected int padding;

        protected int buttonWidth;
        protected int buttonHeight;

        protected int physicalButtonWidth;
        protected int physicalButtonHeight;

        protected int spacing;
        
        #endregion 

        #region init
        
        public ItemSelectionPanel(int positionX, int positionY, int _width, int _height, int itemWidth, int itemHeight, int _scale, Action[] buttonEvents, Texture2D uiTexture, Texture2D lineTexture, SpriteFont _font, InputHandler _input) : base(positionX, positionY, _width, _height, _scale, uiTexture, lineTexture, _font, _input)
        {
            font = _font;

            buttonWidth = 22;
            buttonHeight = 16;

            physicalButtonWidth = buttonWidth * scale;
            physicalButtonHeight = buttonHeight * scale;

            padding = 4 * scale;
            spacing = 3 * scale;
        }


        #endregion

        #region abstract_methods

        protected abstract void SetupButtons(Action[] buttonEvents, SpriteFont font, int offsetY);
        
        #endregion

        #region methods

        protected void ScrollUp()
        {
            selector.ScrollUp();
        }

        protected void ScrollDown()
        {
            selector.ScrollDown();
        }

        public void ClearSelector()
        {
            selector.Clear();
        }

        public void SelectorPressed()
        {
            if (selector.InBounds(input.MousePosition()))
            {
                selector.CheckSelection();
            }
        }

        public void Deselect()
        {
            selector.Deselect();
        }

        public int GetSelection()
        {
            return selector.SelectedID;
        }

        public void LoadItem(int itemID, string itemName, StillFrame icon)
        {
            selector.InsertItem(itemID, itemName, icon);
        }

        public void RemoveItem(int itemID)
        {
            selector.RemoveItem(itemID);
        }
        
        public override void Update(GameTime gameTime)
        {
            selector.Update();

            if (selector.InBounds(input.MousePosition()) && input.MouseOnePress())
            {
                SelectorPressed();
            }

            itemButton.Update();
            deleteButton.Update();
            scrollUpButton.Update();
            scrollDownButton.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            itemButton.Draw(spriteBatch);
            deleteButton.Draw(spriteBatch);
            scrollUpButton.Draw(spriteBatch);
            scrollDownButton.Draw(spriteBatch);

            selector.Draw(spriteBatch);
        }

        public override void DrawTooltip(SpriteBatch spriteBatch)
        {
            itemButton.DrawTooltip(spriteBatch);
            deleteButton.DrawTooltip(spriteBatch);

            selector.DrawTooltip(spriteBatch);
        }

        public void DrawDebug(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(lineTexture, new Rectangle((int)position.X - 1, (int)position.Y - 1, physicalWidth + (1 * 2), 1), Color.Red);
            spriteBatch.Draw(lineTexture, new Rectangle((int)position.X - 1, (int)position.Y + physicalHeight, physicalWidth + (1 * 2), 1), Color.Red);
            spriteBatch.Draw(lineTexture, new Rectangle((int)position.X + physicalWidth, (int)position.Y - 1, 1, physicalHeight + (1 * 2)), Color.Red);
            spriteBatch.Draw(lineTexture, new Rectangle((int)position.X - 1, (int)position.Y - 1, 1, physicalHeight + (1 * 2)), Color.Red);
        }

        #endregion 
    }
}
