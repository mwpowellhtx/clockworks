using System.Diagnostics;
using System.Linq;

namespace Kingdom.Unitworks.Dimensions
{
    /// <summary>
    /// Technically planar angles are dimensionless, but they are also technically derived.
    /// </summary>
    public abstract class PlanarAngleBase : DerivedDimension, IPlanarAngle
    {
        private ILength Arc
        {
            get { return Dimensions.OfType<ILength>().ElementAtOrDefault(0); }
        }

        private ILength Radius
        {
            get { return Dimensions.OfType<ILength>().ElementAtOrDefault(1); }
        }

        /// <summary>
        /// Protected Constructor
        /// </summary>
        /// <param name="abbreviation"></param>
        /// <param name="toBase"></param>
        /// <param name="fromBase"></param>
        /// <param name="arc"></param>
        /// <param name="radius"></param>
        protected PlanarAngleBase(string abbreviation,
            IUnitConversion toBase, IUnitConversion fromBase,
            ILength arc, ILength radius)
            : base(abbreviation, toBase, fromBase, arc, radius)
        {
        }

        /// <summary>
        /// Protected Copy Constructor
        /// </summary>
        /// <param name="other"></param>
        protected PlanarAngleBase(PlanarAngleBase other)
            : base(other)
        {
            VerifyDimensions();
        }

        private void VerifyDimensions()
        {
            // Verify that the dimensions are actually dimensionless.
            Debug.Assert(!Dimensions.EnumerateAll().Reduce().Any());

            // ReSharper disable RedundantAssignment
            var arc = this.Arc;
            var radius = this.Radius;

            Debug.Assert(arc != null && arc.Exponent == 1);
            Debug.Assert(radius != null && radius.Exponent == -1);
        }
    }
}
