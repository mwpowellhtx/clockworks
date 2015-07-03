using System;

namespace Kingdom.Clockworks
{
    /// <summary>
    /// Represents a request on a running <see cref="SimulationStopwatch"/>.
    /// </summary>
    public class StopwatchRequest : ISteppableRequest, IEquatable<StopwatchRequest>
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly StopwatchRequest Default = new StopwatchRequest(steps: 0);

        /// <summary>
        /// Direction backing field.
        /// </summary>
        private readonly RunningDirection? _direction;

        /// <summary>
        /// Gets or sets the Direction. 
        /// </summary>
        public RunningDirection? Direction
        {
            get { return _direction; }
        }

        /// <summary>
        /// Steps backing field.
        /// </summary>
        private readonly int _steps;

        /// <summary>
        /// Gets or sets the Steps.
        /// </summary>
        public int Steps
        {
            get
            {
                switch (Direction)
                {
                    case RunningDirection.Forward:
                        return Math.Abs(_steps);
                    case RunningDirection.Backward:
                        return -Math.Abs(_steps);
                }
                return 0;
            }
        }

        /// <summary>
        /// Type backing field.
        /// </summary>
        private readonly RequestType _type;

        /// <summary>
        /// Gets or sets the request Type.
        /// </summary>
        public RequestType Type
        {
            get { return _type; }
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="steps"></param>
        /// <param name="type"></param>
        public StopwatchRequest(
            RunningDirection? direction = null,
            int steps = 1,
            RequestType type = RequestType.Instantaneous)
        {
            _direction = direction;
            _steps = steps;
            _type = type;
        }

        /// <summary>
        /// Gets whether IsInstantaneous.
        /// </summary>
        public bool IsInstantaneous
        {
            get { return Type == RequestType.Instantaneous; }
        }

        /// <summary>
        /// Gets whether IsContinuous.
        /// </summary>
        public bool IsContinuous
        {
            get { return Type == RequestType.Continuous; }
        }

        /// <summary>
        /// Gets whether WillRun. This means the request has a <see cref="Direction"/>, a non-zero
        /// number of <see cref="Steps"/>, and runs <see cref="RequestType.Continuous"/>ly.
        /// </summary>
        public bool WillRun
        {
            get { return Direction.HasValue && Steps > 0 && IsContinuous; }
        }

        /// <summary>
        /// Gets whether WillNotRun.
        /// </summary>
        public bool WillNotRun
        {
            get { return !WillRun; }
        }

        #region Equatable Members

        /// <summary>
        /// Returns whether 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals(StopwatchRequest a, StopwatchRequest b)
        {
            return ReferenceEquals(a, b) || (!(a == null || b == null)
                                             && (a.WillRun == b.WillRun && a.IsContinuous == b.IsContinuous
                                                 && a.Direction == b.Direction && a.Steps == b.Steps));
        }

        /// <summary>
        /// Returns whether 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equals(ISteppableRequest a, ISteppableRequest b)
        {
            return Equals(a as StopwatchRequest, b as StopwatchRequest);
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
