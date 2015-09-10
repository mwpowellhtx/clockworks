using System.Linq;
using NUnit.Framework;

namespace Kingdom.Unitworks.Calculators
{
    using V = Dimensions.Systems.SI.Velocity;

    public class VelocityValuesAttribute : ValuesAttribute
    {
        private static readonly object[] Values;

        static VelocityValuesAttribute()
        {
            var metersPerSecond = V.MetersPerSecond;

            var values = new[] {20d, 35d, 50d};

            Values = (from x in values
                select new Quantity(x, metersPerSecond))
                .OfType<object>().ToArray();
        }

        internal VelocityValuesAttribute()
            : base(Values)
        {
        }
    }
}
