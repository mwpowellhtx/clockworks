namespace Kingdom.Unitworks.Dimensions.Systems.Commons
{
    /// <summary>
    /// Amount of substance is a standards-defined quantity that measures the size of an ensemble
    /// of elementary entities, such as atoms, molecules, electrons, and other particles. It is
    /// sometimes referred to as chemical amount. The International System of Units (SI) defines
    /// the amount of substance to be proportional to the number of elementary entities present.
    /// Typically abbreviated as &quot;N&quot; when performing dimensional analyses. This dimension
    /// is common across several unit systems, not just SI.
    /// </summary>
    /// <a href="!:http://www.unit-conversion.info/amount-of-substance.html" ></a>
    public class SubstanceAmount : BaseDimension, ISubstanceAmount
    {
        internal const double MolesPerMillimole = 1e-3;
        internal const double MolesPerKilomole = 1e3;

        private const double MillimolesPerMole = 1d/MolesPerMillimole;
        private const double KilomolesPerMole = 1d/MolesPerKilomole;

        /// <summary>
        /// Millimole unit of <see cref="ISubstanceAmount"/> measure.
        /// </summary>
        public static readonly ISubstanceAmount Millimole = new SubstanceAmount("mmol",
            new BaseDimensionUnitConversion(MolesPerMillimole),
            new BaseDimensionUnitConversion(MillimolesPerMole));

        /// <summary>
        /// The mole is a unit of measurement for amount of substance.
        /// </summary>
        /// <a href="!:http://en.wikipedia.org/wiki/Mole_%28unit%29" >Mole (unit)</a>
        /// <a href="!:http://physics.stackexchange.com/questions/174541/why-is-the-mole-amount-of-substance-a-dimensional-quantity" >Why
        /// is the mole/&quot;amount of substance&quot; a dimensional quantity?</a>
        public static readonly ISubstanceAmount Mole = new SubstanceAmount("mol",
            BaseDimensionUnitConversion.DefaultConversion,
            BaseDimensionUnitConversion.DefaultConversion);

        /// <summary>
        /// Kilomole unit of <see cref="ISubstanceAmount"/> measure.
        /// </summary>
        public static readonly ISubstanceAmount Kilomole = new SubstanceAmount("kmol",
            new BaseDimensionUnitConversion(MolesPerKilomole),
            new BaseDimensionUnitConversion(KilomolesPerMole));

        private SubstanceAmount(string abbreviation, IUnitConversion toBase, IUnitConversion fromBase)
            : base(abbreviation, toBase, fromBase)
        {
        }

        private SubstanceAmount(SubstanceAmount other)
            : base(other)
        {
        }

        public override IDimension GetBase()
        {
            return GetBase<SubstanceAmount, ISubstanceAmount>();
        }

        public override object Clone()
        {
            return new SubstanceAmount(this);
        }
    }
}
