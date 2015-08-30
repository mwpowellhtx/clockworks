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
        /// Gets the StartingQty.
        /// </summary>
        public readonly IQuantity StartingQty;

        /// <summary>
        /// Starting backing field.
        /// </summary>
        private TimeSpan? _starting;

        /// <summary>
        /// gets the Target <see cref="TimeSpan"/>.
        /// </summary>
        /// <see cref="StartingQty"/>
        public TimeSpan Starting
        {
            get { return (_starting ?? (_starting = StartingQty.ToTimeSpan())).Value; }
        }

        /// <summary>
        /// Gets the RemainingQty.
        /// </summary>
        public readonly IQuantity RemainingQty;

        /// <summary>
        /// Remaining backing field.
        /// </summary>
        private TimeSpan? _remaining;

        /// <summary>
        /// Gets the Remaining <see cref="TimeSpan"/>.
        /// </summary>
        public TimeSpan Remaining
        {
            get { return (_remaining ?? (_remaining = RemainingQty.ToTimeSpan())).Value; }
        }

        /// <summary>
        /// Internal Constructor
        /// </summary>
        /// <param name="request"></param>
        /// <param name="elapsedQty"></param>
        /// <param name="currentQty"></param>
        /// <param name="startingQty"></param>
        /// <param name="remainingQty"></param>
        internal TimerElapsedEventArgs(TimerRequest request, IQuantity elapsedQty,
            IQuantity currentQty, IQuantity startingQty, IQuantity remainingQty)
            : base(request, elapsedQty, currentQty)
        {
            StartingQty = (IQuantity) startingQty.Clone();
            RemainingQty = (IQuantity) remainingQty.Clone();
        }
    }
}
