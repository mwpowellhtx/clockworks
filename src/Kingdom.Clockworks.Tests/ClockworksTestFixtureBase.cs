using System;
using System.Collections.Generic;
using System.Linq;
using Kingdom.Clockworks.Units;
using NUnit.Framework;

namespace Kingdom.Clockworks
{
    public abstract class ClockworksTestFixtureBase : TestFixtureBase
    {
        /// <summary>
        /// Returns the <paramref name="value"/> converted to the base unit.
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected double ConvertToBase(TimeUnit unit, double value)
        {
            return value*_factors[unit];
        }

        /// <summary>
        /// Returns the <paramref name="value"/> converted from the base unit.
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected double ConvertFromBase(TimeUnit unit, double value)
        {
            return value/_factors[unit];
        }

        /// <summary>
        /// Factors backing field.
        /// </summary>
        private readonly IDictionary<TimeUnit, double> _factors
            = new Dictionary<TimeUnit, double>();

        /// <summary>
        /// Sets up the test fixture prior to running any tests.
        /// </summary>
        public override void TestFixtureSetUp()
        {
            base.TestFixtureSetUp();

            // For test purposes only.
            _factors[TimeUnit.Week] = 604800d;
            _factors[TimeUnit.Day] = 86400d;
            _factors[TimeUnit.Hour] = 3600d;
            _factors[TimeUnit.Minute] = 60d;
            _factors[TimeUnit.Second] = 1d;
            _factors[TimeUnit.Millisecond] = 0.001d;
        }

        /// <summary>
        /// Tears down the test fixture after running all of the tests.
        /// </summary>
        public override void TestFixtureTearDown()
        {
            _factors.Clear();

            base.TestFixtureTearDown();
        }

        protected class TimeQuantityValuesAttribute : ValuesAttribute
        {
            public TimeQuantityValuesAttribute()
                : base(1d, 3d, 5d, 7d)
            {
            }
        }

        protected class TimeUnitValuesAttribute : ValuesAttribute
        {
            public TimeUnitValuesAttribute()
                : base(Enum.GetValues(typeof (TimeUnit)).OfType<object>().ToArray())
            {
            }
        }
    }
}
