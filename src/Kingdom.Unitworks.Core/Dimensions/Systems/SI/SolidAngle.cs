namespace Kingdom.Unitworks.Dimensions.Systems.SI
{
    using Theta = PlanarAngle;

    /// <summary>
    /// 
    /// </summary>
    public class SolidAngle : SolidAngleBase
    {
        /// <a href="!:http://en.wikipedia.org/wiki/Steradian" >Steradian</a>
        public static readonly ISolidAngle Steradian = new SolidAngle("sr", (IPlanarAngle) Theta.Radian.Squared());

        private SolidAngle(string abbreviation, IPlanarAngle squarePlanarAngle)
            : base(abbreviation, squarePlanarAngle)
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
