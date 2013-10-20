using System;
using System.Collections.Generic;
using System.Linq;

namespace Fjord.Mesa.Bus
{
    /// <summary>
    /// Helper class, which dispatches incoming messages to subscribed message handlers.
    /// Dispatching is simply passing message to handling method on available instance.
    /// This class is ported from Event Store and is a more polished implementation
    /// of "RedirectToWhen" from Lokad.CQRS
    /// </summary>
    public sealed class InMemoryBus : IBus, IPublisher, ISubscriber, IHandle<Message>
    {
        private readonly Dictionary<Type, List<IMessageHandler>> _typeLookup = new Dictionary<Type, List<IMessageHandler>>();

        /// <summary>
        /// Subscribes handler (capable of handling messages of <typeparamref name="T"/>)
        /// to appropriate messages that go through this bus
        /// </summary>
        /// <typeparam name="T">type of message to subscribe to</typeparam>
        /// <param name="handler">instance of the handler to subscribe</param>
        public void Subscribe<T>(IHandle<T> handler) where T : Message
        {
            Ensure.NotNull(handler, "handler");
            List<IMessageHandler> handlers;
            var type = typeof(T);
            if (!_typeLookup.TryGetValue(type, out handlers))
            {
                _typeLookup.Add(type, handlers = new List<IMessageHandler>());
            }
            if (!handlers.Any(h => h.IsSame(handler)))
            {
                handlers.Add(new MessageHandler<T>(handler, handler.GetType().Name));
            }
        }

        /// <summary>
        /// Unsubscribes handler from all messages
        /// </summary>
        /// <typeparam name="T">type of the messages this handler can deal with</typeparam>
        /// <param name="handler">actual instance of handler to unsubscribe</param>
        public void Unsubscribe<T>(IHandle<T> handler) where T : Message
        {
            Ensure.NotNull(handler, "handler");
            List<IMessageHandler> list;
            if (_typeLookup.TryGetValue(typeof(T), out list))
            {
                list.RemoveAll(x => x.IsSame(handler));
            }
        }

        /// <summary>
        /// Human-readable name for debugging purposes
        /// </summary>
        public string BusName { get; private set; }

        public InMemoryBus(string name)
        {
            BusName = name;
        }

        /// <summary>
        /// Publishes instance of the message to all subscribers
        /// </summary>
        public void Publish(Message message)
        {
            Ensure.NotNull(message, "message");
            DispatchByType(message);
        }
        /// <summary>
        /// Publishes instance of the message to all subscribers
        /// </summary>
        public void Handle(Message message)
        {
            Ensure.NotNull(message, "message");
            DispatchByType(message);
        }

        void DispatchByType(Message message)
        {
            var type = message.GetType();
            do
            {
                DispatchByType(message, type);
                type = type.BaseType;
            } while (type != typeof(object));
        }

        private void DispatchByType(Message message, Type type)
        {
            List<IMessageHandler> list;
            if (!_typeLookup.TryGetValue(type, out list)) return;

            foreach (var handler in list)
            {
                handler.TryHandle(message);
            }
        }
    }
}