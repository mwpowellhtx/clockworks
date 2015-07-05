using NUnit.Framework;

namespace Kingdom.Unitworks
{
    public class TimeQuantityValuesAttribute : ValuesAttribute
    {
        public TimeQuantityValuesAttribute()
            : base(1d, 3d, 5d, 7d)
        {
        }
    }
}
