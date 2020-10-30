using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jenky.Geo
{
    public class Ellipse
    {
        #region vars

        public float RadiusA { get; private set; } //Horizontal radius
        public float RadiusB { get; private set; } //Vertical radius

        public Vector2 Origin { get; private set; }

        #endregion

        #region init

        public Ellipse(float originX, float originY, float _RadiusA, float _RadiusB, float scale)
        {
            RadiusA = _RadiusA * scale;
            RadiusB = _RadiusB * scale;
            Origin = new Vector2(originX, originY);
        }

        #endregion

        #region methods

        public Vector2 PointOnEllipse(Vector2 focus)
        {
            Vector2 direction = new Vector2(focus.X - Origin.X, focus.Y - Origin.Y);
            float slope = direction.Y / direction.X;
            float pointX = (float)((RadiusA * RadiusB) / (Math.Sqrt((Math.Pow(RadiusA, 2) * Math.Pow(direction.Y, 2)) + (Math.Pow(RadiusB, 2) * Math.Pow(direction.X, 2)))) * direction.X);
            float pointY = (float)((RadiusA * RadiusB) / (Math.Sqrt((Math.Pow(RadiusA, 2) * Math.Pow(direction.Y, 2)) + (Math.Pow(RadiusB, 2) * Math.Pow(direction.X, 2)))) * direction.Y);

            return new Vector2(pointX, pointY);
        }

        public void SetOrigin(Vector2 _origin)
        {
            Origin = _origin;
        }

        #endregion
    }
}
