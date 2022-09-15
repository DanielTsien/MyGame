using Network;

namespace MyGame
{
    public interface IController : IGetArchitecture, IGetModel, IGetUtility, IGetSystem, IRegisterEvent, ISendEvent, ISendCommand, IRegisterNetEvent, ISendNetMessage
    {
        
    }
}