using System.Linq;
using NUnit.Framework;

namespace Kingdom.Unitworks.Calculators
{
    using Theta = Dimensions.Systems.US.PlanarAngle;

    public class PlanarAngleValuesAttribute : ValuesAttribute
    {
        private static readonly object[] Values;

        static PlanarAngleValuesAttribute()
        {
            var deg = Theta.Degree;

            var values = new[] {35d, 45d, 55d};

            Values = (from x in values
                select new Quantity(x, deg))
                .OfType<object>().ToArray();
        }

        internal PlanarAngleValuesAttribute()
            : base(Values)
        {
        }
    }
}
