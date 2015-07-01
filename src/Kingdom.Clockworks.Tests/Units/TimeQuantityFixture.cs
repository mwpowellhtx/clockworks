using Kingdom.Unitworks;

namespace Kingdom.Clockworks.Units
{
    /// <summary>
    /// Derives from the base <see cref="TimeQuantity"/> for test purposes.
    /// </summary>
    public class TimeQuantityFixture : TimeQuantity
    {
        /// <summary>
        /// Exposes the <see cref="UnitConverter{TUnit,TValue}"/>  for test purposes.
        /// </summary>
        internal static TimeConverter InternalConverter
        {
            get { return Converter; }
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        internal TimeQuantityFixture()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unit"></param>
        internal TimeQuantityFixture(TimeUnit unit)
            : base(unit)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="value"></param>
        internal TimeQuantityFixture(double value)
            : base(value)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="value"></param>
        internal TimeQuantityFixture(TimeUnit unit, double value)
            : base(unit, value)
        {
        }
    }
}
