namespace Kingdom.Unitworks.Calculators.Trajectories
{
    /// <summary>
    /// Represents trajectory parameters with additional atmospheric concerns.
    /// </summary>
    public interface IAtmosphericTrajectoryParameters : ITrajectoryParameters
    {
        /// <summary>
        /// Gets or sets the ProjectileMassQty.
        /// </summary>
        IQuantity ProjectileMassQty { get; set; }

        /// <summary>
        /// Gets the calculated <see cref="ProjectileMassQty"/> force due to gravity.
        /// This is calculated afresh with every new mass quantity being set.
        /// </summary>
        IQuantity ProjectileForceGQty { get; }

        /// <summary>
        /// Gets or sets the AirDensityQty (&#961;, rho).
        /// </summary>
        IQuantity AirDensityQty { get; set; }

        /// <summary>
        /// Gets or sets the ProjectileAreaQty.
        /// </summary>
        IQuantity ProjectileAreaQty { get; set; }

        /// <summary>
        /// Gets or sets the DragCoefficientQty.
        /// </summary>
        IQuantity DragCoefficientQty { get; set; }
    }
}
