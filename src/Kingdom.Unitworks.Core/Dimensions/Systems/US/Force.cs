namespace Kingdom.Unitworks.Dimensions.Systems.US
{
    using M = Mass;
    using Accel = Acceleration;

    /// <summary>
    /// 
    /// </summary>
    public class Force : ForceBase
    {
        private const double MeterPerSquareSecond = 9.80665d;

        private const double FeetPerMeter = 3.28084d;

        private const double PoundPerKilogram = 2.20462d;

        internal const double PoundForceToNewton = PoundPerKilogram/(FeetPerMeter*FeetPerMeter);
        internal const double NewtonToPoundForce = (FeetPerMeter*FeetPerMeter)/PoundPerKilogram;

        ////TODO: may look into this one if the conversions aren't too crazy: i.e. running into a mystery catch-22: lbm versus lbf
        //public static readonly IForce FootPound = null;

        /// <summary>
        /// Pound-force is a unit of <see cref="IForce"/> measure.
        /// </summary>
        /// <a href="!:http://en.wikipedia.org/wiki/Pound_%28force%29" ></a>
        public static readonly IForce PoundForce = new Force("lbf",
            new BaseDimensionUnitConversion(PoundForceToNewton),
            new BaseDimensionUnitConversion(NewtonToPoundForce),
            M.Pound, Accel.FeetPerSecondSquared);

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
