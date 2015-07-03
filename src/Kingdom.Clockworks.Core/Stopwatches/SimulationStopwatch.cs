using System;
using System.Collections.Concurrent;
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
            get { lock (this) return _elapsed; }
        }

        /// <summary>
        /// Gets the ElapsedQuantity <see cref="TimeQuantity"/>.
        /// </summary>
        TimeQuantity IMeasurableStopwatch.ElapsedQuantity
        {
            get { lock (this) return _elapsedQuantity; }
        }

        /// <summary>
        /// Gets the CurrentRequest.
        /// </summary>
        StopwatchRequest IMeasurableStopwatch.CurrentRequest
        {
            get { lock (this) return _lastRequest; }
        }

        /// <summary>
        /// Gets whether the stopwatch IsRunning.
        /// </summary>
        public bool IsRunning
        {
            get
            {
                lock (this)
                {
                    //TODO: Timeout + Direction? Timeout represents whether the _timer is moving or not.
                    return Timeout.Infinite != (long) _timerIntervalTimeSpan.TotalMilliseconds
                           && (_lastRequest != null && _lastRequest.WillRun);
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
            _elapsedQuantity = 0d.ToTimeQuantity(TimeUnit.Millisecond);

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
        /// ElapsedQuantity backing field.
        /// </summary>
        private TimeQuantity _elapsedQuantity;

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
        /// Timer backing field.
        /// </summary>
        private readonly Timer _timer;

        /// <summary>
        /// Requests backing field.
        /// </summary>
        private readonly ConcurrentStack<StopwatchRequest> _requests = new ConcurrentStack<StopwatchRequest>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private StopwatchRequest GetNextRequest(StopwatchRequest request)
        {
            _requests.Push(request);
            return GetNextRequest();
        }

        /// <summary>
        /// LastRequest backing field.
        /// </summary>
        private StopwatchRequest _lastRequest;

        /// <summary>
        /// Returns the next <see cref="StopwatchRequest"/>.
        /// </summary>
        /// <returns></returns>
        private StopwatchRequest GetNextRequest()
        {
            if (_requests.IsEmpty)
                return _lastRequest = StopwatchRequest.Default;

            StopwatchRequest request;

            // ReSharper disable once RedundantAssignment
            var popped = _requests.TryPop(out request);
            Debug.Assert(popped);
            Debug.Assert(request != null);

            // Push it back onto the stack if continuous.
            if (request.IsContinuous)
                _requests.Push(request);

            return _lastRequest = request;
        }

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
                Next(GetNextRequest());
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
                var request = GetNextRequest();

                if (request.WillNotRun)
                {
                    _requests.Clear();
                    _requests.Push(new StopwatchRequest(RunningDirection.Forward, 1, RequestType.Continuous));
                }

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

                if (GetNextRequest().WillRun) _requests.Clear();
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
                _elapsedQuantity = 0d.ToTimeQuantity(TimeUnit.Millisecond);
            }
        }

        #endregion

        #region Steppable Stopwatch Members

        /// <summary>
        /// This portion of the stopwatch is consistent regardless of whether we are
        /// talking about <see cref="Next"/> or <see cref="NextAsync"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private SimulatedElapsedEventArgs GetNextEventArgs(ISteppableRequest request)
        {
            // The important moving parts are tucked away in their single areas of responsibility.
            var intervalQuantity = _intervalRatio.ToTimeQuantity()
                .ToTimeQuantity(TimeUnit.Millisecond)*request.Steps;

            var current = TimeSpan.FromMilliseconds(intervalQuantity.Value);

            return new SimulatedElapsedEventArgs(
                request as StopwatchRequest, intervalQuantity, current,
                _elapsedQuantity += intervalQuantity, _elapsed += current);
        }

        /// <summary>
        /// Processes the next stopwatch <paramref name="request"/> asynchronously.
        /// </summary>
        /// <param name="request"></param>
        private void NextAsync(ISteppableRequest request)
        {
            IAsyncResult elapsedRaised;

            lock (this)
            {
                // Defer event handling free the lock in case anyone wants to callback into the stopwatch.
                elapsedRaised = RaiseElapsedAsync(GetNextEventArgs(request));
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
        /// Processes the next stopwatch <paramref name="request"/>.
        /// </summary>
        /// <param name="request"></param>
        private void Next(ISteppableRequest request)
        {
            lock (this)
            {
                // Defer event handling free the lock in case anyone wants to callback into the stopwatch.
                RaiseElapsed(GetNextEventArgs(request));
            }
        }

        /* TODO: TBD: increment/decrement? Direction? continuous/latching? or leave the previous state alone?
         * "Next" almost needs to include Direction + steps and that's it. Some of which is internal state,
         * or instantaneous, as necessary. */

        /// <summary>
        /// Increments the stopwatch by one <see cref="RequestType.Instantaneous"/> step.
        /// </summary>
        public void Increment()
        {
            Increment(1, RequestType.Instantaneous);
        }

        /// <summary>
        /// Decrements the stopwatch by one <see cref="RequestType.Instantaneous"/> step.
        /// </summary>
        public void Decrement()
        {
            Decrement(1, RequestType.Instantaneous);
        }

        /// <summary>
        /// Increments the stopwatch given a number of <paramref name="steps"/> and <paramref name="type"/>.
        /// </summary>
        /// <param name="steps"></param>
        /// <param name="type"></param>
        public void Increment(int steps, RequestType type = RequestType.Continuous)
        {
            lock (this)
            {
                Next(GetNextRequest(new StopwatchRequest(RunningDirection.Forward, steps, type)));
            }
        }

        /// <summary>
        /// Decrements the stopwatch given a number of <paramref name="steps"/> and <paramref name="type"/>.
        /// </summary>
        /// <param name="steps"></param>
        /// <param name="type"></param>
        public void Decrement(int steps, RequestType type = RequestType.Continuous)
        {
            lock (this)
            {
                Next(GetNextRequest(new StopwatchRequest(RunningDirection.Backward, steps, type)));
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
