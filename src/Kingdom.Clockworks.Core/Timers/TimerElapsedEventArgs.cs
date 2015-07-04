using System;
using Kingdom.Clockworks.Units;

namespace Kingdom.Clockworks.Timers
{
    /// <summary>
    /// 
    /// </summary>
    public class TimerElapsedEventArgs : ElapsedEventArgsBase<TimerRequest>
    {
        /// <summary>
        /// Gets the TotalRemaining <see cref="TimeSpan"/>.
        /// </summary>
        public readonly TimeSpan TotalRemaining;

        /// <summary>
        /// Gets the TotalRemainingQuantity.
        /// </summary>
        public readonly TimeQuantity TotalRemainingQuantity;

        /// <summary>
        /// Internal Constructor
        /// </summary>
        /// <param name="request"></param>
        /// <param name="intervalQuantity"></param>
        /// <param name="currentElapsed"></param>
        /// <param name="totalRemainingQuantity"></param>
        /// <param name="totalRemaining"></param>
        internal TimerElapsedEventArgs(TimerRequest request, TimeQuantity intervalQuantity,
            TimeSpan currentElapsed, TimeQuantity totalRemainingQuantity, TimeSpan totalRemaining)
            : base(request, intervalQuantity, currentElapsed)
        {
            TotalRemaining = totalRemaining;
            TotalRemainingQuantity = totalRemainingQuantity;
        }
    }
}
