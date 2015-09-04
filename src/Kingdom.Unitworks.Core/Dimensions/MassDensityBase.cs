using System.Diagnostics;
using System.Linq;

namespace Kingdom.Unitworks.Dimensions
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class MassDensityBase : DerivedDimension, IMassDensity
    {
        /// <summary>
        /// 
        /// </summary>
        protected IMass Mass
        {
            get { return Dimensions.OfType<IMass>().SingleOrDefault(); }
        }

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
        protected IVolume Volume
        {
            get { return Dimensions.OfType<IVolume>().SingleOrDefault(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mass"></param>
        /// <param name="volume"></param>
        protected MassDensityBase(IMass mass, ILength volume)
            : base(mass, volume)
        {
            VerifyDimensions();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mass"></param>
        /// <param name="volume"></param>
        protected MassDensityBase(IMass mass, IVolume volume)
            : base(mass, volume)
        {
            VerifyDimensions();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        protected MassDensityBase(MassDensityBase other)
            : base(other)
        {
            VerifyDimensions();
        }

        private void VerifyDimensions()
        {
            // ReSharper disable RedundantAssignment
            var m = Mass;
            var cl = CubicLength;
            var v = Volume;

            Debug.Assert(m != null && m.Exponent == 1);

            Debug.Assert(
                (v != null && v.Exponent == -1)
                || (cl != null && cl.Exponent == -3)
                );
        }
    }
}
