using System;

namespace MyGame
{
    public class Registration<T> : IRegistration
    {
        public Action<T> OnEvent;
    }
}