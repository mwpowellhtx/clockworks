namespace Kingdom.Unitworks.Dimensions.Systems.SI
{
    using M = Mass;
    using T = Commons.Time;
    using L = Length;

    /// <summary>
    /// 
    /// </summary>
    /// <a href="!:http://en.wikipedia.org/wiki/Newton_%28unit%29" />
    public class Force : ForceBase
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly IForce Newton = new Force("N",
            M.Kilogram, (ITime) T.Second.Squared(), (ILength) L.Meter.Invert());

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
            return GetBase<Force, IForce>();
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
