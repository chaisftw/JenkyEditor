using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Jenky.Graphics;
using Jenky.UI;
using Jenky.IO;

namespace JenkyEditor
{
    public class ColorSelectable : Selectable
    {
        private Color itemColor;
        private StillFrame frame;

        public ColorSelectable(int positionX, int positionY, int _width, int _height, int _scale, int itemID, string name, Texture2D lineTexture, SpriteFont font, Color backgroundColor, Color lineColor, Color _itemColor, InputHandler input) :base(positionX, positionY, _width, _height, _scale, itemID, name, lineTexture, font, backgroundColor, lineColor, input)
        {
            itemColor = _itemColor;
            frame = new StillFrame(0, 0, 1, 1);
            frame.Resize(physicalWidth, physicalHeight);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(lineTexture, new Rectangle((int)position.X, (int)position.Y, frame.DestinationRectangle.Width, frame.DestinationRectangle.Height), frame.SourceRectangle, itemColor);

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
