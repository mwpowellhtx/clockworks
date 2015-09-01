using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Kingdom.Clockworks.Timers;
using Kingdom.Unitworks;
using NUnit.Framework;

namespace Kingdom.Clockworks
{
    using T = Unitworks.Dimensions.Systems.Commons.Time;

    public abstract class TimeableClockTestFixtureBase<TClock, TRequest, TTimerElapsedEventArgs> : TestFixtureBase
        where TClock : class, IClockBase<TRequest>, IStartableClock<TTimerElapsedEventArgs>, new()
        where TRequest : TimeableRequestBase
        where TTimerElapsedEventArgs : EventArgs
    {
        protected IQuantity StartingQty { get; set; }

        public override void SetUp()
        {
            base.SetUp();

            StartingQty = Quantity.Zero(T.Millisecond);
        }

        protected TClock CreateClock()
        {
            var clock = new TClock();
            VerifyClock(clock);
            return clock;
        }

        protected TClock CreateClock(IQuantity intervalTimePerTimeQty, IQuantity timePerStepQty)
        {
            var clock = new TClock
            {
                StartingQty = StartingQty,
                IntervalTimePerTimeQty = intervalTimePerTimeQty,
                TimePerStepQty = timePerStepQty
            };
            VerifyClock(clock, intervalTimePerTimeQty, timePerStepQty);
            return clock;
        }

        protected virtual void VerifyClock(TClock clock,
            IQuantity intervalTimePerTimeQty = null,
            IQuantity timePerStepQty = null,
            IQuantity timerIntervalQty = null)
        {
            //TODO: should establish these as known defaults some where in the clock works assembly ...
            // Initialize with nominal expected values.
            intervalTimePerTimeQty
                = intervalTimePerTimeQty
                  ?? new Quantity(1, clock.IntervalTimePerTimeQty.Dimensions);

            timePerStepQty
                = timePerStepQty
                  ?? new Quantity(1, clock.TimePerStepQty.Dimensions);

            timerIntervalQty
                = timerIntervalQty
                  ?? new Quantity(double.NegativeInfinity, clock.TimerIntervalQty.Dimensions);

            // Better organized verification.
            Assert.That(clock, Is.Not.Null);
            Assert.That(clock.CurrentRequest, Is.Null);

            Assert.That(clock.IsRunning, Is.False);
            Assert.That(clock.TimerIntervalQty.Equals(timerIntervalQty));

            Assert.That(clock.IntervalTimePerTimeQty.Equals(intervalTimePerTimeQty));
            Assert.That(clock.TimePerStepQty.Equals(timePerStepQty));

            Assert.That(clock.StartingQty.Equals(StartingQty));

            Assert.That(clock.Elapsed, Is.EqualTo(TimeSpan.Zero));
            Assert.That(clock.ElapsedQty.Equals(Quantity.Zero(clock.ElapsedQty.Dimensions)));
        }

        /// <summary>
        /// Verifies that <paramref name="change"/> is <paramref name="expected"/>.
        /// </summary>
        /// <param name="change"></param>
        /// <param name="expected"></param>
        private static void VerifyChange(ChangeType change, params ChangeType[] expected)
        {
            // Perform this handshake that the changes are expected.
            CollectionAssert.IsNotEmpty(expected);
            Assert.That(expected.Any(x => change == x));
        }

        protected abstract IQuantity CalculateEstimated(ChangeType change, int steps,
            IQuantity intervalTimePerTimeQty, IQuantity timePerStepQty);

        protected virtual TClock ChangeClock(TClock clock, object obj, ChangeType change,
            IEnumerable<Type> types, params object[] args)
        {
            Assert.That(clock, Is.Not.Null);

            lock (clock)
            {
                var clockType = typeof (TClock);

                var flags
                    = BindingFlags.Public
                      | (ReferenceEquals(obj, null)
                          ? BindingFlags.Static
                          : BindingFlags.Instance);

                var methodName = change.GetMethodName();

                var methodInfo = clockType.GetMethod(methodName, flags, Type.DefaultBinder, types.ToArray(), null);

                Assert.That(methodInfo, Is.Not.Null);

                Console.WriteLine("Changing clock with: {0}", methodInfo);

                methodInfo.Invoke(obj, args);

                return clock;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clock"></param>
        /// <param name="change"></param>
        /// <param name="steps"></param>
        /// <param name="timePerStepQty"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual TClock ChangeClock(TClock clock, ChangeType change, int steps,
            IQuantity timePerStepQty, RequestType request = RequestType.Instantaneous)
        {
            VerifyChange(change, ChangeType.Increment, ChangeType.Decrement);
            var types = new[] {typeof (int), typeof (IQuantity), typeof (RequestType)};
            return ChangeClock(clock, clock, change, types, steps, timePerStepQty, request);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clock"></param>
        /// <param name="change"></param>
        /// <returns></returns>
        public virtual TClock ChangeClock(TClock clock, ChangeType change)
        {
            VerifyChange(change,
                ChangeType.Operator | ChangeType.Increment,
                ChangeType.Operator | ChangeType.Decrement);
            var types = new[] {typeof (TClock)};
            return ChangeClock(clock, null, change, types, clock);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clock"></param>
        /// <param name="change"></param>
        /// <param name="steps"></param>
        /// <returns></returns>
        public virtual TClock ChangeClock(TClock clock, ChangeType change, int steps)
        {
            VerifyChange(change,
                ChangeType.Operator | ChangeType.Addition,
                ChangeType.Operator | ChangeType.Subtraction);
            var types = new[] {typeof (TClock), typeof (int)};
            return ChangeClock(clock, null, change, types, clock, steps);
        }

        [Test]
        public void Verify_default()
        {
            using (CreateClock())
            {
            }
        }

        [Test]
        public void Verify_instantaneous_instance_method(
            [InstanceChangeTypeValues] ChangeType change,
            [IntervalTimePerTimeValues] IQuantity intervalTimePerTimeQty,
            [Values(1, 2, 3)] int steps,
            [TimePerStepValues] IQuantity timePerStepQty)
        {
            using (var clock = CreateClock(intervalTimePerTimeQty, timePerStepQty))
            {
                var estimatedQty = CalculateEstimated(change, steps, intervalTimePerTimeQty, timePerStepQty);

                ChangeClock(clock, change, steps, timePerStepQty)
                    .Verify(c =>
                    {
                        Assert.That(c.CurrentRequest, Is.Null);

                        var estimatedElapsed = estimatedQty.ToTimeSpan();
                        Assert.That(c.Elapsed, Is.EqualTo(estimatedElapsed));
                    });
            }
        }

        [Test]
        public void Verify_instantaneous_unary_operator_overload(
            [UnaryOperatorChangeTypeValues] ChangeType change,
            [IntervalTimePerTimeValues] IQuantity intervalTimePerTimeQty,
            [TimePerStepValues] IQuantity timePerStepQty)
        {
            using (var clock = CreateClock(intervalTimePerTimeQty, timePerStepQty))
            {
                const int steps = 1;

                var estimatedQty = CalculateEstimated(change, steps, intervalTimePerTimeQty, timePerStepQty);

                ChangeClock(clock, change)
                    .Verify(c =>
                    {
                        Assert.That(c.CurrentRequest, Is.Null);

                        var estimatedElapsed = estimatedQty.ToTimeSpan();
                        Assert.That(c.Elapsed, Is.EqualTo(estimatedElapsed));
                    });
            }
        }

        [Test]
        public void Verify_instantaneous_binary_operator_overload(
            [BinaryOperatorChangeTypeValues] ChangeType change,
            [IntervalTimePerTimeValues] IQuantity intervalTimePerTimeQty,
            [Values(1, 2, 3)] int steps,
            [TimePerStepValues] IQuantity timePerStepQty)
        {
            using (var clock = CreateClock(intervalTimePerTimeQty, timePerStepQty))
            {
                var estimatedQty = CalculateEstimated(change, steps, intervalTimePerTimeQty, timePerStepQty);

                ChangeClock(clock, change, steps)
                    .Verify(c =>
                    {
                        Assert.That(c.CurrentRequest, Is.Null);

                        var estimatedElapsed = estimatedQty.ToTimeSpan();
                        Assert.That(c.Elapsed, Is.EqualTo(estimatedElapsed));
                    });
            }
        }
    }
}

//using System;
//using System.Reactive.Linq;
//using System.Threading;
//using Kingdom.Unitworks;
//using Kingdom.Unitworks.Dimensions;
//using NUnit.Framework;

//namespace Kingdom.Clockworks
//{
//    using T = Unitworks.Dimensions.Systems.Commons.Time;

//    /// <summary>
//    /// Provides basic timeable clock test opportunities.
//    /// </summary>
//    /// <typeparam name="TClock"></typeparam>
//    /// <typeparam name="TRequest"></typeparam>
//    /// <typeparam name="TElapsedEventArgs"></typeparam>
//    public abstract class TimeableClockTestFixtureBase<TClock, TRequest, TElapsedEventArgs> : TestFixtureBase
//        where TClock : TimeableClockBase<TRequest, TElapsedEventArgs>, new()
//        where TRequest : TimeableRequestBase
//        where TElapsedEventArgs : ElapsedEventArgsBase<TRequest>
//    {
//        //TODO: refactor Epsilon to base class once I figure out what happened with TimeUnitTestFixturebase ...
//        private const double Epsilon = 1e3;

//        protected TRequest MakeRequest(RunningDirection? direction = null, double millisecondsPerStep = 1000d,
//            int steps = 1, RequestType type = RequestType.Instantaneous)
//        {
//            return MakeRequest(direction, new Quantity(millisecondsPerStep, T.Millisecond), steps, type);
//        }

//        /// <summary>
//        /// Returns a created <typeparamref name="TRequest"/> given the arguments.
//        /// </summary>
//        /// <param name="direction"></param>
//        /// <param name="timePerStepQty"></param>
//        /// <param name="steps"></param>
//        /// <param name="type"></param>
//        /// <returns></returns>
//        protected abstract TRequest MakeRequest(RunningDirection? direction = null,
//            IQuantity timePerStepQty = null, int steps = 1, RequestType type = RequestType.Instantaneous);

//        /// <summary>
//        /// Runs the timer <paramref name="scenario"/>.
//        /// </summary>
//        /// <param name="scenario"></param>
//        protected static void RunClockScenario(Action<TClock> scenario)
//        {
//            Assert.That(scenario, Is.Not.Null);
//            using (var clock = new TClock())
//            {
//                scenario(clock);
//            }
//        }

//        /// <summary>
//        /// Returns the estimated event count calculating <paramref name="intervalMilliseconds"/> into
//        /// <paramref name="durationQty"/>.
//        /// </summary>
//        /// <param name="intervalMilliseconds">A timer interval in milliseconds.</param>
//        /// <param name="durationQty">Duration during which to run.</param>
//        /// <returns></returns>
//        protected static int GetEstimatedEventCount(int intervalMilliseconds, IQuantity durationQty)
//        {
//            durationQty = durationQty.ConvertTo(T.Millisecond);
//            var intervalQty = new Quantity(intervalMilliseconds, T.Millisecond);
//            var result = (durationQty/intervalQty).Value;
//            return (int) result;
//        }

//        /// <summary>
//        /// Makes the necessary arrangements supporting the
//        /// <see cref="Elapsed_events_yield_expected_event_counts"/> tests.
//        /// </summary>
//        /// <param name="aClock"></param>
//        /// <param name="durationQty"></param>
//        protected abstract void MakeElapsedEventYieldArrangements(TClock aClock, IQuantity durationQty);

//        /// <summary>
//        /// Tests that elapsed events yield the expected event count. The arrangements
//        /// of which depend on the <typeparamref name
//        /// ="TClock"/>.
//        /// </summary>
//        /// <param name="interval"></param>
//        /// <param name="durationSeconds"></param>
//        /// <param name="epsilon"></param>
//        /// <see cref="MakeElapsedEventYieldArrangements"/>
//        [Test]
//        [TestCase(100, 1d, 1)]
//        [TestCase(125, 3d, 1)]
//        [TestCase(150, 5d, 1)]
//        [TestCase(175, 7d, 3)]
//        public void Elapsed_events_yield_expected_event_counts(int interval, double durationSeconds, int epsilon)
//        {
//            var durationQty = new Quantity(durationSeconds, T.Second).ConvertTo(T.Millisecond);

//            var expectedEventCount = GetEstimatedEventCount(interval, durationQty);

//            var actualEventCount = 0;

//            var durationTimeSpan = durationQty.ToTimeSpan();

//            RunClockScenario(clock =>
//            {
//                clock.Elapsed += delegate { actualEventCount++; };

//                MakeElapsedEventYieldArrangements(clock, durationQty);

//                clock.Start(interval);

//                Thread.Sleep(durationTimeSpan);
//            });

//            Assert.That(actualEventCount, Is.GreaterThan(0));
//            Assert.That(actualEventCount, Is.EqualTo(expectedEventCount).Within(epsilon));
//        }

//        #region Per Step Members

//        /// <summary>
//        /// Test that setting and getting <see cref="TimeableClockBase.SecondsPerStep"/>
//        /// or one of its friends occurs consistently.
//        /// </summary>
//        /// <param name="timePerStep">The value to attempt to set in terms of the <paramref name="setTime"/>.</param>
//        /// <param name="setTime">The unit to use in the setter.</param>
//        /// <param name="getTime">The unit to use in the getter.</param>
//        /// <see cref="IQuantity"></see>
//        [Test]
//        [Combinatorial]
//        public void Clock_set_time_per_step_is_gotten_consistently(
//            [TimePerStepValues] double timePerStep,
//            [CommonsTimesValues] ITime setTime,
//            [CommonsTimesValues] ITime getTime)
//        {
//            var expectedQty = new Quantity(timePerStep, setTime).ConvertTo(getTime);

//            RunClockScenario(clock =>
//            {
//                clock[setTime] = timePerStep;
//                var clockTimePerStep = clock[getTime];
//                Assert.That(clockTimePerStep, Is.EqualTo(expectedQty.Value).Within(Epsilon));
//            });
//        }

//        /// <summary>
//        /// Setting <see cref="TimeableClockBase.SecondsPerStep"/> or one of its friends to a
//        /// negative value should throw an <see cref="ArgumentException"/>.
//        /// </summary>
//        /// <param name="timePerStep">May be positive or negative. The test will ensure that a negative version of itself is used.</param>
//        /// <param name="setTime">The unit to use in the setter.</param>
//        [Test]
//        [Combinatorial]
//        public void Clock_negative_time_per_step_throws_argument_ex(
//            [TimePerStepValues] double timePerStep,
//            [CommonsTimesValues] ITime setTime)
//        {
//            this.Throws<ArgumentException>(() =>
//            {
//                RunClockScenario(clock =>
//                {
//                    // TODO: should throw ArgumentException ...

//                    // Yes this is intentionally negative.
//                    clock[setTime] = -Math.Abs(timePerStep);
//                });
//            });
//        }

//        #endregion

//        #region Overloaded Operators

//        /// <summary>
//        /// Returns whether <paramref name="a"/> Equals <paramref name="b"/>.
//        /// </summary>
//        /// <param name="a"></param>
//        /// <param name="b"></param>
//        /// <returns></returns>
//        protected abstract bool Equals(TRequest a, TRequest b);

//        /// <summary>
//        /// Returns whether <paramref name="a"/> Equals <paramref name="b"/>.
//        /// </summary>
//        /// <param name="a"></param>
//        /// <param name="b"></param>
//        /// <returns></returns>
//        protected static bool Equals(ISteppableRequest a, ISteppableRequest b)
//        {
//            return a.Equals(b);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="e"></param>
//        /// <param name="expectedElapsedQty"></param>
//        protected virtual void VerifyElapsedEventResults(TElapsedEventArgs e, IQuantity expectedElapsedQty)
//        {
//            Assert.That(e, Is.Not.Null);

//            var ms = T.Millisecond;

//            //TODO: Equals and/or logical operators would be great ...
//            var currentQty = e.CurrentQty.ConvertTo(ms);
//            var elapsedQty = e.ElapsedQty.ConvertTo(ms);

//            var expectedElapsedValue = expectedElapsedQty.ConvertTo(ms).Value;

//            // It so happens that we're expecting the one interval.
//            Assert.That(currentQty.Value, Is.EqualTo(expectedElapsedValue).Within(Epsilon));

//            // Also verify in total.
//            Assert.That(elapsedQty.Value, Is.EqualTo(expectedElapsedValue).Within(Epsilon));
//        }

//        /// <summary>
//        /// Verifies that the "star"-ncrement operations function correctly.
//        /// </summary>
//        /// <param name="intervalQty"></param>
//        /// <param name="expectedRequest"></param>
//        /// <param name="e"></param>
//        protected void VerifyStarNcrementRequest(IQuantity intervalQty,
//            TRequest expectedRequest, TElapsedEventArgs e)
//        {
//            Assert.That(expectedRequest, Is.Not.Null);

//            // TODO: in and of themselves these really deserve a dedicated StopwatchRequestsTests suite...
//            // TODO: however, for the same of establishing "SameAs", this may be interesting...

//            // Rule out a couple of seemingly obvious test cases, laying the foundation for what's coming next.
//            Assert.That(Equals(expectedRequest, expectedRequest));
//            Assert.That(Equals(e.Request, e.Request));

//            Assert.That(Equals((ISteppableRequest) expectedRequest, expectedRequest));
//            Assert.That(Equals((ISteppableRequest) e.Request, (ISteppableRequest) e.Request));

//            // We establish that they should equal the other, but that they should not be the same.
//            Assert.That(e.Request, Is.Not.SameAs(expectedRequest));

//            Assert.That(Equals(e.Request, expectedRequest));
//            Assert.That(Equals((ISteppableRequest) e.Request, (ISteppableRequest) expectedRequest));

//            var expectedElapsedQty = (Quantity) expectedRequest.TimePerStepQty*intervalQty*expectedRequest.Steps;

//            VerifyElapsedEventResults(e, expectedElapsedQty);
//        }

//        /// <summary>
//        /// Initializes <paramref name="theClock"/> given the arguments.
//        /// </summary>
//        /// <param name="theClock"></param>
//        /// <param name="intervalTimePerTimeQty"></param>
//        /// <param name="timePerStepQty"></param>
//        protected virtual void InitializeStarNcrementClock(TClock theClock,
//            IQuantity intervalTimePerTimeQty, IQuantity timePerStepQty)
//        {
//            theClock.IntervalTimePerTimeQty = intervalTimePerTimeQty;
//            theClock.TimePerStepQty = timePerStepQty;
//        }

//        /// <summary>
//        /// Verifies the outcome regardless whether <see cref="ISteppableClock.Increment()"/>,
//        /// <see cref="ISteppableClock.Decrement()"/>, or one of its neighbors were involved.
//        /// </summary>
//        /// <param name="intervalSecondsPerSecond"></param>
//        /// <param name="millisecondsPerStep"></param>
//        /// <param name="expectedRequest"></param>
//        /// <param name="theAction"></param>
//        protected void MakeSureStarNcrementCorrect(double intervalSecondsPerSecond,
//            double millisecondsPerStep, TRequest expectedRequest, Action<TClock> theAction)
//        {
//            Assert.That(theAction, Is.Not.Null);

//            /* I am really loathe to take on too many in the way of dependencies for a test suite,
//             * but this particular set of tests does really lend itself to listening reactively, in
//             * an observable manner, for one event outcomes. */

//            var intervalTimePerTimeQty = new Quantity(intervalSecondsPerSecond, T.Second, T.Second.Invert());
//            var timePerStepQty = new Quantity(millisecondsPerStep, T.Millisecond);

//            RunClockScenario(clock =>
//            {
//                InitializeStarNcrementClock(clock, intervalTimePerTimeQty, timePerStepQty);

//                var observableElapsed = Observable.FromEventPattern<TElapsedEventArgs>(
//                    handler => clock.Elapsed += handler, handler => clock.Elapsed -= handler)
//                    .Select(p => p.EventArgs);

//                using (observableElapsed.Subscribe(e => VerifyStarNcrementRequest(intervalTimePerTimeQty, expectedRequest, e)))
//                {
//                    theAction(clock);
//                }
//            });
//        }

//        /// <summary>
//        /// Makes sure that the <see cref="ISteppableClock.Increment(int,double,RequestType)"/> method
//        /// works correctly.
//        /// </summary>
//        /// <param name="intervalSecondsPerSecond"></param>
//        /// <param name="millisecondsPerStep"></param>
//        /// <param name="steps"></param>
//        /// <param name="type"></param>
//        [Test]
//        [Combinatorial]
//        public void Make_sure_increment_method_correct(
//            [TimeQuantityValues] double intervalSecondsPerSecond,
//            [TimePerStepValues] double millisecondsPerStep,
//            [StepValues] int steps, [RequestTypeValues] RequestType type)
//        {
//            var request = MakeRequest(RunningDirection.Forward, millisecondsPerStep, steps, type);

//            MakeSureStarNcrementCorrect(intervalSecondsPerSecond, millisecondsPerStep,
//                request, c => c.Increment(steps, new Quantity(millisecondsPerStep, T.Millisecond), type));
//        }

//        /// <summary>
//        /// Makes sure that the <see cref="ISteppableClock.Decrement(int,double,RequestType)"/> method
//        /// works correctly.
//        /// </summary>
//        /// <param name="intervalSecondsPerSecond"></param>
//        /// <param name="millisecondsPerStep"></param>
//        /// <param name="steps"></param>
//        /// <param name="type"></param>
//        [Test]
//        [Combinatorial]
//        public void Make_sure_decrement_method_correct(
//            [TimeQuantityValues] double intervalSecondsPerSecond,
//            [TimePerStepValues] double millisecondsPerStep,
//            [StepValues] int steps, [RequestTypeValues] RequestType type)
//        {
//            var request = MakeRequest(RunningDirection.Backward, millisecondsPerStep, steps, type);

//            MakeSureStarNcrementCorrect(intervalSecondsPerSecond, millisecondsPerStep,
//                request, c => c.Decrement(steps, new Quantity(millisecondsPerStep, T.Millisecond), type));
//        }

//        /// <summary>
//        /// Makes sure that the <see cref="ISteppableClock.Increment()"/> method works correctly.
//        /// </summary>
//        /// <param name="intervalSecondsPerSecond"></param>
//        /// <param name="millisecondsPerStep"></param>
//        [Test]
//        [Combinatorial]
//        public void Make_sure_increment_method_no_args_correct(
//            [TimeQuantityValues] double intervalSecondsPerSecond,
//            [TimePerStepValues] double millisecondsPerStep)
//        {
//            var request = MakeRequest(RunningDirection.Forward, millisecondsPerStep);

//            MakeSureStarNcrementCorrect(intervalSecondsPerSecond, millisecondsPerStep,
//                request, c => c.Increment());
//        }

//        /// <summary>
//        /// Makes sure that the <see cref="ISteppableClock.Decrement()"/> method works correctly.
//        /// </summary>
//        /// <param name="intervalSecondsPerSecond"></param>
//        /// <param name="millisecondsPerStep"></param>
//        [Test]
//        [Combinatorial]
//        public void Make_sure_decrement_method_no_args_correct(
//            [TimeQuantityValues] double intervalSecondsPerSecond,
//            [TimePerStepValues] double millisecondsPerStep)
//        {
//            var request = MakeRequest(RunningDirection.Backward, millisecondsPerStep);

//            MakeSureStarNcrementCorrect(intervalSecondsPerSecond, millisecondsPerStep,
//                request, c => c.Decrement());
//        }

//        /// <summary>
//        /// Provides post operator invocation handling.
//        /// </summary>
//        /// <param name="theClock"></param>
//        /// <param name="expectedClock"></param>
//        /// <returns></returns>
//        public static TClock PostOperatorInvocation(TClock theClock, TClock expectedClock)
//        {
//            Assert.That(theClock, Is.Not.Null);
//            Assert.That(theClock, Is.SameAs(expectedClock));
//            return theClock;
//        }

//        /// <summary>
//        /// Makes sure that the increment operator works correctly.
//        /// </summary>
//        /// <param name="intervalSecondsPerSecond"></param>
//        /// <param name="millisecondsPerStep"></param>
//        /// <a href="!:http://msdn.microsoft.com/en-us/library/36x43w8w.aspx"> ++ Operator (C# Reference) </a>
//        [Test]
//        [Combinatorial]
//        public void Make_sure_increment_operator_correct(
//            [TimeQuantityValues] double intervalSecondsPerSecond,
//            [TimePerStepValues] double millisecondsPerStep)
//        {
//            var parts = new[] {OperatorPart.Increment};

//            var request = MakeRequest(RunningDirection.Forward, millisecondsPerStep);

//            MakeSureStarNcrementCorrect(intervalSecondsPerSecond, millisecondsPerStep, request,
//                c => this.InvokeOperator<TClock, TClock>(parts, x => PostOperatorInvocation(x as TClock, c), c));
//        }

//        /// <summary>
//        /// Implement this method in order to expose the type specific operator overload.
//        /// </summary>
//        /// <param name="timer"></param>
//        /// <returns></returns>
//        /// <a href="!:http://msdn.microsoft.com/en-us/library/36x43w8w.aspx"> ++ Operator (C# Reference) </a>
//        protected abstract TClock PrefixIncrementOperator(TClock timer);

//        /// <summary>
//        /// Implement this method in order to expose the type specific operator overload.
//        /// </summary>
//        /// <param name="timer"></param>
//        /// <returns></returns>
//        /// <a href="!:http://msdn.microsoft.com/en-us/library/36x43w8w.aspx"> ++ Operator (C# Reference) </a>
//        protected abstract TClock PostfixIncrementOperator(TClock timer);

//        /// <summary>
//        /// Makes sure that the increment operator postfix form works correctly.
//        /// </summary>
//        /// <param name="intervalSecondsPerSecond"></param>
//        /// <param name="millisecondsPerStep"></param>
//        /// <a href="!:http://msdn.microsoft.com/en-us/library/36x43w8w.aspx"> ++ Operator (C# Reference) </a>
//        [Test]
//        [Combinatorial]
//        public void Make_sure_increment_operator_postfix_correct(
//            [TimeQuantityValues] double intervalSecondsPerSecond,
//            [TimePerStepValues] double millisecondsPerStep)
//        {
//            var request = MakeRequest(RunningDirection.Forward, millisecondsPerStep);

//            MakeSureStarNcrementCorrect(intervalSecondsPerSecond, millisecondsPerStep,
//                request, c => Assert.That(PostfixIncrementOperator(c), Is.SameAs(c)));
//        }

//        /// <summary>
//        /// Makes sure that the increment operator prefix form works correctly.
//        /// </summary>
//        /// <param name="intervalSecondsPerSecond"></param>
//        /// <param name="millisecondsPerStep"></param>
//        /// <a href="!:http://msdn.microsoft.com/en-us/library/36x43w8w.aspx"> ++ Operator (C# Reference) </a>
//        [Test]
//        [Combinatorial]
//        public void Make_sure_increment_operator_prefix_correct(
//            [TimeQuantityValues] double intervalSecondsPerSecond,
//            [TimePerStepValues] double millisecondsPerStep)
//        {
//            var request = MakeRequest(RunningDirection.Forward, millisecondsPerStep);

//            MakeSureStarNcrementCorrect(intervalSecondsPerSecond, millisecondsPerStep,
//                request, c => Assert.That(PrefixIncrementOperator(c), Is.SameAs(c)));
//        }

//        /// <summary>
//        /// Makes sure that the decrement operator works correctly.
//        /// </summary>
//        /// <param name="intervalSecondsPerSecond"></param>
//        /// <param name="millisecondsPerStep"></param>
//        /// <a href="!:http://msdn.microsoft.com/en-us/library/wc3z3k8c.aspx"> -- Operator (C# Reference) </a>
//        [Test]
//        [Combinatorial]
//        public void Make_sure_decrement_operator_correct(
//            [TimeQuantityValues] double intervalSecondsPerSecond,
//            [TimePerStepValues] double millisecondsPerStep)
//        {
//            var parts = new[] {OperatorPart.Decrement};

//            var request = MakeRequest(RunningDirection.Backward, millisecondsPerStep);

//            MakeSureStarNcrementCorrect(intervalSecondsPerSecond, millisecondsPerStep, request,
//                c => this.InvokeOperator<TClock, TClock>(parts, x => PostOperatorInvocation(x as TClock, c), c));
//        }

//        /// <summary>
//        /// Implement this method in order to expose the type specific operator overload.
//        /// </summary>
//        /// <param name="timer"></param>
//        /// <returns></returns>
//        /// <a href="!:http://msdn.microsoft.com/en-us/library/wc3z3k8c.aspx"> -- Operator (C# Reference) </a>
//        protected abstract TClock PrefixDecrementOperator(TClock timer);

//        /// <summary>
//        /// Implement this method in order to expose the type specific operator overload.
//        /// </summary>
//        /// <param name="timer"></param>
//        /// <returns></returns>
//        /// <a href="!:http://msdn.microsoft.com/en-us/library/wc3z3k8c.aspx"> -- Operator (C# Reference) </a>
//        protected abstract TClock PostfixDecrementOperator(TClock timer);

//        /// <summary>
//        /// Makes sure that the decrement operator postfix form works correctly.
//        /// </summary>
//        /// <param name="intervalSecondsPerSecond"></param>
//        /// <param name="millisecondsPerStep"></param>
//        /// <a href="!:http://msdn.microsoft.com/en-us/library/wc3z3k8c.aspx"> -- Operator (C# Reference) </a>
//        [Test]
//        [Combinatorial]
//        public void Make_sure_decrement_operator_postfix_correct(
//            [TimeQuantityValues] double intervalSecondsPerSecond,
//            [TimePerStepValues] double millisecondsPerStep)
//        {
//            var request = MakeRequest(RunningDirection.Backward, millisecondsPerStep);

//            MakeSureStarNcrementCorrect(intervalSecondsPerSecond, millisecondsPerStep,
//                request, c => Assert.That(PostfixDecrementOperator(c), Is.SameAs(c)));
//        }

//        /// <summary>
//        /// Makes sure that the decrement operator prefix form works correctly.
//        /// </summary>
//        /// <param name="intervalSecondsPerSecond"></param>
//        /// <param name="millisecondsPerStep"></param>
//        /// <a href="!:http://msdn.microsoft.com/en-us/library/wc3z3k8c.aspx"> -- Operator (C# Reference) </a>
//        [Test]
//        [Combinatorial]
//        public void Make_sure_decrement_operator_prefix_correct(
//            [TimeQuantityValues] double intervalSecondsPerSecond,
//            [TimePerStepValues] double millisecondsPerStep)
//        {
//            var request = MakeRequest(RunningDirection.Backward, millisecondsPerStep);

//            MakeSureStarNcrementCorrect(intervalSecondsPerSecond, millisecondsPerStep,
//                request, c => Assert.That(PrefixDecrementOperator(c), Is.SameAs(c)));
//        }

//        /// <summary>
//        /// Makes sure that the addition operator works correctly.
//        /// </summary>
//        /// <param name="intervalSecondsPerSecond"></param>
//        /// <param name="millisecondsPerStep"></param>
//        /// <param name="steps"></param>
//        /// <a href="!:http://msdn.microsoft.com/en-us/library/k1a63xkz.aspx"> + Operator (C# Reference) </a>
//        [Test]
//        [Combinatorial]
//        public void Make_sure_addition_operator_correct(
//            [TimeQuantityValues] double intervalSecondsPerSecond,
//            [TimePerStepValues] double millisecondsPerStep,
//            [StepValues] int steps)
//        {
//            var parts = new[] {OperatorPart.Addition};

//            var request = MakeRequest(RunningDirection.Forward, millisecondsPerStep, steps);

//            MakeSureStarNcrementCorrect(intervalSecondsPerSecond, millisecondsPerStep, request,
//                c => this.InvokeOperator<TClock, TClock>(parts, x => PostOperatorInvocation(x as TClock, c), c, steps));
//        }

//        /// <summary>
//        /// Implement this method in order to expose the type specific operator overload.
//        /// </summary>
//        /// <param name="clock"></param>
//        /// <param name="steps"></param>
//        /// <returns></returns>
//        /// <a href="!:http://msdn.microsoft.com/en-us/library/sa7629ew.aspx"> += Operator (C# Reference) </a>
//        protected abstract TClock AdditionAssignmentOperator(TClock clock, int steps);

//        /// <summary>
//        /// Makes sure that the addition assignment operator works correctly. Technically, if the
//        /// binary operator works the unary assignment should work as well, but we will do this
//        /// for sake of being thorough since it takes hardly any time at all to support.
//        /// </summary>
//        /// <param name="intervalSecondsPerSecond"></param>
//        /// <param name="millisecondsPerStep"></param>
//        /// <param name="steps"></param>
//        /// <a href="!:http://msdn.microsoft.com/en-us/library/sa7629ew.aspx"> += Operator (C# Reference) </a>
//        [Test]
//        [Combinatorial]
//        public void Make_sure_addition_assignment_operator_correct(
//            [TimeQuantityValues] double intervalSecondsPerSecond,
//            [TimePerStepValues] double millisecondsPerStep,
//            [StepValues] int steps)
//        {
//            var request = MakeRequest(RunningDirection.Forward, millisecondsPerStep, steps);

//            MakeSureStarNcrementCorrect(intervalSecondsPerSecond, millisecondsPerStep, request,
//                c => Assert.That(AdditionAssignmentOperator(c, steps), Is.SameAs(c)));
//        }

//        /// <summary>
//        /// Makes sure that the subtraction operator works correctly.
//        /// </summary>
//        /// <param name="intervalSecondsPerSecond"></param>
//        /// <param name="millisecondsPerStep"></param>
//        /// <param name="steps"></param>
//        /// <a href="!:http://msdn.microsoft.com/en-us/library/wch5w409.aspx"> - Operator (C# Reference) </a>
//        [Test]
//        [Combinatorial]
//        public void Make_sure_subtraction_operator_correct(
//            [TimeQuantityValues] double intervalSecondsPerSecond,
//            [TimePerStepValues] double millisecondsPerStep,
//            [StepValues] int steps)
//        {
//            var parts = new[] {OperatorPart.Subtraction};

//            var request = MakeRequest(RunningDirection.Backward, millisecondsPerStep, steps);

//            MakeSureStarNcrementCorrect(intervalSecondsPerSecond, millisecondsPerStep, request,
//                c => this.InvokeOperator<TClock, TClock>(parts, x => PostOperatorInvocation(x as TClock, c), c, steps));
//        }

//        /// <summary>
//        /// Implement this method in order to expose the type specific operator overload.
//        /// </summary>
//        /// <param name="clock"></param>
//        /// <param name="steps"></param>
//        /// <returns></returns>
//        /// <a href="!:http://msdn.microsoft.com/en-us/library/2y9zhhx1.aspx"> -= Operator (C# Reference) </a>
//        protected abstract TClock SubtractionAssignmentOperator(TClock clock, int steps);

//        /// <summary>
//        /// Makes sure that the subtraction assignment operator works correctly. Technically, if the
//        /// binary operator works the unary assignment should work as well, but we will do this for
//        /// sake of being thorough since it takes hardly any time at all to support.
//        /// </summary>
//        /// <param name="intervalSecondsPerSecond"></param>
//        /// <param name="millisecondsPerStep"></param>
//        /// <param name="steps"></param>
//        /// <a href="!:http://msdn.microsoft.com/en-us/library/2y9zhhx1.aspx"> -= Operator (C# Reference) </a>
//        [Test]
//        [Combinatorial]
//        public void Make_sure_subtraction_assignment_operator_correct(
//            [TimeQuantityValues] double intervalSecondsPerSecond,
//            [TimePerStepValues] double millisecondsPerStep,
//            [StepValues] int steps)
//        {
//            var request = MakeRequest(RunningDirection.Backward, millisecondsPerStep, steps);

//            MakeSureStarNcrementCorrect(intervalSecondsPerSecond, millisecondsPerStep, request,
//                c => Assert.That(SubtractionAssignmentOperator(c, steps), Is.SameAs(c)));
//        }

//        #endregion
//    }
//}
