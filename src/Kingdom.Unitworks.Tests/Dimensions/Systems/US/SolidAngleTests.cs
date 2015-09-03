using System.Collections.Generic;
using NUnit.Framework;

namespace Kingdom.Unitworks.Dimensions.Systems.US
{
    using Theta = PlanarAngle;
    using Omega = SolidAngle;

    public class SolidAngleTests : DerivedDimensionTestFixtureBase<SolidAngle, ISolidAngle>
    {
        protected override IEnumerable<TestCaseData> InstanceNamesTestCases
        {
            get { yield return new TestCaseData("SquareDegree"); }
        }

        protected override IEnumerable<TestCaseData> BaseUnitTestCases
        {
            get
            {
                const bool nonBaseUnit = false;

                yield return new TestCaseData("SquareDegree", nonBaseUnit);
            }
        }

        protected override IEnumerable<TestCaseData> ToBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                yield return new TestCaseData("SquareDegree", value*Theta.RadianPerDegree.Squared());
            }
        }

        protected override IEnumerable<TestCaseData> FromBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                yield return new TestCaseData("SquareDegree", value/Theta.RadianPerDegree.Squared());
            }
        }
    }
}
