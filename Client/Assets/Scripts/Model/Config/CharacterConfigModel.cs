using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace MyGame
{
    public interface ICharacterConfigModel : IModel
    {
        
    }
    
    public class CharacterConfigModel : ModelBase, ICharacterConfigModel
    {
        public Dictionary<int, CharacterConfig> m_characters = null;
        protected override void OnInit()
        {
            string json = File.ReadAllText("Data/Character.txt");
            m_characters = JsonConvert.DeserializeObject<Dictionary<int, CharacterConfig>>(json);
        }
    }
}