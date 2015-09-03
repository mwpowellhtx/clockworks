using System.Collections.Generic;
using System.Linq;

namespace Kingdom.Unitworks.Dimensions
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class DerivedDimension : Dimension, IDerivedDimension
    {
        private readonly List<IDimension> _dimensions = new List<IDimension>();

        /// <summary>
        /// 
        /// </summary>
        public override bool IsBaseUnit
        {
            get
            {
                //Debug.Assert(false);

                //Console.WriteLine("Any dimensions: {0} Count base units: {1}",
                //    Dimensions.Any(), Dimensions.Count(x => x.IsBaseUnit));

                return !Dimensions.Any() || Dimensions.All(d => d.IsBaseUnit);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<IDimension> Dimensions
        {
            get { return _dimensions; }
        }

        internal IList<IDimension> InternalDimensions
        {
            get { return _dimensions; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dimensions"></param>
        protected DerivedDimension(params IDimension[] dimensions)
            : this(null, null, null, dimensions)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abbreviation"></param>
        /// <param name="dimensions"></param>
        protected DerivedDimension(string abbreviation, params IDimension[] dimensions)
            : this(abbreviation, null, null, dimensions)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="abbreviation"></param>
        /// <param name="toBase"></param>
        /// <param name="fromBase"></param>
        /// <param name="dimensions"></param>
        protected DerivedDimension(string abbreviation,
            IUnitConversion toBase, IUnitConversion fromBase,
            params IDimension[] dimensions)
            : base(abbreviation)
        {
            //TODO: may order these, by exponent?
            _dimensions.AddRange(dimensions);
            ToBase = toBase ?? new DerivedUnitConversion(() => Dimensions, d => d.ToBase);
            FromBase = fromBase ?? new DerivedUnitConversion(() => Dimensions, d => d.FromBase);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        protected DerivedDimension(DerivedDimension other)
            : this(other.Abbreviation, other.ToBase, other.FromBase,
                (from d in other.Dimensions select (IDimension) d.Clone()).ToArray())
        {
            Exponent = other.Exponent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public bool TryReplace(IDimension unit)
        {
            var result = true;

            for (var i = 0; i < _dimensions.Count; i++)
            {
                var d = _dimensions[i];

                {
                    /* If this level of derivation did not have the unit then perhaps any of the
                     * contained derivations does. Replaces the first such occurrence. */

                    var derived = d as IDerivedDimension;

                    if (derived != null)
                    {
                        result = result && derived.TryReplace(unit);
                        continue;
                    }
                }

                if (d.DimensionId != unit.DimensionId) continue;

                var replacing = _dimensions[i] = (IDimension) d.Clone();

                replacing.Exponent = d.Exponent;
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public IDerivedDimension Replace(IDimension unit)
        {
            TryReplace(unit);
            return this;
        }

        private static bool Equals(IDerivedDimension a, IDimension b)
        {
            return !(a == null || b == null)
                   && a.IsDimensionEquals(b)
                   && a.AreEquivalent(b as IDerivedDimension)
                   && a.Exponent == b.Exponent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(IDimension other)
        {
            return Equals(this, other);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (Exponent == 0)
                return string.Empty;

            var text = Abbreviation;

            if (string.IsNullOrEmpty(text))
            {
                var formattedDimensions = from d in Dimensions select d.ToString();
                text = string.Format("( {0} )", string.Join(" ", formattedDimensions));
            }

            var formatted = text;

            if (Exponent != 1)
                formatted = string.Format("{0}^{1}", text, Exponent);

            return formatted;
        }
    }
}
