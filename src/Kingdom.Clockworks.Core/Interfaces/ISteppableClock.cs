namespace Kingdom.Clockworks
{
    /// <summary>
    /// Represents the capability to step the timeable clock along.
    /// </summary>
    public interface ISteppableClock
    {
        /// <summary>
        /// Increments the timeable clock by one <see cref="RequestType.Instantaneous"/> step.
        /// </summary>
        void Increment();

        /// <summary>
        /// Decrements the timeable clock by one <see cref="RequestType.Instantaneous"/> step.
        /// </summary>
        void Decrement();

        /// <summary>
        /// Increments the timeable clock given a number of <paramref name="steps"/> and <paramref name="type"/>.
        /// </summary>
        /// <param name="steps"></param>
        /// <param name="type"></param>
        void Increment(int steps, RequestType type = RequestType.Continuous);

        /// <summary>
        /// Decrements the timeable clock given a number of <paramref name="steps"/> and <paramref name="type"/>.
        /// </summary>
        /// <param name="steps"></param>
        /// <param name="type"></param>
        void Decrement(int steps, RequestType type = RequestType.Continuous);
    }
}
