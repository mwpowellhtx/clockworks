using Kingdom.Unitworks;

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
        /// 
        /// </summary>
        IQuantity TimePerStepQty { get; set; }
    }
}
