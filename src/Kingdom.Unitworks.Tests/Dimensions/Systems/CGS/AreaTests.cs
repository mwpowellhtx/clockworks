using System.Collections.Generic;
using NUnit.Framework;

namespace Kingdom.Unitworks.Dimensions.Systems.CGS
{
    using L = Length;

    public class AreaTests : DerivedDimensionTestFixtureBase<Area, IArea>
    {
        protected override IEnumerable<TestCaseData> InstanceNamesTestCases
        {
            get
            {
                yield return new TestCaseData("SquareCentimeter");
            }
        }

        protected override IEnumerable<TestCaseData> BaseUnitTestCases
        {
            get
            {
                const bool notBaseUnit = false;

                yield return new TestCaseData("SquareCentimeter", notBaseUnit);
            }
        }

        protected override IEnumerable<TestCaseData> ToBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                yield return new TestCaseData("SquareCentimeter", value*L.MetersPerCentimeter.Squared());
            }
        }

        protected override IEnumerable<TestCaseData> FromBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                yield return new TestCaseData("SquareCentimeter", value/L.MetersPerCentimeter.Squared());
            }
        }
    }
}
