using Kingdom.Unitworks;

namespace Kingdom.Clockworks.Units
{
    /// <summary>
    /// 
    /// </summary>
    public enum TimeUnit
    {
        /// <summary>
        /// 
        /// </summary>
        [UnitConversion(0.001d)]
        Millisecond,

        /// <summary>
        /// 
        /// </summary>
        [BaseUnit]
        Second,

        /// <summary>
        /// 
        /// </summary>
        [UnitConversion(60d)]
        Minute,

        /// <summary>
        /// 
        /// </summary>
        [UnitConversion(3600d)]
        Hour,

        /// <summary>
        /// 
        /// </summary>
        [UnitConversion(86400d)]
        Day,

        /// <summary>
        /// 
        /// </summary>
        [UnitConversion(604800d)]
        Week
    }
}
