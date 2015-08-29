namespace Kingdom.Unitworks.Dimensions.Systems.US
{
    //TODO: this may not be the best way to organize the systems ...
    /// <summary>
    /// 
    /// </summary>
    public class Length : BaseDimension, ILength
    {
        /// <summary>
        /// 
        /// </summary>
        /// <a href="!:http://www.google.com/?gws_rd=ssl#q=inch+to+meter" />
        internal const double MetersPerInch = 0.0254d;

        /// <summary>
        /// 
        /// </summary>
        /// <a href="!:http://www.google.com/?gws_rd=ssl#q=foot+to+meter" />
        internal const double MetersPerFoot = 0.3048d;

        /// <summary>
        /// 
        /// </summary>
        /// <a href="!:http://www.google.com/?gws_rd=ssl#q=yard+to+meter" />
        internal const double MetersPerYard = 0.9144d;

        /// <summary>
        /// 
        /// </summary>
        /// <a href="!:http://www.google.com/?gws_rd=ssl#q=mile+to+meter" />
        internal const double MetersPerMile = 1609.34d;

        /// <summary>
        /// 
        /// </summary>
        public static readonly ILength Inch = new Length("in",
            new BaseDimensionUnitConversion(MetersPerInch),
            new BaseDimensionUnitConversion(1d/MetersPerInch));

        /// <summary>
        /// 
        /// </summary>
        public static readonly ILength Foot = new Length("ft",
            new BaseDimensionUnitConversion(MetersPerFoot),
            new BaseDimensionUnitConversion(1d/MetersPerFoot));

        /// <summary>
        /// 
        /// </summary>
        public static readonly ILength Yard = new Length("yd",
            new BaseDimensionUnitConversion(MetersPerYard),
            new BaseDimensionUnitConversion(1d/MetersPerYard));

        /// <summary>
        /// 
        /// </summary>
        public static readonly ILength Mile = new Length("mi",
            new BaseDimensionUnitConversion(MetersPerMile),
            new BaseDimensionUnitConversion(1d/MetersPerMile));

        private Length(string abbreviation, IUnitConversion toBase, IUnitConversion fromBase)
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
