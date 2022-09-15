using System;
using System.Collections.Generic;

namespace MyGame
{
    public class EventSystem : IEventSystem
    {
        private Dictionary<Type, IRegistration> m_registrations = new();

        public void Send<T>() where T : new()
        {
            var e = new T();
            Send(e);
        }

        public void Send<T>(T e)
        {
            var type = typeof(T);
            if (m_registrations.TryGetValue(type, out IRegistration registration))
            {
                ((Registration<T>) registration).OnEvent.Invoke(e);
            }
        }

        public void Register<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            if (!m_registrations.TryGetValue(type, out IRegistration registration))
            {
                registration = new Registration<T>();
                m_registrations[type] = registration;
            }

            ((Registration<T>) registration).OnEvent += onEvent;
        }

        public void Unregister<T>(Action<T> onEvent)
        {
            var type = typeof(T);
            if (m_registrations.TryGetValue(type, out IRegistration registration))
            {
                ((Registration<T>) registration).OnEvent -= onEvent;
            }
        }
    }
}