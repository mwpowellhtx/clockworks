
using Kingdom.Unitworks;

namespace Kingdom.Clockworks
{
    /// <summary>
    /// Represents the scalability of a clock.
    /// </summary>
    public interface IScaleableClock
    {
        /// <summary>
        /// Gets or sets how much time should pass between clock intervals. This may
        /// be as compressed or expanded as is necessary to perform simulation.
        /// </summary>
        IQuantity IntervalTimePerTimeQty { get; set; }
    }
}
