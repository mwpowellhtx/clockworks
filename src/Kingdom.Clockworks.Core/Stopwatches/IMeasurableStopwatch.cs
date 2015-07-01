using System;

namespace Kingdom.Clockworks
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMeasurableStopwatch
    {
        /// <summary>
        /// 
        /// </summary>
        TimeSpan Elapsed { get; }

        /// <summary>
        /// 
        /// </summary>
        bool IsRunning { get; set; }
    }
}
