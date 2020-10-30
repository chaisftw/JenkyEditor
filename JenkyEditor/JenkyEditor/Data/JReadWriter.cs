using System.IO;

using Newtonsoft.Json;

namespace JenkyEditor
{
    public class JReadWriter
    {
        public JObjectData GetAssets(string filePath)
        {
            //Read text from file
            string json = File.ReadAllText(filePath);
            
            //Deserialize to object format
            return JsonConvert.DeserializeObject<JObjectData>(json);
        }

        public JMapData GetJMap(string filePath)
        {
            //Read text from file
            string json = File.ReadAllText(filePath);

            //Deserialize to object format
            return JsonConvert.DeserializeObject<JMapData>(json);
        }

        public void SaveJMap(string filePath, JMapData map)
        {
            //Serialize to json string
            string json = JsonConvert.SerializeObject(map, Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            File.WriteAllText(filePath, json);
        }

        public void SaveJObjectData(string filePath, JObjectData objectData)
        {
            //Serialize to json string
            string json = JsonConvert.SerializeObject(objectData);
            File.WriteAllText(filePath, json);
        }
    }
}
