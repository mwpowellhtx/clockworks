using NUnit.Framework;

namespace Kingdom.Unitworks
{
    public class MassQuantityValuesAttribute : ValuesAttribute
    {
        public MassQuantityValuesAttribute()
            : base(1d, 3d, 5d, 7d)
        {
        }
    }
}
