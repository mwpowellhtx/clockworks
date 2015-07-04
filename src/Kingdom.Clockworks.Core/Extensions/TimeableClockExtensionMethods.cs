using System;

namespace Kingdom.Clockworks
{
    /// <summary>
    /// 
    /// </summary>
    public static class TimeableClockExtensionMethods
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TClock"></typeparam>
        /// <param name="clock"></param>
        /// <returns></returns>
        public static TClock Pause<TClock>(this TClock clock)
            where TClock : TimeableClockBase
        {
            clock.Stop();
            return clock;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TClock"></typeparam>
        /// <param name="clock"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static TClock Resume<TClock>(this TClock clock, int interval)
            where TClock : TimeableClockBase
        {
            clock.Start(interval);
            return clock;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TClock"></typeparam>
        /// <param name="clock"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static TClock Resume<TClock>(this TClock clock, uint interval)
            where TClock : TimeableClockBase
        {
            clock.Start(interval);
            return clock;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TClock"></typeparam>
        /// <param name="clock"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static TClock Resume<TClock>(this TClock clock, long interval)
            where TClock : TimeableClockBase
        {
            clock.Start(interval);
            return clock;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TClock"></typeparam>
        /// <param name="clock"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static TClock Resume<TClock>(this TClock clock, TimeSpan interval)
            where TClock : TimeableClockBase
        {
            clock.Start(interval);
            return clock;
        }
    }
}
