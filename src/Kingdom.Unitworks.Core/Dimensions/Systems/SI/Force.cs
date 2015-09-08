namespace Kingdom.Unitworks.Dimensions.Systems.SI
{
    using M = Mass;
    using Accel = Acceleration;

    /// <summary>
    /// 
    /// </summary>
    /// <a href="!:http://en.wikipedia.org/wiki/Newton_%28unit%29" />
    public class Force : ForceBase
    {
        /// <summary>
        /// Derived unit of <see cref="IForce"/> measurement.
        /// </summary>
        /// <a href="!:http://en.wikipedia.org/wiki/Newton_%28unit%29" >Newton (unit)</a>
        public static readonly IForce Newton = new Force("N",
            BaseDimensionUnitConversion.DefaultConversion,
            BaseDimensionUnitConversion.DefaultConversion,
            M.Kilogram, Accel.MetersPerSecondSquared);

        /// <summary>
        /// Private Constructor
        /// </summary>
        /// <param name="abbreviation"></param>
        /// <param name="toBase"></param>
        /// <param name="fromBase"></param>
        /// <param name="mass"></param>
        /// <param name="acceleration"></param>
        private Force(string abbreviation,
            IUnitConversion toBase, IUnitConversion fromBase,
            IMass mass, IAcceleration acceleration)
            : base(abbreviation, toBase, fromBase, mass, acceleration)
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
