using System;
using System.Linq;
using Kingdom.Unitworks.Dimensions;

namespace Kingdom.Unitworks.Calculators
{
    /// <summary>
    /// Calculator base class.
    /// </summary>
    public abstract class CalculatorBase : ICalculator
    {
        /// <summary>
        /// Returns the <paramref name="qty"/> with verified <paramref name="dimensions"/>.
        /// </summary>
        /// <param name="qty"></param>
        /// <param name="dimensions"></param>
        /// <returns></returns>
        protected static IQuantity VerifyDimensions(IQuantity qty, params IDimension[] dimensions)
        {
            if (ReferenceEquals(null, qty))
                throw new ArgumentNullException("qty", "qty instance is expected.");

            // ReSharper disable once InvertIf
            if (!qty.Dimensions.AreCompatible(dimensions, true))
            {
                var message = string.Format("qty force dimensions incorrect expected: {{{0}}} but was: {{{1}}}",
                    string.Join(" ", from x in dimensions select x.ToString()), qty);
                throw new ArgumentException(message, "qty");
            }

            return qty;
        }
    }
}
