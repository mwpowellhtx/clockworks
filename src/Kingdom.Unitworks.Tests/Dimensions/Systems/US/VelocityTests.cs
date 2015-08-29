using System.Collections.Generic;
using NUnit.Framework;

namespace Kingdom.Unitworks.Dimensions.Systems.US
{
    using L = Length;
    using T = Commons.Time;

    public class VelocityTests : DerivedDimensionTestFixtureBase<Velocity, IVelocity>
    {
        protected override IEnumerable<TestCaseData> InstanceNamesTestCases
        {
            get
            {
                yield return new TestCaseData("InchesPerMillisecond");
                yield return new TestCaseData("InchesPerSecond");
                yield return new TestCaseData("InchesPerMinute");
                yield return new TestCaseData("InchesPerHour");

                yield return new TestCaseData("FeetPerMillisecond");
                yield return new TestCaseData("FeetPerSecond");
                yield return new TestCaseData("FeetPerMinute");
                yield return new TestCaseData("FeetPerHour");

                yield return new TestCaseData("YardsPerMillisecond");
                yield return new TestCaseData("YardsPerSecond");
                yield return new TestCaseData("YardsPerMinute");
                yield return new TestCaseData("YardsPerHour");

                yield return new TestCaseData("MilesPerMillisecond");
                yield return new TestCaseData("MilesPerSecond");
                yield return new TestCaseData("MilesPerMinute");
                yield return new TestCaseData("MilesPerHour");
            }
        }

        protected override IEnumerable<TestCaseData> BaseUnitTestCases
        {
            get
            {
                const bool notBaseUnit = false;

                yield return new TestCaseData("InchesPerMillisecond", notBaseUnit);
                yield return new TestCaseData("InchesPerSecond", notBaseUnit);
                yield return new TestCaseData("InchesPerMinute", notBaseUnit);
                yield return new TestCaseData("InchesPerHour", notBaseUnit);

                yield return new TestCaseData("FeetPerMillisecond", notBaseUnit);
                yield return new TestCaseData("FeetPerSecond", notBaseUnit);
                yield return new TestCaseData("FeetPerMinute", notBaseUnit);
                yield return new TestCaseData("FeetPerHour", notBaseUnit);

                yield return new TestCaseData("YardsPerMillisecond", notBaseUnit);
                yield return new TestCaseData("YardsPerSecond", notBaseUnit);
                yield return new TestCaseData("YardsPerMinute", notBaseUnit);
                yield return new TestCaseData("YardsPerHour", notBaseUnit);

                yield return new TestCaseData("MilesPerMillisecond", notBaseUnit);
                yield return new TestCaseData("MilesPerSecond", notBaseUnit);
                yield return new TestCaseData("MilesPerMinute", notBaseUnit);
                yield return new TestCaseData("MilesPerHour", notBaseUnit);
            }
        }

        private static double CalculateFactor(double length, double time = 1d)
        {
            return length / time;
        }

        protected override IEnumerable<TestCaseData> ToBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                yield return new TestCaseData("InchesPerMillisecond", value*CalculateFactor(L.MetersPerInch, T.SecondsPerMillisecond));
                yield return new TestCaseData("InchesPerSecond", value*CalculateFactor(L.MetersPerInch));
                yield return new TestCaseData("InchesPerMinute", value*CalculateFactor(L.MetersPerInch, T.SecondsPerMinute));
                yield return new TestCaseData("InchesPerHour", value*CalculateFactor(L.MetersPerInch, T.SecondsPerHour));

                yield return new TestCaseData("FeetPerMillisecond", value*CalculateFactor(L.MetersPerFoot, T.SecondsPerMillisecond));
                yield return new TestCaseData("FeetPerSecond", value*CalculateFactor(L.MetersPerFoot));
                yield return new TestCaseData("FeetPerMinute", value*CalculateFactor(L.MetersPerFoot, T.SecondsPerMinute));
                yield return new TestCaseData("FeetPerHour", value*CalculateFactor(L.MetersPerFoot, T.SecondsPerHour));

                yield return new TestCaseData("YardsPerMillisecond", value*CalculateFactor(L.MetersPerYard, T.SecondsPerMillisecond));
                yield return new TestCaseData("YardsPerSecond", value*CalculateFactor(L.MetersPerYard));
                yield return new TestCaseData("YardsPerMinute", value*CalculateFactor(L.MetersPerYard, T.SecondsPerMinute));
                yield return new TestCaseData("YardsPerHour", value*CalculateFactor(L.MetersPerYard, T.SecondsPerHour));

                yield return new TestCaseData("MilesPerMillisecond", value*CalculateFactor(L.MetersPerMile, T.SecondsPerMillisecond));
                yield return new TestCaseData("MilesPerSecond", value*CalculateFactor(L.MetersPerMile));
                yield return new TestCaseData("MilesPerMinute", value*CalculateFactor(L.MetersPerMile, T.SecondsPerMinute));
                yield return new TestCaseData("MilesPerHour", value*CalculateFactor(L.MetersPerMile, T.SecondsPerHour));
            }
        }

        protected override IEnumerable<TestCaseData> FromBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                yield return new TestCaseData("InchesPerMillisecond", value*CalculateFactor(L.MetersPerInch, T.SecondsPerMillisecond).Inverted());
                yield return new TestCaseData("InchesPerSecond", value*CalculateFactor(L.MetersPerInch).Inverted());
                yield return new TestCaseData("InchesPerMinute", value*CalculateFactor(L.MetersPerInch, T.SecondsPerMinute).Inverted());
                yield return new TestCaseData("InchesPerHour", value*CalculateFactor(L.MetersPerInch, T.SecondsPerHour).Inverted());

                yield return new TestCaseData("FeetPerMillisecond", value*CalculateFactor(L.MetersPerFoot, T.SecondsPerMillisecond).Inverted());
                yield return new TestCaseData("FeetPerSecond", value*CalculateFactor(L.MetersPerFoot).Inverted());
                yield return new TestCaseData("FeetPerMinute", value*CalculateFactor(L.MetersPerFoot, T.SecondsPerMinute).Inverted());
                yield return new TestCaseData("FeetPerHour", value*CalculateFactor(L.MetersPerFoot, T.SecondsPerHour).Inverted());

                yield return new TestCaseData("YardsPerMillisecond", value*CalculateFactor(L.MetersPerYard, T.SecondsPerMillisecond).Inverted());
                yield return new TestCaseData("YardsPerSecond", value*CalculateFactor(L.MetersPerYard).Inverted());
                yield return new TestCaseData("YardsPerMinute", value*CalculateFactor(L.MetersPerYard, T.SecondsPerMinute).Inverted());
                yield return new TestCaseData("YardsPerHour", value*CalculateFactor(L.MetersPerYard, T.SecondsPerHour).Inverted());

                yield return new TestCaseData("MilesPerMillisecond", value*CalculateFactor(L.MetersPerMile, T.SecondsPerMillisecond).Inverted());
                yield return new TestCaseData("MilesPerSecond", value*CalculateFactor(L.MetersPerMile).Inverted());
                yield return new TestCaseData("MilesPerMinute", value*CalculateFactor(L.MetersPerMile, T.SecondsPerMinute).Inverted());
                yield return new TestCaseData("MilesPerHour", value*CalculateFactor(L.MetersPerMile, T.SecondsPerHour).Inverted());
            }
        }
    }
}
