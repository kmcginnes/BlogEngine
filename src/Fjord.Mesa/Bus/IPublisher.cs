namespace Fjord.Mesa.Bus
{
    /// <summary>
    /// Provides publishing interface (can accept message and send them around)
    /// </summary>
    public interface IPublisher
    {
        /// <summary>
        /// Publishes instance of the message
        /// </summary>
        void Publish(Message message);
    }
}