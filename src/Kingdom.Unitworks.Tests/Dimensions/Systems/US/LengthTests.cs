using System.Collections.Generic;
using NUnit.Framework;

namespace Kingdom.Unitworks.Dimensions.Systems.US
{
    using L = Length;

    public class LengthTests : BaseDimensionTestFixtureBase<L, ILength>
    {
        protected override IEnumerable<TestCaseData> InstanceNamesTestCases
        {
            get
            {
                yield return new TestCaseData("Inch");
                yield return new TestCaseData("Foot");
                yield return new TestCaseData("Yard");
                yield return new TestCaseData("Mile");
            }
        }

        protected override IEnumerable<TestCaseData> BaseUnitTestCases
        {
            get
            {
                const bool notBaseUnit = false;

                yield return new TestCaseData("Inch", notBaseUnit);
                yield return new TestCaseData("Foot", notBaseUnit);
                yield return new TestCaseData("Yard", notBaseUnit);
                yield return new TestCaseData("Mile", notBaseUnit);
            }
        }

        protected override IEnumerable<TestCaseData> ToBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                yield return new TestCaseData("Inch", value*L.MetersPerInch);
                yield return new TestCaseData("Foot", value*L.MetersPerFoot);
                yield return new TestCaseData("Yard", value*L.MetersPerYard);
                yield return new TestCaseData("Mile", value*L.MetersPerMile);
            }
        }

        protected override IEnumerable<TestCaseData> FromBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                yield return new TestCaseData("Inch", value/L.MetersPerInch);
                yield return new TestCaseData("Foot", value/L.MetersPerFoot);
                yield return new TestCaseData("Yard", value/L.MetersPerYard);
                yield return new TestCaseData("Mile", value/L.MetersPerMile);
            }
        }
    }
}
