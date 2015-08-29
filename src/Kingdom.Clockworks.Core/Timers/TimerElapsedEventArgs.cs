using System;
using Kingdom.Unitworks;

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
        public readonly IQuantity TargetQuantity;

        /// <summary>
        /// Target backing field.
        /// </summary>
        private TimeSpan? _target;

        /// <summary>
        /// gets the Target <see cref="TimeSpan"/>.
        /// </summary>
        public TimeSpan Target
        {
            get { return (_target ?? (_target = TargetQuantity.ToTimeSpan())).Value; }
        }

        /// <summary>
        /// Gets the RemainingQuantity.
        /// </summary>
        public readonly IQuantity RemainingQuantity;

        /// <summary>
        /// Remaining backing field.
        /// </summary>
        private TimeSpan? _remaining;

        /// <summary>
        /// Gets the Remaining <see cref="TimeSpan"/>.
        /// </summary>
        public TimeSpan Remaining
        {
            get { return (_remaining ?? (_remaining = RemainingQuantity.ToTimeSpan())).Value; }
        }

        /// <summary>
        /// Internal Constructor
        /// </summary>
        /// <param name="request"></param>
        /// <param name="elapsedQuantity"></param>
        /// <param name="currentQuantity"></param>
        /// <param name="targetQuantity"></param>
        /// <param name="remainingQuantity"></param>
        internal TimerElapsedEventArgs(TimerRequest request,
            IQuantity elapsedQuantity, IQuantity currentQuantity,
            IQuantity targetQuantity, IQuantity remainingQuantity)
            : base(request, elapsedQuantity, currentQuantity)
        {
            TargetQuantity = (IQuantity) targetQuantity.Clone();
            RemainingQuantity = (IQuantity) remainingQuantity.Clone();
        }
    }
}
