using Jenky.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Jenky.UI
{
    public class Label : UIElement
    {
        #region vars

        public string Text { get; set; }

        private Texture2D lineTexture;
        private Color labelColor;

        private SpriteFont font;

        private Vector2 labelPosition;

        #endregion

        #region init

        public Label(int positionX, int positionY, int _width, int _height, int _scale, string _Text, Texture2D _lineTexture, SpriteFont _font, Color _labelColor) : base(positionX, positionY, _width, _height, _scale)
        {
            lineTexture = _lineTexture;
            labelColor = _labelColor;

            font = _font;

            labelPosition = new Vector2(positionX + scale,positionY + ((height - font.LineSpacing) / 2) * scale);
            Text = _Text;
        }

        #endregion

        #region methods

        public override void Draw(SpriteBatch spriteBatch)
        {
            //drawing bounds
            spriteBatch.Draw(lineTexture, new Rectangle((int)position.X - 1, (int)position.Y - 1, physicalWidth + 2, 1), labelColor);
            spriteBatch.Draw(lineTexture, new Rectangle((int)position.X - 1, (int)position.Y + physicalHeight, physicalWidth + 2, 1), labelColor);
            spriteBatch.Draw(lineTexture, new Rectangle((int)position.X + physicalWidth, (int)position.Y - 1, 1, physicalHeight + 2), labelColor);
            spriteBatch.Draw(lineTexture, new Rectangle((int)position.X - 1, (int)position.Y - 1, 1, physicalHeight + 2), labelColor);

            spriteBatch.DrawString(font, Text, labelPosition, labelColor, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        #endregion
    }
}
