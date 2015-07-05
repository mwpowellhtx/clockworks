using System;
using Kingdom.Unitworks.Units;

namespace Kingdom.Clockworks.Timers
{
    /// <summary>
    /// 
    /// </summary>
    public class TimerElapsedEventArgs : ElapsedEventArgsBase<TimerRequest>
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly TimeQuantity TargetQuantity;

        /// <summary>
        /// gets the Target <see cref="TimeSpan"/>.
        /// </summary>
        public readonly TimeSpan Target;

        /// <summary>
        /// Gets the RemainingQuantity.
        /// </summary>
        public readonly TimeQuantity RemainingQuantity;

        /// <summary>
        /// Gets the Remaining <see cref="TimeSpan"/>.
        /// </summary>
        public readonly TimeSpan Remaining;

        /// <summary>
        /// Internal Constructor
        /// </summary>
        /// <param name="request"></param>
        /// <param name="elapsedQuantity"></param>
        /// <param name="elapsed"></param>
        /// <param name="currentQuantity"></param>
        /// <param name="current"></param>
        /// <param name="targetQuantity"></param>
        /// <param name="target"></param>
        /// <param name="remainingQuantity"></param>
        /// <param name="remaining"></param>
        internal TimerElapsedEventArgs(TimerRequest request,
            TimeQuantity elapsedQuantity, TimeSpan elapsed,
            TimeQuantity currentQuantity, TimeSpan current,
            TimeQuantity targetQuantity, TimeSpan target,
            TimeQuantity remainingQuantity, TimeSpan remaining)
            : base(request, elapsedQuantity, elapsed, currentQuantity, current)
        {
            Target = target;
            TargetQuantity = targetQuantity;
            //TODO: may even be able to do a simple calculation for any of these...
            Remaining = remaining;
            RemainingQuantity = remainingQuantity;
        }
    }
}
