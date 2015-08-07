namespace Kingdom.Unitworks.Units
{
    /// <summary>
    /// Derives from the base <see cref="MassQuantity"/> for test purposes.
    /// </summary>
    public class MassQuantityFixture : MassQuantity
    {
        /// <summary>
        /// Exposes the <see cref="UnitConverter{TUnit,TValue}"/>  for test purposes.
        /// </summary>
        internal static MassConverter InternalConverter
        {
            get { return Converter; }
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        internal MassQuantityFixture()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unit"></param>
        internal MassQuantityFixture(MassUnit unit)
            : base(unit)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value"></param>
        internal MassQuantityFixture(double value)
            : base(value)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="value"></param>
        internal MassQuantityFixture(MassUnit unit, double value)
            : base(unit, value)
        {
        }
    }
}
