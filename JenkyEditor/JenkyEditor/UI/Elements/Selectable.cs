using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Jenky.Graphics;
using Jenky.UI;
using Jenky.IO;

namespace JenkyEditor
{
    public abstract class Selectable : UIElement
    {
        public int itemID;
        public Rectangle destinationRectangle;

        protected InputHandler input;

        protected Texture2D lineTexture;
        protected SpriteFont font;
        protected Tooltip tooltip;
        protected bool hovering;
        protected int lineThickness;
        protected Color lineColor;

        public Selectable(int positionX, int positionY, int _width, int _height, int _scale, int _itemID, string name, Texture2D _lineTexture, SpriteFont _font, Color backgroundColor, Color _lineColor, InputHandler _input) : base(positionX, positionY, _width, _height, _scale)
        {
            input = _input;

            font = _font;

            lineTexture = _lineTexture;
            lineColor = _lineColor;

            itemID = _itemID;

            tooltip = new Tooltip(positionX, positionY, physicalWidth, physicalHeight, _itemID + ":" + name, scale, lineTexture, font, backgroundColor, lineColor);
            hovering = false;
            lineThickness = 2;
        }


        public void RePosition(Vector2 _position, SpriteFont font)
        {
            position = _position;
            tooltip.RePosition((int)position.X, (int)position.Y, physicalWidth, physicalHeight, font);
            animations.position = position;
        }

        public void Update()
        {
            var mousePosition = input.MousePosition();
            if (hovering)
            {
                if (!InBounds(input.MousePosition()))
                {
                    hovering = false;

                }
            }
            else
            {
                if (InBounds(input.MousePosition()))
                {
                    hovering = true;

                }
            }
        }

        public void DrawTooltip(SpriteBatch spriteBatch, int selectedID)
        {
            if (hovering)
            {
                tooltip.Draw(spriteBatch);
            }
        }
    }
}
