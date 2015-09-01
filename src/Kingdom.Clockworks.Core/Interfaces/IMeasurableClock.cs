using System;
using Kingdom.Unitworks;

namespace Kingdom.Clockworks
{
    using T = Unitworks.Dimensions.Systems.Commons.Time;

    /// <summary>
    /// Provides a way of measuring the stopwatch behavior.
    /// </summary>
    public interface IMeasurableClock : IDisposable
    {
        /// <summary>
        /// Gets the Starting <see cref="TimeSpan"/>.
        /// </summary>
        TimeSpan Starting { get; }

        /// <summary>
        /// Gets or sets the StartingQty.
        /// </summary>
        IQuantity StartingQty { get; set; }

        /// <summary>
        /// Gets the Elapsed <see cref="TimeSpan"/>.
        /// </summary>
        TimeSpan Elapsed { get; }

        /// <summary>
        /// Gets or sets the ElapsedQuantity <see cref="IQuantity"/>.
        /// </summary>
        IQuantity ElapsedQty { get; set; }
    }

    /// <summary>
    /// Provides a way of measuring the stopwatch behavior.
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    public interface IMeasurableClock<out TRequest> : IMeasurableClock
        where TRequest : TimeableRequestBase
    {
        /// <summary>
        /// Gets the CurrentRequest.
        /// </summary>
        TRequest CurrentRequest { get; }

        //TODO: I can envision negative values being possible... so always apply the current, regardless. may stop if flagged "CanBeNegative"
    }

    /// <summary>
    /// 
    /// </summary>
    public static class MeasurableClockExtensionMethods
    {
        /// <summary>
        /// Resets the <see cref="IMeasurableClock.ElapsedQty"/> value.
        /// </summary>
        /// <typeparam name="TClock"></typeparam>
        /// <param name="clock"></param>
        /// <param name="elapsedMilliseconds"></param>
        /// <returns></returns>
        public static TClock Reset<TClock>(this TClock clock, double elapsedMilliseconds = 0d)
            where TClock : IMeasurableClock
        {
            clock.ElapsedQty = new Quantity(elapsedMilliseconds, T.Millisecond);
            return clock;
        }

        /// <summary>
        /// Resets the <see cref="IMeasurableClock.StartingQty"/> value.
        /// </summary>
        /// <typeparam name="TClock"></typeparam>
        /// <param name="clock"></param>
        /// <param name="startingMilliseconds"></param>
        /// <returns></returns>
        public static TClock Restart<TClock>(this TClock clock, double startingMilliseconds = 0d)
            where TClock : IMeasurableClock
        {
            clock.ElapsedQty = new Quantity(startingMilliseconds, T.Millisecond);
            return clock.Reset();
        }
    }
}
