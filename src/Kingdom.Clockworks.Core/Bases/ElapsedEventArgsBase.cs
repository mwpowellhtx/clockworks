using System;
using Kingdom.Unitworks;

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
        /// Elapsed backing field.
        /// </summary>
        private TimeSpan? _elapsed;

        /// <summary>
        /// Gets the Elapsed <see cref="TimeSpan"/>.
        /// </summary>
        public TimeSpan Elapsed
        {
            get { return (_elapsed ?? (_elapsed = ElapsedQty.ToTimeSpan())).Value; }
        }

        /// <summary>
        /// Gets the ElapsedQty.
        /// </summary>
        public readonly IQuantity ElapsedQty;

        /// <summary>
        /// Current backing field.
        /// </summary>
        private TimeSpan? _current;

        /// <summary>
        /// Gets the CurrentElapsed <see cref="TimeSpan"/>.
        /// </summary>
        public TimeSpan Current
        {
            get { return (_current ?? (_current = CurrentQty.ToTimeSpan())).Value; }
        }

        /// <summary>
        /// Gets the CurrentQty.
        /// </summary>
        public readonly IQuantity CurrentQty;

        /// <summary>
        /// Internal Constructor
        /// </summary>
        /// <param name="request"></param>
        /// <param name="elapsedQty"></param>
        /// <param name="currentQty"></param>
        protected ElapsedEventArgsBase(TRequest request,
            IQuantity elapsedQty, IQuantity currentQty)
        {
            Request = request;
            ElapsedQty = (IQuantity) elapsedQty.Clone();
            CurrentQty = (IQuantity) currentQty.Clone();
        }
    }
}
