using System;

namespace Kingdom.Unitworks.Dimensions
{
    /// <summary>
    /// Base dimensions unit conversion class.
    /// </summary>
    public class BaseDimensionUnitConversion : UnitConversionBase, IBaseDimensionUnitConversion
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly IUnitConversion DefaultConversion = new BaseDimensionUnitConversion();

        /// <summary>
        /// 
        /// </summary>
        public double? InnerOffset { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public double? OuterOffset { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public double? Factor { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="factor"></param>
        /// <param name="outerOffset"></param>
        /// <param name="innerOffset"></param>
        internal BaseDimensionUnitConversion(double? factor = null, double? outerOffset = null, double? innerOffset = null)
        {
            Factor = factor;
            OuterOffset = outerOffset;
            InnerOffset = innerOffset;
        }

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

            if (inverted) value = 1/value;

            exponent = Math.Abs(exponent);

            //TODO: will need to double check this ...
            while (exponent-- > 0)
            {
                if (InnerOffset != null)
                    value += InnerOffset.Value;

                if (Factor != null)
                    value *= Factor.Value;

                if (OuterOffset != null)
                    value += OuterOffset.Value;
            }

            if (inverted) value = 1/value;

            return value;
        }

        /// <summary>
        /// Gets whether the conversion IsIdentity.
        /// </summary>
        public override bool IsIdentity
        {
            get { return InnerOffset == null && OuterOffset == null && Factor == null; }
        }
    }
}
