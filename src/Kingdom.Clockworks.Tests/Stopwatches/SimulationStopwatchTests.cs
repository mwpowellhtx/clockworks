using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Kingdom.Clockworks.Units;
using NUnit.Framework;

namespace Kingdom.Clockworks
{
    public class SimulationStopwatchTests : ClockworksTestFixtureBase
    {
        /// <summary>
        /// Runs the stopwatch <paramref name="scenario"/>.
        /// </summary>
        /// <param name="scenario"></param>
        private static void RunStopwatchScenario(Action<SimulationStopwatch> scenario)
        {
            Assert.That(scenario, Is.Not.Null);
            using (var stopwatch = new SimulationStopwatch())
            {
                scenario(stopwatch);
            }
        }

        /// <summary>
        /// Returns the property value accessed by forming the property name from
        /// the <paramref name="numeratorUnit"/> and <paramref name="denominatorUnit"/>.
        /// </summary>
        /// <param name="scalable"></param>
        /// <param name="numeratorUnit"></param>
        /// <param name="denominatorUnit"></param>
        /// <returns></returns>
        private static double? GetStopwatchMeasure(IScalableStopwatch scalable,
            TimeUnit numeratorUnit, TimeUnit denominatorUnit)
        {
            var name = string.Format(@"{0}sPer{1}", numeratorUnit, denominatorUnit);
            const BindingFlags flags = BindingFlags.Public | BindingFlags.GetProperty | BindingFlags.Instance;
            var property = scalable.GetType().GetProperty(name, flags);
            return property == null ? (double?) null : (double) property.GetValue(scalable);
        }

        /// <summary>
        /// Returns the raw ratio whose result should be expressed in terms of the numerator
        /// and denominator units.
        /// </summary>
        /// <param name="intervalSecondsPerSecond"></param>
        /// <param name="numeratorUnit"></param>
        /// <param name="denominatorUnit"></param>
        /// <returns></returns>
        private double GetRawTimePerTime(double intervalSecondsPerSecond,
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
        private double GetTimePerTime(double intervalSecondsPerSecond,
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

        //TODO: may capture these items into a SimulationStopwatchFiture of sorts...

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
        /// Returns the estimated event count calculating <paramref name="interval"/> into
        /// <paramref name="durationSeconds"/>.
        /// </summary>
        /// <param name="interval">A stopwatch period in milliseconds.</param>
        /// <param name="durationSeconds">The number of seconds in which to run.</param>
        /// <returns></returns>
        private static int GetEstimatedEventCount(double interval, double durationSeconds)
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

            RunStopwatchScenario(ssw =>
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
        /// <param name="stopwatch"></param>
        /// <param name="intervalSecondsPerSecond"></param>
        /// <param name="numeratorUnit"></param>
        /// <param name="denominatorUnit"></param>
        private void VerifyUnitPerUnitProperties(IScalableStopwatch stopwatch,
            double intervalSecondsPerSecond, TimeUnit numeratorUnit, TimeUnit denominatorUnit)
        {
            var actualValue = GetStopwatchMeasure(stopwatch, numeratorUnit, denominatorUnit);

            if (actualValue == null) return;

            var expectedValue = GetTimePerTime(intervalSecondsPerSecond,
                numeratorUnit, denominatorUnit);

            Assert.That(actualValue.Value, Is.EqualTo(expectedValue).Within(Epsilon),
                @"Verification failed for numerator {0} and denominator {1}.",
                numeratorUnit, denominatorUnit);
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

            RunStopwatchScenario(ssw =>
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

        public class TimerIntervalValuesAttribute : ValuesAttribute
        {
            internal TimerIntervalValuesAttribute()
                : base(100, 250, 500, 750)
            {
            }
        }

        public class DurationValuesAttribute : ValuesAttribute
        {
            internal DurationValuesAttribute()
                : base(1d, 2d, 3d)
            {
            }
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

                RunStopwatchScenario(ssw =>
                {
                    ssw[numeratorUnit, denominatorUnit] = stopwatchInterval;

                    ssw.Elapsed += (sender, e) =>
                    {
                        quantitySum += e.IntervalQuantity;
                        elapsed += e.Elapsed;
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
    }
}
