using System;

namespace Kingdom.Clockworks.Stopwatches
{
    /// <summary>
    /// Represents a request on a running <see cref="SimulationStopwatch"/>.
    /// </summary>
    public class StopwatchRequest
        : TimeableRequestBase
            , ISteppableRequest
            , IEquatable<StopwatchRequest>
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly StopwatchRequest Default = new StopwatchRequest(steps: 0);

        /// <summary>
        /// Returns the number of <see cref="TimeableRequestBase.Steps"/> depending on the
        /// <see cref="RunningDirection"/> in which the clock is moving. Remember that to
        /// a stopwatch forward is to add time.
        /// </summary>
        /// <param name="startingFrom"></param>
        /// <returns></returns>
        protected override int GetSteps(int startingFrom)
        {
            switch (Direction)
            {
                case RunningDirection.Forward:
                    return Math.Abs(startingFrom);
                case RunningDirection.Backward:
                    return -Math.Abs(startingFrom);
            }
            return 0;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="steps"></param>
        /// <param name="type"></param>
        public StopwatchRequest(
            RunningDirection? direction = null,
            int steps = 1,
            RequestType type = RequestType.Instantaneous)
            : base(direction, steps, type)
        {
        }

        #region Equatable Members

        /// <summary>
        /// Returns whether <paramref name="a"/> Equals <paramref name="b"/>.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals(StopwatchRequest a, StopwatchRequest b)
        {
            return Equals<StopwatchRequest>(a, b);
        }

        /// <summary>
        /// Returns whether this object Equals an <paramref name="other"/> one.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ISteppableRequest other)
        {
            return Equals(this, other);
        }

        /// <summary>
        /// Returns whether this object Equals an <paramref name="other"/> one.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(StopwatchRequest other)
        {
            return Equals(this, other);
        }

        #endregion
    }
}
