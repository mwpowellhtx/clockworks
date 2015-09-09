using System;
using System.Collections.Generic;
using Kingdom.Unitworks.Dimensions;

namespace Kingdom.Unitworks
{
    /// <summary>
    /// Performs an accurate comparison of two quantities. First the
    /// <see cref="IQuantity.Dimensions"/> must be compatible. Incompatible dimensions
    /// yields -2 result. Value comparisons are performed using base units within an
    /// <see cref="Epsilon"/> accuracy.
    /// </summary>
    public class QuantityComparer : Comparer<IQuantity>
    {
        /// <summary>
        /// Epsilon backing field.
        /// </summary>
        public readonly double Epsilon;

        /// <summary>
        /// Constructor
        /// </summary>
        public QuantityComparer()
            : this(0)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="precision">Epsilon will be ten to this power.</param>
        public QuantityComparer(int precision)
        {
            Epsilon = Math.Pow(10d, precision);
        }

        /// <summary>
        /// Returns the accurate comparising of <paramref name="x"/> with <paramref name="y"/>.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public override int Compare(IQuantity x, IQuantity y)
        {
            if (ReferenceEquals(null, x) && ReferenceEquals(null, y))
                return -3;

            if (ReferenceEquals(null, y)) return 1;

            if (ReferenceEquals(null, x)) return -1;

            if (!x.Dimensions.AreEquivalent(y.Dimensions))
                return -2;

            // Sometimes there is an appropriate response when NaN is involved.
            if (double.IsNaN(x.Value) && double.IsNaN(y.Value)) return 0;
            if (double.IsNaN(x.Value)) return -1;
            if (double.IsNaN(y.Value)) return 1;

            //TODO: TBD: for Intinity?

            var xBase = x.ToBase();
            var yBase = y.ToBase();

            var xValue = xBase.Value;
            var yValue = yBase.Value;

            var delta = Math.Abs(xValue - yValue);

            return delta < Epsilon
                ? 0
                : (xValue < yValue ? -1 : 1);
        }
    }
}
