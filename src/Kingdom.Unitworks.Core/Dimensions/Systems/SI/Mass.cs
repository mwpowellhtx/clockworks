namespace Kingdom.Unitworks.Dimensions.Systems.SI
{
    /// <summary>
    /// 
    /// </summary>
    public class Mass : BaseDimension, IMass
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly IMass Kilogram = new Mass("kg");

        private Mass(string abbreviation)
            : base(abbreviation, null, null)
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
            return GetBase<Mass, IMass>();
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
