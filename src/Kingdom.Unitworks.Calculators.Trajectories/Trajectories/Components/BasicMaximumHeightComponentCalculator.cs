namespace Kingdom.Unitworks.Calculators.Trajectories.Components
{
    using L = Dimensions.Systems.SI.Length; 
    using V = Dimensions.Systems.SI.Velocity;
    using Theta = Dimensions.Systems.SI.PlanarAngle;
    using Values = Dimensions.Systems.SI.Values;

    /// <summary>
    /// Basic maximum range component calculator.
    /// </summary>
    /// <a href="!:http://hyperphysics.phy-astr.gsu.edu/hbase/traj.html" >Trajectories</a>
    public class BasicMaximumHeightComponentCalculator : TrajectoryComponentCalculatorBase
    {
        /// <summary>
        /// Gets the <see cref="TrajectoryComponent.MaxHeight"/>.
        /// </summary>
        public override TrajectoryComponent Type
        {
            get { return TrajectoryComponent.MaxHeight; }
        }

        /// <summary>
        /// Calculates the <see cref="TrajectoryComponent.MaxHeight"/> component. Effectively
        /// ignores the <paramref name="timeQty"/> part, for purposes of this component.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="timeQty"></param>
        /// <returns></returns>
        public override IQuantity Calculate(ITrajectoryParameters parameters, IQuantity timeQty)
        {
            var ivv = VerifyDimensions(parameters.InitialVerticalVelocityQty, V.MetersPerSecond);
            var vla = VerifyDimensions(parameters.VerticalLaunchAngleQty, Theta.Radian);

            var g = Values.G;

            //TODO: also does not take into consideration the initial launch height, i.e. from which thrown, punted, etc.

            // Also, leverages trig identity: sin( 2 Θ ) = 2 sin( Θ ) cos( Θ )
            var resultQty = ((Quantity) ivv.Squared()*((Quantity) vla*2d).Sin())/g;

            return VerifyDimensions(resultQty, L.Meter);
        }
    }
}
