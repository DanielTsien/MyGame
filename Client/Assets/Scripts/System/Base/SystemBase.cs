namespace MyGame
{
    public class SystemBase : ISystem
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

        void ISystem.Init()
        {
            OnInit();
        }

        protected virtual void OnInit()
        {
            
        }
    }
}