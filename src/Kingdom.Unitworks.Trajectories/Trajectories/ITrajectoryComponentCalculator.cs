using Kingdom.Unitworks.Calculators;

namespace Kingdom.Unitworks.Trajectories
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITrajectoryComponentCalculator : ICalculator
    {
        /// <summary>
        /// Gets the Type of component calculator it is.
        /// </summary>
        TrajectoryComponent Type { get; }

        /// <summary>
        /// Calculates a trajectory component given <paramref name="parameters"/> and
        /// <paramref name="timeQty"/>. Component usually refers to one of either X, Y, or Z parts.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="timeQty"></param>
        /// <returns></returns>
        IQuantity Calculate(ITrajectoryParameters parameters, IQuantity timeQty);
    }
}
