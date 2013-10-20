namespace Fjord.Mesa.Bus
{
    /// <summary>
    /// Defines instance of the message handling object (class which
    /// contains code to be executed when some message arrives)
    /// </summary>
    internal interface IMessageHandler
    {
        /// <summary>
        /// Name of this handler (for debugging purposes)
        /// </summary>
        string HandlerName { get; }
        /// <summary>
        /// Attempts to handle the message
        /// </summary>
        /// <param name="message">actuall message to handle</param>
        /// <returns>True if handled</returns>
        bool TryHandle(Message message);
        /// <summary>
        /// Explicit comparison between multiple handlers (to allow
        /// preventing multiple calls in complex subscription scenarios)
        /// </summary>
        /// <param name="handler">another handler object</param>
        /// <returns>True if both handlers are the same</returns>
        bool IsSame(object handler);
    }
}