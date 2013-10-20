namespace Fjord.Mesa.Bus
{
    /// <summary>
    /// Named instance, which can both publish messages and sibscribe handlers to them
    /// </summary>
    public interface IBus : IPublisher, ISubscriber
    {
        /// <summary>
        /// Human-readable name for debugging purposes
        /// </summary>
        string BusName { get; }
    }
}