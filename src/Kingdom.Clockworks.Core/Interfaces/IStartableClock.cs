using System;

namespace Kingdom.Clockworks
{
    /// <summary>
    /// 
    /// </summary>
    public interface IStartableClock
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
        /// <param name="elapsedMilliseconds"></param>
        void Reset(double elapsedMilliseconds);
    }
}
