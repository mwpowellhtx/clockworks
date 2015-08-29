namespace Kingdom.Unitworks.Dimensions
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class BaseDimension : Dimension, IBaseDimension
    {
        /// <summary>
        /// 
        /// </summary>
        public override bool IsBaseUnit
        {
            get { return ToBase.IsIdentity && FromBase.IsIdentity; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abbreviation"></param>
        /// <param name="toBase"></param>
        /// <param name="fromBase"></param>
        protected BaseDimension(string abbreviation, IUnitConversion toBase, IUnitConversion fromBase)
            : base(abbreviation, toBase, fromBase)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        protected BaseDimension(BaseDimension other)
            : base(other)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var formatted = Exponent == 0
                ? string.Empty
                : (Abbreviation + ExponentText);
            return formatted;
        }
    }
}
