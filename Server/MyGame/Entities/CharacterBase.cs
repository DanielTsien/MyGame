using MyGame.Data;
using MyGame.Manager;
using MyGame.Proto;
using MyGame.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Entities
{
    internal class CharacterBase : Entity

    {
        public int Id { get; set; }
        public string Name => Info.Name;
        public CharacterInfo Info;
        public CharacterConfig Config;

        public CharacterBase(Vector3Int pos, Vector3Int dir) : base(pos, dir)
        {

        }

        public CharacterBase(CHARACTER_TYPE type, int configId, int level, Vector3Int pos, Vector3Int dir) : base(pos ,dir)
        {
            Info = new CharacterInfo();
            Info.Type = type;
            Info.Level = level;
            Info.Entity = EntityData;
            Info.EntityId = EntityId;
            Info.ConfigId = configId;
            Config = DataManager.Instance.Characters[configId];
            Info.Name = Config.Name;
        }
    }
}
