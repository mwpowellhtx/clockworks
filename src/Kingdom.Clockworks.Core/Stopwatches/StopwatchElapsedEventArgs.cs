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
        /// <param name="currentQuantity"></param>
        internal StopwatchElapsedEventArgs(StopwatchRequest request,
            TimeQuantity elapsedQuantity, TimeQuantity currentQuantity)
            : base(request, elapsedQuantity, currentQuantity)
        {
        }
    }
}
