namespace Kingdom.Unitworks.Dimensions.Systems.US
{
    using L = Length;
    using T = Commons.Time;

    /// <summary>
    /// 
    /// </summary>
    public class Acceleration : DerivedDimension, IAcceleration
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly IAcceleration InchesPerMillisecondSquared = new Acceleration(L.Inch, (ITime) T.Millisecond.Squared().Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IAcceleration InchesPerSecondSquared = new Acceleration(L.Inch, (ITime) T.Second.Squared().Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IAcceleration InchesPerMinuteSquared = new Acceleration(L.Inch, (ITime) T.Minute.Squared().Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IAcceleration InchesPerHourSquared = new Acceleration(L.Inch, (ITime) T.Hour.Squared().Invert());


        /// <summary>
        /// 
        /// </summary>
        public static readonly IAcceleration FeetPerMillisecondSquared = new Acceleration(L.Foot, (ITime) T.Millisecond.Squared().Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IAcceleration FeetPerSecondSquared = new Acceleration(L.Foot, (ITime) T.Second.Squared().Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IAcceleration FeetPerMinuteSquared = new Acceleration(L.Foot, (ITime) T.Minute.Squared().Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IAcceleration FeetPerHourSquared = new Acceleration(L.Foot, (ITime) T.Hour.Squared().Invert());


        /// <summary>
        /// 
        /// </summary>
        public static readonly IAcceleration YardsPerMillisecondSquared = new Acceleration(L.Yard, (ITime) T.Millisecond.Squared().Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IAcceleration YardsPerSecondSquared = new Acceleration(L.Yard, (ITime) T.Second.Squared().Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IAcceleration YardsPerMinuteSquared = new Acceleration(L.Yard, (ITime) T.Minute.Squared().Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IAcceleration YardsPerHourSquared = new Acceleration(L.Yard, (ITime) T.Hour.Squared().Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IAcceleration MilesPerMillisecondSquared = new Acceleration(L.Mile, (ITime) T.Millisecond.Squared().Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IAcceleration MilesPerSecondSquared = new Acceleration(L.Mile, (ITime) T.Second.Squared().Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IAcceleration MilesPerMinuteSquared = new Acceleration(L.Mile, (ITime) T.Minute.Squared().Invert());

        /// <summary>
        /// 
        /// </summary>
        public static readonly IAcceleration MilesPerHourSquared = new Acceleration(L.Mile, (ITime) T.Hour.Squared().Invert());

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
            return GetBase<SI.Acceleration, IAcceleration>();
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
