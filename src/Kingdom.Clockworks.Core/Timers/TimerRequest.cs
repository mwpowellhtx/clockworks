﻿using System;

namespace Kingdom.Clockworks.Timers
{
    /// <summary>
    /// Represents a timer request.
    /// </summary>
    public class TimerRequest
        : TimeableRequestBase
            , ISteppableRequest
            , IEquatable<TimerRequest>
    {
        /// <summary>
        /// Gets the Default <see cref="TimerRequest"/> instance.
        /// </summary>
        public static readonly TimerRequest DefaultRequest = new TimerRequest(steps: 0);

        /// <summary>
        /// Returns the number of <see cref="TimeableRequestBase.Steps"/> depending on the
        /// <see cref="RunningDirection"/> in which the clock is moving. Remember that to
        /// a timer moving forward is to substract time.
        /// </summary>
        /// <param name="startingFrom"></param>
        /// <returns></returns>
        protected override int GetSteps(int startingFrom)
        {
            switch (Direction)
            {
                case RunningDirection.Forward:
                    return -Math.Abs(startingFrom);
                case RunningDirection.Backward:
                    return Math.Abs(startingFrom);
            }
            return 0;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="millisecondsPerStep"></param>
        /// <param name="steps"></param>
        /// <param name="type"></param>
        public TimerRequest(
            RunningDirection? direction = null,
            double millisecondsPerStep = OneSecondMilliseconds,
            int steps = One,
            RequestType type = RequestType.Instantaneous)
            : base(direction, millisecondsPerStep, steps, type)
        {
        }

        #region Equatable Members

        /// <summary>
        /// Returns whether <paramref name="a"/> Equals <paramref name="b"/>.
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals(TimerRequest a, TimerRequest b)
        {
            return Equals<TimerRequest>(a, b);
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
        public bool Equals(TimerRequest other)
        {
            return Equals(this, other);
        }

        #endregion
    }
}
