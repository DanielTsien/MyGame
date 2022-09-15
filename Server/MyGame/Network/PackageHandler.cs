using MyGame.Proto;
using Google.Protobuf;
using System;
using System.IO;

namespace MyGame.Network
{
    internal class PackageHandler
    {
        private MemoryStream m_stream = new MemoryStream(64 * 1024);
        private int m_readOffset;
        private NetConnection m_sender;

        public PackageHandler(NetConnection sender)
        {
            m_sender = sender;
        }

        public void ReceiveData(byte[] data, int offset, int count)
        {
            if(m_stream.Position + count > m_stream.Capacity)
            {
                throw new Exception("PackageHandler write buff overflow");
            }
            m_stream.Write(data, offset, count);

            ParsePackage();
        }

        private bool ParsePackage()
        {
            if(m_readOffset + NetConstant.PACKET_HEAD_LENGTH < m_stream.Position)
            {
                short packetId = BitConverter.ToInt16(m_stream.GetBuffer(), m_readOffset);
                short packageSize = BitConverter.ToInt16(m_stream.GetBuffer(), m_readOffset + NetConstant.PACKET_ID_BITS);
                //package is valid
                if (packageSize + m_readOffset + NetConstant.PACKET_HEAD_LENGTH <= m_stream.Position)
                {
                    IMessage message = UnpackMessage(packetId, m_stream.GetBuffer(), m_readOffset + NetConstant.PACKET_HEAD_LENGTH, packageSize);
                    if(message == null)
                    {
                        throw new Exception("PackageHandler ParsePackage faild, invalid package");
                    }
                    MessageDistributer.Instance.ReceiveMessage(m_sender, message);
                    m_readOffset += (packageSize + NetConstant.PACKET_HEAD_LENGTH);
                    return ParsePackage();
                }
            }

            if(m_readOffset > 0)
            {
                long size = m_stream.Position - m_readOffset;
                if(size > 0)
                {
                    Array.Copy(m_stream.GetBuffer(), m_readOffset, m_stream.GetBuffer(), 0, size);
                }
                m_readOffset = 0;
                m_stream.Position = size;
                m_stream.SetLength(size);
            }

            return true;
        }

        public byte[] PackMessage(PacketId packetId, IMessage message)
        {
            byte[] bytes = message.ToByteArray();
            byte[] package = new byte[bytes.Length + NetConstant.PACKET_HEAD_LENGTH];
            Buffer.BlockCopy(BitConverter.GetBytes((short)packetId), 0, package, 0, NetConstant.PACKET_ID_BITS);
            Buffer.BlockCopy(BitConverter.GetBytes((short)bytes.Length), 0, package, NetConstant.PACKET_ID_BITS, NetConstant.PACKET_LENGTH_BITS);
            Buffer.BlockCopy(bytes, 0, package, NetConstant.PACKET_HEAD_LENGTH, bytes.Length);
            return package;
        }

        //need packetId -- IMessage
        private IMessage UnpackMessage(short packetId, byte[] package, int offset, int size)
        {
            IMessage message = ProtobufDescriptor.ParserFrom((PacketId)packetId, package, offset, size);
            return message;
        }
    }
}
