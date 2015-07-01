using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Kingdom.Clockworks.Units;

namespace Kingdom.Clockworks
{
    /// <summary>
    /// Represents a simulation stopwatch. This does not depend on a live system clock, but,
    /// rather, provides a stable internal clock source for purposes of incrementally moving
    /// in internal state.
    /// </summary>
    public class SimulationStopwatch : Disposable, ISimulationStopwatch
    {
        #region Measureable Stopwatch Members

        /// <summary>
        /// Elapsed event.
        /// </summary>
        public event EventHandler<SimulatedElapsedEventArgs> Elapsed;

        /// <summary>
        /// Raises the <see cref="Elapsed"/> event. Returns the asynchronous event handler.
        /// Returns an empty task runner when the event handler is empty.
        /// </summary>
        /// <param name="e"></param>
        private IAsyncResult RaiseElapsedAsync(SimulatedElapsedEventArgs e)
        {
            return Elapsed == null
                ? Task.Run(() => { })
                : Elapsed.BeginInvoke(this, e, null, null);
        }

        /// <summary>
        /// Raises the <see cref="Elapsed"/> event.
        /// </summary>
        /// <param name="e"></param>
        private void RaiseElapsed(SimulatedElapsedEventArgs e)
        {
            if (Elapsed == null) return;
            Elapsed(this, e);
        }

        /// <summary>
        /// Gets the current Elapsed TimeSpan.
        /// </summary>
        TimeSpan IMeasurableStopwatch.Elapsed
        {
            get { return _elapsed; }
        }

        /// <summary>
        /// Gets or sets whether IsRunning. If the stopwatch can resume from the last known
        /// <see cref="Direction"/> it will. Otherwise assumes <see cref="RunningDirection.Forward"/>.
        /// </summary>
        public bool IsRunning
        {
            get
            {
                //TODO: Timeout + Direction? Timeout represents whether the _timer is moving or not.
                return Timeout.Infinite != (long) _timerIntervalTimeSpan.TotalMilliseconds
                       && Direction != null;
            }
            set
            {
                switch (_lastDirection)
                {
                    case null:
                        Direction = RunningDirection.Forward;
                        break;

                    default:
                        Direction = _lastDirection;
                        break;
                }
            }
        }

        #endregion

        #region Scalable Stopwatch Members

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numeratorUnit"></param>
        /// <param name="denominatorUnit"></param>
        /// <returns></returns>
        private double GetScaledIntervalTime(
            TimeUnit numeratorUnit = TimeUnit.Second,
            TimeUnit denominatorUnit = TimeUnit.Second)
        {
            lock (this)
            {
                // Remember the base "units" are SecondsPerSecond.
                var num = _intervalRatio.ToTimeQuantity().ToTimeQuantity(numeratorUnit);
                var denom = 1d.ToTimeQuantity().ToTimeQuantity(denominatorUnit);
                return num/denom;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="SecondsPerSecond"/> given the ratio described
        /// by <paramref name="numeratorUnit"/> and <paramref name="denominatorUnit"/>.
        /// </summary>
        /// <param name="numeratorUnit"></param>
        /// <param name="denominatorUnit"></param>
        /// <returns></returns>
        public double this[TimeUnit numeratorUnit, TimeUnit denominatorUnit]
        {
            get { return GetScaledIntervalTime(numeratorUnit, denominatorUnit); }
            set { SecondsPerSecond = value.ToTimeQuantity(numeratorUnit)/1d.ToTimeQuantity(denominatorUnit); }
        }

        /// <summary>
        /// Gets the MillisecondsPerMillisecond.
        /// </summary>
        /// <see cref="TimeUnit.Millisecond"/>
        public double MillisecondsPerMillisecond
        {
            get { return GetScaledIntervalTime(TimeUnit.Millisecond, TimeUnit.Millisecond); }
        }

        /// <summary>
        /// Gets the SecondsPerMillisecond.
        /// </summary>
        /// <see cref="TimeUnit.Millisecond"/>
        /// <see cref="TimeUnit.Second"/>
        public double SecondsPerMillisecond
        {
            get { return GetScaledIntervalTime(TimeUnit.Second, TimeUnit.Millisecond); }
        }

        /// <summary>
        /// Gets the MinutesPerMillisecond.
        /// </summary>
        /// <see cref="TimeUnit.Minute"/>
        /// <see cref="TimeUnit.Millisecond"/>
        public double MinutesPerMillisecond
        {
            get { return GetScaledIntervalTime(TimeUnit.Minute, TimeUnit.Millisecond); }
        }

        /// <summary>
        /// Gets the HoursPerMillisecond.
        /// </summary>
        /// <see cref="TimeUnit.Hour"/>
        /// <see cref="TimeUnit.Millisecond"/>
        public double HoursPerMillisecond
        {
            get { return GetScaledIntervalTime(TimeUnit.Hour, TimeUnit.Millisecond); }
        }

        /// <summary>
        /// Gets the MillisecondsPerSecond.
        /// </summary>
        /// <see cref="TimeUnit.Millisecond"/>
        /// <see cref="TimeUnit.Second"/>
        public double MillisecondsPerSecond
        {
            get { return GetScaledIntervalTime(TimeUnit.Millisecond); }
        }

        /// <summary>
        /// Gets the SecondsPerSecond. This is considered the base unit of measure
        /// for the stopwatch. All other calculations are performed with this unit
        /// of measure at its core.
        /// </summary>
        /// <see cref="TimeUnit.Second"/>
        public double SecondsPerSecond
        {
            get { return GetScaledIntervalTime(); }
            set { _intervalRatio = value; }
        }

        /// <summary>
        /// Gets the MinutesPerSecond.
        /// </summary>
        /// <see cref="TimeUnit.Minute"/>
        /// <see cref="TimeUnit.Second"/>
        public double MinutesPerSecond
        {
            get { return GetScaledIntervalTime(TimeUnit.Minute); }
        }

        /// <summary>
        /// Gets the HoursPerSecond.
        /// </summary>
        /// <see cref="TimeUnit.Hour"/>
        /// <see cref="TimeUnit.Second"/>
        public double HoursPerSecond
        {
            get { return GetScaledIntervalTime(TimeUnit.Hour); }
        }

        /// <summary>
        /// Gets the MillisecondsPerMinute.
        /// </summary>
        /// <see cref="TimeUnit.Millisecond"/>
        /// <see cref="TimeUnit.Minute"/>
        public double MillisecondsPerMinute
        {
            get { return GetScaledIntervalTime(TimeUnit.Millisecond, TimeUnit.Minute); }
        }

        /// <summary>
        /// Gets the SecondsPerMinute.
        /// </summary>
        /// <see cref="TimeUnit.Second"/>
        /// <see cref="TimeUnit.Minute"/>
        public double SecondsPerMinute
        {
            get { return GetScaledIntervalTime(TimeUnit.Second, TimeUnit.Minute); }
        }

        /// <summary>
        /// Gets the SecondsPerMinute.
        /// </summary>
        /// <see cref="TimeUnit.Minute"/>
        public double MinutesPerMinute
        {
            get { return GetScaledIntervalTime(TimeUnit.Minute, TimeUnit.Minute); }
        }

        /// <summary>
        /// Gets the HoursPerMinute.
        /// </summary>
        /// <see cref="TimeUnit.Hour"/>
        /// <see cref="TimeUnit.Minute"/>
        public double HoursPerMinute
        {
            get { return GetScaledIntervalTime(TimeUnit.Hour, TimeUnit.Minute); }
        }

        /// <summary>
        /// Gets the MillisecondsPerHour.
        /// </summary>
        /// <see cref="TimeUnit.Millisecond"/>
        /// <see cref="TimeUnit.Hour"/>
        public double MillisecondsPerHour
        {
            get { return GetScaledIntervalTime(TimeUnit.Millisecond, TimeUnit.Hour); }
        }

        /// <summary>
        /// Gets the SecondsPerHour.
        /// </summary>
        /// <see cref="TimeUnit.Second"/>
        /// <see cref="TimeUnit.Hour"/>
        public double SecondsPerHour
        {
            get { return GetScaledIntervalTime(TimeUnit.Second, TimeUnit.Hour); }
        }

        /// <summary>
        /// Gets the MinutesPerHour.
        /// </summary>
        /// <see cref="TimeUnit.Minute"/>
        /// <see cref="TimeUnit.Hour"/>
        public double MinutesPerHour
        {
            get { return GetScaledIntervalTime(TimeUnit.Minute, TimeUnit.Hour); }
        }

        /// <summary>
        /// Gets the HoursPerHour.
        /// </summary>
        /// <see cref="TimeUnit.Hour"/>
        public double HoursPerHour
        {
            get { return GetScaledIntervalTime(TimeUnit.Hour, TimeUnit.Hour); }
        }

        #endregion

        /// <summary>
        /// Default Constructor
        /// </summary>
        public SimulationStopwatch()
        {
            _intervalRatio = 1d;
            _elapsed = TimeSpan.Zero;

            _steps = 1;
            _direction = null;

            const int defaultPeriod = Timeout.Infinite;
            _timer = new Timer(StopwatchCallback, null, defaultPeriod, defaultPeriod);
        }

        #region Private Members

        //TODO: TBD: TimeSpan? or long, or double?
        /// <summary>
        /// Elapsed backing field.
        /// </summary>
        private TimeSpan _elapsed;

        /// <summary>
        /// Represents the intenral interval ratio. This is always in terms of seconds per second.
        /// </summary>
        /// <see cref="SecondsPerSecond"/>
        private double _intervalRatio;

        /// <summary>
        /// Keeps track of the <see cref="_timer"/> interval.
        /// </summary>
        private TimeSpan _timerIntervalTimeSpan;

        /// <summary>
        /// Keeps track of in which direction the stopwatch is moving.
        /// </summary>
        private RunningDirection? _direction;

        /// <summary>
        /// Keeps track of the number of steps that are occurring at every stopwatch timer event.
        /// </summary>
        private int _steps;

        /// <summary>
        /// Timer backing field.
        /// </summary>
        private readonly Timer _timer;

        /// <summary>
        /// Receives the stopwatch timer callback.
        /// </summary>
        /// <param name="state"></param>
        private void StopwatchCallback(object state)
        {
            lock (this)
            {
                Debug.Assert(state == null);
                //TODO: should consider passing both direction and steps with every increment, whether automatically timed or manual...
                Next(_steps);
            }
        }

        #endregion

        #region Simulated Stopwatch Members

        /// <summary>
        /// Starts the stopwatch timer running.
        /// The default interval is every half second or 500 milliseconds.
        /// </summary>
        public void Start()
        {
            const double interval = 500d;
            Start(TimeSpan.FromMilliseconds(interval));
        }

        /// <summary>
        /// Starts the stopwatch timer running with specified <paramref name="interval"/>.
        /// </summary>
        /// <param name="interval"></param>
        public void Start(int interval)
        {
            Start(TimeSpan.FromMilliseconds(interval));
        }

        /// <summary>
        /// Starts the stopwatch timer running with specified <paramref name="interval"/>.
        /// </summary>
        /// <param name="interval"></param>
        public void Start(long interval)
        {
            Start(TimeSpan.FromMilliseconds(interval));
        }

        /// <summary>
        /// Starts the stopwatch timer running with specified <paramref name="interval"/>.
        /// </summary>
        /// <param name="interval"></param>
        public void Start(uint interval)
        {
            Start(TimeSpan.FromMilliseconds(interval));
        }

        /// <summary>
        /// Starts the stopwatch timer running with specified <paramref name="interval"/>.
        /// </summary>
        /// <param name="interval"></param>
        public void Start(TimeSpan interval)
        {
            lock (this)
            {
                //TODO: assuming start means forward: or should it mean lastDirection|Forward
                Direction = RunningDirection.Forward;
                _timerIntervalTimeSpan = interval;
                _timer.Change(_timerIntervalTimeSpan, _timerIntervalTimeSpan);
            }
        }

        /// <summary>
        /// Stops the stopwatch timer from running.
        /// </summary>
        public void Stop()
        {
            lock (this)
            {
                const int period = Timeout.Infinite;
                _timer.Change(period, period);
            }
        }

        /// <summary>
        /// Resets the stopwatch timer.
        /// </summary>
        public void Reset()
        {
            lock (this)
            {
                _elapsed = TimeSpan.Zero;
                //TODO: reset whatever store we will use to keep track of stopwatch intervals ...
            }
        }

        #endregion

        #region Steppable Stopwatch Members

        /// <summary>
        /// Keeps track of the last <see cref="Direction"/> which is used when pausing
        /// and resuming the stopwatch from running.
        /// </summary>
        private RunningDirection? _lastDirection;

        /// <summary>
        /// Gets or sets the Direction in which the stopwatch is moving.
        /// </summary>
        /// <see cref="RunningDirection.Forward"/>
        /// <see cref="RunningDirection.Backward"/>
        public RunningDirection? Direction
        {
            get { lock (this) return _direction; }
            set
            {
                lock (this)
                {
                    if (value == _direction) return;

                    _lastDirection = value;

                    switch (_direction = value)
                    {
                        case RunningDirection.Forward:
                            _steps = Math.Abs(_steps);
                            break;

                        case RunningDirection.Backward:
                            _steps = -Math.Abs(_steps);
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the next <paramref name="steps"/> asynchronously within itself. This should
        /// permit event handlers external to the stopwatch to call back into the properties and
        /// such if necessary since the lock will have released during the event handler.
        /// </summary>
        /// <param name="steps"></param>
        private void NextAsync(int steps)
        {
            IAsyncResult elapsedRaised;

            lock (this)
            {
                var intervalQuantity = _intervalRatio.ToTimeQuantity()*steps;

                var current = _direction.HasValue ? TimeSpan.FromSeconds(intervalQuantity.Value) : TimeSpan.Zero;

                _elapsed += current;

                // Defer event handling free the lock in case anyone wants to callback into the stopwatch.
                elapsedRaised = RaiseElapsedAsync(new SimulatedElapsedEventArgs(intervalQuantity, current));
            }

            //TODO: wait with a time out?
            // Wait for the elapsed event to complete.
            elapsedRaised.AsyncWaitHandle.WaitOne();

            lock (this)
            {
                if (Elapsed == null) return;
                Elapsed.EndInvoke(elapsedRaised);
            }
        }

        /// <summary>
        /// Moves the simulation stopwatch to the next step.
        /// </summary>
        /// <param name="steps"></param>
        private void Next(int steps)
        {
            lock (this)
            {
                var intervalQuantity = _intervalRatio.ToTimeQuantity()*steps;

                var current = _direction.HasValue ? TimeSpan.FromSeconds(intervalQuantity.Value) : TimeSpan.Zero;

                _elapsed += current;

                // Defer event handling free the lock in case anyone wants to callback into the stopwatch.
                RaiseElapsed(new SimulatedElapsedEventArgs(intervalQuantity, current));
            }
        }

        /* TODO: TBD: increment/decrement? Direction? continuous/latching? or leave the previous state alone?
         * "Next" almost needs to include Direction + steps and that's it. Some of which is internal state,
         * or instantaneous, as necessary. */

        /// <summary>
        /// Moves the stopwatch forward by the specified number of <paramref name="steps"/>.
        /// Optionally specifies whether the move is <paramref name="continuous"/>.
        /// </summary>
        /// <param name="steps"></param>
        /// <param name="continuous"></param>
        public void Increment(int steps = 1, bool continuous = false)
        {
            lock (this)
            {
                var actualSteps = steps;
                if (continuous) _steps = actualSteps;
                Next(actualSteps);
            }
        }

        /// <summary>
        /// Moves the stopwatch backward by the specified number of <paramref name="steps"/>.
        /// Optionally specifies whether the move is <paramref name="continuous"/>.
        /// </summary>
        /// <param name="steps"></param>
        /// <param name="continuous"></param>
        public void Decrement(int steps = 1, bool continuous = false)
        {
            lock (this)
            {
                var actualSteps = -steps;
                if (continuous) _steps = actualSteps;
                Next(actualSteps);
            }
        }

        #endregion

        #region Disposable Members

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && !IsDisposed)
            {
                using (var signal = new AutoResetEvent(false))
                {
                    _timer.Dispose(signal);
                    signal.WaitOne();
                }
            }

            base.Dispose(disposing);
        }

        #endregion
    }
}
