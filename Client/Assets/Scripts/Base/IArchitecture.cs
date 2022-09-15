using System;
using Google.Protobuf;
using MyGame.Proto;

namespace MyGame
{
    public interface IArchitecture
    {
        void RegisterModel<T>(T model) where T : IModel;
        T GetModel<T>() where T : class, IModel;

        void RegisterSystem<T>(T system) where T : ISystem;
        T GetSystem<T>() where T : class, ISystem;

        void RegisterUtility<T>(T utility) where T : IUtility;
        T GetUtility<T>() where T : class, IUtility;

        void RegisterEvent<T>(Action<T> onEvent);
        void SendEvent<T>() where T : new();
        void SendEvent<T>(T e);
        void UnregisterEvent<T>(Action<T> onEvent);

        void SendCommand<T>() where T : ICommand, new();
        void SendCommand<T>(T command) where T : ICommand;
        
        void RegisterNetEvent(PacketId packetId, Action<IMessage> callback);
        void SendNetMessage<T>(PacketId packetId) where T : IMessage, new();
        void SendNetMessage<T>(PacketId packetId, T message) where T : IMessage;
        void UnregisterNetEvent(PacketId packetId, Action<IMessage> callback);
        
        void Dispose();
    }
}