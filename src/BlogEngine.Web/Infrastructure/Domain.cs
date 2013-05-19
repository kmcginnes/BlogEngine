using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;

namespace BlogEngine
{
    // ReSharper disable InconsistentNaming
    
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
    
    // ReSharper restore InconsistentNaming

    public interface IApplicationService
    {
        void Execute(object command);
    }

    public interface IAggregateRoot
    {
        List<Event> EventsThatCausedChange { get; }
    }

    public interface IAggregateState
    {
        bool IsNew { get; }
        void Apply(Event @event);
    }

    public abstract class AggregateState : IAggregateState
    {
        public bool IsNew { get; private set; }

        public AggregateState()
        {
            IsNew = true;
        }

        [DebuggerStepThrough]
        public void Apply(Event @event)
        {
            ((dynamic)this).When((dynamic)@event);
            IsNew = false;
        }
    }

    public abstract class AggregateRoot<TState> : IAggregateRoot 
        where TState : IAggregateState, new()
    {
        protected readonly TState State;
        public List<Event> EventsThatCausedChange { get; private set; }

        public AggregateRoot(TState state)
        {
            State = state;
            EventsThatCausedChange = new List<Event>();
        }

        protected void ApplyChange(Event @event)
        {
            State.Apply(@event);
            EventsThatCausedChange.Add(@event);
        }

        protected EnsureSubject<T> Ensure<T>(T subject)
        {
            return new EnsureSubject<T>(subject);
        }
    }

    public interface IAppService
    {
        void Execute(Command command);
    }

    public abstract class AppServiceBase<TAggregateRoot> : IAppService
        where TAggregateRoot : IAggregateRoot
    {
        protected readonly IEventStore EventStore;

        public AppServiceBase(IEventStore eventStore)
        {
            EventStore = eventStore;
        }

        [DebuggerStepThrough]
        public void Execute(Command command)
        {
            ((dynamic)this).When((dynamic)command);
        }

        public IAggregateState BuildStateFromEventHistory(List<Event> eventHistory)
        {
            var aggregateStateType = typeof(TAggregateRoot).BaseType.GetGenericArguments().First();
            var aggState = Activator.CreateInstance(aggregateStateType) as IAggregateState;

            foreach (var eventThatHappened in eventHistory)
            {
                aggState.Apply(eventThatHappened);
            }
            return aggState;
        }

        // lifetime change management
        // atomic consistency boundary of an Aggregate & its contents
        protected void ChangeAggregate(
            Identity aggregateIdOf, Action<TAggregateRoot> usingThisMethod)
        {
            // Load event history
            var eventStreamId = ((dynamic)aggregateIdOf).Id.ToString();
            var eventStream = EventStore.LoadEventStream(eventStreamId);

            var aggStateBeforeChanges =
                BuildStateFromEventHistory(eventStream.Events);

            var aggregateToChange = (TAggregateRoot) Activator.CreateInstance(
                typeof (TAggregateRoot), aggStateBeforeChanges);

            // Execute command
            usingThisMethod(aggregateToChange);

            // Save results to event stream
            EventStore.AppendEventsToStream(
                eventStreamId,
                eventStream.StreamVersion,
                aggregateToChange.EventsThatCausedChange);
        }
    }

    /// <summary>
    /// Special exception that is thrown by application services
    /// when something goes wrong in an expected way. This exception
    /// bears human-readable code (the name property acting as sort of a 'key') which is used to verify it
    /// in the tests.
    /// An Exception name is a hard-coded identifier that is still human readable but is not likely to change.
    /// </summary>
    [Serializable]
    public class DomainError : Exception
    {
        public DomainError(string message) : base(message) { }

        /// <summary>
        /// Creates domain error exception with a string name that is easily identifiable in the tests
        /// </summary>
        /// <param name="name">The name to be used to identify this exception in tests.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public static DomainError Named(string name, string format, params object[] args)
        {
            return new DomainError(string.Format(format, args))
            {
                Name = name
            };
        }

        public string Name { get; private set; }

        public DomainError(string message, Exception inner) : base(message, inner) { }

        protected DomainError(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context) { }
    }
}
