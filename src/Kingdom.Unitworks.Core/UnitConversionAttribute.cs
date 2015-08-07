using System;

namespace Kingdom.Unitworks
{
    /// <summary>
    /// 
    /// </summary>
    public class UnitConversionAttribute : Attribute
    {
        /// <summary>
        /// Gets the Factor for the unit conversion to the base unit.
        /// </summary>
        public double Factor { get; private set; }

        /// <summary>
        /// Gets the Offset if applicable converting to the base unit.
        /// </summary>
        public double? Offset { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="factor"></param>
        public UnitConversionAttribute(double factor)
            : this(factor, null)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="factor"></param>
        /// <param name="offset"></param>
        public UnitConversionAttribute(double factor, double? offset)
        {
            Factor = factor;
            Offset = offset;
        }
    }
}
