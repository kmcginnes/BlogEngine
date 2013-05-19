using System.Diagnostics;

namespace BlogEngine
{
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
}