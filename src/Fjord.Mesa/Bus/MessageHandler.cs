namespace Fjord.Mesa.Bus
{
    /// <summary>
    /// Possible implementation of typed <see cref="IMessageHandler"/>
    /// which can handle only a single type of message and allows providing 
    /// human-readable name
    /// </summary>
    /// <typeparam name="T">type of the message this class can deal with</typeparam>
    public sealed class MessageHandler<T> : IMessageHandler where T : Message
    {

        readonly IHandle<T> _handler;
        public MessageHandler(IHandle<T> handler, string handlerName)
        {
            HandlerName = handlerName ?? "";
            _handler = handler;
        }

        public string HandlerName { get; private set; }

        /// <summary>
        /// Attempts to handle the message
        /// </summary>
        /// <param name="message">actuall message to handle</param>
        /// <returns>True if handled</returns>
        public bool TryHandle(Message message)
        {
            var msg = message as T;

            if (msg != null)
            {
                _handler.Handle(msg);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Explicit comparison between multiple handlers (to allow
        /// preventing multiple calls in complex subscription scenarios)
        /// </summary>
        /// <param name="handler">another handler object</param>
        /// <returns>True if both handlers are the same</returns>
        public bool IsSame(object handler)
        {
            return ReferenceEquals(_handler, handler);
        }

        public override string ToString()
        {
            return string.IsNullOrEmpty(HandlerName) ? _handler.ToString() : HandlerName;
        }
    }
}