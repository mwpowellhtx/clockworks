namespace Kingdom.Unitworks.Dimensions.Systems.SI
{
    /// <summary>
    /// 
    /// </summary>
    public class PlanarAngle : BaseDimension, IPlanarAngle
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly IPlanarAngle Radian = new PlanarAngle("rad");

        private PlanarAngle(string abbreviation)
            : base(abbreviation, null, null)
        {
        }

        private PlanarAngle(PlanarAngle other)
            : base(other)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDimension GetBase()
        {
            return GetBase<PlanarAngle, IPlanarAngle>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new PlanarAngle(this);
        }
    }
}
