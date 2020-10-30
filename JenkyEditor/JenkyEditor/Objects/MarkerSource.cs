using Jenky.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenkyEditor
{
    public class MarkerSource
    {
        public string Name { get; private set; }
        public Color MarkerColor { get; private set; }

        public MarkerSource(string _Name, Color _ColliderColor)
        {
            Name = _Name;
            MarkerColor = _ColliderColor;
        }
    }
}
