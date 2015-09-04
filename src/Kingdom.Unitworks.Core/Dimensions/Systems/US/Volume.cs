namespace Kingdom.Unitworks.Dimensions.Systems.US
{
    using L = Length;

    /// <summary>
    /// 
    /// </summary>
    public class Volume : VolumeBase
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly IVolume CubicInch = new Volume((ILength) Length.Inch.Cubed());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IVolume CubicFoot = new Volume((ILength) Length.Foot.Cubed());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IVolume CubicYard = new Volume((ILength) Length.Yard.Cubed());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IVolume CubicMile = new Volume((ILength) Length.Mile.Cubed());

        private Volume(ILength cubicLength)
            : base(cubicLength)
        {
        }

        private Volume(Volume other)
            : base(other)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDimension GetBase()
        {
            return GetBase<SI.Volume, IVolume>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new Volume(this);
        }
    }
}
