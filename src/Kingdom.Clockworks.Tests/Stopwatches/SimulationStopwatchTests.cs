using System;
using System.Threading;
using System.Threading.Tasks;
using Kingdom.Unitworks;
using Kingdom.Unitworks.Units;
using NUnit.Framework;

namespace Kingdom.Clockworks.Stopwatches
{
    /// <summary>
    /// Extends the <see cref="TimeableClockTestFixtureBase{TClock, TRequest, TElapsedEventArgs}"/>
    /// to support the <see cref="SimulationStopwatch"/> and supporting cast of types.
    /// </summary>
    public class SimulationStopwatchTests
        : TimeableClockTestFixtureBase<
            SimulationStopwatch
            , StopwatchRequest
            , StopwatchElapsedEventArgs>
    {
        /// <summary>
        /// Returns a created <see cref="StopwatchRequest"/> given the arguments.
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="steps"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        protected override StopwatchRequest MakeRequest(RunningDirection? direction = null,
            int steps = 1, RequestType type = RequestType.Instantaneous)
        {
            return new StopwatchRequest(direction, steps, type);
        }

        //TODO: may capture these items into a SimulationStopwatchFiture of sorts...

        /// <summary>
        /// Makes the arrangements supporting the
        /// <see cref="TimeableClockTestFixtureBase{SimulationStopwatch, StopwatchRequest, StopwatchElapsedEventArgs}.Elapsed_events_yield_expected_event_counts"/>
        /// tests.
        /// </summary>
        /// <param name="stopwatch"></param>
        /// <param name="durationSeconds"></param>
        protected override void MakeElapsedEventYieldArrangements(SimulationStopwatch stopwatch, double durationSeconds)
        {
            // Nothing to do for timer purposes.
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
                //TODO: It's one thing for Stopwatch to free-run like this, but we would want for the Timer range to be somewhat controlled
                var scaledStopwatchInterval = stopwatchInterval.ToTimeQuantity(numeratorUnit)/
                                              1d.ToTimeQuantity(denominatorUnit);

                var estimatedEventCount = GetEstimatedEventCount(timerInterval, durationSeconds);

                var eventCount = 0;

                // A running quantity sum is going to be about as accurate as can be for test purposes.
                var quantitySum = 0d.ToTimeQuantity();

                // Elapsed is less meaningful because the TimeSpan just does not have the precision necessary.
                var elapsed = TimeSpan.Zero;

                RunClockScenario(stopwatch =>
                {
                    stopwatch[numeratorUnit, denominatorUnit] = stopwatchInterval;

                    stopwatch.Elapsed += (sender, e) =>
                    {
                        quantitySum += e.CurrentQuantity;
                        elapsed += e.Current;
                        eventCount++;
                    };

                    stopwatch.Start(timerInterval);

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

        #region Overloaded Operators

        ///// <summary>
        ///// Verifies that the <paramref name="e"/> outcomes are accurate.
        ///// </summary>
        ///// <param name="e"></param>
        ///// <param name="expectedElapsedMilliseconds"></param>
        //protected override void VerifyElapsedEventResults(StopwatchElapsedEventArgs e, double expectedElapsedMilliseconds)
        //{
        //    base.VerifyElapsedEventResults(e, expectedElapsedMilliseconds);
        //}

        /// <summary>
        /// Returns whether <paramref name="a"/> Equals <paramref name="b"/>.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected override bool Equals(StopwatchRequest a, StopwatchRequest b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// Implement this method in order to expose the prefix <see cref="SimulationStopwatch.op_Increment"/>
        /// operator. Technically the language handles this for us, and it should, but this gives us a code
        /// level handshake that indeed the operator was available and it did.
        /// </summary>
        /// <param name="stopwatch"></param>
        /// <returns></returns>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/36x43w8w.aspx"> ++ Operator (C# Reference) </a>
        protected override SimulationStopwatch PrefixIncrementOperator(SimulationStopwatch stopwatch)
        {
            return ++stopwatch;
        }

        /// <summary>
        /// Implement this method in order to expose the postfix <see cref="SimulationStopwatch.op_Increment"/>
        /// operator. Technically the language handles this for us, and it should, but this gives us a code
        /// level handshake that indeed the operator was available and it did.
        /// </summary>
        /// <param name="stopwatch"></param>
        /// <returns></returns>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/36x43w8w.aspx"> ++ Operator (C# Reference) </a>
        protected override SimulationStopwatch PostfixIncrementOperator(SimulationStopwatch stopwatch)
        {
            // ReSharper disable once RedundantAssignment
            return stopwatch++;
        }

        /// <summary>
        /// Implement this method in order to expose the prefix <see cref="SimulationStopwatch.op_Decrement"/>
        /// operator. Technically the language handles this for us, and it should, but this gives us a code
        /// level handshake that indeed the operator was available and it did.
        /// </summary>
        /// <param name="stopwatch"></param>
        /// <returns></returns>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/wc3z3k8c.aspx"> -- Operator (C# Reference) </a>
        protected override SimulationStopwatch PrefixDecrementOperator(SimulationStopwatch stopwatch)
        {
            return --stopwatch;
        }

        /// <summary>
        /// Implement this method in order to expose the postfix <see cref="SimulationStopwatch.op_Decrement"/>
        /// operator. Technically the language handles this for us, and it should, but this gives us a code
        /// level handshake that indeed the operator was available and it did.
        /// </summary>
        /// <param name="stopwatch"></param>
        /// <returns></returns>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/wc3z3k8c.aspx"> -- Operator (C# Reference) </a>
        protected override SimulationStopwatch PostfixDecrementOperator(SimulationStopwatch stopwatch)
        {
            // ReSharper disable once RedundantAssignment
            return stopwatch--;
        }

        /// <summary>
        /// Implement this method in order to expose the <see cref="SimulationStopwatch.op_Addition"/>
        /// assignment operator. Technically the language handles this for us, and it should, but this gives
        /// us a code level handshake that indeed the operator was available and it did.
        /// </summary>
        /// <param name="stopwatch"></param>
        /// <param name="steps"></param>
        /// <returns></returns>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/sa7629ew.aspx"> += Operator (C# Reference) </a>
        protected override SimulationStopwatch AdditionAssignmentOperator(SimulationStopwatch stopwatch, int steps)
        {
            return stopwatch += steps;
        }

        /// <summary>
        /// Implement this method in order to expose the <see cref="SimulationStopwatch.op_Subtraction"/>
        /// assignment operator. Technically the language handles this for us, and it should, but this gives
        /// us a code level handshake that indeed the operator was available and it did.
        /// </summary>
        /// <param name="stopwatch"></param>
        /// <param name="steps"></param>
        /// <returns></returns>
        /// <a href="!:http://msdn.microsoft.com/en-us/library/2y9zhhx1.aspx"> -= Operator (C# Reference) </a>
        protected override SimulationStopwatch SubtractionAssignmentOperator(SimulationStopwatch stopwatch, int steps)
        {
            return stopwatch -= steps;
        }

        #endregion
    }
}
