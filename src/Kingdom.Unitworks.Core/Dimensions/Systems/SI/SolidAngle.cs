namespace Kingdom.Unitworks.Dimensions.Systems.SI
{
    using A = Area;
    using L = Length;
    using Theta = PlanarAngle;

    /// <summary>
    /// Represents the SI system SolidAngle.
    /// </summary>
    public class SolidAngle : SolidAngleBase
    {
        /// <a href="!:http://en.wikipedia.org/wiki/Steradian" >Steradian</a>
        public static readonly ISolidAngle Steradian = new SolidAngle("sr",
            BaseDimensionUnitConversion.DefaultConversion,
            BaseDimensionUnitConversion.DefaultConversion,
            A.SquareMeter, (ILength) L.Meter.Squared().Invert());

        private SolidAngle(string abbreviation,
            IUnitConversion toBase, IUnitConversion fromBase,
            IArea surfaceArea, ILength squareRadius)
            : base(abbreviation, toBase, fromBase, surfaceArea, squareRadius)
        {
        }

        private SolidAngle(SolidAngle other)
            : base(other)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDimension GetBase()
        {
            return Steradian;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new SolidAngle(this);
        }
    }
}
