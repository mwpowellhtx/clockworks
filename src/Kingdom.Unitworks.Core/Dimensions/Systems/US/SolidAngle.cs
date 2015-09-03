namespace Kingdom.Unitworks.Dimensions.Systems.US
{
    using Theta = PlanarAngle;

    /// <summary>
    /// 
    /// </summary>
    public class SolidAngle : SolidAngleBase
    {
        /// <a href="!:http://www.calculator.org/property.aspx?name=solid+angle" >Units converter for solid angle</a>
        public static readonly ISolidAngle SquareDegree = new SolidAngle((IPlanarAngle) Theta.Degree.Squared());

        private SolidAngle(IPlanarAngle squareAngle)
            : base(null, squareAngle)
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
