using System.Collections.Generic;

namespace Kingdom.Unitworks.Units
{
    public abstract class MassUnitTestFixtureBase : UnitsTestFixtureBase<MassUnit>
    {
        /// <summary>
        /// Factors backing field.
        /// </summary>
        private readonly IDictionary<MassUnit, double> _factors
            = new Dictionary<MassUnit, double>();

        /// <summary>
        /// Returns the Factor corresponding with the <paramref name="unit"/>.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        protected override double GetFactor(MassUnit unit)
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
            _factors[MassUnit.Kilogram] = 2.20462d;
            _factors[MassUnit.Ounce] = 1 / 16d;
            _factors[MassUnit.Pound] = 1d;
            _factors[MassUnit.Stone] = 14d;
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
