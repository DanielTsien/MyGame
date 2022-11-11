using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace MyGame
{
    public interface IMapConfigModel : IModel
    {
        MapConfig Get(int id);
    }
    
    public class MapConfigModel : ModelBase, IMapConfigModel
    {
        public Dictionary<int, MapConfig> m_maps = null;
        protected override void OnInit()
        {
            string json = File.ReadAllText("Assets/Config/Map.txt");
            m_maps = JsonConvert.DeserializeObject<Dictionary<int, MapConfig>>(json);
        }

        public MapConfig Get(int id)
        {
            return m_maps[id];
        }
    }
}