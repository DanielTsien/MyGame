using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Google.Protobuf;
using MyGame.Proto;
using Network;
using Utility;

namespace MyGame
{
    public class TcpSocket
    {
        public delegate void ConnectResultDelegate(bool isSuccess, string msg);
        public delegate void SocketExceptionDelegate(SocketError errCode, string msg);

        private INetworkSystem m_networkSystem;
        private Socket m_socket;
        private IPEndPoint m_endPoint;
        private byte[] m_recvBuffer= new byte[1024 * 5];
        private Queue<byte[]> m_sendBuffers = new();
        private readonly ManualResetEvent m_timeoutObj = new(false);
        private ConnectResultDelegate m_connectResultHandler;
        private SocketExceptionDelegate m_socketExceptionHandler;
        private bool m_isRunning;
        private PacketParser m_packetParser = new();
        
        public bool Connected => m_socket?.Connected ?? false;

        public PacketParser.Packet TryGetRecvPacket()
        {
            return m_packetParser?.TryGetRecvPacket();
        }
        
        public void Connect(string ip, int port, ConnectResultDelegate connectResultHandler)
        {
            m_connectResultHandler = connectResultHandler;
            if (!Connected)
            {
                m_endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                BeginConnect(); 
            }
        }

        public void SetSocketExceptionHandler(SocketExceptionDelegate handler)
        {
            m_socketExceptionHandler = handler;
        }

        private void BeginConnect()
        {
            try
            {
                m_socket?.Close();
                m_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                m_socket.SendTimeout = NetConstant.SEND_TIMEOUT;
                m_timeoutObj.Reset();
                m_socket.BeginConnect(m_endPoint, OnConnectSuccess, m_socket);

            }
            catch (Exception e)
            {
                InvokeConnectResult(false, e.Message);
                return;
            }

            Action<TcpSocket> action = WaitConnectTimeout;
            action.BeginInvoke(this, ar => action.EndInvoke(ar), null);
        }

        private void OnConnectSuccess(IAsyncResult ar)
        {
            Socket socket = (Socket)ar.AsyncState;
            try
            {
                //EndConnect completes the operation started by BeginConnect.
                //You need to pass the IAsyncResult created by the matching BeginConnect call.
                //EndConnect will block the calling thread until the operation is completed.
                socket.EndConnect(ar);
                m_isRunning = true;
                Thread recvThread = new Thread(OnRecvSocketBuffer);
                recvThread.IsBackground = true;
                recvThread.Start();
                InvokeConnectResult(true, $"Connect Success {m_endPoint}");
            }
            catch (Exception e)
            {
                InvokeConnectResult(false, e.Message);
            }
            finally
            {
                m_timeoutObj.Set();
            }
        }

        private void OnRecvSocketBuffer()
        {
            while (m_isRunning)
            {
                if (!Connected)
                {
                    InvokeSocketException(SocketError.NotConnected, "Connection is disconnected");
                    break;
                }

                try
                {
                    if (!m_socket.Poll(0, SelectMode.SelectRead))
                    {
                        Thread.Sleep(50);
                        continue;
                    }

                    int recvLength = m_socket.Receive(m_recvBuffer);
                    if (recvLength > 0)
                    {
                        m_packetParser.ReceiveBuffer(m_recvBuffer, recvLength);
                    }
                    else
                    {
                        InvokeSocketException(SocketError.NotConnected, "Network is disconnected");
                        break;
                    }
                }
                catch (SocketException e)
                {
                    InvokeSocketException(e.SocketErrorCode, e.Message);
                    break;
                }
                catch (Exception e)
                {
                    InvokeSocketException(SocketError.SocketError, e.Message);
                    break;
                }
            }
        }

        private void WaitConnectTimeout(TcpSocket socket)
        {
            if (socket == null)
            {
                return;
            }

            if (!m_timeoutObj.WaitOne(NetConstant.CONNECT_TIMEOUT, false))
            {
                InvokeConnectResult(false,$"connect timeout {m_endPoint}");
            }
        }

        private void InvokeConnectResult(bool isSuccess, string msg)
        {
            if (m_connectResultHandler != null)
            {
                Loom.QueueOnMainThread(() =>
                {
                    m_connectResultHandler?.Invoke(isSuccess, msg);
                });
            }
        }

        private void InvokeSocketException(SocketError errCode, string msg)
        {
            if (m_socketExceptionHandler != null)
            {
                Loom.QueueOnMainThread(() =>
                {
                    m_socketExceptionHandler?.Invoke(errCode, msg);
                });
            }
        }
        
        private byte[] TryGetSendBuffer(int length)
        {
            lock (this)
            {
                if (m_sendBuffers.Count > 0 && m_sendBuffers.Peek().Length > length)
                {
                    return m_sendBuffers.Dequeue();
                }
            }

            return new byte[Math.Max(length, 1024)];
        }
        
        public void SendMessage(PacketId packetId, IMessage message)
        {
            if (!Connected)
            {
                InvokeSocketException(SocketError.NotConnected, "Network not connect");
                return;
            }

            byte[] msgBytes = message.ToByteArray();
            int msgLength = msgBytes.Length;
            int packetLength = msgLength + NetConstant.PACKET_HEAD_LENGTH;
            byte[] sendBuffer = TryGetSendBuffer(packetLength);
            
            Buffer.BlockCopy(BitConverter.GetBytes((short)packetId), 0, sendBuffer, 0, NetConstant.PACKET_ID_BITS);
            Buffer.BlockCopy(BitConverter.GetBytes((short)msgLength), 0, sendBuffer, NetConstant.PACKET_ID_BITS, NetConstant.PACKET_LENGTH_BITS);
            Buffer.BlockCopy(msgBytes, 0, sendBuffer, NetConstant.PACKET_HEAD_LENGTH, msgLength);

            m_socket.BeginSend(sendBuffer, 0, packetLength, SocketFlags.None,
                OnSendMessageResult, sendBuffer);
        }

        private void OnSendMessageResult(IAsyncResult ar)
        {
            if (ar.IsCompleted && ar.AsyncState != null)
            {
                lock (this)
                {
                    m_sendBuffers.Enqueue(ar.AsyncState as byte[]);
                }
            }
        }

        public void Close()
        {
            m_timeoutObj.Reset();
            m_isRunning = false;
            if (m_socket != null)
            {
                try
                {
                    m_socket.Close();
                    m_socket = null;
                }
                catch
                {
                    // ignored
                }
            }
        }

        public void Dispose()
        {
            Close();
        }
    }
}