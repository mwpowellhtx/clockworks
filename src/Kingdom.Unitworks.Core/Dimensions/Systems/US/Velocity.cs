namespace Kingdom.Unitworks.Dimensions.Systems.US
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
        public static readonly IVelocity InchesPerMillisecond = new Velocity(L.Inch, (ITime) T.Millisecond.Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IVelocity InchesPerSecond = new Velocity(L.Inch, (ITime) T.Second.Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IVelocity InchesPerMinute = new Velocity(L.Inch, (ITime) T.Minute.Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IVelocity InchesPerHour = new Velocity(L.Inch, (ITime) T.Hour.Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IVelocity FeetPerMillisecond = new Velocity(L.Foot, (ITime) T.Millisecond.Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IVelocity FeetPerSecond = new Velocity(L.Foot, (ITime) T.Second.Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IVelocity FeetPerMinute = new Velocity(L.Foot, (ITime) T.Minute.Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IVelocity FeetPerHour = new Velocity(L.Foot, (ITime) T.Hour.Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IVelocity YardsPerMillisecond = new Velocity(L.Yard, (ITime) T.Millisecond.Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IVelocity YardsPerSecond = new Velocity(L.Yard, (ITime) T.Second.Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IVelocity YardsPerMinute = new Velocity(L.Yard, (ITime) T.Minute.Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IVelocity YardsPerHour = new Velocity(L.Yard, (ITime) T.Hour.Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IVelocity MilesPerMillisecond = new Velocity(L.Mile, (ITime) T.Millisecond.Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IVelocity MilesPerSecond = new Velocity(L.Mile, (ITime) T.Second.Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IVelocity MilesPerMinute = new Velocity(L.Mile, (ITime) T.Minute.Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IVelocity MilesPerHour = new Velocity(L.Mile, (ITime) T.Hour.Invert());

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
            return GetBase<SI.Velocity, IVelocity>();
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
