namespace MyGame
{
    public interface IGetSystem : IGetArchitecture
    {

    }

    public static class GetSystemExtension
    {
        public static T GetSystem<T>(this IGetSystem self) where T : class, ISystem
        {
            return self.GetArchitecture().GetSystem<T>();
        }
    }
}