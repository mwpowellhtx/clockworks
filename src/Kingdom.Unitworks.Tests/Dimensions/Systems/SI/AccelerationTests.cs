using System.Collections.Generic;
using NUnit.Framework;

namespace Kingdom.Unitworks.Dimensions.Systems.SI
{
    using L = Length;
    using T = Commons.Time;

    public class AccelerationTests : DerivedDimensionTestFixtureBase<Acceleration, IAcceleration>
    {
        protected override IEnumerable<TestCaseData> InstanceNamesTestCases
        {
            get
            {
                yield return new TestCaseData("MetersPerMillisecondSquared");
                yield return new TestCaseData("MetersPerSecondSquared");
                yield return new TestCaseData("MetersPerMinuteSquared");
                yield return new TestCaseData("MetersPerHourSquared");

                yield return new TestCaseData("KilometersPerMillisecondSquared");
                yield return new TestCaseData("KilometersPerSecondSquared");
                yield return new TestCaseData("KilometersPerMinuteSquared");
                yield return new TestCaseData("KilometersPerHourSquared");
            }
        }

        protected override IEnumerable<TestCaseData> BaseUnitTestCases
        {
            get
            {
                const bool notBaseUnit = false;
                const bool baseUnit = true;

                yield return new TestCaseData("MetersPerMillisecondSquared", notBaseUnit);
                yield return new TestCaseData("MetersPerSecondSquared", baseUnit);
                yield return new TestCaseData("MetersPerMinuteSquared", notBaseUnit);
                yield return new TestCaseData("MetersPerHourSquared", notBaseUnit);

                yield return new TestCaseData("KilometersPerMillisecondSquared", notBaseUnit);
                yield return new TestCaseData("KilometersPerSecondSquared", notBaseUnit);
                yield return new TestCaseData("KilometersPerMinuteSquared", notBaseUnit);
                yield return new TestCaseData("KilometersPerHourSquared", notBaseUnit);
            }
        }

        private static double CalculateFactor(double length, double time)
        {
            return length/(time*time);
        }

        protected override IEnumerable<TestCaseData> ToBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                yield return new TestCaseData("MetersPerMillisecondSquared", value*CalculateFactor(1d, T.SecondsPerMillisecond));
                yield return new TestCaseData("MetersPerSecondSquared", value);
                yield return new TestCaseData("MetersPerMinuteSquared", value*CalculateFactor(1d, T.SecondsPerMinute));
                yield return new TestCaseData("MetersPerHourSquared", value*CalculateFactor(1d, T.SecondsPerHour));

                yield return new TestCaseData("KilometersPerMillisecondSquared", value*CalculateFactor(L.MetersPerKilometer, T.SecondsPerMillisecond));
                yield return new TestCaseData("KilometersPerSecondSquared", value*CalculateFactor(L.MetersPerKilometer, 1d));
                yield return new TestCaseData("KilometersPerMinuteSquared", value*CalculateFactor(L.MetersPerKilometer, T.SecondsPerMinute));
                yield return new TestCaseData("KilometersPerHourSquared", value*CalculateFactor(L.MetersPerKilometer, T.SecondsPerHour));
            }
        }

        protected override IEnumerable<TestCaseData> FromBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                yield return new TestCaseData("MetersPerMillisecondSquared", value*CalculateFactor(1d, T.SecondsPerMillisecond).Inverted());
                yield return new TestCaseData("MetersPerSecondSquared", value);
                yield return new TestCaseData("MetersPerMinuteSquared", value*CalculateFactor(1d, T.SecondsPerMinute).Inverted());
                yield return new TestCaseData("MetersPerHourSquared", value*CalculateFactor(1d, T.SecondsPerHour).Inverted());

                yield return new TestCaseData("KilometersPerMillisecondSquared", value*CalculateFactor(L.MetersPerKilometer, T.SecondsPerMillisecond).Inverted());
                yield return new TestCaseData("KilometersPerSecondSquared", value*CalculateFactor(L.MetersPerKilometer, 1d).Inverted());
                yield return new TestCaseData("KilometersPerMinuteSquared", value*CalculateFactor(L.MetersPerKilometer, T.SecondsPerMinute).Inverted());
                yield return new TestCaseData("KilometersPerHourSquared", value*CalculateFactor(L.MetersPerKilometer, T.SecondsPerHour).Inverted());
            }
        }
    }
}
