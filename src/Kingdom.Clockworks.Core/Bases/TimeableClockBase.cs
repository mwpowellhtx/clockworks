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
        ////TODO: TBD: not sure what I needed with this one? probably want to preclude negative values; but should permit NegativeInfinity/PositiveInfinity, depending on the value...
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
            const int infinite = Timeout.Infinite;
            _timer = new Timer(ProtectedTimerCallback, null, infinite, infinite);

            var s = T.Second;

            TimePerStepQty = new Quantity(1d, s);
            _timerIntervalQty = new Quantity(double.NegativeInfinity, s);
        }

        #region Internal Members

        /// <summary>
        /// ProtectedTimerCallback
        /// </summary>
        /// <param name="state"></param>
        protected abstract void ProtectedTimerCallback(object state);

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

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// TimerIntervalQuantity backing field.
        /// </summary>
        protected IQuantity _timerIntervalQty;

        /// <summary>
        /// Gets or sets the internal TimerIntervalQty.
        /// </summary>
        public IQuantity TimerIntervalQty
        {
            get { return _timerIntervalQty; }
            set
            {
                _timerIntervalQty = value ?? new Quantity(double.NegativeInfinity, T.Second);
                if (_timerIntervalQty.IsInfinity)
                {
                    Stop();
                    return;
                }
                Start(_timerIntervalQty.ToTimeSpan());
            }
        }

        /// <summary>
        /// Gets whether the timerable clock IsRunning.
        /// </summary>
        public bool IsRunning
        {
            get { lock (this) return !TimerIntervalQty.IsInfinity; }
        }

        /// <summary>
        /// Starts the timerable clock timer running. The default interval is every
        /// half second, or 500 milliseconds.
        /// </summary>
        public void Start()
        {
            Start(500d);
        }

        /// <summary>
        /// Starts the timerable clock timer running with specified <paramref name="interval"/>.
        /// </summary>
        /// <param name="interval"></param>
        public void Start(int interval)
        {
            Start(interval == Timeout.Infinite ? double.NegativeInfinity : interval);
        }

        /// <summary>
        /// Starts the timerable clock timer running with specified <paramref name="interval"/>.
        /// </summary>
        /// <param name="interval"></param>
        public void Start(long interval)
        {
            Start(interval == Timeout.Infinite ? double.NegativeInfinity : interval);
        }

        /// <summary>
        /// Starts the timerable clock timer running with specified <paramref name="interval"/>.
        /// </summary>
        /// <param name="interval"></param>
        public void Start(uint interval)
        {
            Start(interval == Timeout.Infinite ? double.NegativeInfinity : interval);
        }

        /// <summary>
        /// Starts the clock timer running with the <paramref name="interval"/>.
        /// </summary>
        /// <param name="interval"></param>
        private void Start(double interval)
        {
            TimerIntervalQty = new Quantity(interval, T.Millisecond);
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

        #endregion

        #region Steppable Clock Members

        #region Per Step Members

        private IQuantity _timePerStepQty;

        public IQuantity TimePerStepQty
        {
            get { return _timePerStepQty; }
            set { SetQuantity(value, new Quantity(1, T.Second), out _timePerStepQty); }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueQty"></param>
        /// <param name="defaultQty"></param>
        /// <param name="fieldQty"></param>
        protected static void SetQuantity(IQuantity valueQty, IQuantity defaultQty, out IQuantity fieldQty)
        {
            // There's a bit of overhead involved here, but it's unavoidable in keeping with dimensional safety over convenience.
            if (ReferenceEquals(null, defaultQty))
                throw new ArgumentNullException("defaultQty", "Default quantity must not be null.");
            valueQty = (IQuantity)((valueQty ?? defaultQty).Clone());
            VerifyCompatibleDimensions(valueQty.Dimensions, defaultQty.Dimensions.ToArray());
            fieldQty = valueQty;
        }

        /// <summary>
        /// Verifies that the dimensions are compatible.
        /// </summary>
        /// <param name="dimensions"></param>
        /// <param name="expected"></param>
        private static void VerifyCompatibleDimensions(IEnumerable<IDimension> dimensions,
            params IDimension[] expected)
        {
            if (dimensions == null)
                throw new ArgumentNullException("dimensions", "There must be dimensions.");

            var local = dimensions.ToArray();

            if (local.AreCompatible(expected)) return;

            var message = string.Format("Expecting dimensions {{{0}}} but was {{{1}}}.",
                string.Join(" ", expected.Select(d => d.ToString())),
                string.Join(" ", local.Select(d => d.ToString())));

            throw new IncompatibleDimensionsException(message);
        }

        /* TODO: TBD: increment/decrement? Direction? continuous/latching? or leave the previous state alone?
         * "Next" almost needs to include Direction + steps and that's it. Some of which is internal state,
         * or instantaneous, as necessary. */

        /// <summary>
        /// Increments the timerable clock by one <see cref="RequestType.Instantaneous"/> step.
        /// </summary>
        public void Increment()
        {
            Increment(1, (IQuantity) _timePerStepQty.Clone(), RequestType.Instantaneous);
        }

        /// <summary>
        /// Decrements the timerable clock by one <see cref="RequestType.Instantaneous"/> step.
        /// </summary>
        public void Decrement()
        {
            Decrement(1, (IQuantity) _timePerStepQty.Clone(), RequestType.Instantaneous);
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
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TTimerElapsedEventArgs"></typeparam>
    public abstract class TimeableClockBase<TRequest, TTimerElapsedEventArgs>
        : TimeableClockBase
            , IScaleableClock
            , IStartableClock<TTimerElapsedEventArgs>
            , IMeasurableClock<TRequest>
        where TRequest : TimeableRequestBase
        where TTimerElapsedEventArgs : EventArgs
    {
        #region Measurable Clock Members

        /// <summary>
        /// StartingQty backing field.
        /// </summary>
        private IQuantity _startingQty;

        /// <summary>
        /// Gets or sets the StartingQty.
        /// </summary>
        /// <see cref="Starting"/>
        public IQuantity StartingQty
        {
            get { return _startingQty; }
            set { SetQuantity(value, Quantity.Zero(T.Second), out _startingQty); }
        }

        /// <summary>
        /// Gets the Starting <see cref="TimeSpan"/>.
        /// </summary>
        /// <see cref="StartingQty"/>
        public TimeSpan Starting
        {
            get { return _startingQty.ToTimeSpan(); }
        }

        /// <summary>
        /// TimerElapsed event.
        /// </summary>
        public event EventHandler<TTimerElapsedEventArgs> TimerElapsed;

        /// <summary>
        /// Raises the <see cref="TimerElapsed"/> event. Returns the asynchronous event handler.
        /// Returns an empty task runner when the event handler is empty.
        /// </summary>
        /// <param name="e"></param>
        private IAsyncResult RaiseTimerElapsedAsync(TTimerElapsedEventArgs e)
        {
            return TimerElapsed == null
                ? Task.Run(() => { })
                : TimerElapsed.BeginInvoke(this, e, null, null);
        }

        /// <summary>
        /// Ends the <see cref="RaiseTimerElapsedAsync"/> asynchronous invokation.
        /// </summary>
        /// <param name="ar"></param>
        private void EndRaiseTimerElapsedAsync(IAsyncResult ar)
        {
            if (TimerElapsed == null) return;
            TimerElapsed.EndInvoke(ar);
        }

        /// <summary>
        /// Raises the <see cref="TimerElapsed"/> event.
        /// </summary>
        /// <param name="e"></param>
        private void RaiseTimerElapsed(TTimerElapsedEventArgs e)
        {
            if (TimerElapsed == null) return;
            TimerElapsed(this, e);
        }

        /// <summary>
        /// Gets the Elapsed <see cref="TimeSpan"/>.
        /// </summary>
        public TimeSpan Elapsed
        {
            get { lock (this) return ElapsedQty.ToTimeSpan(); }
        }

        /// <summary>
        /// Gets or sets the Elapsed <see cref="TimeSpan"/>.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        protected IQuantity _elapsedQty;

        /// <summary>
        /// Gets the ElapsedQty.
        /// </summary>
        public IQuantity ElapsedQty
        {
            get { lock (this) return _elapsedQty; }
            set
            {
                // Ditto StartingQty re: overhead.
                _elapsedQty = (IQuantity) ((value ?? Quantity.Zero(T.Second)).Clone());
            }
        }

        /// <summary>
        /// Gets the current Elapsed TimeSpan.
        /// </summary>
        TimeSpan IMeasurableClock.Elapsed
        {
            get { return Elapsed; }
        }

        /// <summary>
        /// Gets the <see cref="IMeasurableClock.ElapsedQty"/>.
        /// </summary>
        IQuantity IMeasurableClock.ElapsedQty
        {
            get { return ElapsedQty; }
            set { ElapsedQty = value; }
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
        /// <param name="startingQty"></param>
        protected TimeableClockBase(IQuantity startingQty)
        {
            StartingQty = startingQty;
            ElapsedQty = null;
            IntervalTimePerTimeQty = null;
        }

        #region Internal Timer Concerns

        /// <summary>
        /// This portion of the timerable clock is consistent regardless of whether we are
        /// talking about <see cref="Next"/> or <see cref="NextAsync"/>.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected abstract TTimerElapsedEventArgs GetNextEventArgs(TRequest request);

        /// <summary>
        /// Requests backing field.
        /// </summary>
        protected readonly ConcurrentStack<TRequest> Requests = new ConcurrentStack<TRequest>();

        /// <summary>
        /// Protected clock timer callback.
        /// </summary>
        /// <param name="state"></param>
        protected override void ProtectedTimerCallback(object state)
        {
            lock (this)
            {
                //TODO: TBD: for the moment state is "null", but could easily be "this" ?
                Debug.Assert(state == null);
                // The request mechanism encapsulates the bits that instruct the clock how next to operate.
                Next(GetNextRequest());
            }
        }

        /// <summary>
        /// Returns the next <see cref="TRequest"/> from the <see cref="Requests"/>.
        /// Pushes the request back onto the stack if it was continuous in nature.
        /// Returns the <see cref="DefaultRequest"/> when one cannot be popped.
        /// </summary>
        /// <returns></returns>
        private TRequest GetNextRequest()
        {
            lock (this)
            {
                TRequest request;

                var popped = Requests.TryPop(out request);

                if (popped && request.IsContinuous)
                    Requests.Push(request);

                return request ?? DefaultRequest;
            }
        }

        /// <summary>
        /// Processes the next timerable clock <paramref name="request"/> asynchronously.
        /// </summary>
        /// <param name="request"></param>
        private void NextAsync(TRequest request)
        {
            IAsyncResult eventRaised;

            lock (this)
            {
                // Defer event handling free the lock in case anyone wants to callback into the timerable clock.
                eventRaised = RaiseTimerElapsedAsync(GetNextEventArgs(request));
            }

            //TODO: wait with a time out?
            // Wait for the elapsed event to complete which may require locks apart from the current thread.
            eventRaised.AsyncWaitHandle.WaitOne();

            lock (this)
            {
                EndRaiseTimerElapsedAsync(eventRaised);
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
                RaiseTimerElapsed(GetNextEventArgs(request));
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
                    Requests.Clear();
                    Requests.Push(StartingRequest);
                }

                _timerIntervalQty = interval.ToQuantity();

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

                if (GetNextRequest().WillRun)
                    Requests.Clear();
            }
        }

        #endregion

        #region Scaleable Clock Members

        private IQuantity _intervalTimePerTimeQty;

        /// <summary>
        /// Gets or sets the IntervalTimePerTimeQty.
        /// </summary>
        public IQuantity IntervalTimePerTimeQty
        {
            get { return _intervalTimePerTimeQty; }
            set { SetQuantity(value, new Quantity(1, T.Second, T.Second.Invert()), out _intervalTimePerTimeQty); }
        }

        #endregion
    }
}
