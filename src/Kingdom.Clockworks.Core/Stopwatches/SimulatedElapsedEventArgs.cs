using System;
using Kingdom.Clockworks.Units;

namespace Kingdom.Clockworks.Stopwatches
{
    /// <summary>
    /// 
    /// </summary>
    public class SimulatedElapsedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the Request.
        /// </summary>
        public readonly StopwatchRequest Request;

        /// <summary>
        /// Gets the CurrentElapsed <see cref="TimeSpan"/>.
        /// </summary>
        public readonly TimeSpan CurrentElapsed;

        /// <summary>
        /// Gets the TotalElapsed <see cref="TimeSpan"/>.
        /// </summary>
        public readonly TimeSpan TotalElapsed;

        /// <summary>
        /// Gets the IntervalQuantity.
        /// </summary>
        public readonly TimeQuantity IntervalQuantity;

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
        internal SimulatedElapsedEventArgs(StopwatchRequest request,
            TimeQuantity intervalQuantity, TimeSpan currentElapsed,
            TimeQuantity totalElapsedQuantity, TimeSpan totalElapsed)
        {
            Request = request;
            IntervalQuantity = intervalQuantity;
            CurrentElapsed = currentElapsed;
            TotalElapsedQuantity = totalElapsedQuantity;
            TotalElapsed = totalElapsed;
        }
    }
}
