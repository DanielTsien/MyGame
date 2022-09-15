using MyGame.Proto;
using Google.Protobuf;
using System;
using System.Net;
using System.Net.Sockets;

namespace MyGame.Network
{
    internal class NetConnection
    {

        internal class State
        {
            public DataReceivedCallback DataReceived { get; set; }
            public DisconnectedCallback Disconnected { get; set; }
            public Socket Socket { get; set; }
        }

        private SocketAsyncEventArgs m_args;

        public delegate void DataReceivedCallback(NetConnection sender, DataEventArgs args);
        public delegate void DisconnectedCallback(NetConnection sender, SocketAsyncEventArgs args);

        public PackageHandler PackageHandler { get; set; }
        public NetSession Session { get; private set; }

        public NetConnection(Socket socket, SocketAsyncEventArgs args, DataReceivedCallback dataReceived, DisconnectedCallback disconnected, NetSession session)
        {
            lock(this)
            {
                PackageHandler = new PackageHandler(this);

                State state = new State
                {
                    DataReceived = dataReceived,
                    Disconnected = disconnected,
                    Socket = socket,
                };

                m_args = new SocketAsyncEventArgs();
                m_args.AcceptSocket = socket;
                m_args.Completed += ReceivedCompleted;
                m_args.UserToken = state;
                m_args.SetBuffer(new byte[64 * 1024], 0, 60 * 1024);

                BeginReceive(m_args);
                Session = session;
            }
        }

        public void SendMessage(PacketId id, IMessage message)
        {
            byte[] package = PackageHandler.PackMessage(id, message);
            SendData(package, 0, package.Length);
        }

        public void SendData(byte[] data, int offset, int size)
        {
            lock(this)
            {
                State state = m_args.UserToken as State;
                Socket socket = state.Socket;
                if(socket.Connected)
                {
                    socket.BeginSend(data, 0, size, SocketFlags.None, new AsyncCallback(SendCallback), socket);
                }
            }
        }

        private void SendCallback(IAsyncResult ar)
        {
            try
            {
                Socket client = (Socket)ar.AsyncState;
                // Complete sending the data to the remote device.
                client.EndSend(ar);
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
        }

        private void ReceivedCompleted(object sender, SocketAsyncEventArgs args)
        {
            //This property provides the number of bytes transferred in an asynchronous socket operation that can receive or send data.
            //If ZERO is returned from a read operation, the remote end has closed the connection.
            if (args.BytesTransferred == 0)
            {
                CloseConnection(args);
                return;
            }
            if(args.SocketError != SocketError.Success)
            {
                CloseConnection(args);
                return;
            }

            var state = args.UserToken as State;
            byte[] data = new byte[args.BytesTransferred];
            Array.Copy(args.Buffer, args.Offset, data, 0, data.Length);
            OnDataReceived(data, args.RemoteEndPoint as IPEndPoint, state.DataReceived);

            BeginReceive(args);
        }

        private void BeginReceive(SocketAsyncEventArgs args)
        {
            lock(this)
            {
                Socket socket = (args.UserToken as State).Socket;
                //TODO: args.AcceptSocket ?= socket
                if(socket.Connected)
                {
                    args.AcceptSocket.ReceiveAsync(args);
                }
            }
        }

        private void OnDataReceived(byte[] data, IPEndPoint remoteEndPoint, DataReceivedCallback dataReceived)
        {
            dataReceived(this, new DataEventArgs { 
                Data = data,
                Offset = 0,
                Length = data.Length,
                RemoteEndPoint = remoteEndPoint
            });
        }

        private void CloseConnection(SocketAsyncEventArgs args)
        {
            var state = args.UserToken as State;
            var socket = state.Socket;
            try
            {
                socket.Shutdown(SocketShutdown.Both);
            }
            catch 
            {
                //throw new Exception("client process has already closed");
            }

            socket.Close();
            socket = null;

            args.Completed -= ReceivedCompleted;//Very Important!
            OnDisconnected(args, state.Disconnected);
        }

        private void OnDisconnected(SocketAsyncEventArgs args, DisconnectedCallback disconnected)
        {
            disconnected(this, args);
        }
    }
}
