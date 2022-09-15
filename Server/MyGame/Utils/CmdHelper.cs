using System;

namespace MyGame
{
    internal class CmdHelper
    {
        public static void Run()
        {
            bool run = true;
            while(run)
            {
                Console.WriteLine(">");
                string line = Console.ReadLine();
                switch(line.ToLower().Trim())
                {
                    case "exit":
                        run = false;
                        break;
                    default:
                        Help();
                        break;
                }
            }
        }

        private static void Help()
        {
            Console.WriteLine(@"
Help:
    exit    Exit Game Server
    help    Show Help
");
        }
    }
}
