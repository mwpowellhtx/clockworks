﻿namespace Kingdom.Unitworks.Trajectories
{
    /// <summary>
    /// Represents basic trajectory parameters.
    /// </summary>
    public interface ITrajectoryParameters
    {
        /// <summary>
        /// Gets or sets the InitialLaunchHeightQty.
        /// </summary>
        IQuantity InitialLaunchHeightQty { get; set; }

        /// <summary>
        /// Gets or sets the VerticalLaunchAngleQty.
        /// </summary>
        IQuantity VerticalLaunchAngleQty { get; set; }

        /// <summary>
        /// Gets or sets the HorizontalLaunchAngleQty.
        /// </summary>
        IQuantity HorizontalLaunchAngleQty { get; set; }

        /// <summary>
        /// Gets or sets the InitialVelocityQty.
        /// </summary>
        IQuantity InitialVelocityQty { get; set; }

        /// <summary>
        /// Gets or sets the TimeQty.
        /// </summary>
        IQuantity TimeQty { get; set; }

        /// <summary>
        /// Gets the change in <see cref="TimeQty"/>.
        /// </summary>
        IQuantity DeltaTimeQty { get; }
    }
}
