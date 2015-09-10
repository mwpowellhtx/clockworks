using System;
using System.Reactive.Linq;

namespace Kingdom.Unitworks.Calculators.Trajectories
{
    public class BasicTrajectoryCalculatorFixture : BasicTrajectoryCalculator
    {
        /// <summary>
        /// ObserveCalculated backing field.
        /// </summary>
        private IObservable<TrajectoryCalculatorEventArgs> _observeCalculated;

        /// <summary>
        /// Gets an <see cref="IObservable{TrajectoryCalculatorEventArgs}"/> ObserveCalculated.
        /// </summary>
        internal IObservable<TrajectoryCalculatorEventArgs> ObserveCalculated
        {
            get
            {
                return _observeCalculated ?? (_observeCalculated = Observable.FromEventPattern<
                    TrajectoryCalculatorEventHandler, TrajectoryCalculatorEventArgs>(
                        handler => Calculated += handler, handler => Calculated -= handler)
                    .Select(pattern => pattern.EventArgs));
            }
        }
    }
}
