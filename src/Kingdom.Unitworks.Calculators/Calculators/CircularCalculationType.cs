using System;

namespace Kingdom.Unitworks.Calculators
{
    /// <summary>
    /// Represents a set of <see cref="CircularCalculator"/> types.
    /// </summary>
    [Flags]
    public enum CircularCalculationType : long
    {
        /// <summary>
        /// Circle calculation given Radius.
        /// </summary>
        Radius = 1 << 1,

        /// <summary>
        /// Circle calculation given Diameter.
        /// </summary>
        Diameter = 1 << 2,

        /// <summary>
        /// Circle calculation given Circumference.
        /// </summary>
        Circumference = 1 << 3,

        /// <summary>
        /// Circle calculation given Area.
        /// </summary>
        Area = 1 << 4,
    }

    /// <summary>
    /// 
    /// </summary>
    internal static class AreaTypeExtensionMethods
    {
        /// <summary>
        /// Returns whether <paramref name="value"/> Has the <paramref name="mask"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        public static bool HasMask(this CircularCalculationType value, CircularCalculationType mask)
        {
            return ((long) value).HasMask((long) mask);
        }
    }
}
