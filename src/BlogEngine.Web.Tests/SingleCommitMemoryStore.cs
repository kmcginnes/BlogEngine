using System;
using System.Collections.Generic;
using System.Linq;
using Fjord.Mesa;
using Fjord.Mesa.EventStore;

namespace BlogEngine.Web.Tests
{
    sealed class SingleCommitMemoryStore : IEventStore
    {
        public readonly IList<Tuple<string, Event>> Store = new List<Tuple<string, Event>>();
        public Event[] Appended = null;
        public void Preload(string id, Event e)
        {
            Store.Add(Tuple.Create(id, e));
        }

        EventStream IEventStore.LoadEventStream(string id)
        {
            var events = Store.Where(i => id.Equals((string) i.Item1)).Select(i => i.Item2).ToList();
            return new EventStream
                {
                    Events = events,
                    StreamVersion = events.Count
                };
        }

        void IEventStore.AppendEventsToStream(string id, long expectedVersion, ICollection<Event> events)
        {
            if (Appended != null)
                throw new InvalidOperationException("Only one commit it allowed");
            Appended = events.ToArray();
        }
    }
}