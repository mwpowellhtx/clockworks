namespace Kingdom.Unitworks.Dimensions.Systems.SI
{
    using M = Mass;
    using L = Length;

    /// <summary>
    /// 
    /// </summary>
    public class MassDensity : MassDensityBase
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly IMassDensity KilogramPerCubicMeter = new MassDensity(
            M.Kilogram, (ILength) L.Meter.Cubed().Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IMassDensity KilogramPerCubicKilometer = new MassDensity(
            M.Kilogram, (ILength) L.Kilometer.Cubed().Invert());

        private MassDensity(IMass mass, ILength cubicLength)
            : base(mass, cubicLength)
        {
        }

        private MassDensity(MassDensity other)
            : base(other)
        {
        }

        public override IDimension GetBase()
        {
            return GetBase<MassDensity, IMassDensity>();
        }

        public override object Clone()
        {
            return new MassDensity(this);
        }
    }
}
