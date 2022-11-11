namespace MyGame
{
    public class ModelBase : IModel
    {
        private IArchitecture m_architecture;
        public void SetArchitecture(IArchitecture architecture)
        {
            m_architecture = architecture;
        }

        public IArchitecture GetArchitecture()
        {
            return m_architecture;
        }

        void IModel.Init()
        {
            OnInit();
        }

        protected virtual void OnInit()
        {
            
        }
    }
}