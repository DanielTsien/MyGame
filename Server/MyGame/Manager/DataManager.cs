using Google.Protobuf;
using MyGame.Data;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using MyGame.Utils;

namespace MyGame.Manager
{
    internal class DataManager : Singleton<DataManager>
    {
        private readonly string m_dataPath = "Data/";

        public Dictionary<int, CharacterDefine> Characters = null;

        private DataManager()
        {

        }

        public void Load()
        {
            string json = File.ReadAllText(m_dataPath + "Character.txt");
            Characters = JsonConvert.DeserializeObject<Dictionary<int, CharacterDefine>>(json);
        }
    }
}
