using System;
using System.Collections.Generic;
using System.Linq;

namespace Kingdom.Unitworks.Dimensions
{
    /// <summary>
    /// 
    /// </summary>
    public class DerivedUnitConversion : UnitConversionBase, IDerivedUnitConversion
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dimensionGetter"></param>
        /// <param name="conversionGetter"></param>
        internal DerivedUnitConversion(Func<IEnumerable<IDimension>> dimensionGetter,
            Func<IDimension, IUnitConversion> conversionGetter)
        {
            _dimensionGetter = dimensionGetter;
            _conversionGetter = conversionGetter;
        }

        private readonly Func<IEnumerable<IDimension>> _dimensionGetter;

        private readonly Func<IDimension, IUnitConversion> _conversionGetter;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="exponent"></param>
        /// <returns></returns>
        public override double Convert(double value, int exponent)
        {
            if (exponent == 0) return 1d;

            var inverted = exponent < 0;

            if (inverted) value = 1d/value;

            var converted = _dimensionGetter().Aggregate(value,
                (g, x) => _conversionGetter(x).Convert(g, x.Exponent));

            if (inverted) converted = 1d/converted;

            return converted;
        }

        /// <summary>
        /// 
        /// </summary>
        public override bool IsIdentity
        {
            get { return _dimensionGetter().All(d => _conversionGetter(d).IsIdentity); }
        }
    }
}
