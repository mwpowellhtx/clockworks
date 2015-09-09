using System;
using NUnit.Framework;

namespace Kingdom.Unitworks.Calculators.Fixtures
{
    internal abstract class CalculatorFixtureBase<TCalculator> : IDisposable
        where TCalculator : class, ICalculator, new()
    {
        protected readonly TCalculator Calculator;

        protected readonly QuantityComparer Comparer;

        private const int Precision = 4;

        /// <summary>
        /// Protected Constructor
        /// </summary>
        protected CalculatorFixtureBase()
        {
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
