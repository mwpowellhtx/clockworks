using System.Collections.Generic;
using NUnit.Framework;

namespace Kingdom.Unitworks.Dimensions.Systems.SI
{
    using Theta = Temperature;

    public class TemperatureTests : BaseDimensionTestFixtureBase<Theta, ITemperature>
    {
        protected override IEnumerable<TestCaseData> InstanceNamesTestCases
        {
            get { yield return new TestCaseData("Kelvin"); }
        }

        protected override IEnumerable<TestCaseData> BaseUnitTestCases
        {
            get
            {
                const bool baseUnit = true;

                yield return new TestCaseData("Kelvin", baseUnit);
            }
        }

        protected override IEnumerable<TestCaseData> ToBaseTestCases
        {
            get { yield return new TestCaseData("Kelvin", BaseConversionStartValue); }
        }


        protected override IEnumerable<TestCaseData> FromBaseTestCases
        {
            get { yield return new TestCaseData("Kelvin", BaseConversionStartValue); }
        }
    }
}
