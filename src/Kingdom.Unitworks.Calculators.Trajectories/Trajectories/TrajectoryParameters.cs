﻿using System.Diagnostics;
using Kingdom.Unitworks.Dimensions;

namespace Kingdom.Unitworks.Calculators.Trajectories
{
    using T = Dimensions.Systems.Commons.Time;
    using M = Dimensions.Systems.SI.Mass;
    using L = Dimensions.Systems.SI.Length;
    using V = Dimensions.Systems.SI.Velocity;
    using SiTheta = Dimensions.Systems.SI.PlanarAngle;
    using UsTheta = Dimensions.Systems.US.PlanarAngle;

    /// <summary>
    /// Represents basic trajectory parameters.
    /// </summary>
    public class TrajectoryParameters : ITrajectoryParameters
    {
        /// <summary>
        /// Returns a verified <paramref name="qty"/> given a set of expected
        /// <paramref name="dimensions"/>.
        /// </summary>
        /// <param name="qty"></param>
        /// <param name="dimensions"></param>
        /// <returns></returns>
        protected static IQuantity VerifyDimension(IQuantity qty, params IDimension[] dimensions)
        {
            Debug.Assert(
                ReferenceEquals(null, qty) == false
                && qty.Dimensions.AreCompatible(dimensions, true));
            return qty;
        }

        private IQuantity _ilhQty;

        /// <summary>
        /// Gets or sets the InitialLaunchHeightQty.
        /// </summary>
        public IQuantity InitialLaunchHeightQty
        {
            get { return _ilhQty; }
            set
            {
                var m = L.Meter;
                _ilhQty = VerifyDimension(value ?? Quantity.Zero(m), m);
            }
        }

        private IQuantity _vlaQty;

        /// <summary>
        /// Gets or sets the VerticalLaunchAngleQty.
        /// </summary>
        public IQuantity VerticalLaunchAngleQty
        {
            get { return _vlaQty; }
            set
            {
                var deg = UsTheta.Degree;
                _vlaQty = VerifyDimension(value ?? Quantity.Zero(deg), deg);
            }
        }

        private IQuantity _hlaQty;

        /// <summary>
        /// Gets or sets the HorizontalLaunchAngleQty.
        /// </summary>
        public IQuantity HorizontalLaunchAngleQty
        {
            get { return _hlaQty; }
            set
            {
                var deg = UsTheta.Degree;
                _hlaQty = VerifyDimension(value ?? Quantity.Zero(deg), deg);
            }
        }

        private IQuantity _ivQty;

        /// <summary>
        /// Gets or sets the InitialVelocityQty.
        /// </summary>
        public IQuantity InitialVelocityQty
        {
            get { return _ivQty; }
            set
            {
                var metersPerSecond = V.MetersPerSecond;
                _ivQty = VerifyDimension(value ?? Quantity.Zero(metersPerSecond), metersPerSecond);
            }
        }

        private IQuantity _previousTimeQty;

        /// <summary>
        /// Gets or sets the PreviousTimeQty.
        /// </summary>
        private IQuantity PreviousTimeQty
        {
            get { return _previousTimeQty; }
            set
            {
                var s = T.Second;
                _previousTimeQty = VerifyDimension(value ?? Quantity.Zero(s), s);
            }
        }

        private IQuantity _timeQty;

        /// <summary>
        /// Gets or sets the TimeQty.
        /// </summary>
        public IQuantity TimeQty
        {
            get { return _timeQty; }
            set
            {
                var s = T.Second;
                _timeQty = VerifyDimension(value ?? Quantity.Zero(s), s);
            }
        }

        /// <summary>
        /// Gets the change in <see cref="TimeQty"/>.
        /// </summary>
        public IQuantity DeltaTimeQty
        {
            get { return VerifyDimension((Quantity) TimeQty - PreviousTimeQty, T.Second); }
        }

        private IQuantity _ivvQty;

        /// <summary>
        /// Gets the InitialVerticalVelocityQty.
        /// </summary>
        /// <see cref="InitialVelocityQty"/>
        /// <see cref="VerticalLaunchAngleQty"/>
        /// <see cref="TrigonometricExtensionMethods.Sin"/>
        public IQuantity InitialVerticalVelocityQty
        {
            get { return _ivvQty ?? (_ivvQty = CalculateInitialVerticalVelocity(_ivQty, _vlaQty)); }
        }

        private IQuantity _ihvQty;

        /// <summary>
        /// Gets the InitialHorizontalVelocityQty.
        /// </summary>
        /// <see cref="InitialVelocityQty"/>
        /// <see cref="VerticalLaunchAngleQty"/>
        /// <see cref="TrigonometricExtensionMethods.Cos"/>
        public IQuantity InitialHorizontalVelocityQty
        {
            get { return _ihvQty ?? (_ihvQty = CalculateInitialHorizontalVelocity(_ivQty, _vlaQty)); }
        }

        /// <summary>
        /// Default Constructor
        /// </summary>
        public TrajectoryParameters()
            : this(null, null, null, null)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ilhQty">An initial launch height quantity.</param>
        /// <param name="vlaQty">A vertical launch angle quantity.</param>
        /// <param name="hlaQty">A horizontal launch angle quantity.</param>
        /// <param name="ivQty">An initial velocity quantity.</param>
        public TrajectoryParameters(IQuantity ilhQty, IQuantity vlaQty, IQuantity hlaQty, IQuantity ivQty)
        {
            InitialLaunchHeightQty = ilhQty;
            VerticalLaunchAngleQty = vlaQty;
            HorizontalLaunchAngleQty = hlaQty;
            InitialVelocityQty = ivQty;
            PreviousTimeQty = null;
            TimeQty = null;
        }

        /* TODO: TBD: there might be enough here to justify a small-ish calculator all its own:
         * remember also the horizontal velocity component: IV cos( Θ ) */

        /// <summary>
        /// Calculates the <see cref="InitialVerticalVelocityQty"/> from the
        /// <see cref="InitialVelocityQty"/> and <see cref="VerticalLaunchAngleQty"/> components.
        /// </summary>
        /// <param name="ivQty"></param>
        /// <param name="vlaQty"></param>
        /// <returns></returns>
        /// <see cref="TrigonometricExtensionMethods.Sin"/>
        /// <a href="!:http://hyperphysics.phy-astr.gsu.edu/hbase/traj.html#tra5" >Trajectories,
        /// height of trajectory</a>
        private static IQuantity CalculateInitialVerticalVelocity(IQuantity ivQty, IQuantity vlaQty)
        {
            var metersPerSecond = V.MetersPerSecond;

            var iv = VerifyDimension(ivQty, metersPerSecond);
            var vla = VerifyDimension(vlaQty, SiTheta.Radian);

            var ivvQty = (Quantity) iv*vla.Sin();

            return VerifyDimension(ivvQty, metersPerSecond);
        }

        /// <summary>
        /// Calculates the <see cref="InitialHorizontalVelocityQty"/> from the
        /// <see cref="InitialVelocityQty"/> and <see cref="VerticalLaunchAngleQty"/> components.
        /// </summary>
        /// <param name="ivQty"></param>
        /// <param name="vlaQty"></param>
        /// <returns></returns>
        /// <see cref="TrigonometricExtensionMethods.Cos"/>
        /// <a href="!:http://hyperphysics.phy-astr.gsu.edu/hbase/traj.html#tra5" >Trajectories,
        /// height of trajectory</a>
        private static IQuantity CalculateInitialHorizontalVelocity(IQuantity ivQty, IQuantity vlaQty)
        {
            var metersPerSecond = V.MetersPerSecond;

            var iv = VerifyDimension(ivQty, metersPerSecond);
            var vla = VerifyDimension(vlaQty, SiTheta.Radian);

            var ihvQty = (Quantity) iv*vla.Cos();

            return VerifyDimension(ihvQty, metersPerSecond);
        }
    }
}
