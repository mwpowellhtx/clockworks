using NUnit.Framework;

namespace Kingdom.Clockworks
{
    /// <summary>
    /// 
    /// </summary>
    internal class RunningDirectionValuesAttribute : ValuesAttribute
    {
        /// <summary>
        /// <see cref="RunningDirection"/>
        /// </summary>
        private static readonly RunningDirection? Null = null;

        /// <summary>
        /// <see cref="RunningDirection.Forward"/>
        /// </summary>
        private static readonly RunningDirection? Forward = RunningDirection.Forward;

        /// <summary>
        /// <see cref="RunningDirection.Backward"/>
        /// </summary>
        private static readonly RunningDirection? Backward = RunningDirection.Backward;

        /// <summary>
        /// Constructor
        /// </summary>
        internal RunningDirectionValuesAttribute()
            : base(Null, Forward, Backward)
        {
        }
    }
}
