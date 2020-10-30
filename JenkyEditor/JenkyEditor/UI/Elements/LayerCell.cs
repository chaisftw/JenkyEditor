using Jenky.IO;
using Jenky.UI;
using Jenky.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenkyEditor
{
    public class LayerCell : UIElement
    {
        #region vars

        private InputHandler input;

        private string name;
        private bool hidden;
        
        private Texture2D lineTexture;

        private SpriteFont font;

        private bool hovering;

        private Color lineColor;

        private Vector2 labelPosition;
        
        private HideIcon hideIcon;

        private Action<int> HideLayer;
        private Action<int> ShowLayer;

        #endregion

        #region init

        public LayerCell(int positionX, int positionY, int _width, int _height, int _scale, string _name, Action<int> _HideLayer, Action<int> _ShowLayer, Texture2D uiTexture, Texture2D _lineTexture, SpriteFont _font, Color _lineColor, InputHandler _input) : base(positionX, positionY, _width, _height, _scale)
        {
            input = _input;
            
            lineTexture = _lineTexture;
            lineColor = _lineColor;

            font = _font;

            labelPosition = new Vector2(positionX + (scale * 2), positionY + ((height - font.LineSpacing) / 2) * scale);
            name = _name;

            hidden = false;

            hideIcon = new HideIcon(positionX + physicalWidth - (16 * scale), positionY, 16, height, scale, uiTexture);

            HideLayer = _HideLayer;
            ShowLayer = _ShowLayer;
        }

        #endregion

        #region methods

        public bool CheckSelection(Vector2 mousePosition, int depth)
        {
            if (InBounds(mousePosition))
            {
                if (hideIcon.InBounds(mousePosition))
                {
                    if (hidden)
                    {
                        hideIcon.Show(depth);
                        ShowLayer(depth);
                        hidden = false;
                    }
                    else
                    {
                        hideIcon.Hide(depth);
                        HideLayer(depth);
                        hidden = true;
                    }
                    return false;
                }
                return true;
            }
            return false;
        }

        public void RePosition(Vector2 _position)
        {
            labelPosition -= position;
            var iconPosition = hideIcon.position - position;

            position = _position;

            labelPosition += position;
            iconPosition += position;

            hideIcon.Reposition(iconPosition);
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            //drawing bounds
            spriteBatch.Draw(lineTexture, new Rectangle((int)position.X - 1, (int)position.Y - 1, physicalWidth + 2, 1), lineColor);
            spriteBatch.Draw(lineTexture, new Rectangle((int)position.X - 1, (int)position.Y + physicalHeight, physicalWidth + 1, 1), lineColor);
            spriteBatch.Draw(lineTexture, new Rectangle((int)position.X + physicalWidth, (int)position.Y - 1, 1, physicalHeight + 2), lineColor);
            spriteBatch.Draw(lineTexture, new Rectangle((int)position.X - 1, (int)position.Y - 1, 1, physicalHeight + 2), lineColor);

            spriteBatch.Draw(lineTexture, new Rectangle((int)hideIcon.position.X - 1, (int)position.Y - 1, 1, physicalHeight + 2), lineColor);

            spriteBatch.DrawString(font, name, labelPosition, lineColor, 0, Vector2.Zero, scale, SpriteEffects.None, 0);

            hideIcon.Draw(spriteBatch);
        }

        #endregion
    }
}
