using System;
using System.Reflection;
using Kingdom.Clockworks.Units;
using NUnit.Framework;

namespace Kingdom.Clockworks
{
    public abstract class TimeableClockTestFixtureBase : ClockworksTestFixtureBase
    {
        /// <summary>
        /// Runs the stopwatch <paramref name="scenario"/>.
        /// </summary>
        /// <param name="scenario"></param>
        protected static void RunClockScenario<TClock>(Action<TClock> scenario)
            where TClock : TimeableClockBase, new()
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
        /// <param name="interval">A stopwatch period in milliseconds.</param>
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

        protected class TimerIntervalValuesAttribute : ValuesAttribute
        {
            internal TimerIntervalValuesAttribute()
                : base(100, 250, 500, 750)
            {
            }
        }

        protected class DurationValuesAttribute : ValuesAttribute
        {
            internal DurationValuesAttribute()
                : base(1d, 2d, 3d)
            {
            }
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

        #region Increment and Decrement Members

        /// <summary>
        /// 
        /// </summary>
        internal class RunningDirectionValuesAttribute : ValuesAttribute
        {
            /// <summary>
            /// <see cref="RunningDirection"/>
            /// </summary>
            private static readonly RunningDirection? Null = null;

            /// <summary>
            /// <see cref="RunningDirection.Forward"/>
            /// </summary>
            private static readonly RunningDirection? Forward = RunningDirection.Forward;

            /// <summary>
            /// <see cref="RunningDirection.Backward"/>
            /// </summary>
            private static readonly RunningDirection? Backward = RunningDirection.Backward;

            /// <summary>
            /// Constructor
            /// </summary>
            public RunningDirectionValuesAttribute()
                : base(Null, Forward, Backward)
            {
            }
        }

        internal class StepValuesAttribute : ValuesAttribute
        {
            public StepValuesAttribute()
                : base(0, 1, 2, 3)
            {
            }
        }

        internal class RequestTypeValuesAttribute : ValuesAttribute
        {
            public RequestTypeValuesAttribute()
                : base(RequestType.Instantaneous, RequestType.Continuous)
            {
            }
        }

        #endregion
    }
}
