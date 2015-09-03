using System.Collections.Generic;

namespace Kingdom.Unitworks.Dimensions
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDerivedDimension : IDimension
    {
        /// <summary>
        /// Gets the Dimensions.
        /// </summary>
        ICollection<IDimension> Dimensions { get; }

        /// <summary>
        /// Tries to Replace the <paramref name="unit"/> in this
        /// <see cref="IDerivedDimension"/> or any of its component dimensions.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        bool TryReplace(IDimension unit);

        /// <summary>
        /// Replaces the <see cref="Dimensions"/> with comparable <paramref name="unit"/>.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        IDerivedDimension Replace(IDimension unit);
    }
}
