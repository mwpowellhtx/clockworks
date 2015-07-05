using System;
using System.Collections.Generic;
using System.Linq;
using Kingdom.Unitworks.Units;
using NUnit.Framework;

namespace Kingdom.Unitworks
{
    public class TimeUnitValuesAttribute : ValuesAttribute
    {
        private static IEnumerable<TimeUnit> GetTimeUnits()
        {
            return Enum.GetValues(typeof (TimeUnit)).OfType<TimeUnit>().ToArray();
        }

        public TimeUnitValuesAttribute()
            : base(GetTimeUnits().ToArray())
        {
        }
    }
}
