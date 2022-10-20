using System;
using System.Collections.Generic;
using System.IO;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using MyGame.Proto;

namespace Network
{
    public class PacketParser
    {
        public class Packet
        {
            public PacketId PacketId;
            public IMessage Message;
        }
        
        private  byte[] m_recvBuffer = new byte[10 * 1024];
        private int m_curPosition;
        private short m_curPacketLength;

        private Queue<Packet> m_packets = new();

        public void ReceiveBuffer(byte[] buffer, int length)
        {
            if (m_curPosition + length >= m_recvBuffer.Length)
            {
                byte[] oldRecvBuffer = m_recvBuffer;
                m_recvBuffer = new byte[m_curPosition + length];
                Array.Copy(oldRecvBuffer, 0, m_recvBuffer, 0, m_curPosition);
            }

            Array.Copy(buffer, 0, m_recvBuffer, m_curPosition, length);
            m_curPosition += length;

            ParsePackage();
        }

        private void ParsePackage()
        {
            while (true)
            {
                if (m_curPosition < NetConstant.PACKET_HEAD_LENGTH)
                {
                    break;
                }

                if (m_curPacketLength == 0)
                {
                    m_curPacketLength = BitConverter.ToInt16(m_recvBuffer, NetConstant.PACKET_ID_BITS);
                    m_curPacketLength += NetConstant.PACKET_HEAD_LENGTH;
                }

                if (m_curPacketLength > 0 && m_curPosition >= m_curPacketLength)
                {
                    PacketId packetId = (PacketId)BitConverter.ToInt16(m_recvBuffer, 0);
                    IMessage message = UnpackMessage(packetId, m_recvBuffer, NetConstant.PACKET_HEAD_LENGTH,
                        m_curPacketLength - NetConstant.PACKET_HEAD_LENGTH);
                    
                    lock (this)
                    {
                        m_packets.Enqueue(new Packet
                        {
                            PacketId = packetId,
                            Message = message
                        });
                    }
                    
                    int offset = m_curPosition - m_curPacketLength;
                    if (offset > 0)
                    {
                        Array.Copy(m_recvBuffer, m_curPacketLength, m_recvBuffer, 0, offset);
                    }

                    m_curPosition = offset;
                    m_curPacketLength = 0;
                }
                else
                {
                    break;
                }
            }
        }
        
        private IMessage UnpackMessage(PacketId packetId, byte[] package, int offset, int size)
        {
            IMessage message = ProtobufDescriptor.ParserFrom(packetId, package, offset, size);
            return message;
        }

        public Packet TryGetRecvPacket()
        {
            lock (this)
            {
                if (m_packets.Count == 0)
                {
                    return null;
                }

                return m_packets.Dequeue();
            }
        }
    }
}