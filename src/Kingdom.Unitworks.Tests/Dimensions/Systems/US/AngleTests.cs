using System.Collections.Generic;
using NUnit.Framework;

namespace Kingdom.Unitworks.Dimensions.Systems.US
{
    using Theta = PlanarAngle;

    public class PlanarAngleTests : DimensionlessDimensionTestFixtureBase<PlanarAngle, IPlanarAngle>
    {
        protected override IEnumerable<TestCaseData> InstanceNamesTestCases
        {
            get { yield return new TestCaseData("Degree"); }
        }

        protected override IEnumerable<TestCaseData> BaseUnitTestCases
        {
            get
            {
                const bool nonBaseUnit = false;

                yield return new TestCaseData("Degree", nonBaseUnit);
            }
        }

        protected override IEnumerable<TestCaseData> ToBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                yield return new TestCaseData("Degree", value*Theta.RadianPerDegree);
            }
        }

        protected override IEnumerable<TestCaseData> FromBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                yield return new TestCaseData("Degree", value/Theta.RadianPerDegree);
            }
        }
    }
}
