namespace EntityComponent
{
    public class BaseComponent : IComponent
    {
        public int EntityId { get; set; }
        public bool Removed { get; set; }
        public bool Enabled { get; set; }
    }
}