namespace Fjord.Mesa.Bus
{
    /// <summary>
    /// Marks the class with the capability to handle message <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="TMessage">type of the message this class can handle</typeparam>
    public interface IHandle<in TMessage> where TMessage : Message
    {
        void Handle(TMessage message);
    }
}