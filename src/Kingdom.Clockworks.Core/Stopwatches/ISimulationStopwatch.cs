using System;

namespace Kingdom.Clockworks
{
    //TODO: may need/want high resolution capability besides DateTime ("low" resolution)...

    /// <summary>
    /// Represents the sum total of simulation stopwatch interfaces.
    /// </summary>
    public interface ISimulationStopwatch
        : IScalableStopwatch
            , ISteppableStopwatch
            , IMeasurableStopwatch
            , IDisposable
    {
        /// <summary>
        /// Starts the stopwatch running.
        /// </summary>
        void Start();

        /// <summary>
        /// Starts the stopwatch running using the specified <paramref name="interval"/>.
        /// </summary>
        /// <param name="interval"></param>
        void Start(int interval);

        /// <summary>
        /// Starts the stopwatch running using the specified <paramref name="interval"/>.
        /// </summary>
        /// <param name="interval"></param>
        void Start(long interval);

        /// <summary>
        /// Starts the stopwatch running using the specified <paramref name="interval"/>.
        /// </summary>
        /// <param name="interval"></param>
        void Start(uint interval);

        /// <summary>
        /// Starts the stopwatch running using the specified <paramref name="interval"/>.
        /// </summary>
        /// <param name="interval"></param>
        void Start(TimeSpan interval);

        /// <summary>
        /// Stops the stopwatch from running.
        /// </summary>
        void Stop();

        /// <summary>
        /// Resets the stopwatch.
        /// </summary>
        void Reset();
    }
}
