using System.Diagnostics;
using System.Linq;

namespace Kingdom.Unitworks.Dimensions
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AreaBase : DerivedDimension, IArea
    {
        /// <summary>
        /// 
        /// </summary>
        protected ILength SquareLength
        {
            get { return Dimensions.OfType<ILength>().SingleOrDefault(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="squareLength"></param>
        protected AreaBase(ILength squareLength)
            : base(squareLength)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        protected AreaBase(AreaBase other)
            : base(other)
        {
        }

        private void VerifyDimensions()
        {
            // ReSharper disable once RedundantAssignment
            var l = SquareLength;

            Debug.Assert(l != null && l.Exponent == 2);
        }
    }
}
