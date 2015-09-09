using System.Collections.Generic;
using Kingdom.Unitworks.Dimensions;

namespace Kingdom.Unitworks.Calculators.Fixtures
{
    internal abstract class CircularCalculatorFixtureBase<TCalculator>
        : CalculatorFixtureBase<TCalculator>
        where TCalculator : class, ICalculator, new()
    {
        protected readonly IEnumerable<IDimension> Dimensions;

        /// <summary>
        /// Protected Constructor
        /// </summary>
        /// <param name="dimensions"></param>
        protected CircularCalculatorFixtureBase(params IDimension[] dimensions)
        {
            Dimensions = dimensions;
        }
    }
}
