using System.Collections.Generic;
using System.Linq;
using Kingdom.Unitworks.Calculators.Trajectories.Components;

namespace Kingdom.Unitworks.Calculators.Trajectories
{
    using L = Dimensions.Systems.SI.Length;

    /// <summary>
    /// 
    /// </summary>
    public class TrajectoryCalculator
        : CalculatorBase
            , ITrajectoryCalculator
    {
        private ITrajectoryParameters _parameters;

        /// <summary>
        /// Gets or sets the Parameters.
        /// </summary>
        public ITrajectoryParameters Parameters
        {
            get { return _parameters; }
            set { _parameters = value; }
        }

        private readonly IEnumerable<ITrajectoryComponentCalculator> _calculators;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="calculators"></param>
        public TrajectoryCalculator(params ITrajectoryComponentCalculator[] calculators)
        {
            _calculators = calculators;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="calculators"></param>
        public TrajectoryCalculator(ITrajectoryParameters parameters,
            params ITrajectoryComponentCalculator[] calculators)
            : this(calculators)
        {
            _parameters = parameters;
        }

        /// <summary>
        /// Calculates the trajectory components at the specified <paramref name="timeQty"/>.
        /// </summary>
        /// <param name="timeQty"></param>
        /// <returns></returns>
        public IDictionary<TrajectoryComponent, IQuantity> Calculate(IQuantity timeQty)
        {
            IDictionary<TrajectoryComponent, IQuantity> results;

            TryCalculate(timeQty, out results);

            return results;
        }

        /// <summary>
        /// Tries to calculate the desired <paramref name="results"/> at the specified <paramref name="timeQty"/>.
        /// </summary>
        /// <param name="timeQty"></param>
        /// <param name="results"></param>
        /// <returns></returns>
        public bool TryCalculate(IQuantity timeQty, out IDictionary<TrajectoryComponent, IQuantity> results)
        {
            results = _calculators.ToDictionary(c => c.Type, c => c.Calculate(_parameters, timeQty));

            // Continue trying to calculate as long as we receive feedback in order to do so.
            var e = new TrajectoryCalculatorEventArgs(results);

            OnCalculated(e);

            return e.Continue;
        }

        /// <summary>
        /// Calculated event.
        /// </summary>
        public event TrajectoryCalculatorEventHandler Calculated;

        /// <summary>
        /// Occurs on the next <see cref="Calculated"/> trajectory point.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnCalculated(TrajectoryCalculatorEventArgs e)
        {
            if (Calculated == null) return;
            Calculated(this, e);
        }
    }
}
