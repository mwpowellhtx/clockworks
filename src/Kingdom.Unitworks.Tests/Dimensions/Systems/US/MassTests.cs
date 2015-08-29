using System.Collections.Generic;
using NUnit.Framework;

namespace Kingdom.Unitworks.Dimensions.Systems.US
{
    using M = Mass;

    public class MassTests : BaseDimensionTestFixtureBase<M, IMass>
    {
        protected override IEnumerable<TestCaseData> InstanceNamesTestCases
        {
            get
            {
                yield return new TestCaseData("Ounce");
                yield return new TestCaseData("Pound");
                yield return new TestCaseData("Stone");
            }
        }

        protected override IEnumerable<TestCaseData> BaseUnitTestCases
        {
            get
            {
                const bool notBaseUnit = false;

                yield return new TestCaseData("Ounce", notBaseUnit);
                yield return new TestCaseData("Pound", notBaseUnit);
                yield return new TestCaseData("Stone", notBaseUnit);
            }
        }

        protected override IEnumerable<TestCaseData> ToBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                yield return new TestCaseData("Ounce", value*M.KilogramsPerOunce);
                yield return new TestCaseData("Pound", value*M.KilogramsPerPound);
                yield return new TestCaseData("Stone", value*M.KilogramsPerStone);
            }
        }

        protected override IEnumerable<TestCaseData> FromBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                yield return new TestCaseData("Ounce", value/M.KilogramsPerOunce);
                yield return new TestCaseData("Pound", value/M.KilogramsPerPound);
                yield return new TestCaseData("Stone", value/M.KilogramsPerStone);
            }
        }
    }
}
