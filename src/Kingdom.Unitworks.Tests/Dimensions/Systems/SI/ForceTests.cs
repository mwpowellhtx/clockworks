using System.Collections.Generic;
using NUnit.Framework;

namespace Kingdom.Unitworks.Dimensions.Systems.SI
{
    public class ForceTests : DerivedDimensionTestFixtureBase<Force, IForce>
    {
        protected override IEnumerable<TestCaseData> InstanceNamesTestCases
        {
            get
            {
                yield return new TestCaseData("Newton");
            }
        }

        protected override IEnumerable<TestCaseData> BaseUnitTestCases
        {
            get
            {
                const bool baseUnit = true;

                yield return new TestCaseData("Newton", baseUnit);
            }
        }

        protected override IEnumerable<TestCaseData> ToBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                yield return new TestCaseData("Newton", value);
            }
        }

        protected override IEnumerable<TestCaseData> FromBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                yield return new TestCaseData("Newton", value);
            }
        }
    }
}
