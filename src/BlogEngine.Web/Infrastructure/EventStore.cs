using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace BlogEngine
{
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
                Bus.Publish(@event);
            }
        }
    }

    /// <summary>
    /// Is thrown by event store if there were changes since our last version
    /// </summary>
    [Serializable]
    public class OptimisticConcurrencyException : Exception
    {
        public long ActualVersion { get; private set; }
        public long ExpectedVersion { get; private set; }
        public string Name { get; private set; }
        public IList<Event> ActualEvents { get; private set; }

        OptimisticConcurrencyException(string message, long actualVersion, long expectedVersion, string name,
            IList<Event> serverEvents)
            : base(message)
        {
            ActualVersion = actualVersion;
            ExpectedVersion = expectedVersion;
            Name = name;
            ActualEvents = serverEvents;
        }

        public static OptimisticConcurrencyException Create(long actual, long expected, string name,
            IList<Event> serverEvents)
        {
            var message = string.Format("Expected v{0} but found v{1} in stream '{2}'", expected, actual, name);
            return new OptimisticConcurrencyException(message, actual, expected, name, serverEvents);
        }

        protected OptimisticConcurrencyException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context) { }
    }
}