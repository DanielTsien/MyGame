using System;
using Google.Protobuf;
using MyGame.Proto;

namespace MyGame
{
    public interface INetworkSystem
    {
        void Init();
        void Connect(string ip, int port, TcpSocket.ConnectResultDelegate connectResultHandler);
        void Send<T>(PacketId packetId)where T : IMessage, new();
        void Send<T>(PacketId packetId, T message) where T : IMessage;
        void Register(PacketId packetId, Action<IMessage> callback);
        void Unregister(PacketId packetId, Action<IMessage> callback);

        void Close();
    }
}