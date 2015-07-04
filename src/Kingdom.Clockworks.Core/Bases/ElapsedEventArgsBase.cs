using System;
using Kingdom.Clockworks.Units;

namespace Kingdom.Clockworks
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    public abstract class ElapsedEventArgsBase<TRequest> : EventArgs
        where TRequest : TimeableRequestBase
    {
        /// <summary>
        /// Gets the Request.
        /// </summary>
        public readonly TRequest Request;

        /// <summary>
        /// Gets the CurrentElapsed <see cref="TimeSpan"/>.
        /// </summary>
        public readonly TimeSpan CurrentElapsed;

        /// <summary>
        /// Gets the IntervalQuantity.
        /// </summary>
        public readonly TimeQuantity IntervalQuantity;

        /// <summary>
        /// Internal Constructor
        /// </summary>
        /// <param name="request"></param>
        /// <param name="intervalQuantity"></param>
        /// <param name="currentElapsed"></param>
        protected ElapsedEventArgsBase(TRequest request,
            TimeQuantity intervalQuantity, TimeSpan currentElapsed)
        {
            Request = request;
            IntervalQuantity = intervalQuantity;
            CurrentElapsed = currentElapsed;
        }
    }
}
