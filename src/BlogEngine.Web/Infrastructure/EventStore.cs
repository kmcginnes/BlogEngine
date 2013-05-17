using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace BlogEngine
{
    public interface IApplicationService
    {
        void Execute(object command);
    }

    public interface IEventStore
    {
        EventStream LoadEventStream(string id);
        void AppendEventsToStream(string id, long expectedVersion, ICollection<Event> events);
    }

    public sealed class EventStream
    {
        public long StreamVersion;
        public List<Event> Events = new List<Event>();
    }

    public sealed class InMemoryStore : IEventStore
    {
        readonly ConcurrentDictionary<string, IList<Event>> _store = new ConcurrentDictionary<string, IList<Event>>();

        public EventStream LoadEventStream(string id)
        {
            var stream = _store.GetOrAdd(id, new Event[0]).ToList();

            return new EventStream()
            {
                Events = stream,
                StreamVersion = stream.Count
            };
        }

        public void AppendEventsToStream(string id, long expectedVersion, ICollection<Event> events)
        {
            foreach (var @event in events)
            {
                this.Log().Info("{0}", @event);
            }
            _store.AddOrUpdate(id, events.ToList(), (s, list) => list.Concat(events).ToList());
        }
    }
}