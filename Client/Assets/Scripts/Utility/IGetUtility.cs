namespace MyGame
{
    public interface IGetUtility : IGetArchitecture
    {
        
    }

    public static class GetUtilityExtension
    {
        public static T GetUtility<T>(this IGetUtility self) where T : class, IUtility
        {
            return self.GetArchitecture().GetUtility<T>();
        }
    }
}