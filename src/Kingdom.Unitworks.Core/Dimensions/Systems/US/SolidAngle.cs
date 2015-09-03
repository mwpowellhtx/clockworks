namespace Kingdom.Unitworks.Dimensions.Systems.US
{
    using A = Area;
    using L = Length;
    using Theta = PlanarAngle;

    /// <summary>
    /// 
    /// </summary>
    public class SolidAngle : SolidAngleBase
    {
        /// <summary>
        /// Where 1 sr = (180/PI)^2 square degree, while maintaining the constness of the underlying factors.
        /// </summary>
        /// <a href="http://en.wikipedia.org/wiki/Solid_angle#Definition_and_properties" >Solid angle, definition and properties</a>
        internal const double SteridiansPerSquareDegree = 1d/(Theta.RadianPerDegree*Theta.RadianPerDegree);

        /// <a href="!:http://www.calculator.org/property.aspx?name=solid+angle" >Units converter for solid angle</a>
        public static readonly ISolidAngle SquareDegree
            = new SolidAngle("square degree",
                new BaseDimensionUnitConversion(SteridiansPerSquareDegree),
                new BaseDimensionUnitConversion(1d/SteridiansPerSquareDegree),
                A.SquareFoot, (ILength) L.Foot.Squared().Invert());

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
            return GetBase<SI.SolidAngle, ISolidAngle>();
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
