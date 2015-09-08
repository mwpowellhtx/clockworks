using System;
using Kingdom.Unitworks;
using Kingdom.Unitworks.Dimensions;

namespace Kingdom.Clockworks
{
    /// <summary>
    /// Represents the capability to step the timeable clock along.
    /// </summary>
    public interface ISteppableClock : IDisposable
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
        /// Increments the timerable clock given a number of <paramref name="steps"/> and <paramref name="type"/>.
        /// </summary>
        /// <param name="steps"></param>
        /// <param name="timePerStepQty">Represents a time component per step.</param>
        /// <param name="type"></param>
        void Increment(int steps, IQuantity timePerStepQty = null, RequestType type = RequestType.Continuous);

        /// <summary>
        /// Decrements the timerable clock given a number of <paramref name="steps"/> and <paramref name="type"/>.
        /// </summary>
        /// <param name="steps"></param>
        /// <param name="timePerStepQty">Represents a time component per step.</param>
        /// <param name="type"></param>
        void Decrement(int steps, IQuantity timePerStepQty = null, RequestType type = RequestType.Continuous);

        /// <summary>
        /// Gets or sets the <see cref="ITime"/> per step quantity.
        /// </summary>
        IQuantity TimePerStepQty { get; set; }
    }
}
