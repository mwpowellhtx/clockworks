using System.Collections.Generic;
using NUnit.Framework;

namespace Kingdom.Unitworks.Dimensions.Systems.US
{
    using L = Length;

    public class AreaTests : DerivedDimensionTestFixtureBase<Area, IArea>
    {
        protected override IEnumerable<TestCaseData> InstanceNamesTestCases
        {
            get
            {
                yield return new TestCaseData("SquareInch");
                yield return new TestCaseData("SquareFoot");
                yield return new TestCaseData("SquareYard");
                yield return new TestCaseData("SquareMile");
            }
        }

        protected override IEnumerable<TestCaseData> BaseUnitTestCases
        {
            get
            {
                const bool notBaseUnit = false;

                yield return new TestCaseData("SquareInch", notBaseUnit);
                yield return new TestCaseData("SquareFoot", notBaseUnit);
                yield return new TestCaseData("SquareYard", notBaseUnit);
                yield return new TestCaseData("SquareMile", notBaseUnit);
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

                yield return new TestCaseData("SquareInch", value*CalculateFactor(L.MetersPerInch));
                yield return new TestCaseData("SquareFoot", value*CalculateFactor(L.MetersPerFoot));
                yield return new TestCaseData("SquareYard", value*CalculateFactor(L.MetersPerYard));
                yield return new TestCaseData("SquareMile", value*CalculateFactor(L.MetersPerMile));
            }
        }

        protected override IEnumerable<TestCaseData> FromBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                yield return new TestCaseData("SquareInch", value*CalculateFactor(L.MetersPerInch).Inverted());
                yield return new TestCaseData("SquareFoot", value*CalculateFactor(L.MetersPerFoot).Inverted());
                yield return new TestCaseData("SquareYard", value*CalculateFactor(L.MetersPerYard).Inverted());
                yield return new TestCaseData("SquareMile", value*CalculateFactor(L.MetersPerMile).Inverted());
            }
        }
    }
}
