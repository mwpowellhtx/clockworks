using System.Linq;
using NUnit.Framework;

namespace Kingdom.Unitworks.Attributes
{
    using SiA = Dimensions.Systems.SI.Area;
    using UsA = Dimensions.Systems.US.Area;

    public class AreaValuesAttribute : ValuesAttribute
    {
        private static readonly object[] Values;

        static AreaValuesAttribute()
        {
            var values = new[] {3d, 20d, 100d};

            Values = (from x in values select new Quantity(x, SiA.SquareMeter))
                .Concat(from x in values select new Quantity(x, UsA.SquareFoot))
                .OfType<object>().ToArray();
        }

        public AreaValuesAttribute()
            : base(Values)
        {
        }
    }
}
