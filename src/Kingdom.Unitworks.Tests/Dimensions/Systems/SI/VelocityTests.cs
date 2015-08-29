using System.Collections.Generic;
using NUnit.Framework;

namespace Kingdom.Unitworks.Dimensions.Systems.SI
{
    using L = Length;
    using T = Commons.Time;

    public class VelocityTests : DerivedDimensionTestFixtureBase<Velocity, IVelocity>
    {
        protected override IEnumerable<TestCaseData> InstanceNamesTestCases
        {
            get
            {
                yield return new TestCaseData("MetersPerMillisecond");
                yield return new TestCaseData("MetersPerSecond");
                yield return new TestCaseData("MetersPerMinute");
                yield return new TestCaseData("MetersPerHour");

                yield return new TestCaseData("KilometersPerMillisecond");
                yield return new TestCaseData("KilometersPerSecond");
                yield return new TestCaseData("KilometersPerMinute");
                yield return new TestCaseData("KilometersPerHour");
            }
        }

        protected override IEnumerable<TestCaseData> BaseUnitTestCases
        {
            get
            {
                const bool notBaseUnit = false;
                const bool baseUnit = true;

                yield return new TestCaseData("MetersPerMillisecond", notBaseUnit);
                yield return new TestCaseData("MetersPerSecond", baseUnit);
                yield return new TestCaseData("MetersPerMinute", notBaseUnit);
                yield return new TestCaseData("MetersPerHour", notBaseUnit);

                yield return new TestCaseData("KilometersPerMillisecond", notBaseUnit);
                yield return new TestCaseData("KilometersPerSecond", notBaseUnit);
                yield return new TestCaseData("KilometersPerMinute", notBaseUnit);
                yield return new TestCaseData("KilometersPerHour", notBaseUnit);
            }
        }

        protected override IEnumerable<TestCaseData> ToBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                yield return new TestCaseData("MetersPerMillisecond", value*(1d/T.SecondsPerMillisecond));
                yield return new TestCaseData("MetersPerSecond", value);
                yield return new TestCaseData("MetersPerMinute", value*(1d/T.SecondsPerMinute));
                yield return new TestCaseData("MetersPerHour", value*(1d/T.SecondsPerHour));

                yield return new TestCaseData("KilometersPerMillisecond", value*(L.MetersPerKilometer/T.SecondsPerMillisecond));
                yield return new TestCaseData("KilometersPerSecond", value*L.MetersPerKilometer);
                yield return new TestCaseData("KilometersPerMinute", value*(L.MetersPerKilometer/T.SecondsPerMinute));
                yield return new TestCaseData("KilometersPerHour", value*(L.MetersPerKilometer/T.SecondsPerHour));
            }
        }

        protected override IEnumerable<TestCaseData> FromBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                yield return new TestCaseData("MetersPerMillisecond", value/(1d/T.SecondsPerMillisecond));
                yield return new TestCaseData("MetersPerSecond", value);
                yield return new TestCaseData("MetersPerMinute", value/(1d/T.SecondsPerMinute));
                yield return new TestCaseData("MetersPerHour", value/(1d/T.SecondsPerHour));

                yield return new TestCaseData("KilometersPerMillisecond", value/(L.MetersPerKilometer/T.SecondsPerMillisecond));
                yield return new TestCaseData("KilometersPerSecond", value/L.MetersPerKilometer);
                yield return new TestCaseData("KilometersPerMinute", value/(L.MetersPerKilometer/T.SecondsPerMinute));
                yield return new TestCaseData("KilometersPerHour", value/(L.MetersPerKilometer/T.SecondsPerHour));
            }
        }
    }
}
