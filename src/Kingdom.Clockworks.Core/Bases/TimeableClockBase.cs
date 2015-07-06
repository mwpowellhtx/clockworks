using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Kingdom.Unitworks.Units;

namespace Kingdom.Clockworks
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class TimeableClockBase
        : Disposable
            , IStartableClock
            , ISteppableClock
    {
        private static double VerifyPositive(double value)
        {
            if (value >= 0d) return value;
            throw new ArgumentException(@"value must be positive", @"value");
        }

        /// <summary>
        /// Protected Constructor
        /// </summary>
        protected TimeableClockBase()
        {
            SecondsPerStep = 1d;
            IntervalRatio = 1d;

            const int defaultPeriod = Timeout.Infinite;
            _timer = new Timer(ProtectedTimerCallback, null, defaultPeriod, defaultPeriod);

            Reset(0d);
        }

        #region Internal Members

        /// <summary>
        /// ProtectedTimerCallback
        /// </summary>
        /// <param name="state"></param>
        protected abstract void ProtectedTimerCallback(object state);

        /// <summary>
        /// TimerIntervalQuantity backing field.
        /// </summary>
        private TimeQuantity _timerIntervalQuantity;

        /// <summary>
        /// Gets or sets the TimerIntervalQuantity.
        /// Sets in terms of <see cref="TimeUnit.Millisecond"/> or Null.
        /// </summary>
        protected TimeQuantity TimerIntervalQuantity
        {
            get { return _timerIntervalQuantity; }
            set
            {
                _timerIntervalQuantity = value == null
                    ? null
                    : value.ToTimeQuantity(TimeUnit.Millisecond);
            }
        }

        /// <summary>
        /// Gets the TimerIntervalMilliseconds if possible.
        /// Gets Null if there is no interval currently set.
        /// </summary>
        /// <see cref="TimerIntervalQuantity"/>
        protected double? TimerIntervalMilliseconds
        {
            get
            {
                return _timerIntervalQuantity == null
                    ? (double?) null
                    : _timerIntervalQuantity.Value;
            }
        }

        /// <summary>
        /// Timer backing field.
        /// </summary>
        private readonly Timer _timer;

        /// <summary>
        /// Tries to change the timer.
        /// </summary>
        /// <param name="dueTime"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        protected bool TryChangeTimer(int dueTime, int period)
        {
            return _timer.Change(dueTime, period);
        }

        /// <summary>
        /// Tries to change the timer.
        /// </summary>
        /// <param name="dueTime"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        protected bool TryChangeTimer(TimeSpan dueTime, TimeSpan period)
        {
            return _timer.Change(dueTime, period);
        }

        /// <summary>
        /// IntervalRatio backing field.
        /// </summary>
        private double _intervalRatio;

        /// <summary>
        /// Represents the intenral interval ratio. This is always in terms of seconds per second.
        /// </summary>
        /// <see cref="SecondsPerSecond"/>
        protected double IntervalRatio
        {
            get { lock (this) return _intervalRatio; }
            private set { lock (this) _intervalRatio = VerifyPositive(value); }
        }

        #endregion

        #region Scaleable Clock Members

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
                var num = IntervalRatio.ToTimeQuantity().ToTimeQuantity(numeratorUnit);
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

        //TODO: per second? or per steps? remember, the component I am forgetting about is the timer period in calculating elapsed parts... which is either instant (per request) or timed (with timer component)
        /// <summary>
        /// Gets the SecondsPerSecond. This is considered the base unit of measure for the
        /// timerable clock. All other calculations are performed with this unit of measure
        /// at its core.
        /// </summary>
        /// <see cref="TimeUnit.Second"/>
        public double SecondsPerSecond
        {
            get { return GetScaledIntervalTime(); }
            set { IntervalRatio = value; }
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

        #region Startable Clock Members

        /// <summary>
        /// Starts the timerable clock timer running. The default interval is every
        /// half second, or 500 milliseconds.
        /// </summary>
        public void Start()
        {
            const double interval = 500d;
            Start(TimeSpan.FromMilliseconds(interval));
        }

        /// <summary>
        /// Starts the timerable clock timer running with specified <paramref name="interval"/>.
        /// </summary>
        /// <param name="interval"></param>
        public void Start(int interval)
        {
            Start(TimeSpan.FromMilliseconds(interval));
        }

        /// <summary>
        /// Starts the timerable clock timer running with specified <paramref name="interval"/>.
        /// </summary>
        /// <param name="interval"></param>
        public void Start(long interval)
        {
            Start(TimeSpan.FromMilliseconds(interval));
        }

        /// <summary>
        /// Starts the timerable clock timer running with specified <paramref name="interval"/>.
        /// </summary>
        /// <param name="interval"></param>
        public void Start(uint interval)
        {
            Start(TimeSpan.FromMilliseconds(interval));
        }

        /// <summary>
        /// Starts the timeable clock timer running with specified <paramref name="interval"/>.
        /// </summary>
        /// <param name="interval"></param>
        public abstract void Start(TimeSpan interval);

        /// <summary>
        /// Stops the timerable clock timer from running.
        /// </summary>
        public abstract void Stop();

        /// <summary>
        /// Gets or sets the Elapsed <see cref="TimeSpan"/>.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        protected TimeSpan _elapsed;

        /// <summary>
        /// Gets or sets the ElapsedQuantity.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        protected TimeQuantity _elapsedQuantity;

        /// <summary>
        /// Resets the timeable clock.
        /// </summary>
        /// <param name="elapsedMilliseconds"></param>
        public void Reset(double elapsedMilliseconds)
        {
            lock (this)
            {
                _elapsed = TimeSpan.Zero;
                _elapsedQuantity = elapsedMilliseconds.ToTimeQuantity(TimeUnit.Millisecond);
            }
        }

        #endregion

        #region Steppable Clock Members

        #region Per Step Members

        /// <summary>
        /// TimePerStep backing field.
        /// </summary>
        private double _secondsPerStep;

        /// <summary>
        /// Returns the <see cref="SecondsPerStep"/> in terms of the <paramref name="unit"/>.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        private double GetTimePerStep(TimeUnit unit)
        {
            lock (this) return SecondsPerStep.ToTimeQuantity().ToTimeQuantity(unit).Value;
        }

        /// <summary>
        /// Sets the <see cref="SecondsPerStep"/> in terms of the <paramref name="unit"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="unit"></param>
        private void SetTimePerStep(double value, TimeUnit unit)
        {
            lock (this) SecondsPerStep = value.ToTimeQuantity(unit).ToTimeQuantity().Value;
        }

        /// <summary>
        /// Gets or sets the Milliseconds per Step.
        /// </summary>
        public double MillisecondsPerStep
        {
            get { return GetTimePerStep(TimeUnit.Millisecond); }
            set { SetTimePerStep(value, TimeUnit.Millisecond); }
        }

        /// <summary>
        /// Gets or sets the Seconds per Step.
        /// </summary>
        public double SecondsPerStep
        {
            get { lock (this) return _secondsPerStep; }
            set { lock (this) _secondsPerStep = VerifyPositive(value); }
        }

        /// <summary>
        /// Gets or sets the Minutes per Step.
        /// </summary>
        public double MinutesPerStep
        {
            get { return GetTimePerStep(TimeUnit.Minute); }
            set { SetTimePerStep(value, TimeUnit.Minute); }
        }

        /// <summary>
        /// Gets or sets the Hours per Step.
        /// </summary>
        public double HoursPerStep
        {
            get { return GetTimePerStep(TimeUnit.Hour); }
            set { SetTimePerStep(value, TimeUnit.Hour); }
        }

        #endregion

        /* TODO: TBD: increment/decrement? Direction? continuous/latching? or leave the previous state alone?
         * "Next" almost needs to include Direction + steps and that's it. Some of which is internal state,
         * or instantaneous, as necessary. */

        /// <summary>
        /// Increments the timerable clock by one <see cref="RequestType.Instantaneous"/> step.
        /// </summary>
        public void Increment()
        {
            Increment(One, MillisecondsPerStep, RequestType.Instantaneous);
        }

        /// <summary>
        /// Decrements the timerable clock by one <see cref="RequestType.Instantaneous"/> step.
        /// </summary>
        public void Decrement()
        {
            Decrement(One, MillisecondsPerStep, RequestType.Instantaneous);
        }

        /// <summary>
        /// One: 1
        /// </summary>
        protected const int One = 1;

        /// <summary>
        /// OneSecondMilliseconds: 1000d
        /// </summary>
        /// <see cref="One"/>
        protected internal const double OneSecondMilliseconds = One*1000d;

        /// <summary>
        /// Increments the timerable clock given a number of <paramref name="steps"/> and <paramref name="type"/>.
        /// </summary>
        /// <param name="steps"></param>
        /// <param name="millisecondsPerStep">Represents the milliseconds per step.</param>
        /// <param name="type"></param>
        public abstract void Increment(int steps, double millisecondsPerStep = OneSecondMilliseconds,
            RequestType type = RequestType.Continuous);

        /// <summary>
        /// Decrements the timerable clock given a number of <paramref name="steps"/> and <paramref name="type"/>.
        /// </summary>
        /// <param name="steps"></param>
        /// <param name="millisecondsPerStep">Represents the milliseconds per step.</param>
        /// <param name="type"></param>
        public abstract void Decrement(int steps, double millisecondsPerStep = OneSecondMilliseconds,
            RequestType type = RequestType.Continuous);

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

    /// <summary>
    /// 
    /// </summary>
    public abstract class TimeableClockBase<TRequest, TElapsedEventArgs>
        : TimeableClockBase
            , IScaleableClock
            , IMeasurableClock<TRequest>
        where TRequest : TimeableRequestBase
        where TElapsedEventArgs : EventArgs
    {
        #region Measurable Clock Members

        /// <summary>
        /// Elapsed event.
        /// </summary>
        public event EventHandler<TElapsedEventArgs> Elapsed;

        /// <summary>
        /// Raises the <see cref="Elapsed"/> event. Returns the asynchronous event handler.
        /// Returns an empty task runner when the event handler is empty.
        /// </summary>
        /// <param name="e"></param>
        private IAsyncResult RaiseElapsedAsync(TElapsedEventArgs e)
        {
            return Elapsed == null
                ? Task.Run(() => { })
                : Elapsed.BeginInvoke(this, e, null, null);
        }

        /// <summary>
        /// Raises the <see cref="Elapsed"/> event.
        /// </summary>
        /// <param name="e"></param>
        private void RaiseElapsed(TElapsedEventArgs e)
        {
            if (Elapsed == null) return;
            Elapsed(this, e);
        }

        /// <summary>
        /// Gets whether the timerable clock IsRunning.
        /// </summary>
        public bool IsRunning
        {
            get
            {
                lock (this)
                {
                    //TODO: Timeout + Direction? Timeout represents whether the _timer is moving or not.
                    return TimerIntervalMilliseconds.HasValue
                           && (_lastRequest != null && _lastRequest.WillRun);
                }
            }
        }

        /// <summary>
        /// Gets the current Elapsed TimeSpan.
        /// </summary>
        TimeSpan IMeasurableClock<TRequest>.Elapsed
        {
            get { lock (this) return _elapsed; }
        }

        /// <summary>
        /// Gets the ElapsedQuantity <see cref="TimeQuantity"/>.
        /// </summary>
        TimeQuantity IMeasurableClock<TRequest>.ElapsedQuantity
        {
            get { lock (this) return _elapsedQuantity; }
        }

        /// <summary>
        /// Gets the CurrentRequest.
        /// </summary>
        TRequest IMeasurableClock<TRequest>.CurrentRequest
        {
            get { lock (this) return _lastRequest; }
        }

        #endregion

        /// <summary>
        /// Protected Constructor
        /// </summary>
        protected TimeableClockBase()
        {
            _elapsed = TimeSpan.Zero;
            _elapsedQuantity = 0d.ToTimeQuantity(TimeUnit.Millisecond);
        }

        #region Internal Timer Concerns

        /// <summary>
        /// This portion of the timerable clock is consistent regardless of whether we are
        /// talking about <see cref="Next"/> or <see cref="NextAsync"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected abstract TElapsedEventArgs GetNextEventArgs(TRequest request);

        /// <summary>
        /// Requests backing field.
        /// </summary>
        private readonly ConcurrentStack<TRequest> _requests = new ConcurrentStack<TRequest>();

        /// <summary>
        /// Protected clock timer callback.
        /// </summary>
        /// <param name="state"></param>
        protected override void ProtectedTimerCallback(object state)
        {
            lock (this)
            {
                Debug.Assert(state == null);
                //TODO: should consider passing both direction and steps with every increment, whether automatically timed or manual...
                Next(GetNextRequest());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private TRequest GetNextRequest(TRequest request)
        {
            _requests.Push(request);
            return GetNextRequest();
        }

        /// <summary>
        /// Processes the next timerable clock <paramref name="request"/> asynchronously.
        /// </summary>
        /// <param name="request"></param>
        private void NextAsync(TRequest request)
        {
            IAsyncResult elapsedRaised;

            lock (this)
            {
                // Defer event handling free the lock in case anyone wants to callback into the timerable clock.
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
        /// Processes the next timerable clock <paramref name="request"/>.
        /// </summary>
        /// <param name="request"></param>
        private void Next(TRequest request)
        {
            lock (this)
            {
                // Defer event handling free the lock in case anyone wants to callback into the timerable clock.
                RaiseElapsed(GetNextEventArgs(request));
            }
        }

        /// <summary>
        /// Gets the DefaultRequest.
        /// </summary>
        protected abstract TRequest DefaultRequest { get; }

        /// <summary>
        /// Gets a StartingRequest.
        /// </summary>
        protected abstract TRequest StartingRequest { get; }

        /// <summary>
        /// LastRequest backing field.
        /// </summary>
        private TRequest _lastRequest;

        /// <summary>
        /// Returns the next <see cref="TRequest"/>.
        /// </summary>
        /// <returns></returns>
        private TRequest GetNextRequest()
        {
            if (_requests.IsEmpty)
                return _lastRequest = DefaultRequest;

            TRequest request;

            // ReSharper disable once RedundantAssignment
            var popped = _requests.TryPop(out request);
            Debug.Assert(popped);
            Debug.Assert(request != null);

            // Push it back onto the stack if continuous.
            if (request.IsContinuous)
                _requests.Push(request);

            return _lastRequest = request;
        }

        #endregion

        #region Steppable Clock Members

        /// <summary>
        /// Returns a created <typeparamref name="TRequest"/> given <paramref name="direction"/>,
        /// <paramref name="steps"/>, and <paramref name="type"/>.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="millisecondsPerStep"></param>
        /// <param name="steps"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected abstract TRequest CreateRequest(RunningDirection? direction = null,
            double millisecondsPerStep = OneSecondMilliseconds, int steps = One,
            RequestType type = RequestType.Continuous);

        /* TODO: TBD: increment/decrement? Direction? continuous/latching? or leave the previous state alone?
         * "Next" almost needs to include Direction + steps and that's it. Some of which is internal state,
         * or instantaneous, as necessary. */

        /// <summary>
        /// Increments the timerable clock given a number of <paramref name="steps"/> and <paramref name="type"/>.
        /// </summary>
        /// <param name="steps"></param>
        /// <param name="millisecondsPerStep"> Represents the milliseconds per step.</param>
        /// <param name="type"></param>
        public override void Increment(int steps, double millisecondsPerStep = OneSecondMilliseconds,
            RequestType type = RequestType.Continuous)
        {
            lock (this)
            {
                Next(CreateRequest(RunningDirection.Forward, millisecondsPerStep, steps, type));
            }
        }

        /// <summary>
        /// Decrements the timerable clock given a number of <paramref name="steps"/> and <paramref name="type"/>.
        /// </summary>
        /// <param name="steps"></param>
        /// <param name="millisecondsPerStep">Represents the milliseconds per step.</param>
        /// <param name="type"></param>
        public override void Decrement(int steps, double millisecondsPerStep = OneSecondMilliseconds,
            RequestType type = RequestType.Continuous)
        {
            lock (this)
            {
                Next(CreateRequest(RunningDirection.Backward, millisecondsPerStep, steps, type));
            }
        }

        #endregion

        #region Startable Clock Members

        //TODO: thin-ify the "start/stop" interface especially as relates to TimerIntervalQuantity
        /// <summary>
        /// Starts the timeable clock timer running with specified <paramref name="interval"/>.
        /// </summary>
        /// <param name="interval"></param>
        public override void Start(TimeSpan interval)
        {
            lock (this)
            {
                if (GetNextRequest().WillNotRun)
                {
                    _requests.Clear();
                    _requests.Push(StartingRequest);
                }

                TimerIntervalQuantity = interval.TotalMilliseconds < 0d
                    ? null
                    : interval.TotalMilliseconds.ToTimeQuantity();

                TryChangeTimer(interval, interval);
            }
        }

        /// <summary>
        /// Stops the timerable clock timer from running.
        /// </summary>
        public override void Stop()
        {
            lock (this)
            {
                const int period = Timeout.Infinite;

                TimerIntervalQuantity = null;

                TryChangeTimer(period, period);

                if (GetNextRequest().WillRun) _requests.Clear();
            }
        }

        #endregion
    }
}
