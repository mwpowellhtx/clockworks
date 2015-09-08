namespace Kingdom.Unitworks.Trajectories
{
    using L = Dimensions.Systems.SI.Length;
    using T = Dimensions.Systems.Commons.Time;
    using Theta = Dimensions.Systems.SI.PlanarAngle;

    /// <summary>
    /// 
    /// </summary>
    public class BasicHorizontalComponentCalculator : TrajectoryComponentCalculatorBase
    {
        /// <summary>
        /// Gets the Type of component calculator it is: <see cref="TrajectoryComponent.X"/>.
        /// </summary>
        public override TrajectoryComponent Type
        {
            get { return TrajectoryComponent.X; }
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

            // x = ( V cos A ) t
            var resultQty = (Quantity) iv*vla.Cos()*t;

            // Should be left with a Length dimension.
            return VerifyDimensions(resultQty, m);
        }
    }
}
