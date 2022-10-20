using MyGame.Model;
using MyGame.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Manager
{
    internal class MapManager : Singleton<MapManager>
    {
        private Dictionary<int, Map> m_maps = new Dictionary<int, Map>();

        private MapManager()
        {

        }

        public void Init()
        {
            foreach (var mapConfig in DataManager.Instance.Maps.Values)
            {
                Map map = new Map(mapConfig);
                Log.Info($"MapManager.Init > Map: {mapConfig.ID}:{mapConfig.Name}");
                m_maps[mapConfig.ID] = map;
            }
        }

        public Map this[int key]
        {
            get
            {
                return m_maps[key];
            }
        }

        public void Update()
        {
            foreach (var map in m_maps.Values)
            {
                
            }
        }
    }
}
