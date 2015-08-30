using System;
using Kingdom.Clockworks.Timers;

namespace Kingdom.Clockworks.Extensions
{
    internal static class ClockExtensionMethods
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TClock"></typeparam>
        /// <param name="clock"></param>
        /// <param name="verify"></param>
        /// <returns></returns>
        public static TClock Verify<TClock>(this TClock clock, Action<TClock> verify = null)
            where TClock : IClockBase
        {
            verify = verify ?? (x => { });
            verify(clock);
            return clock;
        }
    }
}
