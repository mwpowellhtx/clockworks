using System.Collections.Generic;
using NUnit.Framework;

namespace Kingdom.Unitworks.Dimensions.Systems.CGS
{
    using L = Length;

    public class VolumeTests : DerivedDimensionTestFixtureBase<Volume, IVolume>
    {
        protected override IEnumerable<TestCaseData> InstanceNamesTestCases
        {
            get
            {
                yield return new TestCaseData("CubicCentimeter");
            }
        }

        protected override IEnumerable<TestCaseData> BaseUnitTestCases
        {
            get
            {
                const bool notBaseUnit = false;

                yield return new TestCaseData("CubicCentimeter", notBaseUnit);
            }
        }

        private static double CalculateFactor(double l)
        {
            return l.Cubed();
        }

        protected override IEnumerable<TestCaseData> ToBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                yield return new TestCaseData("CubicCentimeter", value*CalculateFactor(L.MetersPerCentimeter));
            }
        }

        protected override IEnumerable<TestCaseData> FromBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                yield return new TestCaseData("CubicCentimeter", value*CalculateFactor(L.MetersPerCentimeter).Inverted());
            }
        }
    }
}
