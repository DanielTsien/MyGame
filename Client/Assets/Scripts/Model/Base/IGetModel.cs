using UnityEditor.UI;

namespace MyGame
{
    public interface IGetModel : IGetArchitecture
    {
        
    }

    public static class GetModelExtension
    {
        public static T GetModel<T>(this IGetModel self) where T : class, IModel
        {
            return self.GetArchitecture().GetModel<T>();
        }
    }
}