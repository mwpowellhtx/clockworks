using System;
using Kingdom.Clockworks.Units;

namespace Kingdom.Clockworks
{
    /// <summary>
    /// Provides a way of measuring the stopwatch behavior.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    public interface IMeasurableClock<TRequest>
        where TRequest : TimeableRequestBase
    {
        /// <summary>
        /// Gets the Elapsed <see cref="TimeSpan"/>.
        /// </summary>
        TimeSpan Elapsed { get; }

        /// <summary>
        /// Gets the ElapsedQuantity <see cref="TimeQuantity"/>.
        /// </summary>
        TimeQuantity ElapsedQuantity { get; }

        /// <summary>
        /// Gets whether the stopwatch IsRunning.
        /// </summary>
        bool IsRunning { get; }

        /// <summary>
        /// Gets the CurrentRequest.
        /// </summary>
        TRequest CurrentRequest { get; }
    }
}
