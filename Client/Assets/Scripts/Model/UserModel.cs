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
        public NCharacterInfo CurCharacterInfo{ get; set; }
        
        void SetCharacters(RepeatedField<NCharacterInfo> playerCharacters);
        List<NCharacterInfo> GetCharacters();
    }
    
    public class UserModel : ModelBase, IUserModel
    {
        private readonly List<NCharacterInfo> m_characters = new();

        public int UserId { get; set; }
        public int PlayerId { get; set; }

        public NCharacterInfo CurCharacterInfo { get; set; }
        public List<NCharacterInfo> GetCharacters() => m_characters;
        
        public void SetCharacters(RepeatedField<NCharacterInfo> playerCharacters)
        {
            m_characters.Clear();
            foreach (var character in playerCharacters)
            {
                m_characters.Add(character);
            }
        }
    }
}