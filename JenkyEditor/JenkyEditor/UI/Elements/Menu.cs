using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Jenky.UI;
using Jenky.Graphics;
using Jenky.IO;

namespace JenkyEditor
{
    public abstract class Menu : UIPanel
    {
        #region vars

        public bool Active { get; protected set; }

        protected NineSlice windowSlices;
        protected NineSlice connectedSlices;
        protected NineSlice buttonSlices;
        protected NineSlice inactiveSlices;
        protected NineSlice hoverSlices;
        
        protected Texture2D uiTexture;
        protected Texture2D lineTexture;

        protected Color headingColor;
        protected Color bodyColor;

        protected ThreeSlice headerSlices;
        protected StillFrame headerFront;

        protected InputHandler input;

        #endregion

        #region init

        public Menu(int positionX, int positionY, int _width, int _height, int _scale, Texture2D _uiTexture, Texture2D _lineTexture, SpriteFont spriteFont, InputHandler _input, bool _Active = true) : base(positionX, positionY, _width, _height, _scale)
        {
            Active = _Active;
            input = _input;
            
            uiTexture = _uiTexture;
            lineTexture = _lineTexture;

            windowSlices = new NineSlice(0, 0, 3, 3);
            connectedSlices = new NineSlice(9, 0, 3, 3);
            buttonSlices = new NineSlice(0, 9, 3, 3);
            inactiveSlices = new NineSlice(9, 9, 3, 3);
            hoverSlices = new NineSlice(18, 9, 3, 3);

            headingColor = new Color(39, 43, 66);
            bodyColor = new Color(159, 230, 115);

            headerSlices = new ThreeSlice(0, 18, 23, 12);
            headerFront = new StillFrame(69, 18, 3, 12);
        }

        #endregion

        #region methods

        public void Activate()
        {
            Active = true;
        }

        public void DeActivate()
        {
            Active = false;
        }

        #endregion

        #region abstract_methods

        public abstract void Update(GameTime gameTime);
        public abstract void DrawTooltip(SpriteBatch spriteBatch);

        #endregion
    }
}
