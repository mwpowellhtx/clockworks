namespace Kingdom.Unitworks.Dimensions.Systems.SI
{
    //TODO: might be interesting to have a look at the T4 / ttinclude system for generating these things ... or at least describing more of a meta system that knows how to load them from resources ...
    /// <summary>
    /// 
    /// </summary>
    public class Length : BaseDimension, ILength
    {
        /// <a href="!:http://www.google.com/?gws_rd=ssl#q=ounce+to+kilogram" />
        internal const double MetersPerKilometer = 1e3;

        /// <summary>
        /// 
        /// </summary>
        public static readonly ILength Meter = new Length("m");

        /// <summary>
        /// 
        /// </summary>
        public static readonly ILength Kilometer = new Length("km", MetersPerKilometer, 1d/MetersPerKilometer);

        private Length(string abbreviation, double? toBaseFactor = null, double? fromBaseFactor = null)
            : base(abbreviation,
                new BaseDimensionUnitConversion(toBaseFactor),
                new BaseDimensionUnitConversion(fromBaseFactor))
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
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
            return GetBase<Length, ILength>();
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
