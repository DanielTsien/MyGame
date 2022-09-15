namespace MyGame
{
    public interface ICommand : ISetArchitecture, IGetArchitecture
    {
        void Execute();
    }
}