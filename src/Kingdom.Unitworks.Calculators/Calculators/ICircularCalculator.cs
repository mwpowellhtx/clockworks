namespace Kingdom.Unitworks.Calculators
{
    /// <summary>
    /// Circular calculators methods. Circular meaning we are talking about circles.
    /// But we could also be talking about ellipses.
    /// </summary>
    public interface ICircularCalculator : ICalculator
    {
        /// <summary>
        /// Returns the calculated Radius given the <paramref name="qty"/>.
        /// </summary>
        /// <param name="qty"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IQuantity CalculateRadius(IQuantity qty,
            CircularCalculationType type = CircularCalculationType.Diameter);

        /// <summary>
        /// Returns the calculated Diameter given the <paramref name="qty"/>.
        /// </summary>
        /// <param name="qty"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IQuantity CalculateDiameter(IQuantity qty,
            CircularCalculationType type = CircularCalculationType.Radius);

        /// <summary>
        /// This is the elliptical form of the same area calculation. Instead of Radius times two,
        /// we have simply <paramref name="aQty"/> times <paramref name="bQty"/>.
        /// </summary>
        /// <param name="aQty"></param>
        /// <param name="bQty"></param>
        /// <returns></returns>
        IQuantity CalculateArea(IQuantity aQty, IQuantity bQty);

        /// <summary>
        /// Returns the calculated Area given the <paramref name="qty"/>.
        /// </summary>
        /// <param name="qty"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IQuantity CalculateArea(IQuantity qty,
            CircularCalculationType type = CircularCalculationType.Diameter);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="qty"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IQuantity CalculateCircumference(IQuantity qty,
            CircularCalculationType type = CircularCalculationType.Diameter);
    }
}
