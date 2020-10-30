using System;
using Microsoft.Xna.Framework;
using Jenky.Graphics;
using Jenky.IO;
using Microsoft.Xna.Framework.Graphics;

namespace Jenky.UI
{ 
    public abstract class Button : UIElement
    {
        #region vars

        public event Action pressEvent;

        public bool Active { get; private set; }

        protected bool hovering;

        protected InputHandler input;

        protected Texture2D uiTexture;

        protected NineSlice buttonSlice;
        protected NineSlice inactiveSlice;
        protected NineSlice hoverSlice;

        #endregion

        #region init

        public Button(int positionX, int positionY, int _width, int _height, int _scale, Action _pressEvent, Texture2D _uiTexture, NineSlice _buttonSlice, NineSlice _inactiveSlice, NineSlice _hoverSlice, InputHandler _input) : base(positionX, positionY, _width, _height, _scale)
        {
            input = _input;
            uiTexture = _uiTexture;
            buttonSlice = _buttonSlice;
            inactiveSlice = _inactiveSlice;
            hoverSlice = _hoverSlice;
            pressEvent = _pressEvent;
            hovering = false;
            Active = true;
        }

        #endregion

        #region methods

        public void Update()
        {
            var mousePosition = input.MousePosition();
            if (hovering)
            {
                if (!InBounds(mousePosition))
                {
                    ClearAnimation();
                    StretchNineSlice(buttonSlice);
                    hovering = false;
                }
                else if (input.MouseOnePress())
                {
                    pressEvent();
                }
            }
            else
            {
                if (InBounds(mousePosition))
                {
                    ClearAnimation();
                    StretchNineSlice(hoverSlice);
                    hovering = true;
                }
            }
        }

        public void ButtonPressed()
        {
            if (hovering)
            {
                pressEvent();
            }
        }

        public void Activate()
        {
            ClearAnimation();
            StretchNineSlice(buttonSlice);
            Active = true;
        }

        public void DeActivate()
        {
            ClearAnimation();
            StretchNineSlice(inactiveSlice);
            Active = false;
        }

        #endregion
    }
}
