using System;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using MyGame.Proto;

namespace MyGame
{
    public interface IUserModel : IModel
    {
        public int UserId { get; set; }
        public int PlayerId { get; set; }

        
        Character? GetCharacterById(int id);
        List<Character> GetCharacters();
        void SetCharacters(RepeatedField<CharacterInfo> playerCharacters);
    }
    
    public struct Character
    {
        public int Id;
        public int ConfigId;
        public int EntityId;
        public string Name;
        public CHARACTER_TYPE Type;
        public CHARACTER_CLASS Class;
        public int Level;
    }

    public class UserModel : ModelBase, IUserModel
    {
        private readonly List<Character> m_characters = new();
        
        public int UserId { get; set; }
        public int PlayerId { get; set; }
        
        public Character? GetCharacterById(int id)
        {
            foreach (var character in m_characters)
            {
                if (character.Id == id)
                {
                    return character;
                }
            }

            return null;
        }
        
        public List<Character> GetCharacters() => m_characters;
        public void SetCharacters(RepeatedField<CharacterInfo> playerCharacters)
        {
            m_characters.Clear();
            foreach (var character in playerCharacters)
            {
                m_characters.Add(new Character
                {
                    Id = character.Id,
                    ConfigId = character.ConfigId,
                    EntityId = character.EntityId,
                    Name = character.Name,
                    Type = character.Type,
                    Class = character.Class,
                    Level = character.Level
                });
            }
        }
    }
}