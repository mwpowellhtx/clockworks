using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kingdom.Unitworks;
using Kingdom.Unitworks.Dimensions;

namespace Kingdom.Clockworks
{
    using T = Unitworks.Dimensions.Systems.Commons.Time;

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
            TimePerStepQty = new Quantity(1d, T.Second);

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
        private IQuantity _timerIntervalQty;

        /// <summary>
        /// Gets or sets the TimerIntervalQuantity.
        /// Sets in terms of <see cref="T.Millisecond"/> or Null.
        /// </summary>
        protected IQuantity TimerIntervalQty
        {
            get { return _timerIntervalQty; }
            set { _timerIntervalQty = value; }
        }

        /// <summary>
        /// Gets the TimerIntervalMilliseconds if possible.
        /// Gets Null if there is no interval currently set.
        /// </summary>
        /// <see cref="TimerIntervalQty"/>
        protected double? TimerIntervalMilliseconds
        {
            get
            {
                return _timerIntervalQty == null
                    ? (double?) null
                    : _timerIntervalQty.Value;
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
        protected IQuantity _elapsedQty;

        /// <summary>
        /// Resets the timeable clock.
        /// </summary>
        /// <param name="elapsedMilliseconds"></param>
        public void Reset(double elapsedMilliseconds)
        {
            lock (this)
            {
                _elapsedQty = new Quantity(elapsedMilliseconds, T.Millisecond);
            }
        }

        #endregion

        #region Steppable Clock Members

        #region Per Step Members

        private IQuantity _timePerStepQty;

        public IQuantity TimePerStepQty
        {
            get { return _timePerStepQty; }
            set { _timePerStepQty = value; }
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
            Increment(1, _timePerStepQty, RequestType.Instantaneous);
        }

        /// <summary>
        /// Decrements the timerable clock by one <see cref="RequestType.Instantaneous"/> step.
        /// </summary>
        public void Decrement()
        {
            Decrement(1, _timePerStepQty, RequestType.Instantaneous);
        }

        /// <summary>
        /// Increments the timerable clock given a number of <paramref name="steps"/> and <paramref name="type"/>.
        /// </summary>
        /// <param name="steps"></param>
        /// <param name="timePerStepQty">Represents a time component per step.</param>
        /// <param name="type"></param>
        public abstract void Increment(int steps, IQuantity timePerStepQty = null,
            RequestType type = RequestType.Continuous);

        /// <summary>
        /// Decrements the timerable clock given a number of <paramref name="steps"/> and <paramref name="type"/>.
        /// </summary>
        /// <param name="steps"></param>
        /// <param name="timePerStepQty">Represents a time component per step.</param>
        /// <param name="type"></param>
        public abstract void Decrement(int steps, IQuantity timePerStepQty = null,
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
            get { lock (this) return _elapsedQty.ToTimeSpan(); }
        }

        /// <summary>
        /// Gets the ElapsedQuantity <see cref="IQuantity"/>.
        /// </summary>
        IQuantity IMeasurableClock<TRequest>.ElapsedQty
        {
            get { lock (this) return _elapsedQty; }
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
            _elapsedQty = Quantity.Zero(T.Millisecond);
            IntervalTimePerTimeQty = new Quantity(1d, T.Second, T.Second.Invert());
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
        /// <param name="timePerStepQty"></param>
        /// <param name="steps"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected abstract TRequest CreateRequest(RunningDirection? direction = null, IQuantity timePerStepQty = null,
            int steps = 1, RequestType type = RequestType.Continuous);

        /* TODO: TBD: increment/decrement? Direction? continuous/latching? or leave the previous state alone?
         * "Next" almost needs to include Direction + steps and that's it. Some of which is internal state,
         * or instantaneous, as necessary. */

        private static IQuantity NormalizeTimePerStep(IQuantity quantity)
        {
            return quantity ?? new Quantity(1d, T.Millisecond);
        }

        /// <summary>
        /// Increments the timerable clock given a number of <paramref name="steps"/> and <paramref name="type"/>.
        /// </summary>
        /// <param name="steps"></param>
        /// <param name="timePerStepQty">Represents a time component per step.</param>
        /// <param name="type"></param>
        public override void Increment(int steps, IQuantity timePerStepQty = null, RequestType type = RequestType.Continuous)
        {
            lock (this)
            {
                Next(CreateRequest(RunningDirection.Forward, NormalizeTimePerStep(timePerStepQty), steps, type));
            }
        }

        /// <summary>
        /// Decrements the timerable clock given a number of <paramref name="steps"/> and <paramref name="type"/>.
        /// </summary>
        /// <param name="steps"></param>
        /// <param name="timePerStepQty">Represents a time component per step.</param>
        /// <param name="type"></param>
        public override void Decrement(int steps, IQuantity timePerStepQty = null, RequestType type = RequestType.Continuous)
        {
            lock (this)
            {
                Next(CreateRequest(RunningDirection.Backward, NormalizeTimePerStep(timePerStepQty), steps, type));
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

                var intervalMs = interval.TotalMilliseconds;

                TimerIntervalQty = intervalMs < 0d
                    ? null
                    : new Quantity(intervalMs, T.Millisecond);

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

                TimerIntervalQty = null;

                TryChangeTimer(period, period);

                if (GetNextRequest().WillRun) _requests.Clear();
            }
        }

        #endregion

        #region Scaleable Clock Members

        private IQuantity _intervalTimePerTimeQty = Quantity.Zero(T.Second, T.Second.Invert());

        /// <summary>
        /// 
        /// </summary>
        public IQuantity IntervalTimePerTimeQty
        {
            get { return _intervalTimePerTimeQty; }
            set
            {
                VerifyCompatibleDimensions(value.Dimensions);
                _intervalTimePerTimeQty = (IQuantity)value.Clone();
            }
        }

        private static IEnumerable<IDimension> ComparableDimensions = new[]
        {
            (ITime) T.Second.Clone(),
            (ITime) T.Second.Invert()
        };

        private static void VerifyCompatibleDimensions(IEnumerable<IDimension> dimensions)
        {
            var simulatedTime = (ITime) dimensions.ElementAtOrDefault(0);
            var perTime = (ITime) dimensions.ElementAtOrDefault(1);
            VerifyCompatibleDimensions(simulatedTime, perTime);
        }

        private static void VerifyCompatibleDimensions(ITime simulatedTime, ITime perTimerTime)
        {
            var actualDimensions = new[] {simulatedTime, perTimerTime};

            if (ComparableDimensions.AreCompatible(actualDimensions, true))
                return;

            var message = string.Format("Expecting dimensions {0} but was {1}.",
                string.Join(" ", ComparableDimensions.Select(d => d.ToString())),
                string.Join(" ", actualDimensions.Select(d => d.ToString())));

            throw new IncompatibleDimensionsException(message);
        }

        #endregion
    }
}
