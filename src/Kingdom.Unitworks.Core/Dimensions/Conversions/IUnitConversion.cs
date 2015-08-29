namespace Kingdom.Unitworks.Dimensions
{
    /// <summary>
    /// Unit conversions are not as simple as they first appear.
    /// </summary>
    public interface IUnitConversion
    {
        /// <summary>
        /// 
        /// </summary>
        bool IsIdentity { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="exponent"></param>
        /// <returns></returns>
        double Convert(double value, int exponent);
    }
}
