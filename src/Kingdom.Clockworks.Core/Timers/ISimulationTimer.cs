using Kingdom.Unitworks;

namespace Kingdom.Clockworks.Timers
{
    /// <summary>
    /// Represents the sum total of simulation timer interfaces.
    /// </summary>
    public interface ISimulationTimer<out TRequest>
        : IClockBase<TRequest>
        where TRequest : TimeableRequestBase
    {
        /// <summary>
        /// Gets or sets the TargetTimeQty.
        /// </summary>
        IQuantity TargetTimeQty { get; set; }
    }
}
