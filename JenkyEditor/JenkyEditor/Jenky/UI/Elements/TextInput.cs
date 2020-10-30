using Jenky.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Jenky.UI
{
    public class TextInput : UIElement
    {
        #region vars

        public string Text { get; private set; }
        public bool Focused { get; set; }

        private Texture2D lineTexture;
        SpriteFont font;

        private Color inputColor;
        private int maxChars;
        private Vector2 labelPosition;

        private Keys[] keysToCheck;

        private InputHandler input;

        #endregion

        #region init

        public TextInput(int positionX, int positionY, int _width, int _height, int _scale, int _maxChars, Texture2D _lineTexture, SpriteFont _font, Color _inputColor, InputHandler _input) : base(positionX, positionY, _width, _height, _scale)
        {
            font = _font;

            labelPosition = new Vector2(positionX + scale,positionY + ((height - font.LineSpacing) / 2) * scale);

            lineTexture = _lineTexture;

            maxChars = _maxChars;
            input = _input;
            Focused = false;
            inputColor = _inputColor;
            Text = "";
            SetKeys();
        }

        private void SetKeys()
        {
            keysToCheck = new Keys[]
            {
                Keys.D0, Keys.D1, Keys.D2,Keys.D3,
                Keys.D4, Keys.D5, Keys.D6,Keys.D7,
                Keys.D8, Keys.D9,

                Keys.NumPad0, Keys.NumPad1, Keys.NumPad2,Keys.NumPad3,
                Keys.NumPad4, Keys.NumPad5, Keys.NumPad6,Keys.NumPad7,
                Keys.NumPad8, Keys.NumPad9,

                Keys.A, Keys.B, Keys.C, Keys.D, Keys.E,
                Keys.F, Keys.G, Keys.H, Keys.I, Keys.J,
                Keys.K, Keys.L, Keys.M, Keys.N, Keys.O,
                Keys.P, Keys.Q, Keys.R, Keys.S, Keys.T,
                Keys.U, Keys.V, Keys.W, Keys.X, Keys.Y,
                Keys.Z, Keys.Back, Keys.Space
            };
        }

        #endregion

        #region methods

        public void Update(GameTime gameTime)
        {
            if (input.MouseOnePress())
            {
                if (InBounds(input.MousePosition()))
                {
                    Focused = true;
                }
                else
                {
                    Focused = false;
                }
            }

            if (Focused)
            {
                foreach (Keys key in keysToCheck)
                {
                    if (input.OnPress(key))
                    {
                        AddKeyToText(key);
                        break;
                    }
                }
            }
        }

        public void Reset()
        {
            Text = "";
        }

        private void AddKeyToText(Keys key)
        {
            string newChar = "";

            if (Text.Length >= maxChars && key != Keys.Back)
            {
                return;
            }

            //Alphabet
            switch (key)
            {
                case Keys.A:
                    newChar += "a";
                    break;
                case Keys.B:
                    newChar += "b";
                    break;
                case Keys.C:
                    newChar += "c";
                    break;
                case Keys.D:
                    newChar += "d";
                    break;
                case Keys.E:
                    newChar += "e";
                    break;
                case Keys.F:
                    newChar += "f";
                    break;
                case Keys.G:
                    newChar += "g";
                    break;
                case Keys.H:
                    newChar += "h";
                    break;
                case Keys.I:
                    newChar += "i";
                    break;
                case Keys.J:
                    newChar += "j";
                    break;
                case Keys.K:
                    newChar += "k";
                    break;
                case Keys.L:
                    newChar += "l";
                    break;
                case Keys.M:
                    newChar += "m";
                    break;
                case Keys.N:
                    newChar += "n";
                    break;
                case Keys.O:
                    newChar += "o";
                    break;
                case Keys.P:
                    newChar += "p";
                    break;
                case Keys.Q:
                    newChar += "q";
                    break;
                case Keys.R:
                    newChar += "r";
                    break;
                case Keys.S:
                    newChar += "s";
                    break;
                case Keys.T:
                    newChar += "t";
                    break;
                case Keys.U:
                    newChar += "u";
                    break;
                case Keys.V:
                    newChar += "v";
                    break;
                case Keys.W:
                    newChar += "w";
                    break;
                case Keys.X:
                    newChar += "x";
                    break;
                case Keys.Y:
                    newChar += "y";
                    break;
                case Keys.Z:
                    newChar += "z";
                    break;

                //Numbers
                case Keys.D0:
                    newChar += "0";
                    break;
                case Keys.NumPad0:
                    newChar += "0";
                    break;
                case Keys.D1:
                    newChar += "1";
                    break;
                case Keys.NumPad1:
                    newChar += "1";
                    break;
                case Keys.D2:
                    newChar += "2";
                    break;
                case Keys.NumPad2:
                    newChar += "2";
                    break;
                case Keys.D3:
                    newChar += "3";
                    break;
                case Keys.NumPad3:
                    newChar += "3";
                    break;
                case Keys.D4:
                    newChar += "4";
                    break;
                case Keys.NumPad4:
                    newChar += "4";
                    break;
                case Keys.D5:
                    newChar += "5";
                    break;
                case Keys.NumPad5:
                    newChar += "5";
                    break;
                case Keys.D6:
                    newChar += "6";
                    break;
                case Keys.NumPad6:
                    newChar += "6";
                    break;
                case Keys.D7:
                    newChar += "7";
                    break;
                case Keys.NumPad7:
                    newChar += "7";
                    break;
                case Keys.D8:
                    newChar += "8";
                    break;
                case Keys.NumPad8:
                    newChar += "8";
                    break;
                case Keys.D9:
                    newChar += "9";
                    break;
                case Keys.NumPad9:
                    newChar += "9";
                    break;
                case Keys.Space:
                    newChar += " ";
                    break;
                case Keys.Back:
                    if (Text.Length != 0)
                        Text = Text.Remove(Text.Length - 1);
                    return;
            }
            if (input.IsKeyDown(Keys.RightShift) || input.IsKeyDown(Keys.LeftShift))
            {
                newChar = newChar.ToUpper();
            }


            Text += newChar;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //drawing bounds
            spriteBatch.Draw(lineTexture, new Rectangle((int)position.X - 1, (int)position.Y - 1, physicalWidth + 2, 1), inputColor);
            spriteBatch.Draw(lineTexture, new Rectangle((int)position.X - 1, (int)position.Y + physicalHeight, physicalWidth + 2, 1), inputColor);
            spriteBatch.Draw(lineTexture, new Rectangle((int)position.X + physicalWidth, (int)position.Y - 1, 1, physicalHeight + 2), inputColor);
            spriteBatch.Draw(lineTexture, new Rectangle((int)position.X - 1, (int)position.Y - 1, 1, physicalHeight + 2), inputColor);
        
            if (Focused && Text.Length < maxChars )
            {
                spriteBatch.DrawString(font, Text + "_", labelPosition, inputColor, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            }
            else
            {
                spriteBatch.DrawString(font, Text, labelPosition, inputColor, 0, Vector2.Zero, scale, SpriteEffects.None, 0);
            }
        }

        #endregion
    }
}
