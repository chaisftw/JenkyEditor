using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Jenky.Graphics;
using Jenky.UI;
using Jenky.IO;

namespace JenkyEditor
{
    public class ItemSelectable : Selectable
    {
        private Texture2D itemTexture;

        public ItemSelectable(int positionX, int positionY, int _width, int _height, int _scale, int itemID, string name, StillFrame icon, Texture2D _itemTexture, Texture2D lineTexture, SpriteFont font, Color backgroundColor, Color lineColor, InputHandler input) :base(positionX, positionY, _width, _height, _scale, itemID, name, lineTexture, font, backgroundColor, lineColor, input)
        {
            itemTexture = _itemTexture;

            icon.Resize(width, height);
            animations.AddSetAnimation(icon, Vector2.Zero);
        }

        public void SetItemTexture(Texture2D newTexture)
        {
            itemTexture = newTexture;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            animations.Draw(spriteBatch, itemTexture);
            if (hovering)
            {
                //drawing selector
                spriteBatch.Draw(lineTexture, new Rectangle((int)position.X - lineThickness, (int)position.Y - lineThickness, physicalWidth + (lineThickness * 2), lineThickness), lineColor);
                spriteBatch.Draw(lineTexture, new Rectangle((int)position.X - lineThickness, (int)position.Y + physicalHeight, physicalWidth + (lineThickness * 2), lineThickness), lineColor);
                spriteBatch.Draw(lineTexture, new Rectangle((int)position.X + physicalWidth, (int)position.Y - lineThickness, lineThickness, physicalHeight + (lineThickness * 2)), lineColor);
                spriteBatch.Draw(lineTexture, new Rectangle((int)position.X - lineThickness, (int)position.Y - lineThickness, lineThickness, physicalHeight + (lineThickness * 2)), lineColor);
            }
        }
    }
}
