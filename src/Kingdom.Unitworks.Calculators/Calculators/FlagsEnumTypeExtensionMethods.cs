namespace Kingdom.Unitworks.Calculators
{
    internal static class FlagsEnumTypeExtensionMethods
    {
        /// <summary>
        /// Returns whether <paramref name="value"/> Has the <paramref name="mask"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="mask"></param>
        /// <returns></returns>
        public static bool HasMask(this long value, long mask)
        {
            return (value & mask) == mask;
        }
    }
}
