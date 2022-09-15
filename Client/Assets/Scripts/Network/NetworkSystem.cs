using System;
using System.Collections.Generic;
using Google.Protobuf;
using MyGame.Proto;
using Network;
using UnityEngine;
using Utility;

namespace MyGame
{
    public class NetworkSystem : INetworkSystem
    {
        private readonly TcpSocket m_tcpSocket = new();
        private readonly Dictionary<PacketId, Action<IMessage>> m_tcpListeners = new();

        public void Init()
        {
            GameObject go = new GameObject(nameof(NetworkSystem));
            GameObject.DontDestroyOnLoad(go);
            go.AddComponent<UpdateBehaviour>().OnUpdate += OnUpdate;
        }
        
        public void Connect(string ip, int port, TcpSocket.ConnectResultDelegate connectResultHandler)
        {
            m_tcpSocket.Connect(ip, port, connectResultHandler);
        }

        public void Send<T>(PacketId packetId) where T : IMessage, new()
        {
            Send(packetId, new T());
        }

        public void Send<T>(PacketId packetId, T message) where T : IMessage
        {
            m_tcpSocket?.SendMessage(packetId, message);
        }

        public void Register(PacketId packetId, Action<IMessage> callback)
        {
            if (!m_tcpListeners.ContainsKey(packetId))
            {
                m_tcpListeners[packetId] = null;
            }

            m_tcpListeners[packetId] += callback;
        }

        public void Unregister(PacketId packetId, Action<IMessage> callback)
        {
            if (m_tcpListeners.ContainsKey(packetId))
            {
                m_tcpListeners[packetId] -= callback;
            }
        }

        public void Close()
        {
            m_tcpSocket?.Close();
        }

        private void OnUpdate()
        {
            int handledCount = 0;
            while (handledCount < NetConstant.MAX_HANDLE_PACKET_PERFRAME)
            {
                if (HandleTcpPacket())
                {
                    handledCount++;
                }
                else
                {
                    break;
                }
            }
        }

        private bool HandleTcpPacket()
        {
            if (m_tcpSocket == null) return false;

            var packet = m_tcpSocket.TryGetRecvPacket();
            if (packet == null) return false;

            if (m_tcpListeners.TryGetValue(packet.PacketId, out Action<IMessage> action))
            {
                try
                {
                    action?.Invoke(packet.Message);
                }
                catch
                {
                    // ignored
                }
            }

            return true;
        }
    }
}