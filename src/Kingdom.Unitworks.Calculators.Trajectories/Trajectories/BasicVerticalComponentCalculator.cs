namespace Kingdom.Unitworks.Calculators.Trajectories
{
    using L = Dimensions.Systems.SI.Length;
    using T = Dimensions.Systems.Commons.Time;
    using Theta = Dimensions.Systems.SI.PlanarAngle;
    using Values = Dimensions.Systems.SI.Values;

    /// <summary>
    /// 
    /// </summary>
    public class BasicVerticalComponentCalculator : TrajectoryComponentCalculatorBase
    {
        /// <summary>
        /// Gets the Type of component calculator it is: <see cref="TrajectoryComponent.Y"/>.
        /// </summary>
        public override TrajectoryComponent Type
        {
            get { return TrajectoryComponent.Y; }
        }

        /// <summary>
        /// Calculates a trajectory component given <paramref name="parameters"/> and <paramref name="timeQty"/>.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="timeQty"></param>
        /// <returns></returns>
        public override IQuantity Calculate(ITrajectoryParameters parameters, IQuantity timeQty)
        {
            var m = L.Meter;
            var s = T.Second;

            var t = VerifyDimensions(timeQty, s);

            var iv = VerifyDimensions(parameters.InitialVelocityQty, m, s.Invert());

            var vla = VerifyDimensions(parameters.VerticalLaunchAngleQty, Theta.Radian);

            var g = VerifyDimensions(Values.StandardGravity, m, s.Squared().Invert());

            // y = ( V sin A ) t - g t^2 / 2
            var resultQty = ((Quantity) iv*vla.Sin()) - ((Quantity) g*t.Squared())/2d;

            // Should be left with a Length dimension.
            return VerifyDimensions(resultQty, m);
        }
    }
}
