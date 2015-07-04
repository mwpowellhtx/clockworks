using NUnit.Framework;

namespace Kingdom.Clockworks
{
    internal class TimerIntervalValuesAttribute : ValuesAttribute
    {
        internal TimerIntervalValuesAttribute()
            : base(100, 250, 500, 750)
        {
        }
    }
}