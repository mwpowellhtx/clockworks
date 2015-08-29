using Kingdom.Unitworks;

namespace Kingdom.Clockworks.Timers
{
    using T = Unitworks.Dimensions.Systems.Commons.Time;

    /// <summary>
    /// Represents a simulation timer. This does not depend on a live system clock, but, rather,
    /// provides a stable internal clock source for purposes of incrementally moving in internal
    /// state.
    /// </summary>
    public class SimulationTimer
        : TimeableClockBase<TimerRequest, TimerElapsedEventArgs>
            , ISimulationTimer<TimerRequest>
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public SimulationTimer()
        {
            _targetTimeQty = Quantity.Zero(T.Millisecond);
        }

        #region Internal Timer Concerns

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
                var ms = T.Millisecond;
                //TODO: need to pick up the Timer Intervals: plus some understanding what to do with a "default" timer interval request
                // The important moving parts are tucked away in their single areas of responsibility.
                var candidateQty = (Quantity) request.TimePerStepQty.ConvertTo(ms)*IntervalTimePerTimeQty*request.Steps;

                // Constrain the Candidate quantity by the Balance between here and Starting.
                var remainingQty = Quantity.Min(Quantity.Zero(ms), (Quantity) _targetTimeQty - _elapsedQty);

                // Accepting the lesser of the two values within reach of the Starting spec.
                var currentQty = Quantity.Min(candidateQty, remainingQty).ConvertTo(T.Millisecond);

                _elapsedQty += (Quantity) currentQty;

                // Disengate the timer from running if exceeding the Starting spec.
                if ((_elapsedQty + (Quantity) currentQty).ConvertTo(T.Millisecond).Value
                    >= _targetTimeQty.ConvertTo(T.Millisecond).Value)
                {
                    //TODO: TBD: would be much better to have logical operators implemented on the qty itself...
                    Stop();
                }

                return new TimerElapsedEventArgs(request, _elapsedQty, currentQty, _targetTimeQty, remainingQty);
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
            get
            {
                return new TimerRequest(RunningDirection.Forward, TimePerStepQty, 1, RequestType.Continuous);
            }
        }

        #endregion

        #region Timer Members

        private IQuantity _targetTimeQty;

        /// <summary>
        /// Gets or sets the TargetTimeQty.
        /// </summary>
        public IQuantity TargetTimeQty
        {
            get { lock (this) return _targetTimeQty; }
            set
            {
                lock (this)
                {
                    var ms = T.Millisecond;
                    _targetTimeQty = (value ?? Quantity.Zero(ms)).ConvertTo(ms);
                }
            }
        }

        #endregion

        #region Steppable Clock Members

        /// <summary>
        /// Returns a created <see cref="TimerRequest"/> given <paramref name="direction"/>,
        /// <paramref name="steps"/>, and <paramref name="type"/>.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="timePerStepQty"></param>
        /// <param name="steps"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected override TimerRequest CreateRequest(RunningDirection? direction = null,
            IQuantity timePerStepQty = null, int steps = 1, RequestType type = RequestType.Continuous)
        {
            return new TimerRequest(direction, timePerStepQty, steps, type);
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
            timer.Increment(steps, timer.TimePerStepQty, RequestType.Instantaneous);
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
            timer.Decrement(steps, timer.TimePerStepQty, RequestType.Instantaneous);
            return timer;
        }

        #endregion
    }
}
