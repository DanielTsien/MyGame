using MyGame.Manager;

namespace MyGame
{
    class Program
    {
        static void Main(string[] args)
        {
            DataManager.Instance.Load();
            //Log.Init("GameServer");

            //Log.Warn("Game Server Init");

            //GameServer server = new GameServer();

            //server.Start();
            //Log.Warn("Game Server Runing...");

            //CmdHelper.Run();

            //Log.Info("Game Server Exiting...");
            //server.Stop();
            //Log.Info("Game Server Exited");
        }
    }
}
