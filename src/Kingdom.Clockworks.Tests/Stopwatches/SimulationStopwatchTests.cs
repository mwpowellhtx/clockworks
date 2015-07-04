using System;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kingdom.Clockworks.Units;
using NUnit.Framework;

namespace Kingdom.Clockworks.Stopwatches
{
    public class SimulationStopwatchTests : TimeableClockTestFixtureBase
    {
        //TODO: may capture these items into a SimulationStopwatchFiture of sorts...

        /// <summary>
        /// 
        /// </summary>
        /// <param name="interval"></param>
        /// <param name="durationSeconds"></param>
        [Test]
        [TestCase(100, 1d)]
        [TestCase(250, 3d)]
        [TestCase(500, 5d)]
        [TestCase(750, 7d)]
        public void Elapsed_events_yield_expected_event_counts(int interval, double durationSeconds)
        {
            var expectedEventCount = GetEstimatedEventCount(interval, durationSeconds);
            var actualEventCount = 0;

            RunClockScenario<SimulationStopwatch>(ssw =>
            {
                ssw.Elapsed += (sender, e) => actualEventCount++;
                ssw.Start(interval);
                Thread.Sleep(TimeSpan.FromSeconds(durationSeconds));
            });

            Assert.That(actualEventCount, Is.EqualTo(expectedEventCount).Within(1));
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

            RunClockScenario<SimulationStopwatch>(ssw =>
            {
                var sswIntervalSecondsPerSecond = ssw[TimeUnit.Second, TimeUnit.Second];
                Assert.That(sswIntervalSecondsPerSecond, Is.EqualTo(1d).Within(Epsilon));
                Assert.That(sswIntervalSecondsPerSecond, Is.EqualTo(ssw.SecondsPerSecond));

                // Just be sure that the before and after scaled ratios are the consistent.
                var scaledInterval = GetRawTimePerTime(intervalSecondsPerSecond, numeratorUnit, denominatorUnit);
                ssw[numeratorUnit, denominatorUnit] = scaledInterval;

                Assert.That(ssw[numeratorUnit, denominatorUnit], Is.EqualTo(GetTimePerTime(
                    intervalSecondsPerSecond, numeratorUnit, denominatorUnit)).Within(Epsilon));

                foreach (var numUnit in expectedUnits)
                {
                    foreach (var denomUnit in expectedUnits)
                    {
                        VerifyUnitPerUnitProperties(ssw, intervalSecondsPerSecond, numUnit, denomUnit);
                    }
                }
            });
        }

        /// <summary>
        /// Performs the cumulative testing asynchronously.
        /// </summary>
        /// <param name="timerInterval"></param>
        /// <param name="durationSeconds"></param>
        /// <param name="stopwatchInterval"></param>
        /// <param name="numeratorUnit"></param>
        /// <param name="denominatorUnit"></param>
        /// <returns></returns>
        private static Task CumulativeAsync(int timerInterval, double durationSeconds,
            double stopwatchInterval, TimeUnit numeratorUnit, TimeUnit denominatorUnit)
        {
            return Task.Run(() =>
            {
                var scaledStopwatchInterval = stopwatchInterval.ToTimeQuantity(numeratorUnit)/
                                              1d.ToTimeQuantity(denominatorUnit);

                var estimatedEventCount = GetEstimatedEventCount(timerInterval, durationSeconds);

                var eventCount = 0;

                // A running quantity sum is going to be about as accurate as can be for test purposes.
                var quantitySum = 0d.ToTimeQuantity();

                // Elapsed is less meaningful because the TimeSpan just does not have the precision necessary.
                var elapsed = TimeSpan.Zero;

                RunClockScenario<SimulationStopwatch>(ssw =>
                {
                    ssw[numeratorUnit, denominatorUnit] = stopwatchInterval;

                    ssw.Elapsed += (sender, e) =>
                    {
                        quantitySum += e.IntervalQuantity;
                        elapsed += e.CurrentElapsed;
                        eventCount++;
                    };

                    ssw.Start(timerInterval);

                    Thread.Sleep(TimeSpan.FromSeconds(durationSeconds));
                });

                /* The timing starts to wander (and grow in deficit) when there is more activity. We don't expect more
                 * events, but we do start to see progressively fewer events with a wider range of duration to work with.
                 * Therefore, I don't think it's worth checking this fact, per se. Report it to the console, yes; assert
                 * it as an expected test outcome, no. */

                Console.WriteLine(@"{0} events out of an estimated {1} were processed.",
                    eventCount, estimatedEventCount);

                var expectedElapsedSeconds = scaledStopwatchInterval*eventCount;

                // TimeSpan does not have the resolution to pick up on some of these concerns.
                //Assert.That(elapsed.TotalSeconds, Is.EqualTo(expectedElapsedSeconds).Within(Epsilon));

                // However, the Quantity itself can be tracked accurately.
                Assert.That(quantitySum.Value, Is.EqualTo(expectedElapsedSeconds).Within(Epsilon));
            });
        }

        /// <summary>
        /// Tests that cumulative elapsed times are accurate.
        /// </summary>
        /// <param name="timerInterval"></param>
        /// <param name="stopwatchInterval"></param>
        /// <param name="durationSeconds"></param>
        /// <param name="numeratorUnit"></param>
        /// <param name="denominatorUnit"></param>
        [Test]
        [Combinatorial]
        public async void Cumulative_elapsed_times_are_accurate([TimerIntervalValues] int timerInterval,
            [DurationValues] double durationSeconds, [TimeQuantityValues] double stopwatchInterval,
            [TimeUnitValues] TimeUnit numeratorUnit, [TimeUnitValues] TimeUnit denominatorUnit)
        {
            await CumulativeAsync(timerInterval, durationSeconds, stopwatchInterval,
                numeratorUnit, denominatorUnit);
        }

        #region Increment and Decrement Members

        /// <summary>
        /// Verifies that the "star"-ncrement operations function correctly.
        /// </summary>
        /// <param name="intervalSecondsPerSecond"></param>
        /// <param name="expectedRequest"></param>
        /// <param name="e"></param>
        private static void VerifyStarNcrementRequest(double intervalSecondsPerSecond,
            StopwatchRequest expectedRequest, StopwatchElapsedEventArgs e)
        {
            Assert.That(expectedRequest, Is.Not.Null);

            // TODO: in and of themselves these really deserve a dedicated StopwatchRequestsTests suite...
            // TODO: however, for the same of establishing "SameAs", this may be interesting...
            Assert.That(expectedRequest.Equals(expectedRequest));
            Assert.That(e.Request.Equals(e.Request));
            Assert.That(((ISteppableRequest) expectedRequest).Equals(expectedRequest));
            Assert.That(((ISteppableRequest) e.Request).Equals(e.Request));

            // We establish that they should equal the other, but that they should not be the same.
            Assert.That(e.Request, Is.Not.SameAs(expectedRequest));
            Assert.That(e.Request.Equals(expectedRequest));
            Assert.That(((ISteppableRequest) e.Request).Equals(expectedRequest));

            //TODO: we're probably a little too intwined with the subject being tested... but this is a decent enough start...
            var expectedElapsedMilliseconds = (intervalSecondsPerSecond.ToTimeQuantity()
                .ToTimeQuantity(TimeUnit.Millisecond)*expectedRequest.Steps).Value;

            // It so happens that we're expecting the one interval, also in total.
            Assert.That(e.IntervalQuantity.Value, Is.EqualTo(expectedElapsedMilliseconds).Within(Epsilon));
            Assert.That(e.TotalElapsedQuantity.Value, Is.EqualTo(expectedElapsedMilliseconds).Within(Epsilon));
        }

        /// <summary>
        /// Verifies the outcome regardless whether <see cref="ISteppableClock.Increment()"/>,
        /// <see cref="ISteppableClock.Decrement()"/>, or one of its neighbors were involved.
        /// </summary>
        /// <param name="intervalSecondsPerSecond"></param>
        /// <param name="expectedRequest"></param>
        /// <param name="theAction"></param>
        private static void MakeSureStarNcrementCorrect(double intervalSecondsPerSecond,
            StopwatchRequest expectedRequest, Action<SimulationStopwatch> theAction)
        {
            Assert.That(theAction, Is.Not.Null);

            /* I am really loathe to take on too many in the way of dependencies for a test
             * suite, but this particular set of tests does really lend itself to listening
             * reactively, in an observable manner, for one event outcomes. */

            RunClockScenario<SimulationStopwatch>(ssw =>
            {
                ssw.SecondsPerSecond = intervalSecondsPerSecond;

                var observableElapsed = Observable.FromEventPattern<StopwatchElapsedEventArgs>(
                    handler => ssw.Elapsed += handler, handler => ssw.Elapsed -= handler)
                    .Select(p => p.EventArgs);

                // ReSharper disable once AccessToModifiedClosure
                using (observableElapsed.Subscribe(e => VerifyStarNcrementRequest(
                    intervalSecondsPerSecond, expectedRequest, e)))
                {
                    theAction(ssw);
                }
            });
        }

        /// <summary>
        /// Makes sure that the <see cref="SimulationStopwatch.Increment(int,RequestType)"/> method
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
            MakeSureStarNcrementCorrect(intervalSecondsPerSecond,
                new StopwatchRequest(RunningDirection.Forward, steps, type),
                sw => sw.Increment(steps, type));
        }

        /// <summary>
        /// Makes sure that the <see cref="SimulationStopwatch.Decrement(int,RequestType)"/> method
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
            MakeSureStarNcrementCorrect(intervalSecondsPerSecond,
                new StopwatchRequest(RunningDirection.Backward, steps, type),
                sw => sw.Decrement(steps, type));
        }

        /// <summary>
        /// Makes sure that the <see cref="SimulationStopwatch.Increment()"/> method works correctly.
        /// </summary>
        /// <param name="intervalSecondsPerSecond"></param>
        [Test]
        [Combinatorial]
        public void Make_sure_increment_method_no_args_correct(
            [TimeQuantityValues] double intervalSecondsPerSecond)
        {
            MakeSureStarNcrementCorrect(intervalSecondsPerSecond,
                new StopwatchRequest(RunningDirection.Forward),
                sw => sw.Increment());
        }

        /// <summary>
        /// Makes sure that the <see cref="SimulationStopwatch.Decrement()"/> method works correctly.
        /// </summary>
        /// <param name="intervalSecondsPerSecond"></param>
        [Test]
        [Combinatorial]
        public void Make_sure_decrement_method_no_args_correct(
            [TimeQuantityValues] double intervalSecondsPerSecond)
        {
            MakeSureStarNcrementCorrect(intervalSecondsPerSecond,
                new StopwatchRequest(RunningDirection.Backward),
                sw => sw.Decrement());
        }

        /// <summary>
        /// Makes sure that the <see cref="SimulationStopwatch.op_Increment"/> operator postfix form works correctly.
        /// </summary>
        /// <param name="intervalSecondsPerSecond"></param>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/36x43w8w.aspx"> ++ Operator (C# Reference) </a>
        [Test]
        [Combinatorial]
        public void Make_sure_increment_operator_postfix_correct(
            [TimeQuantityValues] double intervalSecondsPerSecond)
        {
            // ReSharper disable once RedundantAssignment
            MakeSureStarNcrementCorrect(intervalSecondsPerSecond,
                new StopwatchRequest(RunningDirection.Forward),
                sw => sw++);
        }

        /// <summary>
        /// Makes sure that the <see cref="SimulationStopwatch.op_Increment"/> operator prefix form works correctly.
        /// </summary>
        /// <param name="intervalSecondsPerSecond"></param>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/36x43w8w.aspx"> ++ Operator (C# Reference) </a>
        [Test]
        [Combinatorial]
        public void Make_sure_increment_operator_prefix_correct(
            [TimeQuantityValues] double intervalSecondsPerSecond)
        {
            // ReSharper disable once RedundantAssignment
            MakeSureStarNcrementCorrect(intervalSecondsPerSecond,
                new StopwatchRequest(RunningDirection.Forward),
                sw => ++sw);
        }

        /// <summary>
        /// Makes sure that the <see cref="SimulationStopwatch.op_Decrement"/> operator postfix form works correctly.
        /// </summary>
        /// <param name="intervalSecondsPerSecond"></param>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/wc3z3k8c.aspx"> -- Operator (C# Reference) </a>
        [Test]
        [Combinatorial]
        public void Make_sure_decrement_operator_postfix_correct(
            [TimeQuantityValues] double intervalSecondsPerSecond)
        {
            // ReSharper disable once RedundantAssignment
            MakeSureStarNcrementCorrect(intervalSecondsPerSecond,
                new StopwatchRequest(RunningDirection.Backward),
                sw => sw--);
        }

        /// <summary>
        /// Makes sure that the <see cref="SimulationStopwatch.op_Decrement"/> operator prefix form works correctly.
        /// </summary>
        /// <param name="intervalSecondsPerSecond"></param>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/wc3z3k8c.aspx"> -- Operator (C# Reference) </a>
        [Test]
        [Combinatorial]
        public void Make_sure_decrement_operator_prefix_correct(
            [TimeQuantityValues] double intervalSecondsPerSecond)
        {
            // ReSharper disable once RedundantAssignment
            MakeSureStarNcrementCorrect(intervalSecondsPerSecond,
                new StopwatchRequest(RunningDirection.Backward),
                sw => --sw);
        }

        /// <summary>
        /// Makes sure that the <see cref="SimulationStopwatch.op_Addition"/> operator works correctly.
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
            MakeSureStarNcrementCorrect(intervalSecondsPerSecond,
                new StopwatchRequest(RunningDirection.Forward, steps),
                sw => Assert.That(sw + steps, Is.SameAs(sw)));
        }

        /// <summary>
        /// Makes sure that the <see cref="SimulationStopwatch.op_Addition"/> assignment operator
        /// works correctly. Technically, if the binary operator works the unary assignment should
        /// work as well, but we will do this for sake of being thorough since it takes hardly any
        /// time at all to support.
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
            MakeSureStarNcrementCorrect(intervalSecondsPerSecond,
                new StopwatchRequest(RunningDirection.Forward, steps),
                sw => Assert.That(sw += steps, Is.SameAs(sw)));
        }

        /// <summary>
        /// Makes sure that the <see cref="SimulationStopwatch.op_Subtraction"/> operator works correctly.
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
            MakeSureStarNcrementCorrect(intervalSecondsPerSecond,
                new StopwatchRequest(RunningDirection.Backward, steps),
                sw => Assert.That(sw - steps, Is.SameAs(sw)));
        }

        /// <summary>
        /// Makes sure that the <see cref="SimulationStopwatch.op_Subtraction"/> assignment operator
        /// works correctly. Technically, if the binary operator works the unary assignment should
        /// work as well, but we will do this for sake of being thorough since it takes hardly any
        /// time at all to support.
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
            MakeSureStarNcrementCorrect(intervalSecondsPerSecond,
                new StopwatchRequest(RunningDirection.Backward, steps),
                sw => Assert.That(sw -= steps, Is.SameAs(sw)));
        }

        #endregion
    }
}
