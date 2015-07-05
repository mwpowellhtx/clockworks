namespace Kingdom.Clockworks
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class TimeableRequestBase
    {
        /// <summary>
        /// One: 1
        /// </summary>
        protected const int One = 1;

        /// <summary>
        /// OneSecondMilliseconds: 1000d
        /// </summary>
        /// <see cref="One"/>
        protected const double OneSecondMilliseconds = One*1000d;

        /// <summary>
        /// Gets the Direction.
        /// </summary>
        public RunningDirection? Direction { get; private set; }

        /// <summary>
        /// MillisecondsPerStep backing field.
        /// </summary>
        public readonly double MillisecondsPerStep;

        /// <summary>
        /// Steps backing field.
        /// </summary>
        private readonly int _steps;

        /// <summary>
        /// Gets the requested number of Steps.
        /// </summary>
        public int Steps
        {
            get { return GetSteps(_steps); }
        }

        /// <summary>
        /// Returns the calculated number of Steps, depending on the nature of the request.
        /// </summary>
        /// <param name="startingFrom"></param>
        /// <returns></returns>
        protected abstract int GetSteps(int startingFrom);

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
        /// Protected Constructor
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="millisecondsPerStep"></param>
        /// <param name="steps"></param>
        /// <param name="type"></param>
        protected TimeableRequestBase(
            RunningDirection? direction = null,
            double millisecondsPerStep = 1000d,
            int steps = One,
            RequestType type = RequestType.Instantaneous)
        {
            Direction = direction;
            MillisecondsPerStep = millisecondsPerStep;
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
        /// Gets whether WillRun. This means the request has a <see cref="Direction"/>, a non-zero number
        /// of <see cref="Steps"/>, and runs <see cref="RequestType.Continuous"/>ly.
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

        /// <summary>
        /// Returns the <paramref name="candidateMilliseconds"/>, optionally scaled by
        /// <see cref="_millisecondsPerStep"/>.
        /// </summary>
        /// <param name="candidateMilliseconds"></param>
        /// <returns></returns>
        public double GetIntervalCandidate(double candidateMilliseconds)
        {
            //TODO: not multiplying by _millisecondsPerStep, but rather double? timerIntervalRatio ... when ready...
            return candidateMilliseconds*1d;
        }

        #region Equatable Members

        /// <summary>
        /// Returns whether 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        protected static bool Equals<TRequest>(TRequest a, TRequest b)
            where TRequest : ITimeableRequest
        {
            return ReferenceEquals(a, b)
                   || (!(a == null || b == null)
                       && (a.WillRun == b.WillRun && a.IsContinuous == b.IsContinuous
                           && a.Direction.Equals(b.Direction) && a.Steps == b.Steps));
        }

        #endregion
    }
}
