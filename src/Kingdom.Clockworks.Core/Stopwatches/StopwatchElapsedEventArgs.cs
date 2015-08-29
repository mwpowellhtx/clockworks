using Kingdom.Unitworks;

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
        /// <param name="elapsedQty"></param>
        /// <param name="currentQty"></param>
        internal StopwatchElapsedEventArgs(StopwatchRequest request,
            IQuantity elapsedQty, IQuantity currentQty)
            : base(request, elapsedQty, currentQty)
        {
        }
    }
}
