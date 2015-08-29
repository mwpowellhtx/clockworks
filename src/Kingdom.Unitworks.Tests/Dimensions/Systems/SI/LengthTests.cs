using System.Collections.Generic;
using NUnit.Framework;

namespace Kingdom.Unitworks.Dimensions.Systems.SI
{
    using L = Length;

    public class LengthTests : BaseDimensionTestFixtureBase<L, ILength>
    {
        protected override IEnumerable<TestCaseData> InstanceNamesTestCases
        {
            get
            {
                yield return new TestCaseData("Meter");
                yield return new TestCaseData("Kilometer");
            }
        }

        protected override IEnumerable<TestCaseData> BaseUnitTestCases
        {
            get
            {
                const bool notBaseUnit = false;
                const bool baseUnit = true;

                yield return new TestCaseData("Meter", baseUnit);
                yield return new TestCaseData("Kilometer", notBaseUnit);
            }
        }

        protected override IEnumerable<TestCaseData> ToBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                yield return new TestCaseData("Meter", value);
                yield return new TestCaseData("Kilometer", value*L.MetersPerKilometer);
            }
        }

        protected override IEnumerable<TestCaseData> FromBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                yield return new TestCaseData("Meter", value);
                yield return new TestCaseData("Kilometer", value/L.MetersPerKilometer);
            }
        }
    }
}
