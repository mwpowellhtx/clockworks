namespace Kingdom.Unitworks.Dimensions.Systems.SI
{
    using L = Length;
    using T = Commons.Time;

    /// <summary>
    /// 
    /// </summary>
    public class Velocity : VelocityBase
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly IVelocity MetersPerMillisecond = new Velocity(L.Meter, (ITime) T.Millisecond.Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IVelocity MetersPerSecond = new Velocity(L.Meter, (ITime) T.Second.Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IVelocity MetersPerMinute = new Velocity(L.Meter, (ITime) T.Minute.Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IVelocity MetersPerHour = new Velocity(L.Meter, (ITime) T.Hour.Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IVelocity KilometersPerMillisecond = new Velocity(L.Kilometer, (ITime) T.Millisecond.Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IVelocity KilometersPerSecond = new Velocity(L.Kilometer, (ITime) T.Second.Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IVelocity KilometersPerMinute = new Velocity(L.Kilometer, (ITime) T.Minute.Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IVelocity KilometersPerHour = new Velocity(L.Kilometer, (ITime) T.Hour.Invert());

        private Velocity(ILength length, ITime time)
            : base(length, time)
        {
        }

        private Velocity(Velocity other)
            : base(other)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDimension GetBase()
        {
            return GetBase<Velocity, IVelocity>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new Velocity(this);
        }
    }
}
