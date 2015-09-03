using System;

namespace Kingdom.Unitworks.Dimensions.Systems.US
{
    /// <summary>
    /// 
    /// </summary>
    public class PlanarAngle : BaseDimension, IPlanarAngle
    {
        internal const double RadianPerDegree = Math.PI/180d;

        ///
        // or "°" ...
        public static readonly IPlanarAngle Degree = new PlanarAngle("deg",
            new BaseDimensionUnitConversion(RadianPerDegree),
            new BaseDimensionUnitConversion(1d/RadianPerDegree));

        private PlanarAngle(string abbreviation, IUnitConversion toBase = null, IUnitConversion fromBase = null)
            : base(abbreviation, toBase, fromBase)
        {
        }

        private PlanarAngle(PlanarAngle other)
            : base(other)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDimension GetBase()
        {
            return GetBase<SI.PlanarAngle, IPlanarAngle>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new PlanarAngle(this);
        }
    }
}
