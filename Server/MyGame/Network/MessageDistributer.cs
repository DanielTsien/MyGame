using MyGame.Utils;
using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MyGame.Network
{
    internal class MessageDistributer : Singleton<MessageDistributer>
    {
        private struct MessageArgs
        {
            public NetConnection Sender { get; set; }
            public IMessage Message { get; set; }
        }

        private Queue<MessageArgs> m_messageQueue = new Queue<MessageArgs>();
        private Dictionary<Type, MessageHandler> m_messageHandlers = new Dictionary<Type, MessageHandler>();
        private bool m_isRuning;
        private AutoResetEvent m_threadEvent = new AutoResetEvent(true);

        public delegate void MessageHandler(NetConnection sender, IMessage message);
        public int ActiveThreadCount;

        public int ThreadCount { get; set; }
        public bool IsThrowException { get; set; }

        private MessageDistributer()
        {

        }

        public void Start(int threadCount)
        {
            ThreadCount = threadCount < 1 ? 1 : ((threadCount > 1000) ? 1000 : threadCount);
            m_isRuning = true;
            for (int i = 0; i < ThreadCount; i++)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(MessageDistribute));
            }
            while(ActiveThreadCount < ThreadCount)
            {
                Thread.Sleep(100);
            }
        }

        private void MessageDistribute(object state)
        {
            Log.Warn("MessageDistributer thread statr");
            try
            {
                ActiveThreadCount = Interlocked.Increment(ref ActiveThreadCount);
                while(m_isRuning)
                {
                    if(m_messageQueue.Count == 0)
                    {
                        m_threadEvent.WaitOne();
                        continue;
                    }
                    MessageArgs package = m_messageQueue.Dequeue();
                    var type = package.Message.GetType();
                    RaiseMessage(package.Sender, package.Message);
                }
            }
            catch{}
            finally
            {
                ActiveThreadCount = Interlocked.Decrement(ref ActiveThreadCount);
                Log.Warn("MessageDistribute thread end");
            }
        }



        public void Subscribe<T>(MessageHandler messageHandler) where T : IMessage
        {
            Type type = typeof(T);
            if(!m_messageHandlers.ContainsKey(type))
            {
                m_messageHandlers[type] = null;
            }
            m_messageHandlers[type] += messageHandler;
        }

        public void Unsubscribe<T>(MessageHandler messageHandler) where T : IMessage
        {
            Type type = typeof(T);
            if (!m_messageHandlers.ContainsKey(type))
            {
                m_messageHandlers[type] = null;
            }
            m_messageHandlers[type] -= messageHandler;
        }

        public void RaiseMessage(NetConnection sender, IMessage message)
        {
            Type type = message.GetType();
            if(m_messageHandlers.ContainsKey(type))
            {
                MessageHandler handler = m_messageHandlers[type];
                if(handler != null)
                {
                    try
                    {
                        handler(sender, message);
                    }
                    catch(Exception e)
                    {
                        Log.Error($"Message handler exception:{e.InnerException}, {e.Message}, {e.Source}, {e.StackTrace}");
                        if (IsThrowException)
                        {
                            throw e;
                        }
                    }
                }
            }
            else
            {
                Log.Warn($"No handler subscribed for {message}");
            }
        }

        //TODO: here could be a problem 
        public void ReceiveMessage<T>(NetConnection sender, T message) where T : IMessage
        {
            m_messageQueue.Enqueue(new MessageArgs { Sender = sender, Message = message });
            //The Set method releases a single thread
            m_threadEvent.Set();
        }


        public void Stop()
        {
            m_isRuning = false;
            m_messageQueue.Clear();
            while (ActiveThreadCount > 0)
            {
                m_threadEvent.Set();
            }
            Thread.Sleep(100);
        }
    }
}
