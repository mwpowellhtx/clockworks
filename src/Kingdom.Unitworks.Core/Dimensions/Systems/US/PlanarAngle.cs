using System;

namespace Kingdom.Unitworks.Dimensions.Systems.US
{
    using L = Length;

    /// <summary>
    /// 
    /// </summary>
    public class PlanarAngle : PlanarAngleBase
    {
        /// <summary>
        /// <see cref="Math.PI"/> divided by 180.
        /// </summary>
        internal const double RadianPerDegree = Math.PI/180d;

        // or "°" ...
        ///
        public static readonly IPlanarAngle Degree = new PlanarAngle("deg",
            new BaseDimensionUnitConversion(RadianPerDegree),
            new BaseDimensionUnitConversion(1d/RadianPerDegree),
            L.Foot, (ILength) L.Foot.Invert());

        private PlanarAngle(string abbreviation,
            IUnitConversion toBase, IUnitConversion fromBase,
            ILength arc, ILength radius)
            : base(abbreviation, toBase, fromBase, arc, radius)
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
