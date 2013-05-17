using System;
using System.Collections.Generic;
using System.Linq;
using Nancy.TinyIoc;

namespace BlogEngine
{
    public static class Bus
    {
        private static ICommandSender _commandSender;
        private static IEventPublisher _eventPublisher;

        static Bus()
        {
            var nullBus = new NullBus();
            _commandSender = nullBus;
            _eventPublisher = nullBus;
        }

        public static void SetCommandSender(ICommandSender commandSender)
        {
            _commandSender = commandSender;
        }

        public static void SetEventPublisher(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
        }

        public static void Send<T>(T command) where T : Command
        {
            _commandSender.Send(command);
        }

        public static void Publish<T>(T @event) where T : Event
        {
            _eventPublisher.Publish(@event);
        }
    }

    public class NullBus : ICommandSender, IEventPublisher, IRegisterHandler
    {
        public void Send<T>(T command) where T : Command
        {
            this.Log().Error("No command sender set. Null command sender being used.");
        }
        public void Publish<T>(T @event) where T : Event
        {
            this.Log().Error("No event publisher set. Null event publisher being used.");
        }
        public void RegisterHandler<TMessage, THandler>()
            where TMessage : Message
            where THandler : IHandle<TMessage>
        {
            this.Log().Error("No register handler set. Null handler register being used.");
        }
    }

    public class InMemoryBus : ICommandSender, IEventPublisher, IRegisterHandler
    {
        private readonly TinyIoCContainer _container;

        public InMemoryBus(TinyIoCContainer container)
        {
            _container = container;
        }

        public void Send<T>(T command) where T : Command
        {
            var handlers = GetHandlers(command);
            if (handlers.Count() > 1)
                throw new InvalidOperationException("cannot send to more than one handler");
            if (handlers.Count() < 1)
                throw new InvalidOperationException("no handler registered");
            var handler = handlers.First();
            handler.When(command);
        }

        public void Publish<T>(T @event) where T : Event
        {
            var handlers = GetHandlers(@event);
            foreach (var handler in handlers)
            {
                //dispatch on thread pool for added awesomeness
                var handler1 = handler;
                //ThreadPool.QueueUserWorkItem(x => handler1.AsDynamic().Handle(@event));
                handler1.When(@event);
            }
        }

        public void RegisterHandler<TMessage, THandler>()
            where TMessage : Message
            where THandler : IHandle<TMessage>
        {
            
        }

        private IEnumerable<IHandle<Message>> GetHandlers(Message message)
        {
            var handlerType = typeof(IHandle<>).MakeGenericType(message.GetType());
            var handlers = _container.ResolveAll(handlerType) as IEnumerable<IHandle<Message>>;
            return handlers;
        }
    }

    public interface IHandle<in T> where T : Message
    {
        void When(T message);
    }

    public interface ICommandSender
    {
        void Send<T>(T command) where T : Command;

    }
    public interface IEventPublisher
    {
        void Publish<T>(T @event) where T : Event;
    }
    public interface IRegisterHandler
    {
        void RegisterHandler<TMessage, THandler>()
            where TMessage : Message
            where THandler : IHandle<TMessage>;
    }
}