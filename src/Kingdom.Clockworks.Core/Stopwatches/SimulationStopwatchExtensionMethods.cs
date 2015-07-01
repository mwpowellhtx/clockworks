using System;

namespace Kingdom.Clockworks
{
    /// <summary>
    /// 
    /// </summary>
    public static class SimulationStopwatchExtensionMethods
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TStopwatch"></typeparam>
        /// <param name="stopwatch"></param>
        /// <returns></returns>
        public static TStopwatch Pause<TStopwatch>(this TStopwatch stopwatch)
            where TStopwatch : ISimulationStopwatch
        {
            stopwatch.Stop();
            return stopwatch;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TStopwatch"></typeparam>
        /// <param name="stopwatch"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static TStopwatch Resume<TStopwatch>(this TStopwatch stopwatch, int interval)
            where TStopwatch : ISimulationStopwatch
        {
            stopwatch.Start(interval);
            return stopwatch;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TStopwatch"></typeparam>
        /// <param name="stopwatch"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static TStopwatch Resume<TStopwatch>(this TStopwatch stopwatch, uint interval)
            where TStopwatch : ISimulationStopwatch
        {
            stopwatch.Start(interval);
            return stopwatch;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TStopwatch"></typeparam>
        /// <param name="stopwatch"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static TStopwatch Resume<TStopwatch>(this TStopwatch stopwatch, long interval)
            where TStopwatch : ISimulationStopwatch
        {
            stopwatch.Start(interval);
            return stopwatch;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TStopwatch"></typeparam>
        /// <param name="stopwatch"></param>
        /// <param name="interval"></param>
        /// <returns></returns>
        public static TStopwatch Resume<TStopwatch>(this TStopwatch stopwatch, TimeSpan interval)
            where TStopwatch : ISimulationStopwatch
        {
            stopwatch.Start(interval);
            return stopwatch;
        }
    }
}
