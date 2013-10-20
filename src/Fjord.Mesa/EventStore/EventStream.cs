using System.Collections.Generic;

namespace Fjord.Mesa.EventStore
{
    public sealed class EventStream
    {
        public long StreamVersion;
        public List<Event> Events = new List<Event>();
    }
}