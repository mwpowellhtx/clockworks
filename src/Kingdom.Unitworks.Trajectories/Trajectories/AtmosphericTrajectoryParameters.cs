namespace Kingdom.Unitworks.Trajectories
{
    using T = Dimensions.Systems.Commons.Time;
    using M = Dimensions.Systems.SI.Mass;
    using L = Dimensions.Systems.SI.Length;
    using Values = Dimensions.Systems.SI.Values;

    /// <summary>
    /// Represents trajectory parameters with additional atmospheric concerns.
    /// </summary>
    public class AtmosphericTrajectoryParameters
        : TrajectoryParameters
            , IAtmosphericTrajectoryParameters
    {
        private IQuantity _projectileMassQty;

        /// <summary>
        /// Gets or sets the ProjectileMassQty.
        /// </summary>
        public IQuantity ProjectileMassQty
        {
            get { return _projectileMassQty; }
            set
            {
                var m = M.Kilogram;
                _projectileMassQty = VerifyDimension(value ?? Quantity.Zero(m), m);
                _projectileForceGQty = null;
            }
        }

        private IQuantity _projectileForceGQty;

        /// <summary>
        /// Gets the calculated <see cref="ProjectileMassQty"/> force due to gravity.
        /// This is calculated afresh with every new mass quantity being set.
        /// </summary>
        public IQuantity ProjectileForceGQty
        {
            get
            {
                return _projectileForceGQty = VerifyDimension(
                    _projectileForceGQty ?? ((Quantity) ProjectileMassQty*Values.G),
                    M.Kilogram, L.Meter, T.Second.Squared().Invert());
            }
        }

        private IQuantity _airDensityQty;

        /// <summary>
        /// Gets or sets the AirDensityQty (&#961;, rho).
        /// </summary>
        public IQuantity AirDensityQty
        {
            get { return _airDensityQty; }
            set
            {
                var kg = M.Kilogram;
                var perCubicMeter = L.Meter.Cubed().Invert();
                _airDensityQty = VerifyDimension(value ?? Quantity.Zero(kg, perCubicMeter),
                    kg, perCubicMeter);
            }
        }

        private IQuantity _projectileAreaQty;

        /// <summary>
        /// Gets or sets the ProjectileAreaQty.
        /// </summary>
        public IQuantity ProjectileAreaQty
        {
            get { return _projectileAreaQty; }
            set
            {
                var squareMeter = L.Meter.Squared();
                _projectileAreaQty = VerifyDimension(value ?? Quantity.Zero(squareMeter), squareMeter);
            }
        }

        private IQuantity _dragCoefficientQty;

        /// <summary>
        /// Gets or sets the DragCoefficientQty.
        /// </summary>
        public IQuantity DragCoefficientQty
        {
            get { return _dragCoefficientQty; }
            set { _dragCoefficientQty = VerifyDimension(value ?? Quantity.Zero()); }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ilhQty"></param>
        /// <param name="vlaQty"></param>
        /// <param name="hlaQty"></param>
        /// <param name="ivQty"></param>
        /// <param name="projectileMassQty"></param>
        /// <param name="projectileAreaQty"></param>
        /// <param name="rhoQty">An air density (&#961;, rho) quantity.</param>
        /// <param name="cdQty"></param>
        public AtmosphericTrajectoryParameters(IQuantity ilhQty, IQuantity vlaQty, IQuantity hlaQty,
            IQuantity ivQty, IQuantity projectileMassQty, IQuantity projectileAreaQty, IQuantity rhoQty,
            IQuantity cdQty)
            : base(ilhQty, vlaQty, hlaQty, ivQty)
        {
            ProjectileMassQty = projectileMassQty;
            ProjectileAreaQty = projectileAreaQty;
            AirDensityQty = rhoQty;
            DragCoefficientQty = cdQty;
        }
    }
}
