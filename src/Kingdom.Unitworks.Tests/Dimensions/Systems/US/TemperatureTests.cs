using System.Collections.Generic;
using NUnit.Framework;

namespace Kingdom.Unitworks.Dimensions.Systems.US
{
    using Theta = Temperature;

    public class TemperatureTests : BaseDimensionTestFixtureBase<Theta, ITemperature>
    {
        protected override IEnumerable<TestCaseData> InstanceNamesTestCases
        {
            get
            {
                yield return new TestCaseData("Fahrenheit");
                yield return new TestCaseData("Rankine");
            }
        }

        protected override IEnumerable<TestCaseData> BaseUnitTestCases
        {
            get
            {
                const bool notBaseUnit = false;

                yield return new TestCaseData("Fahrenheit", notBaseUnit);
                yield return new TestCaseData("Rankine", notBaseUnit);
            }
        }

        protected override IEnumerable<TestCaseData> ToBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                // This one is a bit different from all the rest.
                yield return new TestCaseData("Fahrenheit", Theta.FahrenheitToBase.Convert(value, 1));
                yield return new TestCaseData("Rankine", value*Theta.KelvinPerRankine);
            }
        }

        protected override IEnumerable<TestCaseData> FromBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                // This one is a bit different from all the rest.
                yield return new TestCaseData("Fahrenheit", Theta.BaseToFahrenheit.Convert(value, 1));
                yield return new TestCaseData("Rankine", value*Theta.RankinePerKelvin);
            }
        }
    }
}
