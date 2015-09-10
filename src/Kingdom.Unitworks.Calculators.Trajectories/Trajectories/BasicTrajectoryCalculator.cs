using Kingdom.Unitworks.Calculators.Trajectories.Components;

namespace Kingdom.Unitworks.Calculators.Trajectories
{
    /// <summary>
    /// Represents a basic <see cref="TrajectoryCalculator"/>.
    /// </summary>
    /// <see cref="BasicHorizontalComponentCalculator"/>
    /// <see cref="BasicVerticalComponentCalculator"/>
    /// <see cref="BasicMaximumTimeComponentCalculator"/>
    /// <see cref="BasicMaximumHeightComponentCalculator"/>
    /// <see cref="BasicMaximumRangeComponentCalculator"/>
    public class BasicTrajectoryCalculator : TrajectoryCalculator
    {
        /// <summary>
        /// Default Constructor
        /// </summary>
        public BasicTrajectoryCalculator()
            : base(new BasicHorizontalComponentCalculator(),
                new BasicVerticalComponentCalculator(),
                new BasicMaximumTimeComponentCalculator(),
                new BasicMaximumHeightComponentCalculator(),
                new BasicMaximumRangeComponentCalculator())
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parameters"></param>
        public BasicTrajectoryCalculator(ITrajectoryParameters parameters)
            : this()
        {
            Parameters = parameters;
        }
    }
}
