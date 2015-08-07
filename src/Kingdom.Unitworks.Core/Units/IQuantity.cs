namespace Kingdom.Unitworks.Units
{
    /// <summary>
    /// Quantity with a base unit.
    /// </summary>
    /// <typeparam name="TDimension"></typeparam>
    public interface IQuantity<out TDimension>
    {
        /// <summary>
        /// 
        /// </summary>
        TDimension BaseUnit { get; }
    }
}
