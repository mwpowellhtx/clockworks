using System.Diagnostics;

namespace Kingdom.Unitworks.Dimensions
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class SteradianBase : DerivedDimension, ISteradian
    {
        private IAngle _squareAngle;

        /// <summary>
        /// 
        /// </summary>
        protected IAngle SquareAngle
        {
            get { return _squareAngle; }
            set
            {
                _squareAngle = value;
                Debug.Assert(value.Exponent == 2);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abbreviation"></param>
        /// <param name="squareAngle"></param>
        protected SteradianBase(string abbreviation, IAngle squareAngle)
            : base(abbreviation, squareAngle)
        {
            SquareAngle = squareAngle;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        protected SteradianBase(SteradianBase other)
            : base(other)
        {
        }
    }
}
