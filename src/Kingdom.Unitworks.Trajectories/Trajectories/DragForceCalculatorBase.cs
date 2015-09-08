using System.Linq;
using Kingdom.Unitworks.Calculators;
using Kingdom.Unitworks.Dimensions;

namespace Kingdom.Unitworks.Trajectories
{
    using F = Dimensions.Systems.SI.Force;
    using M = Dimensions.Systems.SI.Mass;
    using L = Dimensions.Systems.SI.Length;
    using T = Dimensions.Systems.Commons.Time;

    /// <summary>
    /// Drag force calculator working from the default <see cref="ITrajectoryParameters"/>.
    /// </summary>
    public abstract class DragForceCalculatorBase
        : CalculatorBase
            , IDragForceCalculator
    {
        protected static readonly IDimension[] ForceDimensions;

        /// <summary>
        /// Static Constructor
        /// </summary>
        static DragForceCalculatorBase()
        {
            ForceDimensions = F.Newton.Dimensions.EnumerateAll().ToArray();
        }

        /// <summary>
        /// Calculates the drag force given the <paramref name="parameters"/>.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public abstract IQuantity Calculate(ITrajectoryParameters parameters);
    }

    /// <summary>
    /// Drag force calculator where a specialized <typeparamref name="TParameters"/> is given.
    /// </summary>
    /// <typeparam name="TParameters"></typeparam>
    public abstract class DragForceCalculatorBase<TParameters>
        : DragForceCalculatorBase
            , IDragForceCalculator<TParameters>
        where TParameters : ITrajectoryParameters
    {
        /// <summary>
        /// Calculates the drag force given the <paramref name="parameters"/>.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public override sealed IQuantity Calculate(ITrajectoryParameters parameters)
        {
            return Calculate((TParameters) parameters);
        }

        /// <summary>
        /// Calculates the drag force given the <paramref name="parameters"/>.
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public abstract IQuantity Calculate(TParameters parameters);
    }
}
