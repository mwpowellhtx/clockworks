using NUnit.Framework;

namespace Kingdom.Clockworks
{
    internal class TimeQuantityValuesAttribute : ValuesAttribute
    {
        internal TimeQuantityValuesAttribute()
            : base(100d, 200d, 300d)
        {
        }
    }
}
