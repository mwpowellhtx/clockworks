namespace Kingdom.Clockworks.Timers
{
    /// <summary>
    /// Represents a simulation timer interface.
    /// </summary>
    public interface ISimulationTimer
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

    /// <summary>
    /// Represents a simulation timer interface.
    /// </summary>
    public interface ISimulationTimer<out TRequest>
        : ISimulationTimer
            , IClockBase<TRequest>
        where TRequest : TimeableRequestBase
    {
    }
}
