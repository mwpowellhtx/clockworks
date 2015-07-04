using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading;
using Kingdom.Clockworks.Units;
using NUnit.Framework;

namespace Kingdom.Clockworks
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TClock"></typeparam>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TElapsedEventArgs"></typeparam>
    public abstract class TimeableClockTestFixtureBase<TClock, TRequest, TElapsedEventArgs> : ClockworksTestFixtureBase
        where TClock : TimeableClockBase<TRequest, TElapsedEventArgs>, new()
        where TRequest : TimeableRequestBase
        where TElapsedEventArgs : ElapsedEventArgsBase<TRequest>
    {
        /// <summary>
        /// Returns a created <typeparamref name="TRequest"/> given the arguments.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="steps"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected abstract TRequest MakeRequest(RunningDirection? direction = null,
            int steps = 1, RequestType type = RequestType.Instantaneous);

        /// <summary>
        /// Runs the timer <paramref name="scenario"/>.
        /// </summary>
        /// <param name="scenario"></param>
        protected static void RunClockScenario(Action<TClock> scenario)
        {
            Assert.That(scenario, Is.Not.Null);
            using (var clock = new TClock())
            {
                scenario(clock);
            }
        }

        /// <summary>
        /// Returns the property value accessed by forming the property name from
        /// the <paramref name="numeratorUnit"/> and <paramref name="denominatorUnit"/>.
        /// </summary>
        /// <param name="scaleable"></param>
        /// <param name="numeratorUnit"></param>
        /// <param name="denominatorUnit"></param>
        /// <returns></returns>
        protected static double? GetStopwatchMeasure(IScaleableClock scaleable,
            TimeUnit numeratorUnit, TimeUnit denominatorUnit)
        {
            var name = string.Format(@"{0}sPer{1}", numeratorUnit, denominatorUnit);
            const BindingFlags flags = BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance;
            var property = scaleable.GetType().GetProperty(name, flags);
            return property == null ? (double?) null : (double) property.GetValue(scaleable);
        }

        /// <summary>
        /// Returns the raw ratio whose result should be expressed in terms of the numerator
        /// and denominator units.
        /// </summary>
        /// <param name="intervalSecondsPerSecond"></param>
        /// <param name="numeratorUnit"></param>
        /// <param name="denominatorUnit"></param>
        /// <returns></returns>
        protected double GetRawTimePerTime(double intervalSecondsPerSecond,
            TimeUnit numeratorUnit, TimeUnit denominatorUnit)
        {
            // Yes we start from this basis for the ratio.
            var num = ConvertFromBase(numeratorUnit, intervalSecondsPerSecond);
            var denom = ConvertFromBase(denominatorUnit, 1d);
            return num/denom;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="intervalSecondsPerSecond"></param>
        /// <param name="numeratorUnit"></param>
        /// <param name="denominatorUnit"></param>
        /// <returns></returns>
        protected double GetTimePerTime(double intervalSecondsPerSecond,
            TimeUnit numeratorUnit, TimeUnit denominatorUnit)
        {
            // Yes we start from this basis for the ratio.
            var num = ConvertFromBase(numeratorUnit, intervalSecondsPerSecond);
            var denom = ConvertFromBase(denominatorUnit, 1d);

            // Be sure and compare/contrast like with like.
            var baseNum = ConvertToBase(numeratorUnit, num);
            var baseDenom = ConvertToBase(denominatorUnit, denom);

            return baseNum/baseDenom;
        }

        /// <summary>
        /// Returns the estimated event count calculating <paramref name="interval"/> into
        /// <paramref name="durationSeconds"/>.
        /// </summary>
        /// <param name="interval">A timer period in milliseconds.</param>
        /// <param name="durationSeconds">The number of seconds in which to run.</param>
        /// <returns></returns>
        protected static int GetEstimatedEventCount(double interval, double durationSeconds)
        {
            var result = 0;

            while (durationSeconds*1000d
                   > interval*(result == 0 ? 0 : 1)
                   + interval*(result > 0 ? result - 1 : 0))
            {
                result++;
            }

            return result;
        }

        /// <summary>
        /// Makes the necessary arrangements supporting the
        /// <see cref="Elapsed_events_yield_expected_event_counts"/> tests.
        /// </summary>
        /// <param name="clock"></param>
        /// <param name="durationSeconds"></param>
        protected abstract void MakeElapsedEventYieldArrangements(TClock clock, double durationSeconds);

        /// <summary>
        /// Tests that elapsed events yield the expected event count. The arrangements
        /// of which depend on the <typeparamref name="TClock"/>.
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="durationSeconds"></param>
        /// <see cref="MakeElapsedEventYieldArrangements"/>
        [Test]
        [TestCase(100, 1d)]
        [TestCase(250, 3d)]
        [TestCase(500, 5d)]
        [TestCase(750, 7d)]
        public void Elapsed_events_yield_expected_event_counts(int interval, double durationSeconds)
        {
            var expectedEventCount = GetEstimatedEventCount(interval, durationSeconds);

            var actualEventCount = 0;

            RunClockScenario(c =>
            {
                c.Elapsed += delegate { actualEventCount++; };

                MakeElapsedEventYieldArrangements(c, durationSeconds);

                c.Start(interval);

                Thread.Sleep(TimeSpan.FromSeconds(durationSeconds));
            });

            Assert.That(actualEventCount, Is.GreaterThan(0));
            Assert.That(actualEventCount, Is.EqualTo(expectedEventCount).Within(1));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scaleable"></param>
        /// <param name="intervalSecondsPerSecond"></param>
        /// <param name="numeratorUnit"></param>
        /// <param name="denominatorUnit"></param>
        protected void VerifyUnitPerUnitProperties(IScaleableClock scaleable,
            double intervalSecondsPerSecond, TimeUnit numeratorUnit, TimeUnit denominatorUnit)
        {
            var actualValue = GetStopwatchMeasure(scaleable, numeratorUnit, denominatorUnit);

            if (actualValue == null) return;

            var expectedValue = GetTimePerTime(intervalSecondsPerSecond,
                numeratorUnit, denominatorUnit);

            Assert.That(actualValue.Value, Is.EqualTo(expectedValue).Within(Epsilon),
                @"Verification failed for numerator {0} and denominator {1}.",
                numeratorUnit, denominatorUnit);
        }

        [Test]
        [TestCase(1d, TimeUnit.Millisecond, 0.001d)]
        [TestCase(1d, TimeUnit.Second, 1d)]
        [TestCase(1d, TimeUnit.Minute, 60d)]
        [TestCase(1d, TimeUnit.Hour, 3600d)]
        [TestCase(1d, TimeUnit.Day, 86400d)]
        [TestCase(1d, TimeUnit.Week, 604800d)]
        public void Converting_time_quantities_throws_no_exceptions(double value, TimeUnit unit, double expectedValue)
        {
            //TODO: pick up other TimeUnit parts...
            var actualValue = 0d;

            TestDelegate getter = () => actualValue = ConvertToBase(unit, value);

            Assert.That(getter, Throws.Nothing);

            Assert.That(actualValue, Is.EqualTo(expectedValue).Within(Epsilon));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="intervalSecondsPerSecond"></param>
        /// <param name="numeratorUnit"></param>
        /// <param name="denominatorUnit"></param>
        [Test]
        [Combinatorial]
        public void Stopwatch_interval_unit_ratios_are_consistent(
            [TimeQuantityValues] double intervalSecondsPerSecond,
            [TimeUnitValues] TimeUnit numeratorUnit,
            [TimeUnitValues] TimeUnit denominatorUnit)
        {
            var expectedUnits = new[]
            {
                TimeUnit.Millisecond,
                TimeUnit.Second,
                TimeUnit.Minute,
                TimeUnit.Hour
            };

            RunClockScenario(c =>
            {
                var baseInterval = c[TimeUnit.Second, TimeUnit.Second];
                Assert.That(baseInterval, Is.EqualTo(1d).Within(Epsilon));
                Assert.That(baseInterval, Is.EqualTo(c.SecondsPerSecond));

                // Just be sure that the before and after scaled ratios are the consistent.
                var scaledInterval = GetRawTimePerTime(intervalSecondsPerSecond, numeratorUnit, denominatorUnit);
                c[numeratorUnit, denominatorUnit] = scaledInterval;

                Assert.That(c[numeratorUnit, denominatorUnit], Is.EqualTo(GetTimePerTime(
                    intervalSecondsPerSecond, numeratorUnit, denominatorUnit)).Within(Epsilon));

                foreach (var numUnit in expectedUnits)
                {
                    foreach (var denomUnit in expectedUnits)
                    {
                        VerifyUnitPerUnitProperties(c, intervalSecondsPerSecond, numUnit, denomUnit);
                    }
                }
            });
        }

        #region Overloaded Operators

        /// <summary>
        /// Returns whether <paramref name="a"/> Equals <paramref name="b"/>.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected abstract bool Equals(TRequest a, TRequest b);

        /// <summary>
        /// Returns whether <paramref name="a"/> Equals <paramref name="b"/>.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected static bool Equals(ISteppableRequest a, ISteppableRequest b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        /// <param name="expectedElapsedMilliseconds"></param>
        protected virtual void VerifyElapsedEventResults(TElapsedEventArgs e, double expectedElapsedMilliseconds)
        {
            Assert.That(e, Is.Not.Null);

            // It so happens that we're expecting the one interval.
            Assert.That(e.CurrentQuantity.Value, Is.EqualTo(expectedElapsedMilliseconds).Within(Epsilon));

            // Also verify in total.
            Assert.That(e.ElapsedQuantity.Value, Is.EqualTo(expectedElapsedMilliseconds).Within(Epsilon));
        }

        /// <summary>
        /// Verifies that the "star"-ncrement operations function correctly.
        /// </summary>
        /// <param name="intervalSecondsPerSecond"></param>
        /// <param name="expectedRequest"></param>
        /// <param name="e"></param>
        protected void VerifyStarNcrementRequest(double intervalSecondsPerSecond, TRequest expectedRequest,
            TElapsedEventArgs e)
        {
            Assert.That(expectedRequest, Is.Not.Null);

            // TODO: in and of themselves these really deserve a dedicated StopwatchRequestsTests suite...
            // TODO: however, for the same of establishing "SameAs", this may be interesting...

            // Rule out a couple of seemingly obvious test cases, laying the foundation for what's coming next.
            Assert.That(Equals(expectedRequest, expectedRequest));
            Assert.That(Equals(e.Request, e.Request));

            Assert.That(Equals((ISteppableRequest) expectedRequest, expectedRequest));
            Assert.That(Equals((ISteppableRequest) e.Request, (ISteppableRequest) e.Request));

            // We establish that they should equal the other, but that they should not be the same.
            Assert.That(e.Request, Is.Not.SameAs(expectedRequest));

            Assert.That(Equals(e.Request, expectedRequest));
            Assert.That(Equals((ISteppableRequest) e.Request, (ISteppableRequest) expectedRequest));

            //TODO: we're probably a little too intwined with the subject being tested... but this is a decent enough start...
            var expectedElapsedMilliseconds = (intervalSecondsPerSecond.ToTimeQuantity()
                .ToTimeQuantity(TimeUnit.Millisecond)*expectedRequest.Steps).Value;

            VerifyElapsedEventResults(e, expectedElapsedMilliseconds);
        }

        /// <summary>
        /// Initializes <paramref name="theClock"/> given the arguments.
        /// </summary>
        /// <param name="theClock"></param>
        /// <param name="intervalSecondsPerSecond"></param>
        protected virtual void InitializeStarNcrementClock(TClock theClock,
            double intervalSecondsPerSecond)
        {
            theClock.SecondsPerSecond = intervalSecondsPerSecond;
        }

        /// <summary>
        /// Verifies the outcome regardless whether <see cref="ISteppableClock.Increment()"/>,
        /// <see cref="ISteppableClock.Decrement()"/>, or one of its neighbors were involved.
        /// </summary>
        /// <param name="intervalSecondsPerSecond"></param>
        /// <param name="expectedRequest"></param>
        /// <param name="theAction"></param>
        protected void MakeSureStarNcrementCorrect(double intervalSecondsPerSecond,
            TRequest expectedRequest, Action<TClock> theAction)
        {
            Assert.That(theAction, Is.Not.Null);

            /* I am really loathe to take on too many in the way of dependencies for a test suite,
             * but this particular set of tests does really lend itself to listening reactively, in
             * an observable manner, for one event outcomes. */

            RunClockScenario(clock =>
            {
                InitializeStarNcrementClock(clock, intervalSecondsPerSecond);

                var observableElapsed = Observable.FromEventPattern<TElapsedEventArgs>(
                    handler => clock.Elapsed += handler, handler => clock.Elapsed -= handler)
                    .Select(p => p.EventArgs);

                // ReSharper disable once AccessToModifiedClosure
                using (observableElapsed.Subscribe(e => VerifyStarNcrementRequest(
                    intervalSecondsPerSecond, expectedRequest, e)))
                {
                    theAction(clock);
                }
            });
        }

        /// <summary>
        /// Makes sure that the <see cref="ISteppableClock.Increment(int,RequestType)"/> method
        /// works correctly.
        /// </summary>
        /// <param name="intervalSecondsPerSecond"></param>
        /// <param name="steps"></param>
        /// <param name="type"></param>
        [Test]
        [Combinatorial]
        public void Make_sure_increment_method_correct(
            [TimeQuantityValues] double intervalSecondsPerSecond,
            [StepValues] int steps, [RequestTypeValues] RequestType type)
        {
            var request = MakeRequest(RunningDirection.Forward, steps, type);
            MakeSureStarNcrementCorrect(intervalSecondsPerSecond,
                request, c => c.Increment(steps, type));
        }

        /// <summary>
        /// Makes sure that the <see cref="ISteppableClock.Decrement(int,RequestType)"/> method
        /// works correctly.
        /// </summary>
        /// <param name="intervalSecondsPerSecond"></param>
        /// <param name="steps"></param>
        /// <param name="type"></param>
        [Test]
        [Combinatorial]
        public void Make_sure_decrement_method_correct(
            [TimeQuantityValues] double intervalSecondsPerSecond,
            [StepValues] int steps, [RequestTypeValues] RequestType type)
        {
            var request = MakeRequest(RunningDirection.Backward, steps, type);
            MakeSureStarNcrementCorrect(intervalSecondsPerSecond,
                request, c => c.Decrement(steps, type));
        }

        /// <summary>
        /// Makes sure that the <see cref="ISteppableClock.Increment()"/> method works correctly.
        /// </summary>
        /// <param name="intervalSecondsPerSecond"></param>
        [Test]
        [Combinatorial]
        public void Make_sure_increment_method_no_args_correct(
            [TimeQuantityValues] double intervalSecondsPerSecond)
        {
            var request = MakeRequest(RunningDirection.Forward);
            MakeSureStarNcrementCorrect(intervalSecondsPerSecond,
                request, c => c.Increment());
        }

        /// <summary>
        /// Makes sure that the <see cref="ISteppableClock.Decrement()"/> method works correctly.
        /// </summary>
        /// <param name="intervalSecondsPerSecond"></param>
        [Test]
        [Combinatorial]
        public void Make_sure_decrement_method_no_args_correct(
            [TimeQuantityValues] double intervalSecondsPerSecond)
        {
            var request = MakeRequest(RunningDirection.Backward);
            MakeSureStarNcrementCorrect(intervalSecondsPerSecond,
                request, c => c.Decrement());
        }

        /// <summary>
        /// Invokes the operator as instructed via the <paramref name="parts"/> and <paramref name="args"/>.
        /// </summary>
        /// <param name="theClock"></param>
        /// <param name="parts"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        /// <a href="!:http://stackoverflow.com/questions/3016429/reflection-and-operator-overloads-in-c-sharp">
        /// reflection-and-operator-overloads-in-c-sharp</a>
        private static TClock InvokeOperator(TClock theClock, IEnumerable<OperatorPart> parts, params object[] args)
        {
            var name = parts.Select(p => p.ToString()).Aggregate(@"op_", (g, x) => g + x);
            const BindingFlags flags = BindingFlags.Public | BindingFlags.Static;
            var clockType = typeof (TClock);
            var staticArgs = new object[] {theClock}.Concat(args).ToArray();
            var staticArgTypes = staticArgs.Select(a => a.GetType()).ToArray();
            var op = clockType.GetMethod(name, flags, Type.DefaultBinder, staticArgTypes, null);

            Assert.That(op, Is.Not.Null, @"Unable to identify the operator named {0} with {1} binding flags and {2} arguments",
                name, flags, string.Join(@", ", staticArgTypes.Select(t => t.FullName)));

            //TODO: may want to specify the expected argument types...
            //var result2 = clockType.InvokeMember(name, flags, Type.DefaultBinder, null, staticArgs);
            //var result = clockType.InvokeMember(name, flags, Type.DefaultBinder, null, staticArgs);
            var result = op.Invoke(null, staticArgs);
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.InstanceOf<TClock>());
            Assert.That(result, Is.SameAs(theClock));
            return theClock;
        }

        /// <summary>
        /// Makes sure that the increment operator works correctly.
        /// </summary>
        /// <param name="intervalSecondsPerSecond"></param>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/36x43w8w.aspx"> ++ Operator (C# Reference) </a>
        [Test]
        [Combinatorial]
        public void Make_sure_increment_operator_correct(
            [TimeQuantityValues] double intervalSecondsPerSecond)
        {
            var parts = new[] {OperatorPart.Increment};
            var request = MakeRequest(RunningDirection.Forward);
            MakeSureStarNcrementCorrect(intervalSecondsPerSecond,
                request, c => InvokeOperator(c, parts));
        }

        /// <summary>
        /// Implement this method in order to expose the type specific operator overload.
        /// </summary>
        /// <param name="timer"></param>
        /// <returns></returns>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/36x43w8w.aspx"> ++ Operator (C# Reference) </a>
        protected abstract TClock PrefixIncrementOperator(TClock timer);

        /// <summary>
        /// Implement this method in order to expose the type specific operator overload.
        /// </summary>
        /// <param name="timer"></param>
        /// <returns></returns>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/36x43w8w.aspx"> ++ Operator (C# Reference) </a>
        protected abstract TClock PostfixIncrementOperator(TClock timer);

        /// <summary>
        /// Makes sure that the increment operator postfix form works correctly.
        /// </summary>
        /// <param name="intervalSecondsPerSecond"></param>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/36x43w8w.aspx"> ++ Operator (C# Reference) </a>
        [Test]
        [Combinatorial]
        public void Make_sure_increment_operator_postfix_correct(
            [TimeQuantityValues] double intervalSecondsPerSecond)
        {
            var request = MakeRequest(RunningDirection.Forward);
            MakeSureStarNcrementCorrect(intervalSecondsPerSecond, request,
                c => Assert.That(PostfixIncrementOperator(c), Is.SameAs(c)));
        }

        /// <summary>
        /// Makes sure that the increment operator prefix form works correctly.
        /// </summary>
        /// <param name="intervalSecondsPerSecond"></param>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/36x43w8w.aspx"> ++ Operator (C# Reference) </a>
        [Test]
        [Combinatorial]
        public void Make_sure_increment_operator_prefix_correct(
            [TimeQuantityValues] double intervalSecondsPerSecond)
        {
            var request = MakeRequest(RunningDirection.Forward);
            MakeSureStarNcrementCorrect(intervalSecondsPerSecond, request,
                c => Assert.That(PrefixIncrementOperator(c), Is.SameAs(c)));
        }

        /// <summary>
        /// Makes sure that the decrement operator works correctly.
        /// </summary>
        /// <param name="intervalSecondsPerSecond"></param>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/wc3z3k8c.aspx"> -- Operator (C# Reference) </a>
        [Test]
        [Combinatorial]
        public void Make_sure_decrement_operator_correct(
            [TimeQuantityValues] double intervalSecondsPerSecond)
        {
            var parts = new[] {OperatorPart.Decrement};
            var request = MakeRequest(RunningDirection.Backward);
            MakeSureStarNcrementCorrect(intervalSecondsPerSecond,
                request, c => InvokeOperator(c, parts));
        }

        /// <summary>
        /// Implement this method in order to expose the type specific operator overload.
        /// </summary>
        /// <param name="timer"></param>
        /// <returns></returns>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/wc3z3k8c.aspx"> -- Operator (C# Reference) </a>
        protected abstract TClock PrefixDecrementOperator(TClock timer);

        /// <summary>
        /// Implement this method in order to expose the type specific operator overload.
        /// </summary>
        /// <param name="timer"></param>
        /// <returns></returns>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/wc3z3k8c.aspx"> -- Operator (C# Reference) </a>
        protected abstract TClock PostfixDecrementOperator(TClock timer);

        /// <summary>
        /// Makes sure that the decrement operator postfix form works correctly.
        /// </summary>
        /// <param name="intervalSecondsPerSecond"></param>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/wc3z3k8c.aspx"> -- Operator (C# Reference) </a>
        [Test]
        [Combinatorial]
        public void Make_sure_decrement_operator_postfix_correct(
            [TimeQuantityValues] double intervalSecondsPerSecond)
        {
            var request = MakeRequest(RunningDirection.Backward);
            MakeSureStarNcrementCorrect(intervalSecondsPerSecond, request,
                c => Assert.That(PostfixDecrementOperator(c), Is.SameAs(c)));
        }

        /// <summary>
        /// Makes sure that the decrement operator prefix form works correctly.
        /// </summary>
        /// <param name="intervalSecondsPerSecond"></param>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/wc3z3k8c.aspx"> -- Operator (C# Reference) </a>
        [Test]
        [Combinatorial]
        public void Make_sure_decrement_operator_prefix_correct(
            [TimeQuantityValues] double intervalSecondsPerSecond)
        {
            var request = MakeRequest(RunningDirection.Backward);
            MakeSureStarNcrementCorrect(intervalSecondsPerSecond, request,
                c => Assert.That(PrefixDecrementOperator(c), Is.SameAs(c)));
        }

        /// <summary>
        /// Makes sure that the addition operator works correctly.
        /// </summary>
        /// <param name="intervalSecondsPerSecond"></param>
        /// <param name="steps"></param>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/k1a63xkz.aspx"> + Operator (C# Reference) </a>
        [Test]
        [Combinatorial]
        public void Make_sure_addition_operator_correct(
            [TimeQuantityValues] double intervalSecondsPerSecond,
            [StepValues] int steps)
        {
            var parts = new[] {OperatorPart.Addition};
            var request = MakeRequest(RunningDirection.Forward, steps);
            MakeSureStarNcrementCorrect(intervalSecondsPerSecond,
                request, c => InvokeOperator(c, parts, steps));
        }

        /// <summary>
        /// Implement this method in order to expose the type specific operator overload.
        /// </summary>
        /// <param name="clock"></param>
        /// <param name="steps"></param>
        /// <returns></returns>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/sa7629ew.aspx"> += Operator (C# Reference) </a>
        protected abstract TClock AdditionAssignmentOperator(TClock clock, int steps);

        /// <summary>
        /// Makes sure that the addition assignment operator works correctly. Technically, if the
        /// binary operator works the unary assignment should work as well, but we will do this
        /// for sake of being thorough since it takes hardly any time at all to support.
        /// </summary>
        /// <param name="intervalSecondsPerSecond"></param>
        /// <param name="steps"></param>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/sa7629ew.aspx"> += Operator (C# Reference) </a>
        [Test]
        [Combinatorial]
        public void Make_sure_addition_assignment_operator_correct(
            [TimeQuantityValues] double intervalSecondsPerSecond,
            [StepValues] int steps)
        {
            var request = MakeRequest(RunningDirection.Forward, steps);
            MakeSureStarNcrementCorrect(intervalSecondsPerSecond, request,
                c => Assert.That(AdditionAssignmentOperator(c, steps), Is.SameAs(c)));
        }

        /// <summary>
        /// Makes sure that the subtraction operator works correctly.
        /// </summary>
        /// <param name="intervalSecondsPerSecond"></param>
        /// <param name="steps"></param>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/wch5w409.aspx"> - Operator (C# Reference) </a>
        [Test]
        [Combinatorial]
        public void Make_sure_subtraction_operator_correct(
            [TimeQuantityValues] double intervalSecondsPerSecond,
            [StepValues] int steps)
        {
            var parts = new[] {OperatorPart.Subtraction};
            var request = MakeRequest(RunningDirection.Backward, steps);
            MakeSureStarNcrementCorrect(intervalSecondsPerSecond,
                request, c => InvokeOperator(c, parts, steps));
        }

        /// <summary>
        /// Implement this method in order to expose the type specific operator overload.
        /// </summary>
        /// <param name="clock"></param>
        /// <param name="steps"></param>
        /// <returns></returns>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/2y9zhhx1.aspx"> -= Operator (C# Reference) </a>
        protected abstract TClock SubtractionAssignmentOperator(TClock clock, int steps);

        /// <summary>
        /// Makes sure that the subtraction assignment operator works correctly. Technically, if the
        /// binary operator works the unary assignment should work as well, but we will do this for
        /// sake of being thorough since it takes hardly any time at all to support.
        /// </summary>
        /// <param name="intervalSecondsPerSecond"></param>
        /// <param name="steps"></param>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/2y9zhhx1.aspx"> -= Operator (C# Reference) </a>
        [Test]
        [Combinatorial]
        public void Make_sure_subtraction_assignment_operator_correct(
            [TimeQuantityValues] double intervalSecondsPerSecond,
            [StepValues] int steps)
        {
            var request = MakeRequest(RunningDirection.Backward, steps);
            MakeSureStarNcrementCorrect(intervalSecondsPerSecond, request,
                c => Assert.That(SubtractionAssignmentOperator(c, steps), Is.SameAs(c)));
        }

        #endregion
    }
}
