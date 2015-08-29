using System.Diagnostics;
using System.Linq;

namespace Kingdom.Unitworks.Dimensions
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class FrequencyBase : DerivedDimension, IFrequency
    {
        /// <summary>
        /// 
        /// </summary>
        protected ITime PerTime
        {
            get { return Dimensions.OfType<ITime>().SingleOrDefault(); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abbreviation"></param>
        /// <param name="perTime"></param>
        protected FrequencyBase(string abbreviation, ITime perTime)
            : base(abbreviation, perTime)
        {
            VerifyDimensions();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        protected FrequencyBase(FrequencyBase other)
            : base(other)
        {
            VerifyDimensions();
        }

        /// <summary>
        /// 
        /// </summary>
        private void VerifyDimensions()
        {
            var t = this.PerTime;

            Debug.Assert(t != null && t.Exponent == -1);
        }
    }
}
