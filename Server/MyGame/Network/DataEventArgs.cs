
using System.Net;

namespace MyGame.Network
{
    internal class DataEventArgs
    {
        public IPEndPoint RemoteEndPoint { get; set; }
        public byte[] Data { get; set; }
        public int Offset { get; set; }
        public int Length { get; set; }
    }
}
