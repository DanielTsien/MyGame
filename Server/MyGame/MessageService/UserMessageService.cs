using MyGame.Proto;
using MyGame.Network;
using Google.Protobuf;
using System.Linq;
using System.Collections.Generic;
using Google.Protobuf.Collections;
using MyGame.Entities;
using MyGame.Manager;

namespace MyGame
{
    internal class UserMessageService : IMessageService
    {
        public void Subscribes()
        {
            MessageDistributer.Instance.Subscribe<UserLoginRequest>(OnLogin);
            MessageDistributer.Instance.Subscribe<UserRegisterRequest>(OnRegister);
            MessageDistributer.Instance.Subscribe<UserCreateCharacterRequest>(OnCreateCharacter);
            MessageDistributer.Instance.Subscribe<UserGameEnterRequest>(OnUserGameEnter);
        }

        private void OnCreateCharacter(NetConnection sender, IMessage message)
        {
            var msg = message as UserCreateCharacterRequest;
            Log.Info($"UserCreateCharacterRequest: Name:{msg.Name} Class:{msg.Class}");

            var responMsg = new UserCreateCharacterResponse();
            TCharacter character = DatabaseManager.Instance.Entities.TCharacters.Add(new TCharacter
            {
                Name = msg.Name,
                ConfigId = (int)msg.Class,
                Class = (int)msg.Class,
                Level = 1,
                MapId = 1,
            });

            sender.Session.User.TPlayer.TCharacters.Add(character);
            DatabaseManager.Instance.Save(false);
            
            responMsg.Errormsg = "None";
            responMsg.Result = RESULT.Success;
            AddCharacter(sender.Session.User.TPlayer.TCharacters, responMsg.Characters);

            sender.SendMessage(PacketId.UserCreateCharacterResponse, responMsg);
        }

        private void OnRegister(NetConnection sender, IMessage message)
        {
            var msg = message as UserRegisterRequest;
            Log.Info($"UserRegisterRequest: User:{msg.Username} Password:{msg.Passward}");

            var responMsg = new UserRegisterResponse();
            responMsg.Errormsg = "None";
            responMsg.Result = RESULT.Failed;
            TUser user = DatabaseManager.Instance.Entities.TUsers.Where(u => u.Username == msg.Username).FirstOrDefault();
            if(user != null)
            {
                responMsg.Errormsg = "User already exists";
            }
            else
            {
                responMsg.Result = RESULT.Success;
                TPlayer player = DatabaseManager.Instance.Entities.TPlayers.Add(new TPlayer());
                user = DatabaseManager.Instance.Entities.TUsers.Add(new TUser { Username = msg.Username, Password = msg.Passward, TPlayer = player});
                responMsg.Userinfo = new UserInfo { UserId = user.Id, Player = new PlayerInfo { PlayerId = player.Id} };
                DatabaseManager.Instance.Save(false);
            }
            sender.Session.User = user;
            sender.SendMessage(PacketId.UserLoginResponse, responMsg);
        }

        private void OnLogin(NetConnection sender, IMessage message)
        {
            var msg = message as UserLoginRequest;
            Log.Info($"UserLoginRequest: User:{msg.Username} Password:{msg.Passward}");


            var responMsg = new UserLoginResponse();
            responMsg.Errormsg = "None";
            responMsg.Result = RESULT.Failed;
            TUser user = DatabaseManager.Instance.Entities.TUsers.Where(u => u.Username == msg.Username).FirstOrDefault();
            if (user == null)
            {
                responMsg.Errormsg = "The user does not exist";
            }
            else if(user.Password != msg.Passward)
            {
                responMsg.Errormsg = "Password Error";
            }
            else
            {
                responMsg.Result = RESULT.Success;

                responMsg.Userinfo = new UserInfo
                {
                    UserId = user.Id,
                    Player = new PlayerInfo { PlayerId = user.TPlayer.Id}
                };
                AddCharacter(user.TPlayer.TCharacters, responMsg.Userinfo.Player.Characters);

            }
            sender.Session.User = user;
            sender.SendMessage(PacketId.UserLoginResponse, responMsg);
        }

        private void AddCharacter(ICollection<TCharacter> characters, RepeatedField<NCharacterInfo> CharacterInfos)
        {
            foreach (var character in characters)
            {
                NCharacterInfo info = new NCharacterInfo();
                info.Id = character.Id;
                info.Name = character.Name;
                info.Class = (CHARACTER_CLASS)character.Class;
                info.Level = character.Level;
                CharacterInfos.Add(info);
            }
        }

        private void OnUserGameEnter(NetConnection sender, IMessage message)
        {
            var msg = message as UserGameEnterRequest;
            TCharacter dbcCharacter = sender.Session.User.TPlayer.TCharacters.ElementAt(msg.CharacterIdx);
            Log.Info($"UserGameEnterRequest: characterID:{dbcCharacter.Id}:{dbcCharacter.Name} Map:{dbcCharacter.MapId}");

            Character character = CharacterManager.Instance.AddCharacter(dbcCharacter);

            UserGameEnterResponse responMsg = new UserGameEnterResponse();
            responMsg.Result = RESULT.Success;
            responMsg.Errormsg = "None";
            sender.SendMessage(PacketId.UserGameEnterResponse, responMsg);

            sender.Session.Character = character;
            MapManager.Instance[dbcCharacter.MapId].CharacterEnter(sender, character);
        }
    }
}
