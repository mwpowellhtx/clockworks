using System;

namespace Kingdom.Unitworks
{
    /// <summary>
    /// 
    /// </summary>
    public static class MathExtensionMethods
    {
        /// <summary>
        /// Returns the <see cref="Math.Log(double, double)"/> of the <paramref name="qty"/>.
        /// The default <paramref name="newBase"/> is <see cref="Math.E"/>, or the natural
        /// logarithm.
        /// </summary>
        /// <param name="qty"></param>
        /// <param name="newBase"></param>
        /// <returns></returns>
        /// <a href="!:http://en.wikipedia.org/wiki/Natural_logarithm" >Natural logarithm</a>
        public static IQuantity Log(this IQuantity qty, double newBase = Math.E)
        {
            if (ReferenceEquals(null, qty))
                throw new ArgumentNullException("qty");

            if (!qty.IsDimensionless)
                throw new ArgumentException("qty must be dimensionless", "qty");

            // Because it may not truly be "base", but may be dimensionless, after factors reduce.
            var qtyBase = qty.ToBase();

            return new Quantity(Math.Log(qtyBase.Value, newBase));
        }
    }
}
