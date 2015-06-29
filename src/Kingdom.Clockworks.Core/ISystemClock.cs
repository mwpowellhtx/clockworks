using System;

namespace Kingdom.Clockworks
{
    /// <summary>
    /// Represents a disposable system clock. Use this for purposes of overriding
    /// the system clock, such as during unit and integration tests.
    /// </summary>
    public interface ISystemClock : IDisposable
    {
        /// <summary>
        /// Gets the overridden Now as a local time.
        /// </summary>
        DateTime Now { get; }

        /// <summary>
        /// Gets the overridden UtcNow as Coordinated Universal Time (UTC).
        /// </summary>
        DateTime UtcNow { get; }
    }
}
