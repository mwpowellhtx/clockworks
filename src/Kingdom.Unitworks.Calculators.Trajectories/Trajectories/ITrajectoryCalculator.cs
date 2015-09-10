using System.Collections.Generic;

namespace Kingdom.Unitworks.Calculators.Trajectories
{
    using Components;

    /// <summary>
    /// 
    /// </summary>
    public interface ITrajectoryCalculator : ICalculator
    {
        /// <summary>
        /// Gets or sets the Parameters.
        /// </summary>
        ITrajectoryParameters Parameters { get; set; }

        /// <summary>
        /// Calculates the trajectory components at the specified <paramref name="timeQty"/>.
        /// </summary>
        /// <param name="timeQty"></param>
        /// <returns></returns>
        IDictionary<TrajectoryComponent, IQuantity> Calculate(IQuantity timeQty);

        /// <summary>
        /// Tries to calculate the desired <paramref name="results"/> at the specified <paramref name="timeQty"/>.
        /// </summary>
        /// <param name="timeQty"></param>
        /// <param name="results"></param>
        /// <returns></returns>
        bool TryCalculate(IQuantity timeQty, out IDictionary<TrajectoryComponent, IQuantity> results);

        /// <summary>
        /// Calculated event.
        /// </summary>
        event TrajectoryCalculatorEventHandler Calculated;
    }
}
