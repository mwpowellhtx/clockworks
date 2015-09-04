namespace Kingdom.Unitworks.Dimensions.Systems.US
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
        public static readonly IMassDensity PoundPerCubicInch = new MassDensity(
            M.Pound, (ILength) L.Inch.Cubed().Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IMassDensity PoundPerCubicFoot = new MassDensity(
            M.Pound, (ILength) L.Foot.Cubed().Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IMassDensity PoundPerCubicYard = new MassDensity(
            M.Pound, (ILength) L.Yard.Cubed().Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IMassDensity PoundPerCubicMile = new MassDensity(
            M.Pound, (ILength) L.Mile.Cubed().Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IMassDensity OuncePerCubicInch = new MassDensity(
            M.Ounce, (ILength) L.Inch.Cubed().Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IMassDensity OuncePerCubicFoot = new MassDensity(
            M.Ounce, (ILength) L.Foot.Cubed().Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IMassDensity OuncePerCubicYard = new MassDensity(
            M.Ounce, (ILength) L.Yard.Cubed().Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IMassDensity OuncePerCubicMile = new MassDensity(
            M.Ounce, (ILength) L.Mile.Cubed().Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IMassDensity StonePerCubicInch = new MassDensity(
            M.Stone, (ILength) L.Inch.Cubed().Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IMassDensity StonePerCubicFoot = new MassDensity(
            M.Stone, (ILength) L.Foot.Cubed().Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IMassDensity StonePerCubicYard = new MassDensity(
            M.Stone, (ILength) L.Yard.Cubed().Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IMassDensity StonePerCubicMile = new MassDensity(
            M.Stone, (ILength) L.Mile.Cubed().Invert());

        private MassDensity(IMass mass, IVolume volume)
            : base(mass, volume)
        {
        }

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
