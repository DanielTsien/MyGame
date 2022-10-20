using MyGame;

namespace DefaultNamespace
{
    public class Game : Architecture<Game>
    {
        protected override void RegisterModel()
        {
            RegisterModel<IUserModel>(new UserModel());
            RegisterModel<ICharacterConfigModel>(new CharacterConfigModel());
            RegisterModel<IMapConfigModel>(new MapConfigModel());
        }

        protected override void RegisterSystem()
        {
            RegisterSystem<IUserSystem>(new UserSystem());
            RegisterSystem<IMapSystem>(new MapSystem());
        }

        protected override void RegisterUtility()
        {
            
        }
    }
}