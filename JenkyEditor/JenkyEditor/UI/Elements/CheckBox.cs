using Jenky.IO;
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
    public class CheckBox : UIPanel
    {
        #region vars

        private InputHandler input;
        private Texture2D uiTexture;
        private SpriteFont font;

        private Rectangle trueSource;
        private Rectangle falseSource;
        private Rectangle destination;
        public bool Selected;

        #endregion

        #region init

        public CheckBox(int positionX, int positionY, int _width, int _height, int _scale, Texture2D _uiTexture, SpriteFont _font, InputHandler _input) : base(positionX, positionY, _width, _height, _scale)
        {
            input = _input;
            font = _font;

            Selected = true;

            uiTexture = _uiTexture;

            destination = new Rectangle(positionX, positionY, physicalWidth, physicalHeight);
            trueSource = new Rectangle(84, 20, 12, 10);
            falseSource = new Rectangle(72, 20, 12, 10);
        }

        #endregion

        #region methods

        public void Update()
        {
            var mousePosition = input.MousePosition();

            if (!InBounds(mousePosition) && input.MouseOnePress())
            {
                if (Selected)
                {
                    Selected = false;
                }
                else
                {
                    Selected = true;
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Selected)
            {
                spriteBatch.Draw(uiTexture, destination, trueSource, Color.White);
            }
            else
            {
                spriteBatch.Draw(uiTexture, destination, falseSource, Color.White);
            }
        }

        #endregion
    }
}
