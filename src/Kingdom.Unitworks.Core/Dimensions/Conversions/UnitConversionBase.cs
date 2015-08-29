namespace Kingdom.Unitworks.Dimensions
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class UnitConversionBase : IUnitConversion
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="exponent"></param>
        /// <returns></returns>
        public abstract double Convert(double value, int exponent);

        /// <summary>
        /// 
        /// </summary>
        public abstract bool IsIdentity { get; }
    }
}
