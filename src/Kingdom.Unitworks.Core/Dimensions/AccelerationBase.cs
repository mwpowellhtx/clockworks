using System.Diagnostics;
using System.Linq;

namespace Kingdom.Unitworks.Dimensions
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AccelerationBase : DerivedDimension, IAcceleration
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
        /// <param name="perSquareTime"></param>
        protected AccelerationBase(ILength length, ITime perSquareTime)
            : base(length, perSquareTime)
        {
            VerifyDimensions();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        protected AccelerationBase(AccelerationBase other)
            : base(other)
        {
            VerifyDimensions();
        }

        private void VerifyDimensions()
        {
            var l = this.Length;
            var t = this.PerSquareTime;

            Debug.Assert(l != null && l.Exponent == 1);
            Debug.Assert(t != null && t.Exponent == -2);
        }
    }
}
