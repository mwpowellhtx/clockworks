namespace Kingdom.Unitworks.Dimensions.Systems.US
{
    using Theta = Angle;

    /// <summary>
    /// 
    /// </summary>
    public class Steradian : SteradianBase
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly ISteradian SquareDegree = new Steradian((IAngle) Theta.Degree.Squared());

        private Steradian(IAngle squareAngle)
            : base(null, squareAngle)
        {
        }

        private Steradian(Steradian other)
            : base(other)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDimension GetBase()
        {
            return GetBase<SI.Steradian, ISteradian>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new Steradian(this);
        }
    }
}
