using NUnit.Framework;

namespace Kingdom.Clockworks
{
    internal class TimePerStepValuesAttribute : ValuesAttribute
    {
        internal TimePerStepValuesAttribute()
            : base(250d, 500d, 1000d, 1750d, 2500d)
        {
        }
    }
}
