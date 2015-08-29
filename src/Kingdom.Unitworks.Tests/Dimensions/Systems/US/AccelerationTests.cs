using System.Collections.Generic;
using NUnit.Framework;

namespace Kingdom.Unitworks.Dimensions.Systems.US
{
    using L = Length;
    using T = Commons.Time;

    public class AccelerationTests : DerivedDimensionTestFixtureBase<Acceleration, IAcceleration>
    {
        protected override IEnumerable<TestCaseData> InstanceNamesTestCases
        {
            get
            {
                yield return new TestCaseData("InchesPerMillisecondSquared");
                yield return new TestCaseData("InchesPerSecondSquared");
                yield return new TestCaseData("InchesPerMinuteSquared");
                yield return new TestCaseData("InchesPerHourSquared");

                yield return new TestCaseData("FeetPerMillisecondSquared");
                yield return new TestCaseData("FeetPerSecondSquared");
                yield return new TestCaseData("FeetPerMinuteSquared");
                yield return new TestCaseData("FeetPerHourSquared");

                yield return new TestCaseData("YardsPerMillisecondSquared");
                yield return new TestCaseData("YardsPerSecondSquared");
                yield return new TestCaseData("YardsPerMinuteSquared");
                yield return new TestCaseData("YardsPerHourSquared");

                yield return new TestCaseData("MilesPerMillisecondSquared");
                yield return new TestCaseData("MilesPerSecondSquared");
                yield return new TestCaseData("MilesPerMinuteSquared");
                yield return new TestCaseData("MilesPerHourSquared");
            }
        }

        protected override IEnumerable<TestCaseData> BaseUnitTestCases
        {
            get
            {
                const bool notBaseUnit = false;

                yield return new TestCaseData("InchesPerMillisecondSquared", notBaseUnit);
                yield return new TestCaseData("InchesPerSecondSquared", notBaseUnit);
                yield return new TestCaseData("InchesPerMinuteSquared", notBaseUnit);
                yield return new TestCaseData("InchesPerHourSquared", notBaseUnit);

                yield return new TestCaseData("FeetPerMillisecondSquared", notBaseUnit);
                yield return new TestCaseData("FeetPerSecondSquared", notBaseUnit);
                yield return new TestCaseData("FeetPerMinuteSquared", notBaseUnit);
                yield return new TestCaseData("FeetPerHourSquared", notBaseUnit);

                yield return new TestCaseData("YardsPerMillisecondSquared", notBaseUnit);
                yield return new TestCaseData("YardsPerSecondSquared", notBaseUnit);
                yield return new TestCaseData("YardsPerMinuteSquared", notBaseUnit);
                yield return new TestCaseData("YardsPerHourSquared", notBaseUnit);

                yield return new TestCaseData("MilesPerMillisecondSquared", notBaseUnit);
                yield return new TestCaseData("MilesPerSecondSquared", notBaseUnit);
                yield return new TestCaseData("MilesPerMinuteSquared", notBaseUnit);
                yield return new TestCaseData("MilesPerHourSquared", notBaseUnit);
            }
        }

        private static double CalculateFactor(double length, double time = 1d)
        {
            return length/(time*time);
        }


        protected override IEnumerable<TestCaseData> ToBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                yield return new TestCaseData("InchesPerMillisecondSquared", value*CalculateFactor(L.MetersPerInch, T.SecondsPerMillisecond));
                yield return new TestCaseData("InchesPerSecondSquared", value*CalculateFactor(L.MetersPerInch));
                yield return new TestCaseData("InchesPerMinuteSquared", value*CalculateFactor(L.MetersPerInch, T.SecondsPerMinute));
                yield return new TestCaseData("InchesPerHourSquared", value*CalculateFactor(L.MetersPerInch, T.SecondsPerHour));

                yield return new TestCaseData("FeetPerMillisecondSquared", value*CalculateFactor(L.MetersPerFoot, T.SecondsPerMillisecond));
                yield return new TestCaseData("FeetPerSecondSquared", value*CalculateFactor(L.MetersPerFoot));
                yield return new TestCaseData("FeetPerMinuteSquared", value*CalculateFactor(L.MetersPerFoot, T.SecondsPerMinute));
                yield return new TestCaseData("FeetPerHourSquared", value*CalculateFactor(L.MetersPerFoot, T.SecondsPerHour));

                yield return new TestCaseData("YardsPerMillisecondSquared", value*CalculateFactor(L.MetersPerYard, T.SecondsPerMillisecond));
                yield return new TestCaseData("YardsPerSecondSquared", value*CalculateFactor(L.MetersPerYard));
                yield return new TestCaseData("YardsPerMinuteSquared", value*CalculateFactor(L.MetersPerYard, T.SecondsPerMinute));
                yield return new TestCaseData("YardsPerHourSquared", value*CalculateFactor(L.MetersPerYard, T.SecondsPerHour));

                //// Miles per millisecond makes no sense whatsoever because the scales are so massive.
                //yield return new TestCaseData("MilesPerMillisecondSquared", value*CalculateFactor(L.MetersPerMile, T.SecondsPerMillisecond));
                yield return new TestCaseData("MilesPerSecondSquared", value*CalculateFactor(L.MetersPerMile));
                yield return new TestCaseData("MilesPerMinuteSquared", value*CalculateFactor(L.MetersPerMile, T.SecondsPerMinute));
                yield return new TestCaseData("MilesPerHourSquared", value*CalculateFactor(L.MetersPerMile, T.SecondsPerHour));
            }
        }

        protected override IEnumerable<TestCaseData> FromBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                yield return new TestCaseData("InchesPerMillisecondSquared", value*CalculateFactor(L.MetersPerInch, T.SecondsPerMillisecond).Inverted());
                yield return new TestCaseData("InchesPerSecondSquared", value*CalculateFactor(L.MetersPerInch).Inverted());
                yield return new TestCaseData("InchesPerMinuteSquared", value*CalculateFactor(L.MetersPerInch, T.SecondsPerMinute).Inverted());
                yield return new TestCaseData("InchesPerHourSquared", value*CalculateFactor(L.MetersPerInch, T.SecondsPerHour).Inverted());

                yield return new TestCaseData("FeetPerMillisecondSquared", value*CalculateFactor(L.MetersPerFoot, T.SecondsPerMillisecond).Inverted());
                yield return new TestCaseData("FeetPerSecondSquared", value*CalculateFactor(L.MetersPerFoot).Inverted());
                yield return new TestCaseData("FeetPerMinuteSquared", value*CalculateFactor(L.MetersPerFoot, T.SecondsPerMinute).Inverted());
                yield return new TestCaseData("FeetPerHourSquared", value*CalculateFactor(L.MetersPerFoot, T.SecondsPerHour).Inverted());

                yield return new TestCaseData("YardsPerMillisecondSquared", value*CalculateFactor(L.MetersPerYard, T.SecondsPerMillisecond).Inverted());
                yield return new TestCaseData("YardsPerSecondSquared", value*CalculateFactor(L.MetersPerYard).Inverted());
                yield return new TestCaseData("YardsPerMinuteSquared", value*CalculateFactor(L.MetersPerYard, T.SecondsPerMinute).Inverted());
                yield return new TestCaseData("YardsPerHourSquared", value*CalculateFactor(L.MetersPerYard, T.SecondsPerHour).Inverted());

                //// Miles per millisecond makes no sense whatsoever because the scales are so massive.
                //yield return new TestCaseData("MilesPerMillisecondSquared", value*CalculateFactor(L.MetersPerMile, T.SecondsPerMillisecond).Inverted());
                yield return new TestCaseData("MilesPerSecondSquared", value*CalculateFactor(L.MetersPerMile).Inverted());
                yield return new TestCaseData("MilesPerMinuteSquared", value*CalculateFactor(L.MetersPerMile, T.SecondsPerMinute).Inverted());
                yield return new TestCaseData("MilesPerHourSquared", value*CalculateFactor(L.MetersPerMile, T.SecondsPerHour).Inverted());
            }
        }
    }
}
