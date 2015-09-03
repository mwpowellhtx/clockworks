using System;
using System.Collections.Generic;
using Kingdom.Unitworks.Dimensions;
using NUnit.Framework;

namespace Kingdom.Unitworks.Calculators.Fixtures
{
    internal abstract class CircularCalculatorFixtureBase<TCalculator> : IDisposable
        where TCalculator : class, ICalculator, new()
    {
        protected readonly TCalculator Calculator;

        protected readonly IEnumerable<IDimension> Dimensions;

        protected readonly QuantityComparer Comparer;

        private const int Precision = 4;

        /// <summary>
        /// Protected Constructor
        /// </summary>
        /// <param name="dimensions"></param>
        protected CircularCalculatorFixtureBase(
            params IDimension[] dimensions)
        {
            Dimensions = dimensions;
            Calculator = new TCalculator();
            Comparer = new QuantityComparer(Precision);
            // TODO: should be a first class unit test closer to the primary QuantityComparer assembly(ies)...
            Assert.That(Comparer.Epsilon, Is.EqualTo(Math.Pow(10d, Precision)));
        }

        /// <summary>
        /// Disposes the object.
        /// </summary>
        public abstract void Dispose();
    }
}
