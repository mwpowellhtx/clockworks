namespace Kingdom.Clockworks.Stopwatches
{
    /// <summary>
    /// Represents the capability to step the stopwatch along.
    /// </summary>
    public interface ISteppableStopwatch
    {
        /// <summary>
        /// Increments the stopwatch by one <see cref="RequestType.Instantaneous"/> step.
        /// </summary>
        void Increment();

        /// <summary>
        /// Decrements the stopwatch by one <see cref="RequestType.Instantaneous"/> step.
        /// </summary>
        void Decrement();

        /// <summary>
        /// Increments the stopwatch given a number of <paramref name="steps"/> and <paramref name="type"/>.
        /// </summary>
        /// <param name="steps"></param>
        /// <param name="type"></param>
        void Increment(int steps, RequestType type = RequestType.Continuous);

        /// <summary>
        /// Decrements the stopwatch given a number of <paramref name="steps"/> and <paramref name="type"/>.
        /// </summary>
        /// <param name="steps"></param>
        /// <param name="type"></param>
        void Decrement(int steps, RequestType type = RequestType.Continuous);
    }
}
