using NUnit.Framework;

namespace Kingdom.Clockworks
{
    internal class DurationValuesAttribute : ValuesAttribute
    {
        internal DurationValuesAttribute()
            : base(1d, 2d, 3d)
        {
        }
    }
}
