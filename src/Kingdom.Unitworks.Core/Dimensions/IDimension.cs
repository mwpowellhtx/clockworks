using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Kingdom.Unitworks.Dimensions
{
    /// <summary>
    /// 
    /// </summary>
    public interface IDimension : ICloneable, IEquatable<IDimension>
    {
        /// <summary>
        /// 
        /// </summary>
        Type DimensionType { get; }

        /// <summary>
        /// 
        /// </summary>
        Guid DimensionId { get; }

        /// <summary>
        /// 
        /// </summary>
        bool IsBaseUnit { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IDimension GetBase();

        /// <summary>
        /// 
        /// </summary>
        int Exponent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string Abbreviation { get; }

        /// <summary>
        /// 
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// 
        /// </summary>
        IUnitConversion ToBase { get; }

        /// <summary>
        /// 
        /// </summary>
        IUnitConversion FromBase { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        double ConvertToBase(double value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        double ConvertFromBase(double value);

        ///
        //TODO: re-implement this in the base, using Dimension base class, DimensionType, and some helper methods diving into the class hierarchy
        bool IsDimensionEquals(IDimension other);

        /// <summary>
        /// 
        /// </summary>
        string ExponentText { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IDimension Invert();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IDimension Squared();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IDimension Cubed();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="power"></param>
        /// <returns></returns>
        IDimension Power(int power);
    }

    /// <summary>
    /// 
    /// </summary>
    public class SimpleCompatibleDimensionComparer : IEqualityComparer<IDimension>
    {
        public bool Equals(IDimension x, IDimension y)
        {
            // The ids should be "dimensionally sound" well by this point.
            return x.DimensionId == y.DimensionId;
        }

        public int GetHashCode(IDimension obj)
        {
            return obj.DimensionId.GetHashCode();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class DimensionExtensionMethods
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dimensions"></param>
        /// <returns></returns>
        public static IEnumerable<IDimension> Clone(this IEnumerable<IDimension> dimensions)
        {
            // This can just clone the dimensions and not worry about the exponent not being part of them.
            return from d in dimensions
                             ?? new IDimension[0]
                select (IDimension) d.Clone();
        }

        ///// <summary>
        ///// Returns the Reciprocal of the <paramref name="dimension"/>, leaving the original
        ///// <typeparamref name="TDimension"/> intact.
        ///// </summary>
        ///// <typeparam name="TDimension"></typeparam>
        ///// <param name="dimension"></param>
        ///// <returns></returns>
        //public static TDimension Invert<TDimension>(this TDimension dimension)
        //    where TDimension : IDimension
        //{
        //    var inverted = (TDimension) dimension.Clone();
        //    inverted.Exponent = 0 - dimension.Exponent;
        //    return inverted;
        //}

        /// <summary>
        /// Returns the Reciprocalof the <paramref name="dimensions"/>, leaving the original
        /// <see cref="IDimension"/> intact.
        /// </summary>
        /// <param name="dimensions"></param>
        /// <returns></returns>
        public static IEnumerable<IDimension> Invert(this IEnumerable<IDimension> dimensions)
        {
            return from d in dimensions select d.Invert();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dimensions"></param>
        /// <param name="other"></param>
        /// <param name="includeExponents"></param>
        /// <returns></returns>
        public static bool AreCompatible(this IEnumerable<IDimension> dimensions,
            IEnumerable<IDimension> other, bool includeExponents = false)
        {
            // Be sure to enumerate all of the dimensions, including the derived ones.
            dimensions = (from d in dimensions.EnumerateAll()
                orderby d.DimensionId, d.Exponent
                select d).ToArray();

            other = (from d in other.EnumerateAll()
                orderby d.DimensionId, d.Exponent
                select d).ToArray();

            if (dimensions.Count() != other.Count()) return false;

            //TODO: TBD: should reduce units first ? or assume that caller knows what he's doing ...

            return dimensions.Zip(other,
                (a, b) => a.DimensionId.Equals(b.DimensionId)
                          && (!includeExponents || a.Exponent == b.Exponent)).All(x => x);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dimension"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool AreEquivalent(this IDerivedDimension dimension, IDerivedDimension other)
        {
            return !(dimension == null || other == null)
                   && dimension.Dimensions.AreEquivalent(other.Dimensions);
        }

        /// <summary>
        /// Returns whether <paramref name="dimensions"/> and <paramref name="others"/> AreCompatible.
        /// Assumes that they have both been reduced and are in optimum state for comparison, in both
        /// dimension as well as scale.
        /// </summary>
        /// <param name="dimensions"></param>
        /// <param name="others"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool AreEquivalent(this IEnumerable<IDimension> dimensions, IEnumerable<IDimension> others)
        {
            //TODO: this one may be the one to use ... consider removing "arecompatible"
            if (dimensions == null)
                throw new ArgumentNullException("dimensions");

            if (others == null)
                throw new ArgumentNullException("others");

            var dim = dimensions.EnumerateAll().Reduce()
                .OrderBy(d => d.DimensionId).ThenBy(d => d.Exponent).ToArray();

            var oth = others.EnumerateAll().Reduce()
                .OrderBy(d => d.DimensionId).ThenBy(d => d.Exponent).ToArray();

            return dim.Length == oth.Length && dim.Zip(oth,
                (d, o) => d.DimensionId.Equals(o.DimensionId)
                          && d.Exponent == o.Exponent)
                .All(x => x);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="derivedDimension"></param>
        /// <returns></returns>
        private static IEnumerable<IDimension> EnumerateAll(this IDerivedDimension derivedDimension)
        {
            if (derivedDimension == null) yield break;

            foreach (var d in derivedDimension.Dimensions)
            {
                foreach (var c in
                    from x in (d as IDerivedDimension).EnumerateAll()
                    select (IDimension) x.Clone())
                {
                    c.Exponent *= derivedDimension.Exponent;
                    yield return c;
                }

                if (d is IDerivedDimension) continue;

                var cloned = (IDimension) d.Clone();

                yield return cloned;
            }
        }

        /// <summary>
        /// Replaces the <paramref name="dimensions"/> with the desired <paramref name="units"/>
        /// in situ. That is, replaces the instances in the list itself with the desired units.
        /// Drills into any that are <see cref="DerivedDimension"/> in the same manner.
        /// </summary>
        /// <param name="dimensions">It is recommended that this be called on a clone of the original.</param>
        /// <param name="units">The desired units replacing one or more of the <paramref name="dimensions"/>.</param>
        /// <returns>The <paramref name="dimensions"/> list.</returns>
        public static IList<IDimension> ReplaceUnits(this IList<IDimension> dimensions,
            params IDimension[] units)
        {
            //TODO: may put some checks in to verify relative compatibility of each ... i.e. replacing units that are actually there in dimension
            for (var i = 0; i < dimensions.Count; i++)
            {
                var d = dimensions[i];

                var unit = units.FirstOrDefault(x => x.DimensionId == d.DimensionId);

                if (unit != null)
                {
                    var replaced = (IDimension) unit.Clone();
                    replaced.Exponent = d.Exponent;
                    dimensions[i] = replaced;
                    // Do not continue here but let it fall through to do any derived replacing.
                }

                {
                    var derived = dimensions[i] as DerivedDimension;
                    if (derived == null) continue;
                    derived.InternalDimensions.ReplaceUnits(units);
                }
            }

            return dimensions;
        }

        /// <summary>
        /// Returns the set of <see cref="IDimension"/> clones, diving deep into each
        /// <see cref="IDerivedDimension"/> as necessary. This handles applying the correct
        /// <see cref="IDimension.Exponent"/>, especially across derived concerns.
        /// </summary>
        /// <param name="dimensions"></param>
        /// <returns></returns>
        public static IEnumerable<IDimension> EnumerateAll(this IEnumerable<IDimension> dimensions)
        {
            foreach (var d in dimensions)
            {
                Debug.Assert(d != null);

                if (d.Exponent == 0) continue;

                var derived = d as IDerivedDimension;

                if (derived == null)
                {
                    yield return (IDimension) d.Clone();
                    continue;
                }

                foreach (var cloned in (from x in derived.Dimensions.EnumerateAll()
                    select x.Clone()).OfType<IDimension>())
                {
                    cloned.Exponent *= derived.Exponent;
                    yield return cloned;
                }
            }
        }

        /// <summary>
        /// Reduces the given <paramref name="dimensions"/> using standard Dimensional Analysis techniques,
        /// similar to unit-factor method, or factor-label method. In this case, we've got a collection of
        /// dimensions with already-contributing exponents, zero, positive, or negative, and we want to ensure
        /// that we've got the optimum dimension at the end of the operation. The right-most dimension wins since
        /// it will have appeared on the right hand side of the operator, for which the user is performing calculations.
        /// Zero exponents are dropped from the equation altogether as per standard practice.
        /// </summary>
        /// <param name="dimensions"></param>
        /// <returns></returns>
        public static IEnumerable<IDimension> Reduce(this IEnumerable<IDimension> dimensions)
        {
            /* Will assume that the contributing lists have been reduced from prior iterations. Will
             * further assume that whatever merging that has taken place is in order, the right-most
             * one taking precedence concerning unit system. Will further, further assume that whatever
             * scaling to and from base units has already taken place prior to this helper being invokved. */

            var grouped = from d in dimensions group d by d.DimensionType;

            var result = new List<IDimension>();

            foreach (var g in grouped)
            {
                IDimension eligible = null;

                // The right-most one wins the conversation.
                foreach (var next in g.Reverse())
                {
                    if (eligible == null)
                    {
                        // Very important to Clone in order to preserve Exponents, etc, across known instances.
                        eligible = (IDimension) next.Clone();
                        continue;
                    }

                    // Factor in the next dimension.
                    eligible.Exponent += next.Exponent;
                }

                // The dimension has not been factored out of the operation.
                if (eligible != null && eligible.Exponent != 0)
                    result.Add(eligible);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static IEnumerable<IDimension> Multiply(this IEnumerable<IDimension> a, IEnumerable<IDimension> b)
        {
            var multiplied = new List<IDimension>();

            // Reduction should yield cloned dimensions for us for the remainder of the helper method.
            var la = a.EnumerateAll().Reduce().OrderBy(x => x.DimensionId).ToArray();
            var lb = b.EnumerateAll().Reduce().OrderBy(x => x.DimensionId).ToArray();

            var cdc = new SimpleCompatibleDimensionComparer();

            var excepted = la.Except(lb, cdc).Concat(lb.Except(la, cdc)).ToArray();

            multiplied.AddRange(excepted);

            la = la.Except(excepted, cdc).ToArray();
            lb = lb.Except(excepted, cdc).ToArray();

            // By this moment, should be paired up with the same dimension(s).
            var lz = la.Zip(lb, (d, o) => new Tuple<IDimension, IDimension>(d, o)).ToArray();

            foreach (var z in lz)
            {
                var delta = z.Item1.Exponent + z.Item2.Exponent;

                if (delta == 0) continue;

                var cloned = (IDimension) z.Item1.Clone();

                cloned.Exponent = delta;

                multiplied.Add(cloned);
            }

            return multiplied;
        }
    }
}
