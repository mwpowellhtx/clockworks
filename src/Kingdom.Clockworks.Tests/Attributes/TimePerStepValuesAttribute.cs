using NUnit.Framework;

namespace Kingdom.Clockworks
{
    internal class TimePerStepValuesAttribute : ValuesAttribute
    {
        internal TimePerStepValuesAttribute()
            : base(0.25d, 0.5d, 1d, 1.75d, 2.5d)
        {
        }
    }
}
