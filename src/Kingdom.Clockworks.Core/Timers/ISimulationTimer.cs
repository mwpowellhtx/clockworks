namespace Kingdom.Clockworks.Timers
{
    /// <summary>
    /// Represents a simulation timer interface.
    /// </summary>
    public interface ISimulationTimer<out TRequest>
        : IClockBase<TRequest>
        where TRequest : TimeableRequestBase
    {
        /// <summary>
        /// Gets or sets whether CanBeNegative.
        /// </summary>
        bool CanBeNegative { get; set; }

        /// <summary>
        /// Gets or sets whether CannotBeNegative.
        /// </summary>
        bool CannotBeNegative { get; set; }
    }
}
