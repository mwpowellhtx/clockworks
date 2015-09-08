using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Kingdom.Unitworks.Calculators.Trajectories
{
    /// <summary>
    /// Reports calculator progress and provides a way of stopping the calculations from running.
    /// </summary>
    public class TrajectoryCalculatorEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the Components that were calculated.
        /// </summary>
        internal readonly IDictionary<TrajectoryComponent, IQuantity> InternalComponents
            = new Dictionary<TrajectoryComponent, IQuantity>();

        /// <summary>
        /// Gets the Components that were calculated.
        /// </summary>
        public IReadOnlyDictionary<TrajectoryComponent, IQuantity> Components
        {
            get { return new ReadOnlyDictionary<TrajectoryComponent, IQuantity>(InternalComponents); }
        }

        /// <summary>
        /// Gets the Results involved.
        /// </summary>
        public readonly IReadOnlyDictionary<TrajectoryComponent, IQuantity> Results;

        /// <summary>
        /// Gets or sets whether to Continue.
        /// </summary>
        public bool Continue { get; set; }

        /// <summary>
        /// Internal Constructor
        /// </summary>
        /// <param name="results"></param>
        internal TrajectoryCalculatorEventArgs(IDictionary<TrajectoryComponent, IQuantity> results)
        {
            Results = new ReadOnlyDictionary<TrajectoryComponent, IQuantity>(results);
            Continue = true;
        }
    }

    /// <summary>
    /// TrajectoryCalculatorEventHandler delegate.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void TrajectoryCalculatorEventHandler(object sender, TrajectoryCalculatorEventArgs e);
}
