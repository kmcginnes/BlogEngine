using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Fjord.Mesa.Bus;

namespace Fjord.Mesa.EventStore
{
    public sealed class InMemoryStore : IEventStore
    {
        private readonly IHandle<Message> _handler;
        readonly ConcurrentDictionary<string, IList<Event>> _store = new ConcurrentDictionary<string, IList<Event>>();

        public InMemoryStore(IHandle<Message> handler)
        {
            _handler = handler;
        }

        public EventStream LoadEventStream(string id)
        {
            var stream = _store.GetOrAdd(id, new Event[0]).ToList();

            return new EventStream
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
            
            foreach (var @event in events)
            {
                _handler.Handle(@event);
            }
        }
    }
}