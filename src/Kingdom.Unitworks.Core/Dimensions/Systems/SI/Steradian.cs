namespace Kingdom.Unitworks.Dimensions.Systems.SI
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
        public static readonly ISteradian SquareRadian = new Steradian("sr", (IAngle) Theta.Radian.Squared());

        private Steradian(string abbreviation, IAngle squareAngle)
            : base(abbreviation, squareAngle)
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
            return SquareRadian;
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
