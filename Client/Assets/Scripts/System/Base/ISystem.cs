using Network;

namespace MyGame
{
    public interface ISystem : ISetArchitecture, IGetModel, IGetUtility, ISendEvent, IRegisterEvent, IGetSystem, IRegisterNetEvent
    {
        void Init();
    }
}