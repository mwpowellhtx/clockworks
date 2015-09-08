using Kingdom.Unitworks.Calculators;

namespace Kingdom.Unitworks.Trajectories
{
    /// <summary>
    /// 
    /// </summary>
    /// <a href="!:http://www.physics.usyd.edu.au/~cross/TRAJECTORIES/Trajectories.html" >Ball Trajectories</a>
    public abstract class TrajectoryComponentCalculatorBase
        : CalculatorBase
            , ITrajectoryComponentCalculator
    {
        /// <summary>
        /// Gets the Type of component calculator it is.
        /// </summary>
        public abstract TrajectoryComponent Type { get; }

        /// <summary>
        /// Calculates a trajectory component given <paramref name="parameters"/> and <paramref name="timeQty"/>.
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="timeQty"></param>
        /// <returns></returns>
        public abstract IQuantity Calculate(ITrajectoryParameters parameters, IQuantity timeQty);
    }
}
