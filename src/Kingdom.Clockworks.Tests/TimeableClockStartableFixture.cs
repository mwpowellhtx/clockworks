using System;
using System.Reflection;
using System.Threading;
using Kingdom.Clockworks.Stopwatches;
using Kingdom.Clockworks.Timers;
using Kingdom.Unitworks;
using NUnit.Framework;

namespace Kingdom.Clockworks
{
    /// <summary>
    /// This is similar in nature as with <see cref="TimerElapsedEventArgs"/> in that we want
    /// to calculate appropriate <see cref="Steps"/> and related concerns. But that is the only
    /// similarity. The rest is mechanics that allow for easier test fixturing.
    /// </summary>
    /// <typeparam name="TClock"></typeparam>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TTimerElapsedEventArgs"></typeparam>
    internal class TimeableClockStartableFixture<TClock, TRequest, TTimerElapsedEventArgs>
        : Disposable
        where TClock : class
            , IClockBase<TRequest>
            , IStartableClock<TTimerElapsedEventArgs>
        where TRequest : TimeableRequestBase
        where TTimerElapsedEventArgs : EventArgs
    {
        /// <summary>
        /// Clock backing field.
        /// </summary>
        private readonly TClock _clock;

        /// <summary>
        /// Gets the Clock.
        /// </summary>
        internal TClock Clock
        {
            get { return _clock; }
        }

        private int _count;

        private readonly int _durationMilliseconds;

        private IQuantity ExpectedElapsedQty
        {
            get { return (Quantity) Clock.IntervalTimePerTimeQty*Clock.TimePerStepQty*Steps; }
        }

        /// <summary>
        /// TimeableClockStartableFixture TimerElapsed event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimeableClockStartableFixture_TimerElapsed(object sender, TTimerElapsedEventArgs e)
        {
            _count++;
        }

        //TODO: for now will assume "Forward" but will want to expose Forward/Backward.
        private readonly RunningDirection _direction = RunningDirection.Forward;

        /// <summary>
        /// Gets the proper Steps depending on whether <see cref="ISimulationTimer"/> or
        /// <see cref="ISimulationStopwatch"/>, and the state of <see cref="_direction "/>.
        /// </summary>
        private int Steps
        {
            get
            {
                //TODO: for now moving Forward only...
                var count = _direction == RunningDirection.Forward ? _count : -_count;
                var timer = Clock as ISimulationTimer;
                if (!ReferenceEquals(null, timer))
                {
                    count = -count;
                }
                return count;
            }
        }

        /// <summary>
        /// Internal Constructor
        /// </summary>
        /// <param name="clock">The startable fixture assumes ownership of the clock for purposes of disposal.</param>
        /// <param name="durationMilliseconds"></param>
        internal TimeableClockStartableFixture(TClock clock, int durationMilliseconds)
        {
            _durationMilliseconds = durationMilliseconds;

            //TODO: assert some plausible things about _durationMilliseconds.
            Assert.That(_durationMilliseconds, Is.Positive);

            _clock = clock;
            _clock.TimerElapsed += TimeableClockStartableFixture_TimerElapsed;
        }

        /// <summary>
        /// Starts the <see cref="Clock"/> running via reflection. Keys on the supported type
        /// of <paramref name="interval"/> in order to do so.
        /// </summary>
        /// <param name="interval"></param>
        internal void Start(object interval)
        {
            Invoke(GetVerifiedStartMethod(interval), interval);
        }

        /// <summary>
        /// Stops the <see cref="Clock"/> from running.
        /// </summary>
        internal void Stop()
        {
            Clock.Stop();

            Assert.That(Clock.IsRunning, Is.False);

            Assert.That(_count, Is.Positive);

            var elapsedQty = Clock.ElapsedQty;

            Assert.That(elapsedQty.Equals(ExpectedElapsedQty));

            Console.WriteLine("{{{0}}} steps occurred in elapsed {{{1}}} (quantity).", Steps, elapsedQty);
        }

        /// <summary>
        /// Returns a verified <see cref="MethodInfo"/> supported by the the <paramref name="arg"/>.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        private static MethodInfo GetVerifiedStartMethod(object arg)
        {
            const BindingFlags flags
                = BindingFlags.Public
                  | BindingFlags.Instance;

            var clockType = typeof (TClock);

            Assert.That(arg, Is.Not.Null);

            var type = arg.GetType();

            var methodInfo = clockType.GetMethod("Start", flags, Type.DefaultBinder, new[] {type}, null);

            Assert.That(methodInfo, Is.Not.Null);

            return methodInfo;
        }

        /// <summary>
        /// Runs the <paramref name="startMethod"/> given the <paramref name="interval"/>.
        /// </summary>
        /// <param name="startMethod"></param>
        /// <param name="interval"></param>
        private void Invoke(MethodInfo startMethod, object interval)
        {
            Assert.That(startMethod, Is.Not.Null);

            Assert.That(Steps, Is.EqualTo(0));

            var elapsedQty = Clock.ElapsedQty;

            Assert.That(elapsedQty.Equals(Quantity.Zero(elapsedQty.Dimensions)));

            startMethod.Invoke(Clock, new[] {interval});

            Assert.That(Clock.IsRunning, Is.True);
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && !IsDisposed)
            {
                //TODO: this should go in the dispose method...
                Thread.Sleep(_durationMilliseconds);

                Stop();

                _clock.TimerElapsed -= TimeableClockStartableFixture_TimerElapsed;
                _clock.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
