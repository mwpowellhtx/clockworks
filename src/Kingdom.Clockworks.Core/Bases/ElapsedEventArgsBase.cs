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
        /// Elapsed backing field.
        /// </summary>
        private TimeSpan? _elapsed;

        /// <summary>
        /// Gets the Elapsed <see cref="TimeSpan"/>.
        /// </summary>
        public TimeSpan Elapsed
        {
            get { return (_elapsed ?? (_elapsed = ElapsedQuantity.ToTimeSpan())).Value; }
        }

        /// <summary>
        /// Gets the ElapsedQuantity.
        /// </summary>
        public readonly TimeQuantity ElapsedQuantity;

        /// <summary>
        /// Current backing field.
        /// </summary>
        private TimeSpan? _current;

        /// <summary>
        /// Gets the CurrentElapsed <see cref="TimeSpan"/>.
        /// </summary>
        public TimeSpan Current
        {
            get { return (_current ?? (_current = CurrentQuantity.ToTimeSpan())).Value; }
        }

        /// <summary>
        /// Gets the IntervalQuantity.
        /// </summary>
        public readonly TimeQuantity CurrentQuantity;

        /// <summary>
        /// Internal Constructor
        /// </summary>
        /// <param name="request"></param>
        /// <param name="elapsedQuantity"></param>
        /// <param name="currentQuantity"></param>
        protected ElapsedEventArgsBase(TRequest request,
            TimeQuantity elapsedQuantity, TimeQuantity currentQuantity)
        {
            Request = request;
            ElapsedQuantity = elapsedQuantity;
            CurrentQuantity = currentQuantity;
        }
    }
}
