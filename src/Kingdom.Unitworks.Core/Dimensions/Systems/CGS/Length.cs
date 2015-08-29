namespace Kingdom.Unitworks.Dimensions.Systems.CGS
{
    //TODO: might be interesting to have a look at the T4 / ttinclude system for generating these things ... or at least describing more of a meta system that knows how to load them from resources ...
    /// <summary>
    /// 
    /// </summary>
    public class Length : BaseDimension, ILength
    {
        /// <summary>
        /// 
        /// </summary>
        /// <a href="!:http://www.google.com/?gws_rd=ssl#q=centimeter+to+meter" />
        internal const double MetersPerCentimeter = 1e-2;

        /// <summary>
        /// 
        /// </summary>
        public static readonly ILength Centimeter = new Length("cm",
            new BaseDimensionUnitConversion(MetersPerCentimeter),
            new BaseDimensionUnitConversion(1d/MetersPerCentimeter));

        private Length(string abbreviation, IUnitConversion toBase = null, IUnitConversion fromBase = null)
            : base(abbreviation, toBase, fromBase)
        {
        }

        private Length(Length other)
            : base(other)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDimension GetBase()
        {
            return GetBase<SI.Length, ILength>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new Length(this);
        }
    }
}
