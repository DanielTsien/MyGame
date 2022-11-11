namespace MyGame
{
    public abstract class CommandBase : ICommand
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

        void ICommand.Execute()
        {
            OnExecute();
        }
        
        protected virtual void OnExecute()
        {
            
        }
    }
}