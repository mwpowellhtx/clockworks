namespace Kingdom.Unitworks.Dimensions.Systems.US
{
    using L = Length;

    /// <summary>
    /// 
    /// </summary>
    public class Area : DerivedDimension, IArea
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly IArea SquareInch = new Area((ILength) L.Inch.Squared());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IArea SquareFoot = new Area((ILength) L.Foot.Squared());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IArea SquareYard = new Area((ILength) L.Yard.Squared());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IArea SquareMile = new Area((ILength) L.Mile.Squared());

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
