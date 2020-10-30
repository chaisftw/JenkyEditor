using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jenky.IO;
using Jenky.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JenkyEditor
{
    public abstract class ImageSelectionPanel : ItemSelectionPanel
    {
        protected Texture2D itemTexture;
        protected ImageButton pngButton;

        public ImageSelectionPanel(int positionX, int positionY, int _width, int _height, int itemWidth, int itemHeight, int _scale, Action[] buttonEvents, Texture2D _itemTexture, Texture2D uiTexture, Texture2D lineTexture, SpriteFont _font, InputHandler _input) : base(positionX, positionY, _width, _height, itemWidth, itemHeight, _scale, buttonEvents, uiTexture, lineTexture, _font, _input)
        {
            itemTexture = _itemTexture;
        }

        public void SetSourceTexture(Texture2D newTexture)
        {
            itemTexture = newTexture;
            selector.SetItemTexture(newTexture);
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
            pngButton.Update();
            scrollUpButton.Update();
            scrollDownButton.Update();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            pngButton.Draw(spriteBatch);
        }

        public override void DrawTooltip(SpriteBatch spriteBatch)
        {
            itemButton.DrawTooltip(spriteBatch);
            deleteButton.DrawTooltip(spriteBatch);
            pngButton.DrawTooltip(spriteBatch);

            selector.DrawTooltip(spriteBatch);
        }
    }
}
