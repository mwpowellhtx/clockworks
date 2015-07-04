using System;
using Kingdom.Clockworks.Units;

namespace Kingdom.Clockworks.Stopwatches
{
    /// <summary>
    /// 
    /// </summary>
    public class StopwatchElapsedEventArgs : ElapsedEventArgsBase<StopwatchRequest>
    {
        /// <summary>
        /// Gets the TotalElapsed <see cref="TimeSpan"/>.
        /// </summary>
        public readonly TimeSpan TotalElapsed;

        /// <summary>
        /// Gets the TotalElapsedQuantity.
        /// </summary>
        public readonly TimeQuantity TotalElapsedQuantity;

        /// <summary>
        /// Internal Constructor
        /// </summary>
        /// <param name="request"></param>
        /// <param name="intervalQuantity"></param>
        /// <param name="currentElapsed"></param>
        /// <param name="totalElapsedQuantity"></param>
        /// <param name="totalElapsed"></param>
        internal StopwatchElapsedEventArgs(StopwatchRequest request, TimeQuantity intervalQuantity,
            TimeSpan currentElapsed, TimeQuantity totalElapsedQuantity, TimeSpan totalElapsed)
            : base(request, intervalQuantity, currentElapsed)
        {
            TotalElapsed = totalElapsed;
            TotalElapsedQuantity = totalElapsedQuantity;
        }
    }
}
