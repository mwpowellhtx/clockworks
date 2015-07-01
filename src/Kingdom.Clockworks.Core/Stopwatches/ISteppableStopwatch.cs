namespace Kingdom.Clockworks
{
    /// <summary>
    /// Represents the capability to step the stopwatch along.
    /// </summary>
    public interface ISteppableStopwatch
    {
        /// <summary>
        /// Increments the stopwatch using a specified number of <paramref name="steps"/>.
        /// Optionally, this request may perform on a <paramref name="continuous"/> basis.
        /// </summary>
        /// <param name="steps"></param>
        /// <param name="continuous"></param>
        void Increment(int steps = 1, bool continuous = true);

        /// <summary>
        /// Decrements the stopwatch using a specified number of <paramref name="steps"/>.
        /// Optionally, this request may perform on a <paramref name="continuous"/> basis.
        /// </summary>
        /// <param name="steps"></param>
        /// <param name="continuous"></param>
        void Decrement(int steps = 1, bool continuous = true);

        /// <summary>
        /// Gets or sets the Direction in whcih the stopwatch is moving.
        /// </summary>
        RunningDirection? Direction { get; set; }
    }
}
