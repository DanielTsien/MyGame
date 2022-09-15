namespace MyGame
{
    public interface ISendCommand : IGetArchitecture
    {
        
    }

    public static class SendCommandExtension
    {
        public static void SendCommand<T>(this ISendCommand self) where T : ICommand, new()
        {
            self.GetArchitecture().SendCommand<T>();
        }

        public static void SendCommand<T>(this ISendCommand self, T command) where T : ICommand
        {
            self.GetArchitecture().SendCommand(command);
        }
    }
}