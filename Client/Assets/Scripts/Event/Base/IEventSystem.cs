using System;

namespace MyGame
{
    public interface IEventSystem
    {
        void Send<T>() where T : new();
        void Send<T>(T e);
        //后面考虑， 在资源回收/销毁时自动调用
        void Register<T>(Action<T> onEvent); 
        void Unregister<T>(Action<T> onEvent);
    }
}