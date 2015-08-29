using System.Diagnostics;
using System.Linq;

namespace Kingdom.Unitworks.Dimensions
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ForceBase : DerivedDimension, IForce
    {
        private IMass Mass
        {
            get { return Dimensions.OfType<IMass>().SingleOrDefault(); }
        }

        private ITime SquareTime
        {
            get { return Dimensions.OfType<ITime>().SingleOrDefault(); }
        }

        private ILength PerLength
        {
            get { return Dimensions.OfType<ILength>().SingleOrDefault(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abbreviation"></param>
        /// <param name="mass"></param>
        /// <param name="squareTime"></param>
        /// <param name="perLength"></param>
        protected ForceBase(string abbreviation, IMass mass, ITime squareTime, ILength perLength)
            : base(abbreviation, mass, squareTime, perLength)
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
            // This is a better approach because we want to maintain the same references.
            //TODO: TBD: that or just verify that the dimensions are correct when we have them in hand...
            var m = Mass;
            var t = SquareTime;
            var l = PerLength;

            Debug.Assert(m != null && m.Exponent == 1);
            Debug.Assert(t != null && t.Exponent == 2);
            Debug.Assert(l != null && l.Exponent == -1);
        }
    }
}
