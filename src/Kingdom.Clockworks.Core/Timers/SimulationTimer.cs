using Kingdom.Unitworks;
using Kingdom.Unitworks.Dimensions.Systems.Commons;

namespace Kingdom.Clockworks.Timers
{
    using T = Time;

    /// <summary>
    /// Represents a timer used for simulation purposes. This does not depend on a live system
    /// clock, but rather provides a stable internal clock source for purposes of incrementally
    /// changing internal moments in time. <see cref="RunningDirection.Forward"/> always counts
    /// down towards zero.
    /// </summary>
    public class SimulationTimer
        : TimeableClockBase<TimerRequest, TimerElapsedEventArgs>
            , ISimulationTimer<TimerRequest>
    {
        /// <summary>
        /// CanBeNegative backing field.
        /// </summary>
        /// <see cref="CanBeNegative"/>
        /// <see cref="CannotBeNegative"/>
        private bool _canBeNegative;

        /// <summary>
        /// Gets or sets whether CanBeNegative.
        /// </summary>
        public bool CanBeNegative
        {
            get { return _canBeNegative; }
            set { _canBeNegative = value; }
        }

        /// <summary>
        /// Gets or sets whether CannotBeNegative.
        /// </summary>
        public bool CannotBeNegative
        {
            get { return !_canBeNegative; }
            set { _canBeNegative = !value; }
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SimulationTimer()
            : this(null)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="startingQty">Provide a starting <see cref="Time"/> based quantity. Default is zero.</param>
        public SimulationTimer(IQuantity startingQty)
            : base(startingQty)
        {
            CanBeNegative = true;
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
                var candidateQty = (Quantity) request.TimePerStepQty*IntervalTimePerTimeQty*request.Steps;

                IQuantity remainingQty = (Quantity) StartingQty - _elapsedQty;
                var currentQty = candidateQty;

                if (CannotBeNegative)
                {
                    // Stop the timer as soon as we discover it should be stopped.
                    if ((Quantity) _elapsedQty + candidateQty >= StartingQty)
                        Stop();

                    remainingQty = Quantity.Min(Quantity.Zero(T.Millisecond), remainingQty);
                    currentQty = (Quantity) Quantity.Max(candidateQty, remainingQty);
                }

                // Which by now current is what it should be.
                _elapsedQty += currentQty;

                // This looks like a recalculation of the same but it may not be, especially if constrained by zero.
                var trueRemainingQty = (Quantity) StartingQty - _elapsedQty;

                return new TimerElapsedEventArgs(request, _elapsedQty, currentQty, StartingQty, trueRemainingQty);
            }
        }

        /// <summary>
        /// Gets the <see cref="TimerRequest.DefaultRequest"/>.
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
