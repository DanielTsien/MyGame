using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Network
{
    internal class NetConstant
    {
        public const short PACKET_LENGTH_BITS = 2;
        public const short PACKET_ID_BITS = 2;
        public const short PACKET_HEAD_LENGTH = PACKET_LENGTH_BITS + PACKET_ID_BITS;
    }
}
