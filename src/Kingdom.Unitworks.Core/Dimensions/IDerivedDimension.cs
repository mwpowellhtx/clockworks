using System.Collections.Generic;

namespace Kingdom.Unitworks.Dimensions
{
    public interface IDerivedDimension : IDimension
    {
        ICollection<IDimension> Dimensions { get; }

        /// <summary>
        /// Tries to Replace the <paramref name="unit"/> in this
        /// <see cref="IDerivedDimension"/> or any of its component dimensions.
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        bool TryReplace(IDimension unit);

        IDerivedDimension Replace(IDimension unit);
    }
}
