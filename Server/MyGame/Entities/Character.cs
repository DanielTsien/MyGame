using MyGame.Manager;
using MyGame.Proto;
using MyGame.Utils;

namespace MyGame.Entities
{
    internal class Character : CharacterBase
    {
        public TCharacter Data;
        public Character(CHARACTER_TYPE type, TCharacter character) :
            base(new Vector3Int(character.PosX, character.PosY, character.PosZ), new Vector3Int(0, 0, 0))
        {
            Data = character;
            Id = Data.Id;
            Info = new CharacterInfo();
            Info.Type = type;
            Info.Id = Id;
            Info.Name = Data.Name;
            Info.Level = Data.Level;
            Info.Class = (CHARACTER_CLASS)Data.Class;
            Info.Entity = EntityData;
            Info.MapId = Data.MapId;
            Info.ConfigId = Data.ConfigId;
            Config = DataManager.Instance.Characters[Info.ConfigId];
        }
    }
}
