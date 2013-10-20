namespace Fjord.Mesa.Bus
{
    /// <summary>
    /// Inheritors can manage subscriptions for various handlers
    /// </summary>
    public interface ISubscriber
    {
        /// <summary>
        /// Subscribes handler (capable of handling messages of <typeparamref name="T"/>)
        /// to appropriate messages
        /// </summary>
        /// <typeparam name="T">type of message to subscribe to</typeparam>
        /// <param name="handler">instance of the handler to subscribe</param>
        void Subscribe<T>(IHandle<T> handler) where T : Message;
        /// <summary>
        /// Unsubscribes handler from all messages
        /// </summary>
        /// <typeparam name="T">type of the messages this handler can deal with</typeparam>
        /// <param name="handler">actual instance of handler to unsubscribe</param>
        void Unsubscribe<T>(IHandle<T> handler) where T : Message;
    }
}