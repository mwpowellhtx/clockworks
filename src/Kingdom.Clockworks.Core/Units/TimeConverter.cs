using System;
using System.Linq;
using Kingdom.Unitworks;

namespace Kingdom.Clockworks.Units
{
    /// <summary>
    /// Represents a time units converter.
    /// </summary>
    public class TimeConverter : UnitConverter<TimeUnit, double>
    {
        /// <summary>
        /// Static Constructor
        /// </summary>
        static TimeConverter()
        {
            foreach (var unit in Enum.GetValues(typeof(TimeUnit)).OfType<TimeUnit>())
                RegisterConversion(unit, unit.GetConversionPart(x => x.Factor));
        }

        /// <summary>
        /// BaseUnit backing field.
        /// </summary>
        private TimeUnit? _baseUnit;

        /// <summary>
        /// Gets the BaseUnit.
        /// </summary>
        public override TimeUnit BaseUnit
        {
            get
            {
                if (!_baseUnit.HasValue)
                    _baseUnit = Enum.GetValues(typeof (TimeUnit)).OfType<TimeUnit>()
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
