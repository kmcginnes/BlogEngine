namespace BlogEngine
{
    //// ReSharper disable InconsistentNaming
    
    public interface Message {}
    public interface Command : Message {}
    public interface Event : Message {}
    public interface Command<out TIdentity> : Command where TIdentity : Identity
    {
        TIdentity Id { get; }
    }
    public interface Event<out TIdentity> : Event where TIdentity : Identity
    {
        TIdentity Id { get; }
    }
    
    //// ReSharper restore InconsistentNaming
}
