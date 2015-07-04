using NUnit.Framework;

namespace Kingdom.Clockworks
{
    internal class StepValuesAttribute : ValuesAttribute
    {
        internal StepValuesAttribute()
            : base(0, 1, 2, 3)
        {
        }
    }
}
