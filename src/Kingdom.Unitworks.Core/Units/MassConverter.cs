using System;
using System.Linq;

namespace Kingdom.Unitworks.Units
{
    /// <summary>
    /// 
    /// </summary>
    public class MassConverter : UnitConverter<MassUnit, double>
    {
        /// <summary>
        /// Static Constructor
        /// </summary>
        static MassConverter()
        {
            foreach (var unit in Enum.GetValues(typeof (MassUnit)).OfType<MassUnit>())
                RegisterConversion(unit, unit.GetConversionPart(x => x.Factor));
        }

        /// <summary>
        /// BaseUnit backing field.
        /// </summary>
        private MassUnit? _baseUnit;

        /// <summary>
        /// Gets the BaseUnit.
        /// </summary>
        public override MassUnit BaseUnit
        {
            get
            {
                if (!_baseUnit.HasValue)
                    _baseUnit = Enum.GetValues(typeof (MassUnit)).OfType<MassUnit>()
                        .Single(u => u.IsBaseUnit());
                return _baseUnit.Value;
            }
        }

        /// <summary>
        /// Returns the results after multiplying <paramref name="value"/> by <paramref name="factor"/>.
        /// Optionally returns the <paramref name="inverse"/> of the calculation.
        /// </summary>
        /// <param name="factor"></param>
        /// <param name="value"></param>
        /// <param name="inverse"></param>
        /// <returns></returns>
        protected override double Multiply(double factor, double value, bool inverse = false)
        {
            return value*(inverse ? 1d/factor : factor);
        }
    }
}
