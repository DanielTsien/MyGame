using System;
using System.Net;
using System.Net.Sockets;


namespace MyGame.Network
{
    //1.NetworkClient & TcpSocketListener
    //2.PackageHandler
    //3.MessageDistributer
    //4.MessageDispatch / IMessageService
    internal class NetService
    {
        private TcpSocketListener m_serverListener;

        public NetService()
        {
            m_serverListener = new TcpSocketListener(ServerSettings.Default.ServerIP, ServerSettings.Default.ServerPort, 10);
            m_serverListener.SocketConnected += OnSocketConnected;
        }

        public void Start()
        {
            Log.Warn("Starting Listener...");

            m_serverListener.Start();

            MessageDistributer.Instance.Start(4);
            Log.Warn("NetService Started");
        }

        public void Stop()
        {
            Log.Warn("Stop NetService...");

            m_serverListener.Stop();

            Log.Warn("Stoping Message Handler...");
            MessageDistributer.Instance.Stop();
        }

        private void OnSocketConnected(object sender, Socket client)
        {
            IPEndPoint clientIP = (IPEndPoint)client.RemoteEndPoint;
            //do IP compare, eg:blacklist

            SocketAsyncEventArgs args = new SocketAsyncEventArgs();

            NetSession session = new NetSession();

            NetConnection connection = new NetConnection(client, args,
                new NetConnection.DataReceivedCallback(DataReceived),
                new NetConnection.DisconnectedCallback(Disconnected),
                session);

            Log.Warn($"Client[{clientIP}] Connected");
        }

        private void DataReceived(NetConnection sender, DataEventArgs args)
        {
            Log.Info($"Client[{args.RemoteEndPoint}] DataReceived Len:{args.Length}");
            lock(sender.PackageHandler)
            {
                sender.PackageHandler.ReceiveData(args.Data, 0, args.Data.Length);
            }
        }

        private void Disconnected(NetConnection sender, SocketAsyncEventArgs args)
        {
            sender.Session.Disconnected();
            Log.Warn($"Client[{args.RemoteEndPoint}] Disconnected");
        }
    }
}
