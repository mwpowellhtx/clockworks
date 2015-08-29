namespace Kingdom.Unitworks.Dimensions
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBaseDimensionUnitConversion : IUnitConversion
    {
        /// <summary>
        /// 
        /// </summary>
        double? InnerOffset { get; }

        /// <summary>
        /// 
        /// </summary>
        double? OuterOffset { get; }

        /// <summary>
        /// 
        /// </summary>
        double? Factor { get; }
    }
}
