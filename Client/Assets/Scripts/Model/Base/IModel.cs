namespace MyGame
{
    public interface IModel : ISetArchitecture, IGetModel, IGetUtility, ISendEvent
    {
        void Init();
    }
}