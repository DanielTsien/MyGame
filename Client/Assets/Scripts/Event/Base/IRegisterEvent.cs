using System;

namespace MyGame
{
    public interface IRegisterEvent : IGetArchitecture
    {
        
    }

    public static class RegisterEventExtension
    {
        public static void RegisterEvent<T>(this IRegisterEvent self, Action<T> onEvent)
        {
            self.GetArchitecture().RegisterEvent(onEvent);
        }

        public static void UnregisterEvent<T>(this IRegisterEvent self, Action<T> onEvent)
        {
            self.GetArchitecture().UnregisterEvent(onEvent);
        }
    }
}