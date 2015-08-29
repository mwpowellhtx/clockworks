namespace Kingdom.Unitworks.Dimensions.Systems.Commons
{
    /// <summary>
    /// Behaves like a <see cref="Quantity"/> because you may want to combine it in math equations.
    /// But it operates like am <see cref="IDimension"/> in that it modifies display, although not
    /// necessarily value, once applied, of the quantity to which it was applied.
    /// </summary>
    /// <a href="!:http://en.wikipedia.org/wiki/Unit_prefix"> Unit prefix </a>
    internal class Prefix : BaseDimension, IPrefix
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly IPrefix Tera = new Prefix("T", new BaseDimensionUnitConversion(1000000000000d));

        /// <summary>
        /// 
        /// </summary>
        public static readonly IPrefix Giga = new Prefix("G", new BaseDimensionUnitConversion(1000000000d));

        /// <summary>
        /// 
        /// </summary>
        public static readonly IPrefix Mega = new Prefix("M", new BaseDimensionUnitConversion(1000000d));

        /// <summary>
        /// 
        /// </summary>
        public static readonly IPrefix Kilo = new Prefix("k", new BaseDimensionUnitConversion(1000d));

        /// <summary>
        /// 
        /// </summary>
        public static readonly IPrefix Hecto = new Prefix("h", new BaseDimensionUnitConversion(100d));

        /// <summary>
        /// 
        /// </summary>
        public static readonly IPrefix Deca = new Prefix("da", new BaseDimensionUnitConversion(10d));

        /// <summary>
        /// 
        /// </summary>
        public static readonly IPrefix None = new Prefix();

        /// <summary>
        /// 
        /// </summary>
        public static readonly IPrefix Deci = new Prefix("d", new BaseDimensionUnitConversion(0.1d));

        /// <summary>
        /// 
        /// </summary>
        public static readonly IPrefix Centi = new Prefix("c", new BaseDimensionUnitConversion(0.01d));

        /// <summary>
        /// 
        /// </summary>
        public static readonly IPrefix Milli = new Prefix("m", new BaseDimensionUnitConversion(0.001d));

        /// <summary>
        /// 
        /// </summary>
        public static readonly IPrefix Micro = new Prefix("µ", new BaseDimensionUnitConversion(0.00001d));

        /// <summary>
        /// 
        /// </summary>
        public static readonly IPrefix Nano = new Prefix("n", new BaseDimensionUnitConversion(0.000000001d));

        /// <summary>
        /// 
        /// </summary>
        public static readonly IPrefix Pico = new Prefix("p", new BaseDimensionUnitConversion(0.000000000001d));

        private Prefix(string abbreviation = null, IUnitConversion toBase = null, IUnitConversion fromBase = null)
            : base(abbreviation, toBase, fromBase)
        {
        }

        private Prefix(Prefix other)
            : base(other)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDimension GetBase()
        {
            return GetBase<Prefix, IPrefix>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new Prefix(this);
        }
    }
}
