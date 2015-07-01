using System;
using System.Reflection;

namespace Kingdom.Unitworks
{
    /// <summary>
    /// Unit conversion extension methods.
    /// </summary>
    public static class UnitConversionExtensionMethods
    {
        /// <summary>
        /// Returns the <typeparamref name="TAttrib"/> corresponding to the <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <typeparam name="TAttrib"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        private static TAttrib GetAttribute<TEnum, TAttrib>(this TEnum value)
            where TAttrib : Attribute
        {
            var type = typeof(TEnum);

            if (!type.IsEnum)
            {
                throw new InvalidOperationException(
                    string.Format(@"Invalid conversion func request for type {0}", type));
            }

            var fieldName = value.ToString();

            var attrib = type.GetField(fieldName).GetCustomAttribute<TAttrib>();

            return attrib;
        }

        /// <summary>
        /// Returns whether <paramref name="value"/> IsBaseUnit.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsBaseUnit<TEnum>(this TEnum value)
            where TEnum : struct
        {
            return value.GetAttribute<TEnum, BaseUnitAttribute>() != null;
        }

        /// <summary>
        /// Returns the unit conversion from <paramref name="value"/> if possible.
        /// Returns <value>1d</value> by default.
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="value"></param>
        /// <param name="getter"></param>
        /// <returns></returns>
        public static double GetConversionPart<TEnum>(this TEnum value,
            Func<UnitConversionAttribute, double> getter)
            where TEnum : struct
        {
            if (getter == null)
                throw new ArgumentNullException(@"getter");

            var attrib = value.GetAttribute<TEnum, UnitConversionAttribute>();

            return attrib == null ? 1d : getter(attrib);
        }
    }
}
