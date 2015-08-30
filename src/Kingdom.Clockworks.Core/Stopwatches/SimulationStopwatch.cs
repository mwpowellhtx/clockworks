using Kingdom.Unitworks;

namespace Kingdom.Clockworks.Stopwatches
{
    using T = Unitworks.Dimensions.Systems.Commons.Time;

    /// <summary>
    /// Represents a stopwatch used for simulation purposes. This does not depend on a live system
    /// clock, but rather provides a stable internal clock source for purposes of incrementally
    /// changing internal moments in time. <see cref="RunningDirection.Forward"/> always counts
    /// up away from zero.
    /// </summary>
    public class SimulationStopwatch
        : TimeableClockBase<StopwatchRequest, StopwatchElapsedEventArgs>
        , ISimulationStopwatch
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SimulationStopwatch()
            : this(Quantity.Zero(T.Millisecond))
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="startingQty"></param>
        public SimulationStopwatch(IQuantity startingQty)
            : base(startingQty)
        {
        }

        #region Simulated Stopwatch Members

        /// <summary>
        /// Gets the <see cref="StopwatchRequest.DefaultRequest"/> request for the stopwatch.
        /// </summary>
        protected override StopwatchRequest DefaultRequest
        {
            get { return StopwatchRequest.DefaultRequest; }
        }

        /// <summary>
        /// Gets a sufficient starting request for the stopwatch.
        /// </summary>
        protected override StopwatchRequest StartingRequest
        {
            get { return new StopwatchRequest(RunningDirection.Forward, TimePerStepQty, 1, RequestType.Continuous); }
        }

        #endregion

        #region Steppable Clock Members

        /// <summary>
        /// Returns a created <see cref="StopwatchRequest"/> given <paramref name="direction"/>,
        /// <paramref name="steps"/>, and <paramref name="type"/>.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="timePerStepQty"></param>
        /// <param name="steps"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected override StopwatchRequest CreateRequest(RunningDirection? direction = null,
            IQuantity timePerStepQty = null, int steps = 1, RequestType type = RequestType.Continuous)
        {
            return new StopwatchRequest(direction, timePerStepQty, steps, type);
        }

        /// <summary>
        /// Returns an event args to substantiate the next raised
        /// <see cref="TimeableClockBase{TRequest,TElapsedEventArgs}.Elapsed"/> event.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected override StopwatchElapsedEventArgs GetNextEventArgs(StopwatchRequest request)
        {
            lock (this)
            {
                //TODO: need to pick up the Timer Intervals: plus some understanding what to do with a "default" timer interval request
                // The important moving parts are tucked away in their single areas of responsibility.
                var currentQty = (Quantity) TimePerStepQty*IntervalTimePerTimeQty*request.Steps;
                return new StopwatchElapsedEventArgs(request, _elapsedQty += currentQty, currentQty);
            }
        }

        #endregion

        #region Overloaded Operators

        /// <summary>
        /// Increments operator overload increments the stopwatch by one.
        /// </summary>
        /// <param name="stopwatch"></param>
        /// <returns></returns>
        public static SimulationStopwatch operator ++(SimulationStopwatch stopwatch)
        {
            stopwatch.Increment();
            return stopwatch;
        }

        /// <summary>
        /// Decrements operator overload decrements the stopwatch by one.
        /// </summary>
        /// <param name="stopwatch"></param>
        /// <returns></returns>
        public static SimulationStopwatch operator --(SimulationStopwatch stopwatch)
        {
            stopwatch.Decrement();
            return stopwatch;
        }

        /// <summary>
        /// Addition operator overload increments the stopwatch by the number of <paramref name="steps"/>.
        /// </summary>
        /// <param name="stopwatch"></param>
        /// <param name="steps"></param>
        /// <returns></returns>
        public static SimulationStopwatch operator +(SimulationStopwatch stopwatch, int steps)
        {
            stopwatch.Increment(steps, stopwatch.TimePerStepQty, RequestType.Instantaneous);
            return stopwatch;
        }

        /// <summary>
        /// Subtraction operator overload decrements the stopwatch by the number of <paramref name="steps"/>.
        /// </summary>
        /// <param name="stopwatch"></param>
        /// <param name="steps"></param>
        /// <returns></returns>
        public static SimulationStopwatch operator -(SimulationStopwatch stopwatch, int steps)
        {
            stopwatch.Decrement(steps, stopwatch.TimePerStepQty, RequestType.Instantaneous);
            return stopwatch;
        }

        #endregion
    }
}
