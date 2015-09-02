using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Kingdom.Unitworks.Dimensions;

namespace Kingdom.Unitworks
{
    using IDEX = IncompatibleDimensionsException;

    /// <summary>
    /// 
    /// </summary>
    public partial class Quantity : IQuantity
    {
        private readonly IList<IDimension> _dimensions = new List<IDimension>();

        public IReadOnlyCollection<IDimension> Dimensions
        {
            get { return new ReadOnlyCollection<IDimension>(_dimensions); }
        }

        //TODO: dimensions applied as a dictionary/collection of IDimension? including prefix, ordered according to dimensionality, exponent, etc

        /// <summary>
        /// 
        /// </summary>
        public virtual double Value { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Quantity()
            : this(0d)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        public Quantity(double value)
        {
            Initialize(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dimensions"></param>
        public Quantity(double value, IEnumerable<IDimension> dimensions)
            : this(value, dimensions.ToArray())
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="dimensions"></param>
        public Quantity(double value, params IDimension[] dimensions)
        {
            Initialize(value, dimensions);
        }

        private void Initialize(double value, params IDimension[] dimensions)
        {
            Value = value;

            /* For some operations this may seem a little wasteful to constantly be cloning, and
             * discarding, instances. But, in the general use case, this is exactly what we want
             * to be doing, so as to avoid dimensions that communicate across quantities. */

            foreach (var cloned in from d in dimensions select (IDimension) d.Clone())
                _dimensions.Add(cloned);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="units"></param>
        /// <returns></returns>
        public IQuantity ConvertTo(params IDimension[] units)
        {
            //TODO: TBD: use cases like Time per Time (i.e. a ratio of time unit to potentially other time unit)
            //TODO: TBD: therefore, I think I must need to know the dimensions with precise Exponents as well ...
            //TODO: TBD: may then also replace none, some, or all, depending ...

            var destination = (from d in Dimensions select (IDimension) d.Clone()).ToList();

            destination.ReplaceUnits(units);

            /* Remember we want to yield a whole other Quantity and leave the original intact. Every single
             * operation we do during conversion will operate on the clones of the original dimensions. */

            var baseValue = Dimensions.Aggregate(Value, (g, x) => x.ConvertToBase(g));
            var convertedValue = destination.Aggregate(baseValue, (g, x) => x.ConvertFromBase(g));

            return new Quantity(convertedValue, destination.ToArray());
        }

        public virtual IQuantity ToBase()
        {
            var dims = Dimensions.Select(d =>
            {
                var cloned = (IDimension) d.GetBase().Clone();
                cloned.Exponent = d.Exponent;
                return cloned;
            }).ToArray();

            return ConvertTo(dims);
        }


        private static readonly Func<double, double, double> Addition = (a, b) => a + b;
        private static readonly Func<double, double, double> Subtraction = (a, b) => a - b;

        private static IQuantity Add(IQuantity a, IQuantity b, Func<double, double, double> adder)
        {
            a.VerifyDimensions(b);

            var aBase = a.ToBase();
            var bBase = b.ToBase();

            var aDims = aBase.Dimensions.EnumerateAll();
            var bDims = bBase.Dimensions.EnumerateAll();

            return new Quantity(adder(aBase.Value, bBase.Value), aDims.ToArray());
        }

        private static IQuantity Add(IQuantity a, double b, Func<double, double, double> adder)
        {
            a.VerifyDimensions();
            return new Quantity(adder(a.Value, b));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Quantity operator +(Quantity a, Quantity b)
        {
            return (Quantity) Add(a, b, Addition);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Quantity operator +(Quantity a, IQuantity b)
        {
            return (Quantity) Add(a, b, Addition);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Quantity operator +(IQuantity a, Quantity b)
        {
            return (Quantity) Add(a, b, Addition);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Quantity operator +(Quantity a, double b)
        {
            return (Quantity) Add(a, b, Addition);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Quantity operator +(double a, Quantity b)
        {
            return (Quantity) Add(b, a, Addition);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Quantity operator -(Quantity a, Quantity b)
        {
            return (Quantity) Add(a, b, Subtraction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Quantity operator -(Quantity a, IQuantity b)
        {
            return (Quantity) Add(a, b, Subtraction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Quantity operator -(IQuantity a, Quantity b)
        {
            return (Quantity) Add(a, b, Subtraction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Quantity operator -(Quantity a, double b)
        {
            return (Quantity) Add(a, b, Subtraction);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Quantity operator -(double a, Quantity b)
        {
            return (Quantity) Add(b, a, Subtraction);
        }

        private readonly Func<double, double, double> _multiplication = (a, b) => a*b;
        private readonly Func<double, double, double> _division = (a, b) => a*(1d/b);
        private readonly Func<double, double, double> _modulus = (a, b) => a%b;

        private static IQuantity Multiply(IQuantity a, IQuantity b, Func<double, double, double> multiplier,
            Func<IEnumerable<IDimension>, IEnumerable<IDimension>, IEnumerable<IDimension>> dimensionality)
        {
            var aBase = a.ToBase();
            var bBase = b.ToBase();

            var aDims = aBase.Dimensions.EnumerateAll();
            var bDims = bBase.Dimensions.EnumerateAll();

            return new Quantity(multiplier(aBase.Value, bBase.Value),
                dimensionality(aDims, bDims).Reduce().ToArray());
        }

        private static IQuantity Modulus(IQuantity a, IQuantity b, Func<double, double, double> modulus,
            Func<IEnumerable<IDimension>, IEnumerable<IDimension>> dimensionality = null)
        {
            dimensionality = dimensionality ?? (x => x);

            var aBase = a.ToBase();
            var bBase = b.ToBase();

            //TODO: must B really be dimensionless? what are the ramifications for modulus being a close neighbor of division?
            // A can be any dimension, however, b must be dimensionless.
            b.VerifyDimensions();

            var aDims = aBase.Dimensions.EnumerateAll();

            return new Quantity(modulus(aBase.Value, bBase.Value),
                dimensionality(aDims).Reduce().ToArray());
        }

        private static IQuantity Unary(IQuantity a, Func<double, double> function,
            Func<IEnumerable<IDimension>, IEnumerable<IDimension>> dimensionality = null)
        {
            dimensionality = dimensionality ?? (x => x.ToArray());

            var aBase = a.ToBase();

            var aDims = aBase.Dimensions.EnumerateAll();

            return new Quantity(function(aBase.Value),
                dimensionality(aDims).Reduce().ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Quantity operator *(Quantity a, Quantity b)
        {
            return (Quantity) Multiply(a, b, a._multiplication, (x, y) => x.Concat(y));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Quantity operator *(Quantity a, IQuantity b)
        {
            return (Quantity) Multiply(a, b, a._multiplication, (x, y) => x.Concat(y));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Quantity operator *(IQuantity a, Quantity b)
        {
            return (Quantity) Multiply(a, b, b._multiplication, (x, y) => x.Concat(y));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Quantity operator *(Quantity a, double b)
        {
            return (Quantity) Unary(a, x => x*b);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Quantity operator *(double a, Quantity b)
        {
            return (Quantity) Unary(b, x => a*x);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Quantity operator /(Quantity a, Quantity b)
        {
            return (Quantity) Multiply(a, b, a._division, (x, y) => x.Concat(y.Invert()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Quantity operator /(IQuantity a, Quantity b)
        {
            return (Quantity) Multiply(a, b, b._division, (x, y) => x.Concat(y.Invert()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Quantity operator /(Quantity a, IQuantity b)
        {
            return (Quantity) Multiply(a, b, a._division, (x, y) => x.Concat(y.Invert()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Quantity operator /(Quantity a, double b)
        {
            return (Quantity) Unary(a, x => x/b);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Quantity operator /(double a, Quantity b)
        {
            return (Quantity) Unary(b, x => a/x, x => x.Invert());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Quantity operator %(Quantity a, double b)
        {
            return (Quantity) Modulus(a, new Quantity(b), a._modulus);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Quantity operator %(Quantity a, IQuantity b)
        {
            return (Quantity) Modulus(a, b, a._modulus);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static Quantity operator %(Quantity a, Quantity b)
        {
            return (Quantity) Modulus(a, b, a._modulus);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Quantity operator -(Quantity a)
        {
            return (Quantity) Unary(a, x => -x, x => from d in x select (IDimension) d.Clone());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <returns></returns>
        public static Quantity operator +(Quantity a)
        {
            return (Quantity) Unary(a, x => x, x => from d in x select (IDimension) d.Clone());
        }

        public virtual IQuantity Inverse()
        {
            return 1/this;
        }

        public virtual IQuantity Squared()
        {
            return this*this;
        }

        public virtual IQuantity Cubed()
        {
            var squared = this*this;
            return squared*this;
        }

        /// <summary>
        /// Gets whether IsNaN.
        /// </summary>
        public virtual bool IsNaN
        {
            get { return double.IsNaN(Value); }
        }

        /// <summary>
        /// Gets whether IsPositiveInfinity.
        /// </summary>
        public virtual bool IsPositiveInfinity
        {
            get { return double.IsPositiveInfinity(Value); }
        }

        /// <summary>
        /// Gets whether IsNegativeInfinity.
        /// </summary>
        public virtual bool IsNegativeInfinity
        {
            get { return double.IsNegativeInfinity(Value); }
        }

        /// <summary>
        /// Gets whether IsInfinity.
        /// </summary>
        public virtual bool IsInfinity
        {
            get { return double.IsInfinity(Value); }
        }

        /// <summary>
        /// Gets whether IsDimensionless.
        /// </summary>
        public virtual bool IsDimensionless
        {
            get
            {
                // Oops should be returning TRUE here of couse.
                if (!Dimensions.Any()) return true;

                // Followed closely by, take the enumerated dimensions and reduce them.
                var enumerated = Dimensions.EnumerateAll().Reduce().ToArray();

                return !enumerated.Any();
            }
        }

        public override string ToString()
        {
            return string.Format("{0} {1}", Value,
                string.Join(" ", (from d in Dimensions select d.ToString()))).Trim();
        }

        public object Clone()
        {
            return new Quantity(Value, Dimensions.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static IQuantity Min(IQuantity x, IQuantity y)
        {
            if (x == null && y != null) return (IQuantity) y.Clone();
            if (y == null && x != null) return (IQuantity) x.Clone();
            if (x == null) return null;

            if ((Quantity) x > y) return (IQuantity) y.Clone();
            if ((Quantity) x < y) return (IQuantity) x.Clone();

            return (IQuantity) x.Clone();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static IQuantity Max(IQuantity x, IQuantity y)
        {
            if (x == null && y != null) return (IQuantity) y.Clone();
            if (y == null && x != null) return (IQuantity) x.Clone();
            if (x == null) return null;

            if ((Quantity) x < y) return (IQuantity) y.Clone();
            if ((Quantity) x > y) return (IQuantity) x.Clone();

            return (IQuantity) x.Clone();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dimensions"></param>
        /// <returns></returns>
        public static IQuantity Zero(IEnumerable<IDimension> dimensions)
        {
            return Zero(dimensions.ToArray());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dimensions"></param>
        /// <returns></returns>
        public static IQuantity Zero(params IDimension[] dimensions)
        {
            return new Quantity(0d, dimensions);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public static class QuantityExtensionMethods
    {
        /// <summary>
        /// Verifies that the <see cref="IQuantity.Dimensions"/> are aligned with <paramref name="otherQty"/>.
        /// </summary>
        /// <param name="qty"></param>
        /// <param name="otherQty"></param>
        public static void VerifyDimensions(this IQuantity qty, IQuantity otherQty)
        {
            if (qty.Dimensions.EnumerateAll().AreCompatible(otherQty.Dimensions.EnumerateAll())) return;

            var message = string.Format("Quantity {{{0}}} dimensions are incompatible with quantity {{{1}}} dimensions.",
                qty, otherQty);

            throw new IDEX(message, qty, otherQty);
        }

        /// <summary>
        /// Verifies that the <see cref="IQuantity.Dimensions"/> are aligned with <paramref name="dimensions"/>.
        /// </summary>
        /// <param name="qty"></param>
        /// <param name="dimensions"></param>
        public static void VerifyDimensions(this IQuantity qty, params IDimension[] dimensions)
        {
            if (qty.Dimensions.EnumerateAll().AreCompatible(dimensions.EnumerateAll())) return;

            var message = string.Format("Quantity {{{0}}} incompatible with dimensions {{{1}}}.",
                qty, string.Join(", ", from d in dimensions select d.ToString()));

            throw new IDEX(message, qty, Quantity.Zero(dimensions));
        }

        //TODO: refactor me...
        internal static void VerifyEquivalent(this IQuantity x, IQuantity y)
        {
            if (x.Dimensions.AreEquivalent(y.Dimensions)) return;

            var message = string.Format("{{{0}}} is incompatible with {{{1}}}", x, y);

            throw new IncompatibleDimensionsException(message, x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double Inverted(this double x)
        {
            return 1d/x;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double Squared(this double x)
        {
            return x.Power(2d);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static double Cubed(this double x)
        {
            return x.Power(3d);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static double Power(this double x, double y = 0d)
        {
            return y.Equals(0d) ? 1d : Math.Pow(x, y);
        }

        /// <summary>
        /// Returns a new <see cref="IQuantity"/> with <paramref name="value"/> and with the same
        /// dimensionality as <paramref name="qty"/>.
        /// </summary>
        /// <param name="qty"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IQuantity CloneDimensions(this IQuantity qty, double value = default(double))
        {
            return new Quantity(value, ReferenceEquals(null, qty) ?  new IDimension[0] : qty.Dimensions.ToArray());
        }
    }
}
