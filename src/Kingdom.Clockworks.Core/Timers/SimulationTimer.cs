using System;
using Kingdom.Clockworks.Units;

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
            _startingTimerQuantity = 0d.ToTimeQuantity(TimeUnit.Millisecond);
        }

        #region Internal Timer Concerns

        /// <summary>
        /// Returns a remaining time factory.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        private static Func<double, TimeSpan> GetRemainingTimeSpanFactory(TimeUnit unit)
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
        private static TimeSpan GetRemainingTimeSpan(TimeQuantity quantity)
        {
            var factory = GetRemainingTimeSpanFactory(quantity.Unit);
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
                // The important moving parts are tucked away in their single areas of responsibility.
                var candidateQuantity = IntervalRatio.ToTimeQuantity()
                    .ToTimeQuantity(TimeUnit.Millisecond)*request.Steps;

                // Constrain the Candidate quantity by the Balance between here and Starting.
                var remainingQuantity = Math.Max(0d, (_startingTimerQuantity - _elapsedQuantity)
                    .ToTimeQuantity(TimeUnit.Millisecond).Value).ToTimeQuantity(TimeUnit.Millisecond);

                // Accepting the lesser of the two values within reach of the Starting spec.
                var intervalQuantity = Math.Min(candidateQuantity.Value, remainingQuantity.Value)
                    .ToTimeQuantity(TimeUnit.Millisecond);

                var current = TimeSpan.FromMilliseconds(intervalQuantity.Value);

                _elapsedQuantity += intervalQuantity;
                _elapsed += current;

                // Disengate the timer from running if exceeding the Starting spec.
                if (_elapsedQuantity >= _startingTimerQuantity) Stop();

                return new TimerElapsedEventArgs(request, intervalQuantity, current,
                    remainingQuantity, GetRemainingTimeSpan(remainingQuantity));
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
            throw new NotImplementedException();
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

        #region Startable Clock Members

        /// <summary>
        /// StartingTimerQuantity backing field.
        /// </summary>
        private TimeQuantity _startingTimerQuantity;

        /// <summary>
        /// Resets the stopwatch timer.
        /// </summary>
        /// <param name="elapsedMilliseconds"></param>
        public override void Reset(double elapsedMilliseconds)
        {
            lock (this)
            {
                base.Reset(elapsedMilliseconds);
                _startingTimerQuantity = elapsedMilliseconds.ToTimeQuantity(TimeUnit.Millisecond);
            }
        }

        #endregion
    }
}
