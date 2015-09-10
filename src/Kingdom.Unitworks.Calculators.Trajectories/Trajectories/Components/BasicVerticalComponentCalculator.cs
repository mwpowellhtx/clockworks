namespace Kingdom.Unitworks.Calculators.Trajectories.Components
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
        /// Gets the Type: <see cref="TrajectoryComponent.Y"/>.
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

            var ivv = VerifyDimensions(parameters.InitialVerticalVelocityQty, m, s.Invert());

            // TODO: TBD: Forces due to aerodynamic lift must also play a role here, but not in this "simple" component calculator...
            var g = VerifyDimensions(Values.StandardGravity, m, s.Squared().Invert());

            /* y = ( V sin A ) t - g t^2 / 2
             * Or in this case, we have already calculated the initial vertical velocity component.
             * ref: http://hyperphysics.phy-astr.gsu.edu/hbase/traj.html#tra12 */
            var resultQty = (Quantity) ivv*t - ((Quantity) g*t.Squared())/2d;

            // Should be left with a Length dimension.
            return VerifyDimensions(resultQty, m);
        }
    }
}
