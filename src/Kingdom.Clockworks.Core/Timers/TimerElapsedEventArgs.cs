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
        public readonly TimeQuantity RemainingQuantity;

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
            TimeQuantity elapsedQuantity, TimeQuantity currentQuantity,
            TimeQuantity targetQuantity, TimeQuantity remainingQuantity)
            : base(request, elapsedQuantity, currentQuantity)
        {
            TargetQuantity = targetQuantity;
            RemainingQuantity = remainingQuantity;
        }
    }
}
