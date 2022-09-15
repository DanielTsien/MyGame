using MyGame.Proto;
using MyGame.Network;
using System.Threading;

namespace MyGame
{
    internal class GameServer
    {
        private bool m_isRuning = false;
        private NetService m_netService;
        private Thread m_thread;
        public GameServer()
        {
            m_netService = new NetService();
            MessageSubscribes();
            DatabaseManager.Instance.Init();
            m_thread = new Thread(new ThreadStart(Update));
        }

        private void MessageSubscribes()
        {
            new UserMessageService().Subscribes();
        }

        public void Start()
        {
            m_netService.Start();
            m_isRuning = true;
            m_thread.Start();
        }

        public void Stop()
        {
            m_isRuning = false;
            m_thread.Join();
            m_netService.Stop();
        }

        public void Update()
        {
            while(m_isRuning)
            {
                Thread.Sleep(100);
            }
        }
    }
}
