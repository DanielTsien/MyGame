using System;
using UnityEngine;

namespace Singleton
{
    public abstract  class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T m_instance;

        public static T Instance
        {
            get
            {
                if (m_instance == null)
                {
                    m_instance = FindObjectOfType<T>();
                    if (m_instance == null)
                    {
                        GameObject go = new GameObject(typeof(T).Name);
                        DontDestroyOnLoad(go);
                        m_instance = go.AddComponent<T>();
                    }
                    m_instance.OnSingletonInit();
                }
                return m_instance;
            }
        }

        public virtual void OnSingletonInit()
        {
            
        }

        public virtual void Dispose()
        {
            Destroy(gameObject);
        }

        protected virtual void OnDestroy()
        {
            m_instance = null;
        }

        protected virtual void OnApplicationQuit()
        {
            if(m_instance == null) return;
            
            Destroy(m_instance.gameObject);
            m_instance = null;
        }
    }
}