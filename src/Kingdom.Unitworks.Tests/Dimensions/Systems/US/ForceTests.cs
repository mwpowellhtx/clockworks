using System.Collections.Generic;
using NUnit.Framework;

namespace Kingdom.Unitworks.Dimensions.Systems.US
{
    using M = Mass;
    using T = Commons.Time;
    using L = Length;

    public class ForceTests : DerivedDimensionTestFixtureBase<Force, IForce>
    {
        protected override IEnumerable<TestCaseData> InstanceNamesTestCases
        {
            get
            {
                yield return new TestCaseData("Slug");
            }
        }

        protected override IEnumerable<TestCaseData> BaseUnitTestCases
        {
            get
            {
                const bool notBaseUnit = false;

                yield return new TestCaseData("Slug", notBaseUnit);
            }
        }

        private static double CalculateFactor(double mass, double time, double length)
        {
            return mass*time.Squared()*length.Inverted();
        }

        protected override IEnumerable<TestCaseData> ToBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                yield return new TestCaseData("Slug", value*CalculateFactor(M.KilogramsPerPound, 1d, L.MetersPerFoot));
            }
        }

        protected override IEnumerable<TestCaseData> FromBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                yield return new TestCaseData("Slug", value*CalculateFactor(M.KilogramsPerPound, 1d, L.MetersPerFoot).Inverted());
            }
        }
    }
}
