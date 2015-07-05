using Kingdom.Unitworks.Units;
using NUnit.Framework;

namespace Kingdom.Unitworks
{
    public class TimeUnitValuesAttribute : ValuesAttribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <remarks>There are not that many units, so just specify them individually instead of guessing
        /// at the enumerated range of them. This will help the NUnit combinatorial framework.</remarks>
        public TimeUnitValuesAttribute()
            : base(
                TimeUnit.Millisecond
                , TimeUnit.Second
                , TimeUnit.Minute
                , TimeUnit.Hour
                , TimeUnit.Day
                )
        {
        }
    }
}
