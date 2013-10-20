using System.Collections.Generic;

namespace Fjord.Mesa.Domain
{
    public interface IAggregateRoot
    {
        List<Event> EventsThatCausedChange { get; }
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
}