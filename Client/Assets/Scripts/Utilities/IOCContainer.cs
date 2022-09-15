using System;
using System.Collections.Generic;

namespace MyGame
{
    public class IOCContainer
    {
        private Dictionary<Type, object> m_container = new();

        public void Register<T>(T instance)
        {
            Type type = typeof(T);
            m_container[type] = instance;
        }

        public T Get<T>() where T : class
        {
            var type = typeof(T);
            if (m_container.TryGetValue(type, out object result))
            {
                return result as T;
            }

            return null;
        }

        public void Dispose()
        {
            m_container = null;
        }
    }
}