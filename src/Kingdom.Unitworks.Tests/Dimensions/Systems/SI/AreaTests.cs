using System.Collections.Generic;
using NUnit.Framework;

namespace Kingdom.Unitworks.Dimensions.Systems.SI
{
    using L = Length;

    public class AreaTests : DerivedDimensionTestFixtureBase<Area, IArea>
    {
        protected override IEnumerable<TestCaseData> InstanceNamesTestCases
        {
            get
            {
                yield return new TestCaseData("SquareMeter");
                yield return new TestCaseData("SquareKilometer");
            }
        }

        protected override IEnumerable<TestCaseData> BaseUnitTestCases
        {
            get
            {
                const bool baseUnit = true;
                const bool notBaseUnit = false;

                yield return new TestCaseData("SquareMeter", baseUnit);
                yield return new TestCaseData("SquareKilometer", notBaseUnit);
            }
        }

        private static double CalculateFactor(double length)
        {
            return length.Squared();
        }

        protected override IEnumerable<TestCaseData> ToBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                yield return new TestCaseData("SquareMeter", value);
                yield return new TestCaseData("SquareKilometer", value*CalculateFactor(L.MetersPerKilometer));
            }
        }

        protected override IEnumerable<TestCaseData> FromBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                yield return new TestCaseData("SquareMeter", value);
                yield return new TestCaseData("SquareKilometer", value*CalculateFactor(L.MetersPerKilometer).Inverted());
            }
        }
    }
}
