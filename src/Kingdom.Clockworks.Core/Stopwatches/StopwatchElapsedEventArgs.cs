using System;
using Kingdom.Unitworks.Units;

namespace Kingdom.Clockworks.Stopwatches
{
    /// <summary>
    /// 
    /// </summary>
    public class StopwatchElapsedEventArgs : ElapsedEventArgsBase<StopwatchRequest>
    {
        /// <summary>
        /// Internal Constructor
        /// </summary>
        /// <param name="request"></param>
        /// <param name="elapsedQuantity"></param>
        /// <param name="elapsed"></param>
        /// <param name="currentQuantity"></param>
        /// <param name="current"></param>
        internal StopwatchElapsedEventArgs(StopwatchRequest request,
            TimeQuantity elapsedQuantity, TimeSpan elapsed,
            TimeQuantity currentQuantity, TimeSpan current)
            : base(request, elapsedQuantity, elapsed, currentQuantity, current)
        {
        }
    }
}
