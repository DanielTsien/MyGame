using System;
using System.Collections.Generic;
using Google.Protobuf;
using MyGame.Proto;

namespace MyGame
{
    public abstract class Architecture<T> : IArchitecture where T : Architecture<T>, new()
    {
        private IOCContainer m_container = new();
        private bool m_isInit;
        private List<IModel> m_models = new();
        private List<ISystem> m_systems = new();
        private readonly IEventSystem m_eventSystem = new EventSystem();
        private readonly INetworkSystem m_networkSystem = new NetworkSystem();
        private static T m_architecture;

        public static T Interface
        {
            get
            {
                if (m_architecture == null)
                {
                    MakeSureArchitecture();
                }

                return m_architecture;
            }
        }

        public void Init()
        {
            RegisterModel();
            RegisterSystem();
            RegisterUtility();
        }

        public void Connect(string ip, int port)
        {
            m_networkSystem.Connect("127.0.0.1",8000, null);
        }

        protected virtual void RegisterModel()
        {
            
        }
        
        protected virtual void RegisterSystem()
        {
            
        }
        
        protected virtual void RegisterUtility()
        {
            
        }

        private static void MakeSureArchitecture()
        {
            if (m_architecture == null)
            {
                m_architecture = new T();
                m_architecture.Init();

                foreach (var model in m_architecture.m_models)
                {
                    model.Init();
                }

                m_architecture.m_models.Clear();

                foreach (var system in m_architecture.m_systems)
                {
                    system.Init();
                }

                m_architecture.m_systems.Clear();

                m_architecture.m_networkSystem.Init();

                m_architecture.m_isInit = true;
            }
        }

        public void RegisterModel<TModel>(TModel model) where TModel : IModel
        {
            model.SetArchitecture(this);
            m_container.Register(model);
            if (m_isInit)
            {
                model.Init();
            }
            else
            {
                m_models.Add(model);
            }
        }

        public TModel GetModel<TModel>() where TModel : class, IModel
        {
            return m_container.Get<TModel>();
        }

        public void RegisterSystem<TSystem>(TSystem system) where TSystem : ISystem
        {
            system.SetArchitecture(this);
            if (m_isInit)
            {
                system.Init();
            }
            else
            {
                m_systems.Add(system);
            }
        }

        public TSystem GetSystem<TSystem>() where TSystem : class, ISystem
        {
            return m_container.Get<TSystem>();
        }

        public void RegisterUtility<TUtility>(TUtility utility) where TUtility : IUtility
        {
            m_container.Register(utility);
        }

        public TUtility GetUtility<TUtility>() where TUtility : class, IUtility
        {
            return m_container.Get<TUtility>();
        }

        public void RegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            m_eventSystem.Register(onEvent);
        }

        public void SendEvent<TEvent>() where TEvent : new()
        {
            m_eventSystem.Send<TEvent>();
        }

        public void SendEvent<TEvent>(TEvent e)
        {
            m_eventSystem.Send(e);
        }

        public void UnregisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            m_eventSystem.Unregister(onEvent);
        }

        public void SendCommand<TCommand>() where TCommand : ICommand, new()
        {
            var command = new TCommand();
            SendCommand(command);
        }

        public void SendCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            command.SetArchitecture(this);
            command.Execute();
        }

        public void RegisterNetEvent(PacketId packetId, Action<IMessage> callback)
        {
            m_networkSystem.Register(packetId, callback);
        }

        public void SendNetMessage<TMessage>(PacketId packetId) where TMessage : IMessage, new()
        {
            m_networkSystem.Send<TMessage>(packetId);
        }

        public void SendNetMessage<TMessage>(PacketId packetId, TMessage message) where TMessage : IMessage
        {
            m_networkSystem.Send(packetId, message);
        }

        public void UnregisterNetEvent(PacketId packetId, Action<IMessage> callback)
        {
            m_networkSystem.Unregister(packetId, callback);
        }

        public void Dispose()
        {
            m_models.Clear();
            m_models = null;
            m_systems.Clear();
            m_systems = null;
            m_container.Dispose();
            m_container = null;
            m_architecture = null;
        }
    }
}
