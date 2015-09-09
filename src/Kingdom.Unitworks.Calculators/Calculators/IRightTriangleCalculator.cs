using System;

namespace Kingdom.Unitworks.Calculators
{
    /// <summary>
    /// Performs various right triangle calculations.
    /// </summary>
    public interface IRightTriangleCalculator
    {
        /// <summary>
        /// Returns the calculated Hypotenuse given <paramref name="aQty"/> and <paramref name="bQty"/> lengths.
        /// </summary>
        /// <param name="aQty"></param>
        /// <param name="bQty"></param>
        /// <returns></returns>
        IQuantity CalculateHypotenuse(IQuantity aQty, IQuantity bQty);

        /// <summary>
        /// Returns the calculated non-hypotenuse side. This means that <paramref name="abQty"/>
        /// represents either <strong>a</strong> or <strong>b</strong> of the sides. The
        /// calculation results in the other, either <strong>b</strong> or <strong>a</strong>.
        /// It is up to the caller to know <strong>a</strong> from <strong>b</strong>.
        /// </summary>
        /// <param name="cQty"></param>
        /// <param name="abQty"></param>
        /// <returns></returns>
        IQuantity CalculateNonHypotenuse(IQuantity cQty, IQuantity abQty);

        /// <summary>
        /// Returns the calculated angle Beta, ß, given any two of <paramref name="aQty"/>,
        /// <paramref name="bQty"/>, or <paramref name="cQty"/>. If all three are provided, then
        /// the first two are selected. If one or none are provided, then an exception is thrown.
        /// </summary>
        /// <param name="aQty"></param>
        /// <param name="bQty"></param>
        /// <param name="cQty"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        IQuantity CalculateAngleBeta(IQuantity aQty = null, IQuantity bQty = null,
            IQuantity cQty = null);

        /// <summary>
        /// Returns the calculated angle Alpha, α, given any two of <paramref name="aQty"/>,
        /// <paramref name="bQty"/>, or <paramref name="cQty"/>. If all three are provided, then
        /// the first two are selected. If one or none are provided, then an exception is thrown.
        /// </summary>
        /// <param name="aQty"></param>
        /// <param name="bQty"></param>
        /// <param name="cQty"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        IQuantity CalculateAngleAlpha(IQuantity aQty = null, IQuantity bQty = null,
            IQuantity cQty = null);
    }
}
