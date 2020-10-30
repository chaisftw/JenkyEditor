using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jenky.Geo
{
    public class Line
    {
        #region vars

        public Vector2 PointA { get; private set; }
        public Vector2 PointB { get; private set; }
        public int Thickness { get; private set; }

        #endregion

        #region init

        public Line(Vector2 _PointA, Vector2 _PointB, int _Thickness = 1)
        {
            PointA = _PointA;
            PointB = _PointB;
            Thickness = _Thickness;
        }

        #endregion

        #region methods

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            var distance = Vector2.Distance(PointA, PointB);
            var direction = PointA - PointB;
            float rotation = (float)Math.Atan2((double)direction.Y, (double)direction.X);

            var origin = new Vector2(0, 0.5f);
            var scale = new Vector2(distance, 1);

            spriteBatch.Draw(texture, PointA, null, Color.White, rotation, origin, scale, SpriteEffects.None, 0);
        }

        #endregion
    }
}
