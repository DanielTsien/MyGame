using System.Collections.Generic;
using DefaultNamespace;
using Google.Protobuf;
using MyGame.Proto;
using Network;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MyGame
{
    public interface IMapSystem : ISystem
    {
        public int CurMapId { get; set; }
    }

    public class MapSystem : SystemBase, IMapSystem
    {
        private Dictionary<int, NCharacterInfo> m_characters;
        public int CurMapId { get; set; }
        
        
        protected override void OnInit()
        {
            this.RegisterNetEvent(PacketId.MapCharacterEnterResponse, OnMapCharacterEnterResp);
            this.RegisterNetEvent(PacketId.MapCharacterLeaveResponse, OnMapCharacterLeaveResp);
        }

        private void OnMapCharacterEnterResp(IMessage msg)
        {
            if (msg is not MapCharacterEnterResponse message) return;

            foreach (var character in message.Characters)
            {
                if (character.Id == this.GetModel<IUserModel>().CurCharacterInfo.Id)
                {
                    if (CurMapId != message.MapId)
                    {
                        EnterMap(message.MapId);
                    }

                    ClearCharacters();
                    this.GetModel<IUserModel>().CurCharacterInfo = character;
                }
            }
            
            foreach (var character in message.Characters)
            {
                CreateCharacter(character);
                m_characters[character.Id] = character;
                
            }
        }
        
        private void OnMapCharacterLeaveResp(IMessage msg)
        {
            if (msg is not MapCharacterLeaveResponse message) return;
        }

        private void EnterMap(int mapId)
        {
            CurMapId = mapId;
            SceneManager.LoadScene(this.GetModel<IMapConfigModel>().Get(mapId).Resource);
        }

        private void ClearCharacters()
        {
            m_characters.Clear();
        }
        
        private void CreateCharacter(NCharacterInfo characterInfo)
        {
            var cfg = this.GetModel<ICharacterConfigModel>().Get(characterInfo.ConfigId);
            var go = GameObject.Instantiate(Resources.Load<GameObject>(cfg.Resource));
            if (characterInfo.Id == this.GetModel<IUserModel>().CurCharacterInfo.Id)
            {
                this.GetSystem<IInputSystem>().SetController(go);
            }
            
            
        }
    }
}