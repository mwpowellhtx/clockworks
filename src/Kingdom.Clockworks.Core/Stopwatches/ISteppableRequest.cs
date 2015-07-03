using System;

namespace Kingdom.Clockworks.Stopwatches
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISteppableRequest : IEquatable<ISteppableRequest>
    {
        /// <summary>
        /// Gets the requested Direction.
        /// </summary>
        RunningDirection? Direction { get; }

        /// <summary>
        /// Gets the number of requested Steps.
        /// </summary>
        int Steps { get; }

        /// <summary>
        /// Gets whether WillRun.
        /// </summary>
        bool WillRun { get; }

        /// <summary>
        /// Gets whether WillNotRun.
        /// </summary>
        bool WillNotRun { get; }
    }
}
