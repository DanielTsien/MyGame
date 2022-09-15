using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.Network
{
    internal class TcpSocketListener : IDisposable
    {
        private Socket m_listenerSocket;
        private SocketAsyncEventArgs m_args;
        private bool m_disposed;

        public event EventHandler<Socket> SocketConnected;

        public bool IsRunning => m_listenerSocket != null;

        public int ConnectionBacklog { get; set; }


        public IPEndPoint EndPoint { get; set; }


        public TcpSocketListener(IPEndPoint endPoint, int connectionBacklog)
        {
            EndPoint = endPoint;
            ConnectionBacklog = connectionBacklog;

            m_args = new SocketAsyncEventArgs();
            m_args.Completed += OnSocketAccepted;
        }

        public TcpSocketListener(IPAddress address, int port, int connectionBacklog)
            :this(new IPEndPoint(address, port), connectionBacklog)
        {

        }

        public TcpSocketListener(string address, int port, int connectionBacklog)
            : this(IPAddress.Parse(address), port, connectionBacklog)
        {

        }

        public void Start()
        {
            lock(this)
            {
                if(IsRunning)
                {
                    throw new InvalidOperationException("The Server is already runing");
                }
                else
                {
                    m_listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    m_listenerSocket.Bind(EndPoint);
                    m_listenerSocket.Listen(ConnectionBacklog);
                    BeginAccept(m_args);
                }
            }
        }

        private void BeginAccept(SocketAsyncEventArgs args)
        {
            args.AcceptSocket = null;
            m_listenerSocket.AcceptAsync(args);
        }

        private void OnSocketAccepted(object sender, SocketAsyncEventArgs args)
        {
            SocketError error = args.SocketError;
            if (args.SocketError == SocketError.OperationAborted) return;

            if(args.SocketError == SocketError.Success)
            {
                var client = args.AcceptSocket;
                OnSocketConnected(client);
            }

            lock(this)
            {
                BeginAccept(args);
            }
        }

        private void OnSocketConnected(Socket client)
        {
            if(SocketConnected != null)
            {
                SocketConnected(this, client);
            }
        }


        public void Stop()
        {
            lock(this)
            {
                if (m_listenerSocket == null) return;

                m_listenerSocket.Close();
                m_listenerSocket = null;
            }
        }

        ~TcpSocketListener()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if(m_disposed)
            {
                Stop();
                if(m_args != null)
                {
                    m_args.Dispose();
                }
                m_disposed = true;
            }
        }
    }
}
