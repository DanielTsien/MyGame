namespace Network
{
    public class NetConstant
    {
        public const int SEND_TIMEOUT = 15 * 1000;
        public const int CONNECT_TIMEOUT = 5 * 1000;
        public const short PACKET_LENGTH_BITS = 2;
        public const short PACKET_ID_BITS = 2;
        public const short PACKET_HEAD_LENGTH = PACKET_LENGTH_BITS + PACKET_ID_BITS;
        public const int MAX_HANDLE_PACKET_PERFRAME = 30;
    }
}