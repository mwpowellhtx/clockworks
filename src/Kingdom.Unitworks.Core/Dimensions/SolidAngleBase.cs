using System.Diagnostics;
using System.Linq;

namespace Kingdom.Unitworks.Dimensions
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class SolidAngleBase : DerivedDimension, ISolidAngle
    {
        /// <summary>
        /// 
        /// </summary>
        protected IPlanarAngle PlanarAngle
        {
            get { return Dimensions.OfType<IPlanarAngle>().SingleOrDefault(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abbreviation"></param>
        /// <param name="squarePlanarAngle"></param>
        protected SolidAngleBase(string abbreviation, IPlanarAngle squarePlanarAngle)
            : base(abbreviation, squarePlanarAngle)
        {
            VerifyDimensions();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        protected SolidAngleBase(SolidAngleBase other)
            : base(other)
        {
            VerifyDimensions();
        }

        private void VerifyDimensions()
        {
            // ReSharper disable once RedundantAssignment
            var theta = this.PlanarAngle;

            Debug.Assert(theta != null && theta.Exponent == 2);
        }
    }
}
