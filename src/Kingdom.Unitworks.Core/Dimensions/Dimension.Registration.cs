using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Kingdom.Unitworks.Dimensions
{
    public abstract partial class Dimension
    {
        protected static readonly IDictionary<Type, Guid> Registered
            = new ConcurrentDictionary<Type, Guid>();

        /// <summary>
        /// Registers the <paramref name="dimension"/> of type <see cref="IBaseDimension"/>.
        /// </summary>
        /// <param name="dimension"></param>
        protected static void Register(IBaseDimension dimension)
        {
            Register<IBaseDimension>(dimension);
        }

        /// <summary>
        /// Registers the <paramref name="dimension"/> of type <see cref="IDerivedDimension"/>.
        /// </summary>
        /// <param name="dimension"></param>
        protected static void Register(IDerivedDimension dimension)
        {
            Register<IDerivedDimension>(dimension);
        }

        ///// <summary>
        ///// Registers the <paramref name="dimension"/> of type <see cref="IDimensionless"/>.
        ///// </summary>
        ///// <param name="dimension"></param>
        //public static void Register(IDimensionless dimension)
        //{
        //    Register<IDimensionless>(dimension);
        //}

        /// <summary>
        /// Returns with a QualififiedId. That is, the id should be non <see cref="Guid.Empty"/>,
        /// and it should be unique among the registered types. Other than that, since dealing with
        /// these things is a runtime concern, we just need it to be unique at runtime.
        /// </summary>
        /// <returns></returns>
        private static Guid GetQualififiedId()
        {
            var qualifiedId = Guid.Empty;

            // Ensure that it is unique.
            while (qualifiedId == Guid.Empty || Registered.Values.Contains(qualifiedId))
                qualifiedId = Guid.NewGuid();

            return qualifiedId;
        }

        private static Type GetQualifiedDimensionType(Type parentType)
        {
            var eligibleTypes = parentType.GetInterfaces();

            // Must inherit from one and only one of these types.
            var oneAndOnlyOneType = new[]
            {
                typeof (IBaseDimension),
                typeof (IDerivedDimension),
                ////TODO: may include dimensionless among these after all. it's kind of a special case, especially where abbreviations and such are involved...
                //typeof (IDimensionless)
            };

            // Can still return Null, but if we have more than one appearance, that's bad.
            if (eligibleTypes.Count(t => oneAndOnlyOneType.Any(x => t == x)) > 1)
            {
                var message = string.Format(
                    @"Dimension parent type {0} must implement one and only one qualifying dimensional interface: {1}.",
                    parentType, string.Join(", ", from t in oneAndOnlyOneType select t));

                throw new ArgumentException(message, "parentType");
            }

            // Examine the hierarchical levels in waves of eligibility.
            while (eligibleTypes.Any())
            {
                // This is one question we're asking to refine the field of qualififed types.
                var nextTypes = eligibleTypes.Where(t => t.GetInterfaces()
                    .Any(x => oneAndOnlyOneType.Any(y => y == x))).ToArray();

                var qualifiedType = nextTypes.SingleOrDefault();

                if (qualifiedType != null)
                    return qualifiedType;

                eligibleTypes = nextTypes;
            }

            return null;
        }

        /// <summary>
        /// Finally registers the <paramref name="unit"/> given a type
        /// of <typeparamref name="TDimension"/>.
        /// </summary>
        /// <typeparam name="TDimension"></typeparam>
        /// <param name="unit"></param>
        private static void Register<TDimension>(TDimension unit)
            where TDimension : IDimension
        {
            if (unit == null)
                throw new ArgumentNullException("unit");

            var d = unit as Dimension;

            if (d == null)
            {
                var message = string.Format("Dimension must inherit from the {0} class.", typeof(Dimension));
                throw new ArgumentException(message, "unit");
            }

            // We must know the Dimension Type.
            var dimensionType = d.GetType();

            // Obtain the qualififed interface type.
            var qualifiedType = GetQualifiedDimensionType(dimensionType);

            if (qualifiedType == null)
            {
                var message = string.Format("Dimension {0} bad inheritence tree cannot be registered.", dimensionType);
                throw new ArgumentException(message, "unit");
            }

            // Go ahead and set the DimensionType now.
            d.DimensionType = qualifiedType;

            // Register dimensions once and only once.
            if (Registered.ContainsKey(qualifiedType)) return;

            Registered.Add(qualifiedType, GetQualififiedId());
        }

        public Type DimensionType { get; private set; }

        public Guid DimensionId
        {
            get
            {
                var t = DimensionType;
                return (t != null && Registered.ContainsKey(t)) ? Registered[t] : Guid.Empty;
            }
        }

        public bool IsDimensionEquals(IDimension other)
        {
            return DimensionId == other.DimensionId;
        }

        //static Dimension()
        //{
        //    /*
        //     * TODO: the following dimensions/units may require some attention ...
        //     * BASE UNITS
        //     * https://en.wikipedia.org/wiki/SI_base_unit
        //     * meters m (ILength)
        //     * kilogram kg (IMass)
        //     * second s (ITime)
        //     * ampere A (electrical current)
        //     * kelvin K ([thermodynamic] ITemperature)
        //     * mole mol (amount of substance)
        //     * candela cd (luminous intensity)
        //     * 
        //     * DERIVED UNITS
        //     * https://en.wikipedia.org/wiki/SI_derived_unit
        //     * hertz HZ (IFrequency)
        //     * radian rad (IAngle)
        //     * newton N (IForce)
        //     */
        //    /* There may be many several too many to concern ourselves with enumerating them
        //     * by hand. So we will leverage the power of Reflection in order to identify the
        //     * dimensional interfaces themselves. */
        //    var types = new[] { typeof(ILength),typeof(IMass),typeof(ITime) , typeof(ITemperature),};
        //    Registered[typeof (ITime)] = Guid.NewGuid();
        //}
    }
}
