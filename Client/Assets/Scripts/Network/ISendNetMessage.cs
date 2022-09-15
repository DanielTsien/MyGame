using Google.Protobuf;
using MyGame;
using MyGame.Proto;

namespace Network
{
    public interface ISendNetMessage : IGetArchitecture
    {
        
    }

    public static class SendMessageExtension
    {
        public static void SendNetMessage<T>(this ISendNetMessage self, PacketId packetId) where T : IMessage, new()
        {
            self.GetArchitecture().SendNetMessage<T>(packetId);
        }

        public static void SendNetMessage<T>(this ISendNetMessage self, PacketId packetId, T message) where T : IMessage
        {
            self.GetArchitecture().SendNetMessage(packetId, message);
        }
    }
}