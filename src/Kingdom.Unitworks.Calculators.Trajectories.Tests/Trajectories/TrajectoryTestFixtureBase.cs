using System;
using System.Collections.Generic;
using Kingdom.Unitworks.Calculators.Trajectories.Components;
using NUnit.Framework;

namespace Kingdom.Unitworks.Calculators.Trajectories
{
    public abstract class TrajectoryTestFixtureBase<TParameters, TCalculator>
        : TestFixtureBase
        where TParameters : class, ITrajectoryParameters, new()
        where TCalculator : class, ITrajectoryCalculator, new()
    {
        /// <summary>
        /// Gets the Parameters.
        /// </summary>
        protected TParameters Parameters { get; private set; }

        /// <summary>
        /// Gets the Calculator.
        /// </summary>
        protected TCalculator Calculator { get; private set; }

        public override void TestFixtureSetUp()
        {
            base.TestFixtureSetUp();

            Calculator = new TCalculator();

            Assert.That(Calculator, Is.Not.Null);
        }

        public override void TestFixtureTearDown()
        {
            base.TestFixtureTearDown();

            Calculator = null;
        }

        public override void SetUp()
        {
            base.SetUp();

            Parameters = new TParameters();

            Assert.That(Parameters, Is.Not.Null);
            Assert.That(Calculator, Is.Not.Null);

            Calculator.Parameters = Parameters;
        }

        public override void TearDown()
        {
            Parameters = null;

            base.TearDown();
        }
    }

    internal static class TrajectoryCalculatorResultExtensionMethods
    {
        /// <summary>
        /// Returns a verified <see cref="IQuantity"/> from <paramref name="results"/>
        /// corresponding to the <paramref name="component"/>.
        /// </summary>
        /// <param name="results"></param>
        /// <param name="component"></param>
        /// <returns>A verified <see cref="IQuantity"/> component.</returns>
        public static IQuantity Verify(this IReadOnlyDictionary<TrajectoryComponent, IQuantity> results,
            TrajectoryComponent component)
        {
            Assert.That(results.ContainsKey(component), "Expected {{{0}}} component.", component);

            var qty = results[component];

            Assert.That(qty, Is.Not.Null);

            return qty;
        }

        /// <summary>
        /// Tries to verify that <paramref name="results"/> contains the <paramref name="component"/>.
        /// Optionally provide whether such an item is <paramref name="expected"/>.
        /// </summary>
        /// <param name="results"></param>
        /// <param name="component"></param>
        /// <param name="expected"></param>
        /// <returns></returns>
        public static bool TryVerify(this IReadOnlyDictionary<TrajectoryComponent, IQuantity> results,
            TrajectoryComponent component, bool? expected = null)
        {
            var tried = results.ContainsKey(component);

            if (expected != null)
            {
                Assert.That(tried, Is.EqualTo(expected.Value));
            }

            return tried;
        }

        /// <summary>
        /// Verifies that <paramref name="actualQty"/> equals <paramref name="expectedQty"/>.
        /// </summary>
        /// <param name="actualQty"></param>
        /// <param name="expectedQty"></param>
        /// <returns>The <paramref name="actualQty"/>.</returns>
        public static IQuantity Verify(this IQuantity actualQty, IQuantity expectedQty)
        {
            Assert.That(actualQty, Is.Not.Null);
            Assert.That(expectedQty, Is.Not.Null);
            Assert.That(expectedQty, Is.Not.SameAs(actualQty));
            Assert.That(actualQty.Equals(expectedQty), "Expected {{{0}}} but was {{{1}}}",
                expectedQty, actualQty);
            return actualQty;
        }

        public static IQuantity Verify(this IQuantity actualQty, Action<IQuantity> verify = null)
        {
            Assert.That(actualQty, Is.Not.Null);
            verify = verify ?? (x => { });
            verify(actualQty);
            return actualQty;
        }
    }
}
