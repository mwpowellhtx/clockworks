namespace Kingdom.Unitworks.Dimensions.Systems.CGS
{
    using L = Length;

    /// <summary>
    /// 
    /// </summary>
    public class Area : AreaBase
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly IArea SquareCentimeter = new Area((ILength) L.Centimeter.Squared());

        private Area(ILength squareLength)
            : base(squareLength)
        {
        }

        private Area(Area other)
            : base(other)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDimension GetBase()
        {
            return GetBase<SI.Area, IArea>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new Area(this);
        }
    }
}
