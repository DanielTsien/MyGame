namespace MyGame
{
    public interface ISendEvent : IGetArchitecture
    {
        
    }

    public static class SendEventExtension
    {
        public static void SendEvent<T>(this ISendEvent self) where T : new()
        {
            self.GetArchitecture().SendEvent<T>();
        }

        public static void SendEvent<T>(this ISendEvent self, T e)
        {
            self.GetArchitecture().SendEvent(e);
        }
    }
}