namespace Kingdom.Clockworks
{
    /// <summary>
    /// Represents the capability to step the timeable clock along.
    /// </summary>
    public interface ISteppableClock
    {
        #region Per Step Members

        /// <summary>
        /// Gets or sets the Milliseconds per Step.
        /// </summary>
        double MillisecondsPerStep { get; set; }

        /// <summary>
        /// Gets or sets the Seconds per Step.
        /// </summary>
        double SecondsPerStep { get; set; }

        /// <summary>
        /// Gets or sets the Minutes per Step.
        /// </summary>
        double MinutesPerStep { get; set; }

        /// <summary>
        /// Gets or sets the Hours per Step.
        /// </summary>
        double HoursPerStep { get; set; }

        #endregion

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
        /// <param name="millisecondsPerStep">Represents the milliseconds per step.</param>
        /// <param name="type"></param>
        void Increment(int steps, double millisecondsPerStep = TimeableClockBase.OneSecondMilliseconds,
            RequestType type = RequestType.Continuous);

        /// <summary>
        /// Decrements the timeable clock given a number of <paramref name="steps"/> and <paramref name="type"/>.
        /// </summary>
        /// <param name="steps"></param>
        /// <param name="millisecondsPerStep">Represents the milliseconds per step.</param>
        /// <param name="type"></param>
        void Decrement(int steps, double millisecondsPerStep = TimeableClockBase.OneSecondMilliseconds,
            RequestType type = RequestType.Continuous);
    }
}
