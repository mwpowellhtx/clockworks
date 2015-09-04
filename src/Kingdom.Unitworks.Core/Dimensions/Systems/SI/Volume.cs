namespace Kingdom.Unitworks.Dimensions.Systems.SI
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
        public static readonly IVolume CubicMeter = new Volume((ILength) L.Meter.Cubed());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IVolume CubicKilometer = new Volume((ILength) L.Kilometer.Cubed());

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
            return GetBase<Volume, IVolume>();
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
