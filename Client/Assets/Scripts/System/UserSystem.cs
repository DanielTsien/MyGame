using Google.Protobuf;
using MyGame.Proto;
using Network;
using UnityEngine;

namespace MyGame
{
    public interface IUserSystem : ISystem
    {
        
    }
    
    public class UserSystem : SystemBase, IUserSystem
    {
        protected override void OnInit()
        {
            this.RegisterNetEvent(PacketId.UserLoginResponse, OnLoginResp);
            this.RegisterNetEvent(PacketId.UserRegisterResponse, OnRegisterResp);
            this.RegisterNetEvent(PacketId.UserCreateCharacterResponse, OnCreateCharacterResp);
            this.RegisterNetEvent(PacketId.UserGameEnterResponse, OnGameEnterResp);
            this.RegisterNetEvent(PacketId.UserGameLeaveResponse, OnGameLeaveResp);
        }

        private void OnGameLeaveResp(IMessage msg)
        {
            if (msg is not UserGameLeaveResponse message) return;
            
            if (message.Result == RESULT.Success)
            {
                
            }
        }

        private void OnGameEnterResp(IMessage msg)
        {
            if (msg is not UserGameEnterResponse message)
            {
                return;
            }

            if (message.Result != RESULT.Success)
            {
                return;
            }

            this.GetModel<IUserModel>().CurCharacterInfo = message.Character;
        }

        private void OnCreateCharacterResp(IMessage msg)
        {
            
        }

        private void OnRegisterResp(IMessage msg)
        {

        }

        private void OnLoginResp(IMessage msg)
        {
            if (msg is not UserLoginResponse message)
            {
                return;
            }
            
            if (message.Result != RESULT.Success)
            {
                return;
            }
            
            var userModel = this.GetModel<IUserModel>();
            this.GetModel<IUserModel>().UserId = message.Userinfo.UserId;
            this.GetModel<IUserModel>().PlayerId = message.Userinfo.Player.PlayerId;
            userModel.SetCharacters(message.Userinfo.Player.Characters);
            
        }
    }
}