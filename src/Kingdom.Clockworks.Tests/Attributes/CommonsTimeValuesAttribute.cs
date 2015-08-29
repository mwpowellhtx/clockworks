using System.Linq;
using Kingdom.Unitworks.Dimensions.Systems.Commons;
using NUnit.Framework;

namespace Kingdom.Clockworks
{
    internal class CommonsTimesValuesAttribute : ValuesAttribute
    {
        private static readonly object[] Values;

        static CommonsTimesValuesAttribute()
        {
            Values = new[]
            {
                Time.Microsecond,
                Time.Millisecond,
                Time.Second,
                Time.Minute,
                Time.Hour,
                Time.Day,
                Time.Week
            }.OfType<object>().ToArray();
        }

        internal CommonsTimesValuesAttribute()
            : base(Values)
        {
        }
    }
}
