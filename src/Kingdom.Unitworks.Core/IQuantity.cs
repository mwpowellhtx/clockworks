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
        /// 
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
