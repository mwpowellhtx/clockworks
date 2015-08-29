namespace Kingdom.Unitworks.Dimensions.Systems.SI
{
    using L = Length;
    using T = Commons.Time;

    /// <summary>
    /// 
    /// </summary>
    public class Acceleration : AccelerationBase
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly IAcceleration MetersPerMillisecondSquared = new Acceleration(L.Meter, (ITime) T.Millisecond.Squared().Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IAcceleration MetersPerSecondSquared = new Acceleration(L.Meter, (ITime) T.Second.Squared().Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IAcceleration MetersPerMinuteSquared = new Acceleration(L.Meter, (ITime) T.Minute.Squared().Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IAcceleration MetersPerHourSquared = new Acceleration(L.Meter, (ITime) T.Hour.Squared().Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IAcceleration KilometersPerMillisecondSquared = new Acceleration(L.Kilometer, (ITime) T.Millisecond.Squared().Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IAcceleration KilometersPerSecondSquared = new Acceleration(L.Kilometer, (ITime) T.Second.Squared().Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IAcceleration KilometersPerMinuteSquared = new Acceleration(L.Kilometer, (ITime) T.Minute.Squared().Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IAcceleration KilometersPerHourSquared = new Acceleration(L.Kilometer, (ITime) T.Hour.Squared().Invert());

        private Acceleration(ILength length, ITime time)
            : base(length, time)
        {
        }

        private Acceleration(Acceleration other)
            : base(other)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDimension GetBase()
        {
            return GetBase<Acceleration, IAcceleration>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new Acceleration(this);
        }
    }
}
