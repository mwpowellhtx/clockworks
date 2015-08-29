namespace Kingdom.Unitworks.Dimensions.Systems.Commons
{
    using T = Time;

    /// <summary>
    /// 
    /// </summary>
    public class Frequency : FrequencyBase
    {
        //TODO: micro? nano? kilo? mega?
        /// <summary>
        /// 
        /// </summary>
        public static readonly IFrequency Hertz = new Frequency("Hz", (ITime) T.Second.Invert());

        private Frequency(string abbreviation, ITime time)
            : base(abbreviation, time)
        {
        }

        private Frequency(Frequency other)
            : base(other)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDimension GetBase()
        {
            return GetBase<Frequency, IFrequency>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new Frequency(this);
        }
    }
}
