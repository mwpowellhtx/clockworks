using System.Diagnostics;
using System.Linq;

namespace Kingdom.Unitworks.Dimensions
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ForceBase : DerivedDimension, IForce
    {
        // ReSharper disable SuggestBaseTypeForParameter
        /// <summary>
        /// Gets the Mass.
        /// </summary>
        private IMass Mass
        {
            get { return Dimensions.OfType<IMass>().SingleOrDefault(); }
        }

        /// <summary>
        /// Gets the Acceleration.
        /// </summary>
        private IAcceleration Acceleration
        {
            get { return Dimensions.OfType<IAcceleration>().SingleOrDefault(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abbreviation"></param>
        /// <param name="toBase"></param>
        /// <param name="fromBase"></param>
        /// <param name="mass"></param>
        /// <param name="acceleration"></param>
        protected ForceBase(string abbreviation,
            IUnitConversion toBase, IUnitConversion fromBase,
            IMass mass, IAcceleration acceleration)
            : base(abbreviation, toBase, fromBase, mass, acceleration)
        {
            VerifyDimensions();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        protected ForceBase(ForceBase other)
            : base(other)
        {
            VerifyDimensions();
        }

        private void VerifyDimensions()
        {
            // ReSharper disable RedundantAssignment
            var m = Mass;
            var accel = Acceleration;

            Debug.Assert(m != null && m.Exponent == 1);
            Debug.Assert(accel != null && accel.Exponent == 1);
        }
    }
}
