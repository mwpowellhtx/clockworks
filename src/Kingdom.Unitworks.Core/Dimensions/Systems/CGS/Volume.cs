namespace Kingdom.Unitworks.Dimensions.Systems.CGS
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
        public static readonly IVolume CubicCentimeter = new Volume((ILength) Length.Centimeter.Cubed());

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
