namespace EntityComponent
{
    public interface IComponent
    {
        int EntityId { get; set; }
        bool Removed { get; set; }
        bool Enabled { get; set; }
    }
}