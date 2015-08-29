using System.Diagnostics;
using System.Linq;

namespace Kingdom.Unitworks.Dimensions
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class VelocityBase : DerivedDimension, IVelocity
    {
        /// <summary>
        /// 
        /// </summary>
        protected ILength Length
        {
            get { return Dimensions.OfType<ILength>().SingleOrDefault(); }
        }

        /// <summary>
        /// 
        /// </summary>
        protected ITime PerSquareTime
        {
            get { return Dimensions.OfType<ITime>().SingleOrDefault(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="length"></param>
        /// <param name="perTime"></param>
        protected VelocityBase(ILength length, ITime perTime)
            : base(length, perTime)
        {
            VerifyDimensions();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        protected VelocityBase(VelocityBase other)
            : base(other)
        {
            VerifyDimensions();
        }

        private void VerifyDimensions()
        {
            var l = this.Length;
            var t = this.PerSquareTime;

            Debug.Assert(l != null && l.Exponent == 1);
            Debug.Assert(t != null && t.Exponent == -1);
        }
    }
}
