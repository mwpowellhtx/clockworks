using System;
using Kingdom.Unitworks.Units;

namespace Kingdom.Clockworks
{
    //TODO: decide whether TimeSpans or TimeQuantity (since I've adopted that) is the thing to use...
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
        /// Gets the Elapsed <see cref="TimeSpan"/>.
        /// </summary>
        public readonly TimeSpan Elapsed;

        /// <summary>
        /// Gets the ElapsedQuantity.
        /// </summary>
        public readonly TimeQuantity ElapsedQuantity;

        /// <summary>
        /// Gets the CurrentElapsed <see cref="TimeSpan"/>.
        /// </summary>
        public readonly TimeSpan Current;

        /// <summary>
        /// Gets the IntervalQuantity.
        /// </summary>
        public readonly TimeQuantity CurrentQuantity;

        /// <summary>
        /// Internal Constructor
        /// </summary>
        /// <param name="request"></param>
        /// <param name="elapsedQuantity"></param>
        /// <param name="elapsed"></param>
        /// <param name="currentQuantity"></param>
        /// <param name="current"></param>
        protected ElapsedEventArgsBase(TRequest request,
            TimeQuantity elapsedQuantity, TimeSpan elapsed,
            TimeQuantity currentQuantity, TimeSpan current)
        {
            Request = request;
            ElapsedQuantity = elapsedQuantity;
            Elapsed = elapsed;
            CurrentQuantity = currentQuantity;
            Current = current;
        }
    }
}
