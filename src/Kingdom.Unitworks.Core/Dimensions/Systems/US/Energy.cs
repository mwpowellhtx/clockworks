//TODO: will not worry about this dimension in US units for the time being...
namespace Kingdom.Unitworks.Dimensions.Systems.US
{
    using L = Length;
    using F = Force;

    /// <summary>
    /// 
    /// </summary>
    public class Energy : EnergyBase
    {
        /// <a href="!:http://en.wikipedia.org/wiki/Foot-pound_%28energy%29#Energy_units" >Foot-pound
        /// (energy), conversion to other units, energy units</a>
        internal const double JoulesPerFootPound = 1.3558179483314004d;

        private const double FootPoundsPerJoule = 1d/JoulesPerFootPound;

        /* TODO: TBD: other unit conversions are possible, it seems, i.e. inch-pound (?), but the
         * conversions are not exactly 12 inch/foot, for example, so it is not intuitive what exactly
         * they mean. http://www.convertunits.com/info/inch-pound */

        /// <summary>
        /// It is the energy transferred on applying a <see cref="IForce"/> of one pound-force
        /// (lbf) through a displacement of one <see cref="L.Foot"/>. The corresponding SI unit
        /// is the <see cref="SI.Energy.Joule"/>.
        /// </summary>
        /// <a href="!:http://www.convertunits.com/info/foot-pound" >foot-pound</a>
        /// <a href="!:http://www.convertunits.com/info/inch-pound" >inch-pound</a>
        public static readonly IEnergy FootPound = new Energy("ft-lb",
            new BaseDimensionUnitConversion(JoulesPerFootPound),
            new BaseDimensionUnitConversion(FootPoundsPerJoule));

        private Energy(string abbreviation, IUnitConversion toBase, IUnitConversion fromBase)
            : base(abbreviation, toBase, fromBase)
        {
        }

        private Energy(Energy other)
            : base(other)
        {
        }

        public override IDimension GetBase()
        {
            return GetBase<SI.Energy, IEnergy>();
        }

        public override object Clone()
        {
            return new Energy(this);
        }
    }
}
