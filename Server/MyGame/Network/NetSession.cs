namespace MyGame.Network
{
    internal class NetSession : INetSession
    {
        public TUser User { get; set; }
        public void Disconnected()
        {
            
        }
    }
}