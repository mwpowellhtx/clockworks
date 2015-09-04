using System.Collections.Generic;
using NUnit.Framework;

namespace Kingdom.Unitworks.Dimensions.Systems.SI
{
    using L = Length;

    public class VolumeTests : DerivedDimensionTestFixtureBase<Volume, IVolume>
    {
        protected override IEnumerable<TestCaseData> InstanceNamesTestCases
        {
            get
            {
                yield return new TestCaseData("CubicMeter");
                yield return new TestCaseData("CubicKilometer");
            }
        }

        protected override IEnumerable<TestCaseData> BaseUnitTestCases
        {
            get
            {
                const bool baseUnit = true;
                const bool notBaseUnit = false;

                yield return new TestCaseData("CubicMeter", baseUnit);
                yield return new TestCaseData("CubicKilometer", notBaseUnit);
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

                yield return new TestCaseData("CubicMeter", value);
                yield return new TestCaseData("CubicKilometer", value*CalculateFactor(L.MetersPerKilometer));
            }
        }

        protected override IEnumerable<TestCaseData> FromBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                yield return new TestCaseData("CubicMeter", value);
                yield return new TestCaseData("CubicKilometer", value*CalculateFactor(L.MetersPerKilometer).Inverted());
            }
        }
    }
}