using System;
using System.Reflection;

namespace MyGame.Utils
{
    internal class Singleton<T> where T : Singleton<T>
    {
        private static T m_instance;

        public static T Instance
        {
            get
            {
                if(m_instance == null)
                {
                    ConstructorInfo[] ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
                    ConstructorInfo ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);
                    if(ctor == null)
                    {
                        throw new Exception($"NonPublic Constructor not find in {typeof(T)}");
                    }
                    m_instance = ctor.Invoke(null) as T;
                }
                return m_instance;
            }
        }
    }
}
