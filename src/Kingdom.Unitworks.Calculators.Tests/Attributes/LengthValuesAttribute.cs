using System.Linq;
using NUnit.Framework;

namespace Kingdom.Unitworks.Attributes
{
    using SiL = Dimensions.Systems.SI.Length;
    using UsL = Dimensions.Systems.US.Length;

    public class LengthValuesAttribute : ValuesAttribute
    {
        private static readonly object[] Values;

        static LengthValuesAttribute()
        {
            var values = new[] {3d, 20d, 100d};

            Values = (from x in values select new Quantity(x, SiL.Meter))
                .Concat(from x in values select new Quantity(x, UsL.Foot))
                .OfType<object>().ToArray();
        }

        public LengthValuesAttribute()
            : base(Values)
        {
        }
    }
}
