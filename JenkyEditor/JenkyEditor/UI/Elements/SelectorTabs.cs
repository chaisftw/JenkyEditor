using Jenky.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenkyEditor
{
    public class SelectorTabs : UIElement
    {
        #region vars

        public bool tileMode;

        private string tileLabel;
        private string propLabel;

        private Vector2 tileLabelPosition;
        private Vector2 propLabelPosition;

        private Rectangle tileBounds;
        private Rectangle propBounds;

        private Color color;
        private SpriteFont font;

        private Texture2D lineTexture;

        #endregion

        #region init

        public SelectorTabs(int positionX, int positionY, int width, int height, int scale, Texture2D _lineTexture, Color _color, SpriteFont _font) : base(positionX, positionY, width, height, scale)
        {
            tileMode = true;

            font = _font;
            color = _color;
            lineTexture = _lineTexture;

            tileLabel = "Tiles";
            propLabel = "Props";

            int halfWidth = physicalWidth / 2;

            tileBounds = new Rectangle(positionX, positionY, halfWidth, physicalHeight);
            propBounds = new Rectangle(positionX + halfWidth, positionY, halfWidth, physicalHeight);

            Vector2 tileLabelBounds = font.MeasureString(tileLabel) * scale;
            Vector2 propLabelBounds = font.MeasureString(tileLabel) * scale;

            tileLabelPosition = new Vector2(tileBounds.X + (tileBounds.Width / 2) - (tileLabelBounds.X / 2), positionY + (tileBounds.Height / 2) - (tileLabelBounds.Y / 2));
            propLabelPosition = new Vector2(propBounds.X + (propBounds.Width / 2) - (propLabelBounds.X / 2), positionY + (propBounds.Height / 2) - (propLabelBounds.Y / 2));
        }

        #endregion

        #region methods

        public void ChangeTab(Vector2 mousePosition)
        {
            if (mousePosition.X >= tileBounds.X && mousePosition.X <= (tileBounds.X + tileBounds.Width)) // Within X
            {
                if (mousePosition.Y >= tileBounds.Y && mousePosition.Y <= (tileBounds.Y + tileBounds.Height)) // Within Y
                {
                    tileMode = true;
                }

            }
            else
            {
                tileMode = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(lineTexture, new Rectangle((int)position.X, (int)position.Y, physicalWidth, 2), color);
            spriteBatch.Draw(lineTexture, new Rectangle(tileBounds.X, (int)position.Y, 2, physicalHeight), color);
            spriteBatch.Draw(lineTexture, new Rectangle(tileBounds.X + tileBounds.Width - 1, (int)position.Y, 2, physicalHeight), color);

            spriteBatch.Draw(lineTexture, new Rectangle((int)position.X, (int)position.Y, physicalWidth, 2), color);
            spriteBatch.Draw(lineTexture, new Rectangle(propBounds.X + propBounds.Width - 2, (int)position.Y, 2, physicalHeight), color);

            if (tileMode)
            {
                spriteBatch.Draw(lineTexture, new Rectangle(propBounds.X, (int)position.Y + physicalHeight - 2, propBounds.Width, 2), color);
            }
            else
            {
                spriteBatch.Draw(lineTexture, new Rectangle(tileBounds.X, (int)position.Y + physicalHeight - 2, tileBounds.Width, 2), color);
            }

            spriteBatch.DrawString(font, tileLabel, tileLabelPosition, color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            spriteBatch.DrawString(font, propLabel, propLabelPosition, color, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
        }

        #endregion
    }
}
