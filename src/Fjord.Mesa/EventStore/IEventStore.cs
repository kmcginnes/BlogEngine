using System.Collections.Generic;

namespace Fjord.Mesa.EventStore
{
    public interface IEventStore
    {
        EventStream LoadEventStream(string id);
        void AppendEventsToStream(string id, long expectedVersion, ICollection<Event> events);
    }
}