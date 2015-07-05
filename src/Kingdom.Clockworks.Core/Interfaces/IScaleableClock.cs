using Kingdom.Unitworks.Units;

namespace Kingdom.Clockworks
{
    /// <summary>
    /// 
    /// </summary>
    public interface IScaleableClock
    {
        #region Per Millisecond Members

        /// <summary>
        /// 
        /// </summary>
        double MillisecondsPerMillisecond { get; }

        /// <summary>
        /// 
        /// </summary>
        double SecondsPerMillisecond { get; }

        /// <summary>
        /// 
        /// </summary>
        double MinutesPerMillisecond { get; }

        /// <summary>
        /// 
        /// </summary>
        double HoursPerMillisecond { get; }

        #endregion

        #region Per Second Members

        /// <summary>
        /// 
        /// </summary>
        double MillisecondsPerSecond { get; }

        /// <summary>
        /// 
        /// </summary>
        double SecondsPerSecond { get; set; }

        /// <summary>
        /// 
        /// </summary>
        double MinutesPerSecond { get; }

        /// <summary>
        /// 
        /// </summary>
        double HoursPerSecond { get; }

        #endregion

        #region Per Minute Members

        /// <summary>
        /// 
        /// </summary>
        double MillisecondsPerMinute { get; }

        /// <summary>
        /// 
        /// </summary>
        double SecondsPerMinute { get; }

        /// <summary>
        /// 
        /// </summary>
        double MinutesPerMinute { get; }

        /// <summary>
        /// 
        /// </summary>
        double HoursPerMinute { get; }

        #endregion

        #region Per Hour Members

        /// <summary>
        /// 
        /// </summary>
        double MillisecondsPerHour { get; }

        /// <summary>
        /// 
        /// </summary>
        double SecondsPerHour { get; }

        /// <summary>
        /// 
        /// </summary>
        double MinutesPerHour { get; }

        /// <summary>
        /// 
        /// </summary>
        double HoursPerHour { get; }

        #endregion

        /// <summary>
        /// Gets <see cref="SecondsPerSecond"/> in terms of the numerator and denominator units.
        /// </summary>
        /// <param name="numeratorUnit"></param>
        /// <param name="denominatorUnit"></param>
        /// <returns></returns>
        double this[TimeUnit numeratorUnit, TimeUnit denominatorUnit] { get; }
    }
}
