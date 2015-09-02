using System;
using System.Collections.Generic;
using Kingdom.Unitworks.Dimensions;

namespace Kingdom.Unitworks
{
    /// <summary>
    /// 
    /// </summary>
    public interface IQuantity
        : IEquatable<IQuantity>,
            IComparable<IQuantity>,
            ICloneable
    {
        /// <summary>
        /// 
        /// </summary>
        double Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        IReadOnlyCollection<IDimension> Dimensions { get; }

        /// <summary>
        /// Returns the representational equivalent of a Quantity SquareRoot. Will take the
        /// <see cref="Math.Sqrt"/> of the <see cref="Value"/> itself. However, the key is
        /// that the <see cref="Dimensions"/> themselves must also be evenly divisible.
        /// </summary>
        /// <returns></returns>
        IQuantity SquareRoot();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IQuantity Squared();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IQuantity Cubed();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IQuantity Inverse();

        /// <summary>
        /// Gets whether IsNaN.
        /// </summary>
        bool IsNaN { get; }

        /// <summary>
        /// Gets whether IsPositiveInfinity.
        /// </summary>
        bool IsPositiveInfinity { get; }

        /// <summary>
        /// Gets whether IsNegativeInfinity.
        /// </summary>
        bool IsNegativeInfinity { get; }

        /// <summary>
        /// Gets whether IsInfinity.
        /// </summary>
        bool IsInfinity { get; }

        /// <summary>
        /// Gets whether Is Dimensionless.
        /// </summary>
        bool IsDimensionless { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="units"></param>
        /// <returns></returns>
        IQuantity ConvertTo(params IDimension[] units);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IQuantity ToBase();
    }
}
