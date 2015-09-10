namespace Kingdom.Unitworks.Calculators.Trajectories.Components
{
    using L = Dimensions.Systems.SI.Length;
    using V = Dimensions.Systems.SI.Velocity;
    using Theta = Dimensions.Systems.SI.PlanarAngle;
    using Values = Dimensions.Systems.SI.Values;

    /// <summary>
    /// Basic maximum range component calculator.
    /// </summary>
    /// <a href="!:http://scienceblogs.com/dotphysics/2008/12/26/what-angle-should-you-throw-a-football-for-maximum-range/"
    /// >What angle should you throw a football for maximum range?</a>
    public class BasicMaximumRangeComponentCalculator : TrajectoryComponentCalculatorBase
    {
        public override TrajectoryComponent Type
        {
            get { return TrajectoryComponent.MaxRange; }
        }

        private IQuantity _componentQty;

        public override IQuantity Calculate(ITrajectoryParameters parameters, IQuantity timeQty)
        {
            // ReSharper disable once InvertIf
            if (ReferenceEquals(null, _componentQty))
            {
                var iv = VerifyDimensions(parameters.InitialVelocityQty, V.MetersPerSecond);
                var vla = VerifyDimensions(parameters.VerticalLaunchAngleQty, Theta.Radian);

                var g = Values.G;

                //TODO: also does not take into consideration the initial launch height, i.e. from which thrown, punted, etc.

                // Also, leverages trig identity: sin( 2 Θ ) = 2 sin( Θ ) cos( Θ )
                _componentQty = ((Quantity) iv.Squared()*((Quantity) vla*2d).Sin())/g;
            }

            return VerifyDimensions(_componentQty, L.Meter);
        }
    }
}
