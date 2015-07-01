using System;
using Kingdom.Clockworks.Units;

namespace Kingdom.Clockworks
{
    /// <summary>
    /// 
    /// </summary>
    public class SimulatedElapsedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the Elapsed time span.
        /// </summary>
        public readonly TimeSpan Elapsed;

        /// <summary>
        /// 
        /// </summary>
        public readonly TimeQuantity IntervalQuantity;

        /// <summary>
        /// Internal Constructor
        /// </summary>
        /// <param name="elapsed"></param>
        internal SimulatedElapsedEventArgs(TimeQuantity intervalQuantity, TimeSpan elapsed)
        {
            IntervalQuantity = intervalQuantity;
            Elapsed = elapsed;
        }
    }
}
