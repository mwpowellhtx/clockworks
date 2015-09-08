using Kingdom.Unitworks.Calculators;

namespace Kingdom.Unitworks.Trajectories
{
    /// <summary>
    /// Drag force calculator working from the default <see cref="ITrajectoryParameters"/>. Because
    /// of the complexities involved calculating drag forces, indeed calculating forces in general,
    /// the purpose of this interface and the base classes, starting with
    /// <see cref="DragForceCalculatorBase"/>, are intentionally abstract in nature. Best practice
    /// suggests to do the long calculations. I believe the dimensions work out in a safer manner.
    /// However, there are some shorthand calculations that can be done as well. It is not the
    /// purpose of this assembly to determine which calculations to perform, but rather to set the
    /// foundation for those calculations being performed.
    /// </summary>
    public interface IDragForceCalculator : ICalculator
    {
        /// <summary>
        /// Returns the calculated drag force given <paramref name="parameters"/>.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IQuantity Calculate(ITrajectoryParameters parameters);
    }

    /// <summary>
    /// Drag force calculator where a specialized <typeparamref name="TParameters"/> is given.
    /// </summary>
    /// <typeparam name="TParameters"></typeparam>
    public interface IDragForceCalculator<in TParameters> : IDragForceCalculator
        where TParameters : ITrajectoryParameters
    {
        /// <summary>
        /// Returns the calculated drag force given <paramref name="parameters"/>.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        IQuantity Calculate(TParameters parameters);
    }
}
