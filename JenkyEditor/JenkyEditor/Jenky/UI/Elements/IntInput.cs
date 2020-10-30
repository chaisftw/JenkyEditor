using Jenky.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Jenky.UI
{
    public class IntInput : UIElement
    {
        #region vars
        private string Text { get; set; }
        public int Value { get; private set; }
        public bool Focused { get; set; }
        private bool negative;
        private bool clampRange;
        
        private SpriteFont font;
        private Texture2D lineTexture;
        
        private Color inputColor;

        private int maxChars;
        private int maxInt;
        private int minInt;

        private Vector2 labelPosition;

        private Keys[] keysToCheck;

        private InputHandler input;

        #endregion

        #region init

        public IntInput(int positionX, int positionY, int _width, int _height, int _scale, int _maxChars, Texture2D _lineTexture, SpriteFont _font, Color _inputColor, InputHandler _input, bool _negative = false) : base(positionX, positionY, _width, _height, _scale)
        {
            input = _input;
            negative = _negative;

            lineTexture = _lineTexture;
            font = _font;
            
            inputColor = _inputColor;

            labelPosition = new Vector2(positionX + scale,positionY + ((height - font.LineSpacing) / 2) * scale);
            maxChars = _maxChars;

            maxInt = 0;
            minInt = 0;

            Focused = false;

            Value = 0;
            Text = "0";
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

                Keys.Back, Keys.OemMinus
            };
        }

        #endregion

        #region methods

        public void SetRange(int _minInt, int _maxInt)
        {
            minInt = _minInt;
            maxInt = _maxInt;
            clampRange = true;
        }

        public void SetInt(int _Value)
        {
            Value = _Value;
            Text = Value.ToString();
        }

        public void Reset()
        {
            Text = "0";
            Value = 0;
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
                case Keys.OemMinus:
                    if (negative == true && Text.Length == 0)
                    {
                        newChar += "-";
                    }
                    break;
                case Keys.Back:
                    if (Text.Length != 0)
                    { 
                        Text = Text.Remove(Text.Length - 1);
                    }

                    if (Text == "" || Text == "-")
                    {
                        Value = 0;
                    }
                    else
                    {
                        Value = int.Parse(Text);
                    }

                    return;
            }

            if (input.IsKeyDown(Keys.RightShift) || input.IsKeyDown(Keys.LeftShift))
            {
                newChar = newChar.ToUpper();
            }

            Text += newChar;

            if (clampRange && Text != "" && Text != "-")
            {
                if (int.Parse(Text) > maxInt)
                {
                    Text = maxInt.ToString();
                }
                else if (int.Parse(Text) < minInt)
                {
                    Text = minInt.ToString();
                }
            }

            if (Text == ""|| Text == "-")
            {
                Value = 0;
            }
            else
            {
                Value = int.Parse(Text);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (input.MouseOnePress())
            {
                if (InBounds(input.MousePosition()))
                {
                    Focused = true;
                    if (Text == "0")
                    {
                        Text = "";
                    }
                }
                else
                {
                    Focused = false;
                    if (Text == "" || Text == "-")
                    {
                        Text = "0";
                    }
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

        public override void Draw(SpriteBatch spriteBatch)
        {
            //drawing bounds
            spriteBatch.Draw(lineTexture, new Rectangle((int)position.X - 1, (int)position.Y - 1, physicalWidth + 2, 1), inputColor);
            spriteBatch.Draw(lineTexture, new Rectangle((int)position.X - 1, (int)position.Y + physicalHeight, physicalWidth + 2, 1), inputColor);
            spriteBatch.Draw(lineTexture, new Rectangle((int)position.X + physicalWidth, (int)position.Y - 1, 1, physicalHeight + 2), inputColor);
            spriteBatch.Draw(lineTexture, new Rectangle((int)position.X - 1, (int)position.Y - 1, 1, physicalHeight + 2), inputColor);

            if (Focused && Text.Length < maxChars)
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
