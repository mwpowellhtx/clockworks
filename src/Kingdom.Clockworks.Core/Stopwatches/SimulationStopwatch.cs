using System;
using Kingdom.Unitworks.Units;

namespace Kingdom.Clockworks.Stopwatches
{
    /// <summary>
    /// Represents a simulation stopwatch. This does not depend on a live system clock, but,
    /// rather, provides a stable internal clock source for purposes of incrementally moving
    /// in internal state.
    /// </summary>
    public class SimulationStopwatch
        : TimeableClockBase<StopwatchRequest, StopwatchElapsedEventArgs>
        , ISimulationStopwatch
    {
        #region Simulated Stopwatch Members

        /// <summary>
        /// Gets the <see cref="StopwatchRequest.Default"/> request for the stopwatch.
        /// </summary>
        protected override StopwatchRequest DefaultRequest
        {
            get { return StopwatchRequest.Default; }
        }

        /// <summary>
        /// Gets a sufficient starting request for the stopwatch.
        /// </summary>
        protected override StopwatchRequest StartingRequest
        {
            get
            {
                return new StopwatchRequest(RunningDirection.Forward,
                    MillisecondsPerStep, One, RequestType.Continuous);
            }
        }

        #endregion

        #region Steppable Clock Members

        /// <summary>
        /// Returns a created <see cref="StopwatchRequest"/> given <paramref name="direction"/>,
        /// <paramref name="steps"/>, and <paramref name="type"/>.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="millisecondsPerStep"></param>
        /// <param name="steps"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected override StopwatchRequest CreateRequest(RunningDirection? direction = null,
            double millisecondsPerStep = OneSecondMilliseconds, int steps = One,
            RequestType type = RequestType.Continuous)
        {
            return new StopwatchRequest(direction, millisecondsPerStep, steps, type);
        }

        /// <summary>
        /// Returns an event args to substantiate the next raised
        /// <see cref="TimeableClockBase{TRequest,TElapsedEventArgs}.Elapsed"/> event.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected override StopwatchElapsedEventArgs GetNextEventArgs(StopwatchRequest request)
        {
            //TODO: need to pick up the Timer Intervals: plus some understanding what to do with a "default" timer interval request
            // The important moving parts are tucked away in their single areas of responsibility.
            var currentQuantity = request.GetIntervalCandidate(MillisecondsPerStep)
                .ToTimeQuantity(TimeUnit.Millisecond)*IntervalRatio*request.Steps;

            var current = TimeSpan.FromMilliseconds(currentQuantity.Value);

            return new StopwatchElapsedEventArgs(request,
                _elapsedQuantity += currentQuantity, _elapsed += current,
                currentQuantity, current);
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
            stopwatch.Increment(steps, stopwatch.MillisecondsPerStep, RequestType.Instantaneous);
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
            stopwatch.Decrement(steps, stopwatch.MillisecondsPerStep, RequestType.Instantaneous);
            return stopwatch;
        }

        #endregion
    }
}
