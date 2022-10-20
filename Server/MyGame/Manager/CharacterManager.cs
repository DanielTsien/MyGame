using MyGame.Entities;
using MyGame.Proto;
using MyGame.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Manager
{
    internal class CharacterManager : Singleton<CharacterManager>
    {
        public Dictionary<int, Character> Characters = new Dictionary<int, Character>();

        private CharacterManager()
        {

        }

        public void Init()
        {

        }

        public void Dispose()
        {

        }

        public void Clear()
        {
            Characters.Clear(); 
        }

        public Character AddCharacter(TCharacter tCharacter)
        {
            Character character = new Character(CHARACTER_TYPE.Player, tCharacter);
            Characters[character.Id] = character;
            return character;
        }

        public void RemoveCharacter(int characterId)
        {
            Characters.Remove(characterId);
        }
    }
}
