namespace Kingdom.Unitworks.Dimensions.Systems.SI
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
        public static readonly IArea SquareMeter = new Area((ILength) L.Meter.Squared());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IArea SquareKilometer = new Area((ILength) L.Kilometer.Squared());

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
            return GetBase<Area, IArea>();
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
