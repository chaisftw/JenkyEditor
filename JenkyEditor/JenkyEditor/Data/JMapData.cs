using Newtonsoft.Json;
using System.Collections.Generic;

namespace JenkyEditor
{
    public class JMapData
    {
        public List<JLayer> Layers { get; set; }

        public JMapData()
        {

        }

        public JMapData(Map map)
        {
            Layers = new List<JLayer>();

            for (int i = 0; i < map.layers.Count; i++)
            {
                Layers.Add(new JLayer(map.layers[i].Name ,map.layers[i], map.GetMapBounds()));
            }
        }
    }
}
