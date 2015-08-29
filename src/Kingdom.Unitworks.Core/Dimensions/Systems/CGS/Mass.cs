namespace Kingdom.Unitworks.Dimensions.Systems.CGS
{
    /// <summary>
    /// 
    /// </summary>
    public class Mass : BaseDimension, IMass
    {
        /// <summary>
        /// 
        /// </summary>
        /// <a href="!:http://en.wikipedia.org/wiki/Centimetre%E2%80%93gram%E2%80%93second_system_of_units" />
        /// <a href="!:http://www.google.com/?gws_rd=ssl#q=gram+to+kilogram" />
        internal const double KilogramPerGram = 1e-3;

        /// <summary>
        /// 
        /// </summary>
        public static readonly IMass Gram = new Mass("g",
            new BaseDimensionUnitConversion(KilogramPerGram),
            new BaseDimensionUnitConversion(1d / KilogramPerGram));

        private Mass(string abbreviation, IUnitConversion toBase, IUnitConversion fromBase)
            : base(abbreviation, toBase, fromBase)
        {
        }

        private Mass(Mass other)
            : base(other)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDimension GetBase()
        {
            return GetBase<SI.Mass, IMass>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new Mass(this);
        }
    }
}
