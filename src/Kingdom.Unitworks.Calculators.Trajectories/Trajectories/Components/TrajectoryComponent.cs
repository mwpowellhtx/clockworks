namespace Kingdom.Unitworks.Calculators.Trajectories.Components
{
    /// <summary>
    /// Informs the calculators which components are being considered.
    /// </summary>
    public enum TrajectoryComponent
    {
        /// <summary>
        /// The X component.
        /// </summary>
        X,

        /// <summary>
        /// The Y component.
        /// </summary>
        Y,

        /// <summary>
        /// The Z component.
        /// </summary>
        Z,

        /// <summary>
        /// The maximum <see cref="X"/>.
        /// </summary>
        MaxRange,

        /// <summary>
        /// The maximum <see cref="Y"/>.
        /// </summary>
        MaxHeight,

        /// <summary>
        /// The maximum time.
        /// </summary>
        MaxTime,
    }
}
