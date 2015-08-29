using System.Collections.Generic;
using NUnit.Framework;

namespace Kingdom.Unitworks.Dimensions.Systems.Commons
{
    using T = Time;

    public class TimeTests : BaseDimensionTestFixtureBase<T, ITime>
    {
        protected override IEnumerable<TestCaseData> InstanceNamesTestCases
        {
            get
            {
                yield return new TestCaseData("Microsecond");
                yield return new TestCaseData("Millisecond");
                yield return new TestCaseData("Second");
                yield return new TestCaseData("Minute");
                yield return new TestCaseData("Hour");
                yield return new TestCaseData("Day");
                yield return new TestCaseData("Week");
            }
        }

        protected override IEnumerable<TestCaseData> BaseUnitTestCases
        {
            get
            {
                const bool notBaseUnit = false;
                const bool baseUnit = true;

                yield return new TestCaseData("Microsecond", notBaseUnit);
                yield return new TestCaseData("Millisecond", notBaseUnit);
                yield return new TestCaseData("Second", baseUnit);
                yield return new TestCaseData("Minute", notBaseUnit);
                yield return new TestCaseData("Hour", notBaseUnit);
                yield return new TestCaseData("Day", notBaseUnit);
                yield return new TestCaseData("Week", notBaseUnit);
            }
        }

        protected override IEnumerable<TestCaseData> ToBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                yield return new TestCaseData("Microsecond", value*T.SecondsPerMicrosecond);
                yield return new TestCaseData("Millisecond", value*T.SecondsPerMillisecond);
                yield return new TestCaseData("Second", value);
                yield return new TestCaseData("Minute", value*T.SecondsPerMinute);
                yield return new TestCaseData("Hour", value*T.SecondsPerHour);
                yield return new TestCaseData("Day", value*T.SecondsPerDay);
                yield return new TestCaseData("Week", value*T.SecondsPerWeek);
            }
        }

        protected override IEnumerable<TestCaseData> FromBaseTestCases
        {
            get
            {
                const double value = BaseConversionStartValue;

                yield return new TestCaseData("Microsecond", value/T.SecondsPerMicrosecond);
                yield return new TestCaseData("Millisecond", value/T.SecondsPerMillisecond);
                yield return new TestCaseData("Second", value);
                yield return new TestCaseData("Minute", value/T.SecondsPerMinute);
                yield return new TestCaseData("Hour", value/T.SecondsPerHour);
                yield return new TestCaseData("Day", value/T.SecondsPerDay);
                yield return new TestCaseData("Week", value/T.SecondsPerWeek);
            }
        }
    }
}
