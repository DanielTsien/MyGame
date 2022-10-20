using MyGame.Entities;
using MyGame.Proto;

namespace MyGame.Network
{
    internal class NetSession : INetSession
    {
        public TUser User { get; set; }
        public Character Character { get; set; }
        public NEntity Entity { get; set; }
        public void Disconnected()
        {
            
        }
    }
}