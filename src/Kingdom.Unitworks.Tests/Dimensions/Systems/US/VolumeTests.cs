using System.Collections.Generic;
using NUnit.Framework;

namespace Kingdom.Unitworks.Dimensions.Systems.US
{
    using L = Length;

    public class VolumeTests : DerivedDimensionTestFixtureBase<Volume, IVolume>
    {
        protected override IEnumerable<TestCaseData> InstanceNamesTestCases
        {
            get
            {
                yield return new TestCaseData("CubicInch");
                yield return new TestCaseData("CubicFoot");
                yield return new TestCaseData("CubicYard");
                yield return new TestCaseData("CubicMile");
            }
        }

        protected override IEnumerable<TestCaseData> BaseUnitTestCases
        {
            get
            {
                const bool notBaseUnit = false;

                yield return new TestCaseData("CubicInch", notBaseUnit);
                yield return new TestCaseData("CubicFoot", notBaseUnit);
                yield return new TestCaseData("CubicYard", notBaseUnit);
                yield return new TestCaseData("CubicMile", notBaseUnit);
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

                yield return new TestCaseData("CubicInch", value*CalculateFactor(L.MetersPerInch));
                yield return new TestCaseData("CubicFoot", value*CalculateFactor(L.MetersPerFoot));
                yield return new TestCaseData("CubicYard", value*CalculateFactor(L.MetersPerYard));
                yield return new TestCaseData("CubicMile", value*CalculateFactor(L.MetersPerMile));
            }
        }

        protected override IEnumerable<TestCaseData> FromBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                yield return new TestCaseData("CubicInch", value*CalculateFactor(L.MetersPerInch).Inverted());
                yield return new TestCaseData("CubicFoot", value*CalculateFactor(L.MetersPerFoot).Inverted());
                yield return new TestCaseData("CubicYard", value*CalculateFactor(L.MetersPerYard).Inverted());
                yield return new TestCaseData("CubicMile", value*CalculateFactor(L.MetersPerMile).Inverted());
            }
        }
    }
}
