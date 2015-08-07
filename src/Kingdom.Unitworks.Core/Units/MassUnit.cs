namespace Kingdom.Unitworks.Units
{
    /// <summary>
    /// Making the assumption we're talking about Earth gravity, on which Mass calculations heavily depend.
    /// </summary>
    /// <a href="!:https://en.wikipedia.org/wiki/Mass"> Mass (wikipedia) </a>
    public enum MassUnit
    {
        /// <summary>
        /// 
        /// </summary>
        [UnitConversion(2.20462d)]
        Kilogram,

        /// <summary>
        /// 
        /// </summary>
        [UnitConversion(1/16d)]
        Ounce,

        /// <summary>
        /// 
        /// </summary>
        /// <a href="!:https://en.wikipedia.org/wiki/Pound_(mass)"> Pound (mass, wikipedia) </a>
        [BaseUnit]
        Pound,

        /// <summary>
        /// 
        /// </summary>
        [UnitConversion(14d)]
        Stone
    }
}
