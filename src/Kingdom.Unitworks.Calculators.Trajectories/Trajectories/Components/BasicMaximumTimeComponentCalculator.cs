namespace Kingdom.Unitworks.Calculators.Trajectories.Components
{
    using T = Dimensions.Systems.Commons.Time;
    using V = Dimensions.Systems.SI.Velocity;
    using Theta = Dimensions.Systems.SI.PlanarAngle;
    using Values = Dimensions.Systems.SI.Values;

    /// <summary>
    /// Provides a basic estimate of the maximum time in flight.
    /// </summary>
    public class BasicMaximumTimeComponentCalculator : TrajectoryComponentCalculatorBase
    {
        /// <summary>
        /// Gets the Type: <see cref="TrajectoryComponent.MaxTime"/>.
        /// </summary>
        public override TrajectoryComponent Type
        {
            get { return TrajectoryComponent.MaxTime; }
        }

        private IQuantity _componentQty;

        /// <summary>
        /// Calculates the <see cref="TrajectoryComponent.MaxTime"/> component. Effectively
        /// ignores the <paramref name="timeQty"/> part, for purposes of this component.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="timeQty"></param>
        /// <returns></returns>
        public override IQuantity Calculate(ITrajectoryParameters parameters, IQuantity timeQty)
        {
            // ReSharper disable once InvertIf
            if (ReferenceEquals(null, _componentQty))
            {
                var ivv = VerifyDimensions(parameters.InitialVerticalVelocityQty, V.MetersPerSecond);
                var vla = VerifyDimensions(parameters.VerticalLaunchAngleQty, Theta.Radian);

                var g = Values.G;

                _componentQty = (2d*(Quantity) ivv*vla.Sin())/g;
            }

            return VerifyDimensions(_componentQty, T.Second);
        }
    }
}
