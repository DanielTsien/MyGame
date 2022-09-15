using MyGame;

namespace DefaultNamespace
{
    public class Game : Architecture<Game>
    {
        protected override void RegisterModel()
        {
            RegisterModel<IUserModel>(new UserModel());
        }

        protected override void RegisterSystem()
        {
            RegisterSystem<IUserSystem>(new UserSystem());
        }

        protected override void RegisterUtility()
        {
            
        }
    }
}