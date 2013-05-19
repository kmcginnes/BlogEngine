using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BlogEngine
{
    public interface IAppService
    {
        void Execute(Command command);
    }

    public abstract class AppServiceBase<TAggregateRoot> : IAppService
        where TAggregateRoot : IAggregateRoot
    {
        protected readonly IEventStore EventStore;

        protected AppServiceBase(IEventStore eventStore)
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

            var aggregateToChange = (TAggregateRoot)Activator.CreateInstance(
                typeof(TAggregateRoot), aggStateBeforeChanges);

            // Execute command
            usingThisMethod(aggregateToChange);

            // Save results to event stream
            EventStore.AppendEventsToStream(
                eventStreamId,
                eventStream.StreamVersion,
                aggregateToChange.EventsThatCausedChange);
        }
    }
}