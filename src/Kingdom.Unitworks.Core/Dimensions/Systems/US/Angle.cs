using System;

namespace Kingdom.Unitworks.Dimensions.Systems.US
{
    /// <summary>
    /// 
    /// </summary>
    public class Angle : BaseDimension, IAngle
    {
        internal const double RadianPerDegree = Math.PI/180d;

        ///
        // or "°" ...
        public static readonly IAngle Degree = new Angle("deg",
            new BaseDimensionUnitConversion(RadianPerDegree),
            new BaseDimensionUnitConversion(1d/RadianPerDegree));

        private Angle(string abbreviation, IUnitConversion toBase = null, IUnitConversion fromBase = null)
            : base(abbreviation, toBase, fromBase)
        {
        }

        private Angle(Angle other)
            : base(other)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDimension GetBase()
        {
            return GetBase<SI.Angle, IAngle>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new Angle(this);
        }
    }
}
