using MyGame.Data;
using MyGame.Entities;
using MyGame.Network;
using MyGame.Proto;
using System;
using System.Collections.Generic;

namespace MyGame.Model
{
    internal class Map
    {
        private MapConfig m_mapConfig;
        private Dictionary<int, MapCharacter> m_mapCharacters = new Dictionary<int, MapCharacter>();

        public int ID => m_mapConfig.ID;

        public Map(MapConfig mapConfig)
        {
            m_mapConfig = mapConfig;
        }

        public void CharacterEnter(NetConnection sender, Character character)
        {
            Log.Info($"CharacterEnter: Map:{m_mapConfig.ID} CharacterId:{character.Id}");

            MapCharacterEnterResponse response = new MapCharacterEnterResponse();
            response.MapId = m_mapConfig.ID;
            response.Characters.Add(character.Info);

            foreach (var mapCharacter in m_mapCharacters.Values)
            {
                response.Characters.Add(mapCharacter.Character.Info);
                SendCharacterEnterMap(mapCharacter.Sender, character.Info);
            }

            m_mapCharacters[character.Id] = new MapCharacter(sender, character);
            sender.SendMessage(PacketId.MapCharacterEnterResponse, response);
        }

        private void SendCharacterEnterMap(NetConnection sender, CharacterInfo info)
        {
            MapCharacterEnterResponse response = new MapCharacterEnterResponse();
            response.MapId = m_mapConfig.ID;
            response.Characters.Add(info);
            sender.SendMessage(PacketId.MapCharacterEnterResponse, response);
        }

        private void SendCharacterLeaveMap(NetConnection sender, CharacterInfo info)
        {
            MapCharacterLeaveResponse response = new MapCharacterLeaveResponse();
            response.EntityId = info.EntityId;
            sender.SendMessage(PacketId.MapCharacterLeaveResponse, response);
        }
    }
}
