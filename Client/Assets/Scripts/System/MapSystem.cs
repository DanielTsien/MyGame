using Google.Protobuf;
using MyGame.Proto;
using Network;
using UnityEngine.SceneManagement;

namespace MyGame
{
    public interface IMapSystem : ISystem
    {
        
    }

    public class MapSystem : SystemBase, IMapSystem
    {
        protected override void OnInit()
        {
            this.RegisterNetEvent(PacketId.MapCharacterEnterResponse, OnMapCharacterEnterResp);
            this.RegisterNetEvent(PacketId.MapCharacterLeaveResponse, OnMapCharacterLeaveResp);
        }

        private void OnMapCharacterEnterResp(IMessage msg)
        {
            if (msg is not MapCharacterEnterResponse message) return;

            SceneManager.LoadScene(this.GetModel<IMapConfigModel>().GetById(message.MapId).Resource);
        }
        
        private void OnMapCharacterLeaveResp(IMessage msg)
        {
            if (msg is not MapCharacterLeaveResponse message) return;
            
            
        }
    }
}