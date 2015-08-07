using System.Collections.Generic;

namespace Kingdom.Unitworks.Units
{
    public abstract class TimeUnitTestFixtureBase : UnitsTestFixtureBase<TimeUnit>
    {
        /// <summary>
        /// Factors backing field.
        /// </summary>
        private readonly IDictionary<TimeUnit, double> _factors
            = new Dictionary<TimeUnit, double>();

        /// <summary>
        /// Gets the Factor corresponding with the <paramref name="unit"/>.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        protected override double GetFactor(TimeUnit unit)
        {
            return _factors[unit];
        }

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
    }
}
