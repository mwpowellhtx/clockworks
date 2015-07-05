using System;
using Kingdom.Unitworks.Units;

namespace Kingdom.Clockworks.Timers
{
    /// <summary>
    /// Represents a simulation timer. This does not depend on a live system clock, but, rather,
    /// provides a stable internal clock source for purposes of incrementally moving in internal
    /// state.
    /// </summary>
    public class SimulationTimer
        : TimeableClockBase<TimerRequest, TimerElapsedEventArgs>
            , ISimulationTimer
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SimulationTimer()
        {
            _targetQuantity = 0d.ToTimeQuantity(TimeUnit.Millisecond);
        }

        #region Internal Timer Concerns

        /// <summary>
        /// Returns a <see cref="TimeSpan"/> factory.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        private static Func<double, TimeSpan> GetTimeSpanFactory(TimeUnit unit)
        {
            switch (unit)
            {
                case TimeUnit.Day:
                    return TimeSpan.FromDays;

                case TimeUnit.Hour:
                    return TimeSpan.FromHours;

                case TimeUnit.Minute:
                    return TimeSpan.FromMinutes;

                case TimeUnit.Second:
                    return TimeSpan.FromSeconds;

                case TimeUnit.Millisecond:
                    return TimeSpan.FromMilliseconds;
            }

            // Which indeed quantities such as "Weeks" are definitely unsupported...
            throw new InvalidOperationException(
                string.Format(@"Unsupported time unit {0}", unit));
        }

        /// <summary>
        /// Gets the remaining <see cref="TimeSpan"/> given a <paramref name="quantity"/>.
        /// </summary>
        /// <param name="quantity"></param>
        /// <returns></returns>
        private static TimeSpan GetTimeSpan(TimeQuantity quantity)
        {
            var factory = GetTimeSpanFactory(quantity.Unit);
            return factory(quantity.Value);
        }

        /// <summary>
        /// Returns an event args to substantiate the next raised
        /// <see cref="TimeableClockBase{TRequest,TElapsedEventArgs}.Elapsed"/> event.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected override TimerElapsedEventArgs GetNextEventArgs(TimerRequest request)
        {
            lock (this)
            {
                //TODO: need to pick up the Timer Intervals: plus some understanding what to do with a "default" timer interval request
                // The important moving parts are tucked away in their single areas of responsibility.
                var candidateQuantity = MillisecondsPerStep.ToTimeQuantity(TimeUnit.Millisecond)
                                        *IntervalRatio*request.Steps;

                // Constrain the Candidate quantity by the Balance between here and Starting.
                var remainingQuantity = Math.Max(0d, (_targetQuantity - _elapsedQuantity)
                    .ToTimeQuantity(TimeUnit.Millisecond).Value).ToTimeQuantity(TimeUnit.Millisecond);

                // Accepting the lesser of the two values within reach of the Starting spec.
                var currentQuantity = Math.Min(candidateQuantity.Value, remainingQuantity.Value)
                    .ToTimeQuantity(TimeUnit.Millisecond);

                var current = TimeSpan.FromMilliseconds(currentQuantity.Value);

                // Disengate the timer from running if exceeding the Starting spec.
                if (_elapsedQuantity + currentQuantity >= _targetQuantity) Stop();

                return new TimerElapsedEventArgs(request,
                    _elapsedQuantity += currentQuantity, _elapsed += current,
                    currentQuantity, current,
                    _targetQuantity, GetTimeSpan(_targetQuantity),
                    remainingQuantity, GetTimeSpan(remainingQuantity));
            }
        }

        /// <summary>
        /// Gets the DefaultRequest <see cref="TimerRequest"/>.
        /// </summary>
        protected override TimerRequest DefaultRequest
        {
            get { return TimerRequest.DefaultRequest; }
        }

        /// <summary>
        /// Gets a StartingRequest <see cref="TimerRequest"/>.
        /// </summary>
        protected override TimerRequest StartingRequest
        {
            get { return new TimerRequest(RunningDirection.Forward, 1, RequestType.Continuous); }
        }

        #endregion

        #region Timer Members

        private TimeQuantity _targetQuantity;

        /// <summary>
        /// Gets or sets the TargetMilliseconds.
        /// </summary>
        public double TargetMilliseconds
        {
            get { lock (this) return _targetQuantity.ToTimeQuantity(TimeUnit.Millisecond).Value; }
            set { lock (this) _targetQuantity = value.ToTimeQuantity(TimeUnit.Millisecond); }
        }

        #endregion

        #region Steppable Clock Members

        /// <summary>
        /// Returns a created <see cref="TimerRequest"/> given <paramref name="direction"/>,
        /// <paramref name="steps"/>, and <paramref name="type"/>.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="steps"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected override TimerRequest CreateRequest(RunningDirection? direction = null,
            int steps = 1, RequestType type = RequestType.Continuous)
        {
            return new TimerRequest(direction, steps, type);
        }

        #endregion

        #region Overloaded Operators

        /// <summary>
        /// Increments operator overload increments the timer by one.
        /// </summary>
        /// <param name="timer"></param>
        /// <returns></returns>
        public static SimulationTimer operator ++(SimulationTimer timer)
        {
            timer.Increment();
            return timer;
        }

        /// <summary>
        /// Decrements operator overload decrements the timer by one.
        /// </summary>
        /// <param name="timer"></param>
        /// <returns></returns>
        public static SimulationTimer operator --(SimulationTimer timer)
        {
            timer.Decrement();
            return timer;
        }

        /// <summary>
        /// Addition operator overload increments the timer by the number of <paramref name="steps"/>.
        /// </summary>
        /// <param name="timer"></param>
        /// <param name="steps"></param>
        /// <returns></returns>
        public static SimulationTimer operator +(SimulationTimer timer, int steps)
        {
            timer.Increment(steps, RequestType.Instantaneous);
            return timer;
        }

        /// <summary>
        /// Subtraction operator overload decrements the timer by the number of <paramref name="steps"/>.
        /// </summary>
        /// <param name="timer"></param>
        /// <param name="steps"></param>
        /// <returns></returns>
        public static SimulationTimer operator -(SimulationTimer timer, int steps)
        {
            timer.Decrement(steps, RequestType.Instantaneous);
            return timer;
        }

        #endregion
    }
}
