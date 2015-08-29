using System.Collections.Generic;
using NUnit.Framework;

namespace Kingdom.Unitworks.Dimensions.Systems.SI
{
    using Theta = Angle;

    public class SteradianTests : DerivedDimensionTestFixtureBase<Steradian, ISteradian>
    {
        protected override IEnumerable<TestCaseData> InstanceNamesTestCases
        {
            get { yield return new TestCaseData("SquareRadian"); }
        }

        protected override IEnumerable<TestCaseData> BaseUnitTestCases
        {
            get
            {
                const bool baseUnit = true;

                yield return new TestCaseData("SquareRadian", baseUnit);
            }
        }

        protected override IEnumerable<TestCaseData> ToBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                yield return new TestCaseData("SquareRadian", value);
            }
        }

        protected override IEnumerable<TestCaseData> FromBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                yield return new TestCaseData("SquareRadian", value);
            }
        }
    }
}
