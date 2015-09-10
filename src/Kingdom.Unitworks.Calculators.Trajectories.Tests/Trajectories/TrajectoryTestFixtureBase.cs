using System;
using System.Collections.Generic;
using System.Linq;
using Kingdom.Unitworks.Calculators.Trajectories.Components;
using NUnit.Framework;

namespace Kingdom.Unitworks.Calculators.Trajectories
{
    using L = Dimensions.Systems.SI.Length;

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

        /// <summary>
        /// <see cref="ITrajectoryCalculator.Calculated"/> observable handler.
        /// </summary>
        /// <param name="e"></param>
        protected abstract void OnNext(TrajectoryCalculatorEventArgs e);
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

        /// <summary>
        /// Returns the verified <paramref name="actualQty"/>.
        /// </summary>
        /// <param name="actualQty"></param>
        /// <param name="verify"></param>
        /// <returns></returns>
        public static IQuantity Verify(this IQuantity actualQty, Action<IQuantity> verify = null)
        {
            Assert.That(actualQty, Is.Not.Null);
            verify = verify ?? (x => { });
            verify(actualQty);
            return actualQty;
        }

        /// <summary>
        /// Verifies that the <paramref name="trajectory"/> is in fact a trajectory. Assumes
        /// that the items in the collection are presented in the correct order. Even with a
        /// couple of sensible, educated assumptions being made as to what constitutes a
        /// trajectory, this is still a fairly naïve implementation.
        /// </summary>
        /// <param name="trajectory"></param>
        /// <param name="maxHeightQty"></param>
        /// <returns></returns>
        public static IEnumerable<Tuple<IQuantity, IQuantity>> Verify(
            this IEnumerable<Tuple<IQuantity, IQuantity>> trajectory,
            IQuantity maxHeightQty)
        {
            Assert.That(trajectory, Is.Not.Null);
            Assert.That(maxHeightQty, Is.Not.Null);

            // ReSharper disable PossibleMultipleEnumeration
            CollectionAssert.IsNotEmpty(trajectory);

            var zero = Quantity.Zero(L.Meter);

            //TODO: TBD: at this moment, we will assume that the earth is "flat"
            // We want the one with least difference to measured max height.
            var peakItem = (from item in trajectory
                where (Quantity) item.Item2 > zero
                orderby Math.Abs(((Quantity) maxHeightQty - item.Item2).Value)
                select item).FirstOrDefault();

            Assert.That(peakItem, Is.Not.Null);

            var peakIndex = trajectory.ToList().IndexOf(peakItem);

            Assert.That(peakIndex, Is.GreaterThanOrEqualTo(0));

            /* Get the "increasing" and "decreasing" trajectory items this way, because once we
             * factor in aerodynamic resistance forces, we may actually encounter trajectories
             * that "loop back" on themselves, where X is unreliable, per se. */

            // ReSharper disable PossibleNullReferenceException
            var increasingSlopeItems = (from i in Enumerable.Range(0, peakIndex + 1)
                select trajectory.ElementAtOrDefault(i)).ToArray();

            var decreasingSlopeItems = (from i in Enumerable.Range(peakIndex, trajectory.Count() - peakIndex)
                select trajectory.ElementAtOrDefault(i)).ToArray();

            var increasingCount = increasingSlopeItems.Count();
            var decreasingCount = decreasingSlopeItems.Count();

            // Remember the second of the zipped collections occurs "after" the first ones.
            var increasing = increasingSlopeItems.Take(increasingCount - 1)
                .Zip(increasingSlopeItems.Skip(1),
                    (a, b) => (Quantity) b.Item2 - a.Item2 > zero).ToArray();

            var decreasing = decreasingSlopeItems.Take(decreasingCount - 1)
                .Zip(decreasingSlopeItems.Skip(1),
                    (a, b) => (Quantity) b.Item2 - a.Item2 < zero).ToArray();

            Assert.That(increasing.All(x => x));
            Assert.That(decreasing.All(x => x));

            return trajectory;
        }
    }
}
