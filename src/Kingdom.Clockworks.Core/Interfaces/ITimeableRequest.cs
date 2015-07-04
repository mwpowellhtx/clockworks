namespace Kingdom.Clockworks
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITimeableRequest
    {
        /// <summary>
        /// Gets the Direction in which the timeable concern should move.
        /// </summary>
        RunningDirection? Direction { get; }

        /// <summary>
        /// Gets the number of Steps per request.
        /// </summary>
        int Steps { get; }

        /// <summary>
        /// Gets whether WillRun.
        /// </summary>
        bool WillRun { get; }

        /// <summary>
        /// Gets whether WillNotRun.
        /// </summary>
        bool WillNotRun { get; }

        /// <summary>
        /// Gets whether IsContinuous.
        /// </summary>
        bool IsContinuous { get; }

        /// <summary>
        /// Gets whether IsInstantaneous.
        /// </summary>
        bool IsInstantaneous { get; }
    }
}
