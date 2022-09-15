using System;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

namespace Utility
{
    public class Loom : MonoBehaviour
    {
        private struct LoomItem
        {
            public Action<object> Action;
            public object Param;
            public float Time;
        }
        
        private static int m_maxThreads = 8;
        private static int m_numThreads;
        private static bool m_isInited;

        private List<LoomItem> m_loomItems = new();
        private List<LoomItem> m_curLoomItems = new();

        private static Loom m_current;

        public static Loom Current
        {
            get
            {
                Initialize();
                return m_current;
            }
        }

        private static void Initialize()
        {
            if (!m_isInited)
            {
                if (!Application.isPlaying)
                {
                    return;
                }

                m_isInited = true;

                GameObject go = new GameObject(nameof(Loom));
                DontDestroyOnLoad(go);
                m_current = go.AddComponent<Loom>();
            }
        }

        public static void QueueOnMainThread(Action action, float delay = 0)
        {
            QueueOnMainThread(param => action(), delay);
        }

        public static void QueueOnMainThread(Action<object> action, float delay = 0, object param = null)
        {
            if (Current == null) return;

            lock (Current.m_loomItems)
            {
                Current.m_loomItems.Add(new LoomItem
                {
                    Action = action,
                    Param = param,
                    Time = delay,
                });
            }
        }

        public static Thread RunAsync(Action action)
        {
            Initialize();
            while (m_numThreads > m_maxThreads)
            {
                Thread.Sleep(50);
            }

            Interlocked.Increment(ref m_numThreads);
            ThreadPool.QueueUserWorkItem(RunAction, action);
            return null;
        }

        private static void RunAction(object action)
        {
            try
            {
                ((Action)action)();
            }
            catch
            {
                // ignored
            }
            finally
            {
                Interlocked.Decrement(ref m_numThreads);
            }
        }

        private void Update()
        {
            if (m_loomItems.Count == 0) return;
            float time = Time.time;
            lock (m_loomItems)
            {
                if (m_loomItems.Count > 0)
                {
                    foreach (var item in m_loomItems)
                    {
                        if (item.Time <= time)
                        {
                            m_curLoomItems.Add(item);
                        }
                    }
                }
            }

            if (m_curLoomItems.Count > 0)
            {
                foreach (var item in m_curLoomItems)
                {
                    item.Action?.Invoke(item.Param);
                }

                lock (m_loomItems)
                {
                    foreach (var item in m_curLoomItems)
                    {
                        m_loomItems.Remove(item);
                    }
                }
                m_curLoomItems.Clear();
            }
        }
    }
}