﻿using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace MyGame
{
    public interface IMapConfigModel : IModel
    {
        MapConfig GetById(int id);
    }
    
    public class MapConfigModel : ModelBase, IMapConfigModel
    {
        public Dictionary<int, MapConfig> m_maps = null;
        protected override void OnInit()
        {
            string json = File.ReadAllText("Data/Map.txt");
            m_maps = JsonConvert.DeserializeObject<Dictionary<int, MapConfig>>(json);
        }

        public MapConfig GetById(int id)
        {
            return m_maps[id];
        }
    }
}