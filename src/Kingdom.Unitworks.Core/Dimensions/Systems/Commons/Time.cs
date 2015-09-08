namespace Kingdom.Unitworks.Dimensions.Systems.Commons
{
    /// <summary>
    /// Represents a common set of <see cref="ITime"/> dimensional units.
    /// </summary>
    /// <a href="!:http://en.wikipedia.org/wiki/International_System_of_Units" />
    public class Time : BaseDimension, ITime
    {
        internal const double SecondsPerMicrosecond = 1e-6;
        internal const double SecondsPerMillisecond = 1e-3;
        internal const double SecondsPerMinute = 60d;
        internal const double SecondsPerHour = 60*SecondsPerMinute;
        internal const double SecondsPerDay = 24*SecondsPerHour;
        internal const double SecondsPerWeek = 7*SecondsPerDay;

        //TODO: micro? nano?
        /// <summary>
        /// 
        /// </summary>
        public static readonly ITime Microsecond = new Time("µs", SecondsPerMicrosecond, 1d/SecondsPerMicrosecond);

        /// <summary>
        /// 
        /// </summary>
        public static readonly ITime Millisecond = new Time("ms", SecondsPerMillisecond, 1d/SecondsPerMillisecond);

        /// <summary>
        /// 
        /// </summary>
        public static readonly ITime Second = new Time("s");

        /// <summary>
        /// 
        /// </summary>
        public static readonly ITime Minute = new Time("min", SecondsPerMinute, 1d/SecondsPerMinute);

        /// <summary>
        /// 
        /// </summary>
        public static readonly ITime Hour = new Time("hr", SecondsPerHour, 1d/SecondsPerHour);

        /// <summary>
        /// 
        /// </summary>
        public static readonly ITime Day = new Time("day", SecondsPerDay, 1d/SecondsPerDay);

        /// <summary>
        /// 
        /// </summary>
        public static readonly ITime Week = new Time("week", SecondsPerWeek, 1d/SecondsPerWeek);

        private Time(string abbreviation, double? toBaseFactor = null, double? fromBaseFactor = null)
            : base(abbreviation,
                new BaseDimensionUnitConversion(toBaseFactor),
                new BaseDimensionUnitConversion(fromBaseFactor))
        {
        }

        private Time(Time other)
            : base(other)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IDimension GetBase()
        {
            return GetBase<Time, ITime>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override object Clone()
        {
            return new Time(this);
        }
    }
}
