using System;

namespace Fjord.Mesa
{
    public interface ISystemClock
    {
        DateTime GetUtcNow();
    }

    public sealed class RealSystemClock : ISystemClock
    {
        public DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}

