using Google.Protobuf;
using MyGame.Proto;
using Network;

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
        }

        private void OnGameEnterResp(IMessage msg)
        {
            
        }

        private void OnCreateCharacterResp(IMessage msg)
        {
            
        }

        private void OnRegisterResp(IMessage msg)
        {

        }

        private void OnLoginResp(IMessage msg)
        {
            if (msg is not UserLoginResponse message) return;
            if (message.Result == Result.Success)
            {
                var userModel = this.GetModel<IUserModel>();
                this.GetModel<IUserModel>().UserId = message.Userinfo.UserId;
                this.GetModel<IUserModel>().PlayerId = message.Userinfo.Player.PlayerId;
                userModel.SetCharacters(message.Userinfo.Player.Characters);
            }
            
        }
    }
}