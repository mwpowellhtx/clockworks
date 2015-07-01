using System;
using System.Collections.Concurrent;

namespace Kingdom.Unitworks
{
    /// <summary>
    /// Generic conversion class for converting between values of different units.
    /// </summary>
    /// <typeparam name="TUnit">The type representing the unit type (eg. enum)</typeparam>
    /// <typeparam name="TValue">The type of value for this unit (float, decimal, int, etc.)</typeparam>
    public abstract class UnitConverter<TUnit, TValue>
        where TValue : struct, IComparable, IComparable<TValue>, IEquatable<TValue>, IConvertible
    {
        /// <summary>
        /// Gets the BaseUnit, which all calculations will be expressed in terms of.
        /// </summary>
        public abstract TUnit BaseUnit { get; }

        /// <summary>
        /// Conversion factors backing field.
        /// </summary>
        private readonly static ConcurrentDictionary<TUnit, TValue> ConversionFactors
            = new ConcurrentDictionary<TUnit, TValue>();

        /// <summary>
        /// Converts a value from one unit type to another.
        /// </summary>
        /// <param name="value">The value to convert.</param>
        /// <param name="fromUnit">The unit type the provided value is in.</param>
        /// <param name="toUnit">The unit type to convert the value to.</param>
        /// <returns>The converted value.</returns>
        public TValue Convert(TValue value, TUnit fromUnit, TUnit toUnit)
        {
            // Short circuit when the units are the same.
            if (fromUnit.Equals(toUnit))
                return value;

            // Convert into the base unit, if required.
            var baseUnitValue = fromUnit.Equals(BaseUnit)
                ? value
                : Multiply(ConversionFactors[fromUnit], value);

            // Convert from the base unit into the requested unit, if required.
            var requiredUnitValue = toUnit.Equals(BaseUnit)
                ? baseUnitValue
                : Multiply(ConversionFactors[toUnit], baseUnitValue, true);

            return requiredUnitValue;
        }

        /// <summary>
        /// Registers functions for converting to/from a unit.
        /// </summary>
        /// <param name="convertToUnit">The type of unit to convert to/from, from the base unit.</param>
        /// <param name="conversionToFactor">a factor converting into the base unit.</param>
        protected static void RegisterConversion(TUnit convertToUnit, TValue conversionToFactor)
        {
            if (!ConversionFactors.TryAdd(convertToUnit, conversionToFactor))
                throw new ArgumentException("Already exists", "convertToUnit");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="factor"></param>
        /// <param name="value"></param>
        /// <param name="inverse"></param>
        /// <returns></returns>
        protected abstract TValue Multiply(TValue factor, TValue value, bool inverse = false);
    }
}
