using System;
using Google.Protobuf;
using MyGame;
using MyGame.Proto;

namespace Network
{
    public interface IRegisterNetEvent : IGetArchitecture
    {
        
    }

    public static class RegisterNetEventExtension
    {
        public static void RegisterNetEvent(this IRegisterNetEvent self, PacketId packetId, Action<IMessage> callback)
        {
            self.GetArchitecture().RegisterNetEvent(packetId, callback);
        }
        
        public static void UnregisterNetEvent(this IRegisterNetEvent self, PacketId packetId, Action<IMessage> callback)
        {
            self.GetArchitecture().UnregisterNetEvent(packetId, callback);
        }
    }
}