namespace Kingdom.Unitworks.Dimensions.Systems.CGS
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
        public static readonly IMassDensity GramPerCubicCentimeter = new MassDensity(
            M.Gram, (ILength) L.Centimeter.Cubed().Invert());

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
            return GetBase<SI.MassDensity, IMassDensity>();
        }

        public override object Clone()
        {
            return new MassDensity(this);
        }
    }
}
