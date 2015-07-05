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
            get { return new StopwatchRequest(RunningDirection.Forward, 1, RequestType.Continuous); }
        }

        #endregion

        #region Steppable Clock Members

        /// <summary>
        /// Returns a created <see cref="StopwatchRequest"/> given <paramref name="direction"/>,
        /// <paramref name="steps"/>, and <paramref name="type"/>.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="steps"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected override StopwatchRequest CreateRequest(RunningDirection? direction = null,
            int steps = 1, RequestType type = RequestType.Continuous)
        {
            return new StopwatchRequest(direction, steps, type);
        }

        /// <summary>
        /// Returns an event args to substantiate the next raised
        /// <see cref="TimeableClockBase{TRequest,TElapsedEventArgs}.Elapsed"/> event.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected override StopwatchElapsedEventArgs GetNextEventArgs(StopwatchRequest request)
        {
            // The important moving parts are tucked away in their single areas of responsibility.
            var currentQuantity = IntervalRatio.ToTimeQuantity()
                .ToTimeQuantity(TimeUnit.Millisecond)*request.Steps;

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
            stopwatch.Increment(steps, RequestType.Instantaneous);
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
            stopwatch.Decrement(steps, RequestType.Instantaneous);
            return stopwatch;
        }

        #endregion
    }
}
