using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Data
{
    internal class MapConfig
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string SubType { get; set; }
        public bool PKMode { get; set; }
        public string Resource { get; set; }
        public string MiniMap { get; set; }
        public string Music { get; set; }
        //public string Description { get; set; }
    }
}
