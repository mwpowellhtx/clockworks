namespace Kingdom.Unitworks.Dimensions.Systems.SI
{
    using L = Length;

    /// <summary>
    /// 
    /// </summary>
    public class PlanarAngle : PlanarAngleBase
    {
        /// <a href="!:http://en.wikipedia.org/wiki/Steradian#Analogue_to_radians" ></a>
        public static readonly IPlanarAngle Radian = new PlanarAngle("rad",
            BaseDimensionUnitConversion.DefaultConversion,
            BaseDimensionUnitConversion.DefaultConversion,
            L.Meter, (ILength) L.Meter.Invert());

        private PlanarAngle(string abbreviation,
            IUnitConversion toBase, IUnitConversion fromBase,
            ILength arc, ILength radius)
            : base(abbreviation, toBase, fromBase, arc, radius)
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
