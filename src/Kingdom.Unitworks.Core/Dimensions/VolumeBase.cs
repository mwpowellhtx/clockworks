using System.Diagnostics;
using System.Linq;

namespace Kingdom.Unitworks.Dimensions
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class VolumeBase : DerivedDimension, IVolume
    {
        /// <summary>
        /// 
        /// </summary>
        protected ILength CubicLength
        {
            get { return Dimensions.OfType<ILength>().SingleOrDefault(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cubicLength"></param>
        protected VolumeBase(ILength cubicLength)
            : base(cubicLength)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        protected VolumeBase(VolumeBase other)
            : base(other)
        {
        }

        private void VerifyDimensions()
        {
            // ReSharper disable once RedundantAssignment
            var l = CubicLength;

            Debug.Assert(l != null && l.Exponent == 3);
        }
    }
}
