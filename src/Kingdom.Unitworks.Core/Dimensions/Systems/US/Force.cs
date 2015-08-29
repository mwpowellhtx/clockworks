namespace Kingdom.Unitworks.Dimensions.Systems.US
{
    using M = Mass;
    using T = Commons.Time;
    using L = Length;

    /// <summary>
    /// 
    /// </summary>
    public class Force : ForceBase
    {
        ////TODO: may look into this one if the conversions aren't too crazy: i.e. running into a mystery catch-22: lbm versus lbf
        //public static readonly IForce FootPound = null;

        /// <summary>
        /// 
        /// </summary>
        public static readonly IForce Slug = new Force("slug",
            M.Pound, (ITime) T.Second.Squared(), (ILength) L.Foot.Invert());

        private Force(string abbreviation, IMass mass, ITime squareTime, ILength perLength)
            : base(abbreviation, mass, squareTime, perLength)
        {
        }

        private Force(Force other)
            : base(other)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDimension GetBase()
        {
            return GetBase<SI.Force, IForce>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new Force(this);
        }
    }
}
